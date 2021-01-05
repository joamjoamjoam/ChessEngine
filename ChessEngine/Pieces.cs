using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public enum ChessmanColor
    {
        none,
        white,
        black
    }
    public abstract class Chessman
    {
        public BoardSpace position;
        public int score = -1;
        public ChessmanColor color = ChessmanColor.white;




        public abstract List<Move> getAvailableMoves(Board boardState);
        public abstract Image getImage();

    }

    public class Pawn : Chessman
    {
        private Image whiteImage;
        private Image blackImage;
        public  bool canMoveTwice = true;

        public Pawn(BoardSpace initPos, ChessmanColor color)
        {
            score = AIWeights.pawnScore;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhitePawn;
            blackImage = Properties.Resources.BlackPawn;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            List<Move> validMoveList = new List<Move>();
            // Get Valid Moves

            int numMoves = (canMoveTwice) ? 2 : 1;
            BoardSpace toPos = null;

            // Then handle White Games not showing valid moves correctly
            if (true /*!kingIsInCheck*/)
            {
                // Normal Moves

                // First Move
                for (int i = 1; i <= numMoves; i++)
                {
                    toPos = boardState.getSpace(Convert.ToChar(position.position.Item1), (color == ChessmanColor.white) ? position.position.Item2 + i : position.position.Item2 - i);

                    if (toPos != null)
                    {
                        if (toPos.piece == null)
                        {
                            Move newMove = new Move(this, position, toPos, boardState, 0);

                            // Is A Promoting Move
                            if ((color == ChessmanColor.white) ? (toPos.position.Item2 == 8) : (toPos.position.Item2 == 1))
                            {
                                newMove.score = AIWeights.queenScore;
                            }

                            validMoveList.Add(newMove);
                        }
                        else
                        {
                            break;
                        }

                    }
                    else
                    {
                        continue;
                    }

                }

                // Attacking Moves
                toPos = boardState.getSpace(Convert.ToChar(position.position.Item1 + 1), (color == ChessmanColor.white) ? position.position.Item2 + 1 : position.position.Item2 - 1);
                BoardSpace toPos2 = boardState.getSpace(Convert.ToChar(position.position.Item1 - 1), (color == ChessmanColor.white) ? position.position.Item2 + 1 : position.position.Item2 - 1);
                List<BoardSpace> possibleMoves = new List<BoardSpace>() { toPos, toPos2 };
                foreach (BoardSpace space in possibleMoves)
                {
                    if (space != null)
                    {
                        if (space != null && space.piece != null && space.piece.color == Helper.getOpponentColor(color))
                        {
                            validMoveList.Add(new Move(this, position, space, boardState, ((space.piece.color == Helper.getOpponentColor(color)) ? space.piece.score : 0)));
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                // Amend Scores for Pieces that Are now attacking this piece
            }

            return validMoveList;

        }


        public override Image getImage()
        {
            Image rv = null;
            if (color == ChessmanColor.white)
            {
                rv = whiteImage;
            }
            else
            {
                rv = blackImage;
            }

            return rv;
        }

        public override string ToString()
        {
            String rv = "p";
            if (color == ChessmanColor.black)
            {
                rv = rv.ToUpper();
            }
            return rv;
        }


    }

    public class Knight : Chessman
    {
        private Image whiteImage;
        private Image blackImage;
        public Knight(BoardSpace initPos, ChessmanColor color)
        {
            score = AIWeights.knightScore;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteKnight;
            blackImage = Properties.Resources.BlackKnight;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override Image getImage()
        {
            Image rv = null;
            if (color == ChessmanColor.white)
            {
                rv = whiteImage;
            }
            else
            {
                rv = blackImage;
            }

            return rv;
        }

        public override string ToString()
        {
            String rv = "n";
            if (color == ChessmanColor.black)
            {
                rv = rv.ToUpper();
            }
            return rv;
        }
    }

    public class Rook : Chessman
    {
        public bool canCastle = true;
        private Image whiteImage;
        private Image blackImage;
        public Rook(BoardSpace initPos, ChessmanColor color)
        {
            score = AIWeights.rookScore;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteRook;
            blackImage = Properties.Resources.BlackRook;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override Image getImage()
        {
            Image rv = null;
            if (color == ChessmanColor.white)
            {
                rv = whiteImage;
            }
            else
            {
                rv = blackImage;
            }

            return rv;
        }

        public override string ToString()
        {
            String rv = "r";
            if (color == ChessmanColor.black)
            {
                rv = rv.ToUpper();
            }
            return rv;
        }
    }

    public class Bishop : Chessman
    {
        private Image whiteImage;
        private Image blackImage;
        public Bishop(BoardSpace initPos, ChessmanColor color)
        {
            score = AIWeights.bishopScore;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteBishop;
            blackImage = Properties.Resources.BlackBishop;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override Image getImage()
        {
            Image rv = null;
            if (color == ChessmanColor.white)
            {
                rv = whiteImage;
            }
            else
            {
                rv = blackImage;
            }

            return rv;
        }

        public override string ToString()
        {
            String rv = "b";
            if (color == ChessmanColor.black)
            {
                rv = rv.ToUpper();
            }
            return rv;
        }
    }

    public class Queen : Chessman
    {
        private Image whiteImage;
        private Image blackImage;
        public Queen(BoardSpace initPos, ChessmanColor color)
        {
            score = AIWeights.queenScore;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteQueen;
            blackImage = Properties.Resources.BlackQueen;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override Image getImage()
        {
            Image rv = null;
            if (color == ChessmanColor.white)
            {
                rv = whiteImage;
            }
            else
            {
                rv = blackImage;
            }

            return rv;
        }

        public override string ToString()
        {
            String rv = "q";
            if (color == ChessmanColor.black)
            {
                rv = rv.ToUpper();
            }
            return rv;
        }
    }

    public class King : Chessman
    {
        public bool canCastle = true;
        public bool inCheck = false;
        private Image whiteImage;
        private Image blackImage;

        public King(BoardSpace initPos, ChessmanColor color)
        {
            score = AIWeights.queenScore;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteKing;
            blackImage = Properties.Resources.BlackKing;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override Image getImage()
        {
            Image rv = null;
            if (color == ChessmanColor.white)
            {
                rv = whiteImage;
            }
            else
            {
                rv = blackImage;
            }

            return rv;
        }

        public override string ToString()
        {
            String rv = "k";
            if (color == ChessmanColor.black)
            {
                rv = rv.ToUpper();
            }
            return rv;
        }
    }
}
