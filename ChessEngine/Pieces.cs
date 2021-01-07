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

        public Pawn(Chessman fromPiece)
        {
            if (fromPiece.GetType() == typeof(Pawn))
            {
                Pawn from = (Pawn)fromPiece;
                score = from.score;
                position = null;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
                canMoveTwice = from.canMoveTwice;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }
        }

        public Pawn(Chessman fromPiece, BoardSpace initPos)
        {
            if (fromPiece.GetType() == typeof(Pawn))
            {
                Pawn from = (Pawn)fromPiece;
                score = from.score;
                position = initPos;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
                canMoveTwice = from.canMoveTwice;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }

        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            List<Move> validMoveList = new List<Move>();
            // Get Valid Moves

            int numMoves = (canMoveTwice) ? 2 : 1;
            BoardSpace toPos = null;

            if (!boardState.isKingInCheck(color))
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

        public Knight(Chessman fromPiece)
        {
            if (fromPiece.GetType() == typeof(Knight))
            {
                Knight from = (Knight)fromPiece;
                score = from.score;
                position = null;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }
        }

        public Knight(Chessman fromPiece, BoardSpace initPos)
        {
            if (fromPiece.GetType() == typeof(Knight))
            {
                Knight from = (Knight)fromPiece;
                score = from.score;
                position = initPos;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }

        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            List<Move> validMoveList = new List<Move>();

            // Get Valid Moves
            BoardSpace toPos = null;

            if (!boardState.isKingInCheck(color))
            {
                for (int x = -2; x <= 2; x++)
                {
                    for (int y = -2; y <= 2; y++)
                    {
                        if (x != 0 && y != 0 && (Math.Abs(x) != Math.Abs(y)))
                        {
                            toPos = boardState.getSpace(Convert.ToChar(position.position.Item1 + x), position.position.Item2 + y);

                            if (toPos != null)
                            {
                                if (toPos.piece == null || toPos.piece.color != color)
                                {
                                    Move newMove = new Move(this, position, toPos, boardState, (toPos.piece == null) ? 0 : toPos.piece.score);
                                    validMoveList.Add(newMove);
                                }
                            }
                        }
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

        public Rook(Chessman fromPiece)
        {
            if (fromPiece.GetType() == typeof(Rook))
            {
                Rook from = (Rook)fromPiece;
                score = from.score;
                position = null;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }
        }

        public Rook(Chessman fromPiece, BoardSpace initPos)
        {
            if (fromPiece.GetType() == typeof(Rook))
            {
                Rook from = (Rook)fromPiece;
                score = from.score;
                position = initPos;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }

        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            return getMovesForRook(boardState, color, position);
        }

        public static List<Move> getMovesForRook(Board boardState, ChessmanColor color, BoardSpace position)
        {
            List<Move> validMoveList = new List<Move>();

            // Get Valid Moves

            if (!boardState.isKingInCheck(color))
            {
                //Starting At Inital Position moves the 4 directions until the board is gone or a piece is reached
                char col = Convert.ToChar(position.position.Item1);
                int row = position.position.Item2 + 1;

                for (; row <= 8; row++)
                {
                    // increasing Row
                    BoardSpace moveSpace = boardState.getSpace(col, row);
                    if (moveSpace != null)
                    {

                        if (moveSpace.piece == null)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, 0));
                        }
                        else if (moveSpace.piece.color != color)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, moveSpace.piece.score));
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                col = Convert.ToChar(position.position.Item1);
                row = position.position.Item2 - 1;
                for (; row >= 1; row--)
                {
                    // increasing Diagonal
                    BoardSpace moveSpace = boardState.getSpace(col, row);
                    if (moveSpace != null)
                    {

                        if (moveSpace.piece == null)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, 0));
                        }
                        else if (moveSpace.piece.color != color)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, moveSpace.piece.score));
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                col = Convert.ToChar(position.position.Item1 + 1);
                row = position.position.Item2;
                for (; col <= 'H'; col++)
                {
                    // increasing Diagonal
                    BoardSpace moveSpace = boardState.getSpace(col, row);
                    if (moveSpace != null)
                    {

                        if (moveSpace.piece == null)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, 0));
                        }
                        else if (moveSpace.piece.color != color)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, moveSpace.piece.score));
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                col = Convert.ToChar(position.position.Item1 - 1);
                row = position.position.Item2;
                for (; col >= 'A'; col--)
                {
                    // increasing Diagonal
                    BoardSpace moveSpace = boardState.getSpace(col, row);
                    if (moveSpace != null)
                    {

                        if (moveSpace.piece == null)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, 0));
                        }
                        else if (moveSpace.piece.color != color)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, moveSpace.piece.score));
                            break;
                        }
                        else
                        {
                            break;
                        }
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

        public Bishop(Chessman fromPiece)
        {
            if (fromPiece.GetType() == typeof(Bishop))
            {
                Bishop from = (Bishop)fromPiece;
                score = from.score;
                position = null;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }
        }

        public Bishop(Chessman fromPiece, BoardSpace initPos)
        {
            if (fromPiece.GetType() == typeof(Bishop))
            {
                Bishop from = (Bishop)fromPiece;
                score = from.score;
                position = initPos;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }

        }
        public override List<Move> getAvailableMoves(Board boardState)
        {
            return getMovesForBishop(boardState, color, position);
        }

        public static List<Move> getMovesForBishop(Board boardState, ChessmanColor color, BoardSpace position)
        {
            List<Move> validMoveList = new List<Move>();

            // Get Valid Moves
            BoardSpace toPos = null;

            if (!boardState.isKingInCheck(color))
            {
                //Starting At Inital Position moves the 4 directions until the board is gone or a piece is reached
                char col = Convert.ToChar(position.position.Item1 + 1);
                int row = position.position.Item2 + 1;

                for ( ; row <= 8 && col <= 'H'; row++, col++)
                {
                    // increasing Diagonal
                    BoardSpace moveSpace = boardState.getSpace(col, row);
                    if (moveSpace != null)
                    {
                        
                        if (moveSpace.piece == null)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, 0));
                        }
                        else if (moveSpace.piece.color != color)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, moveSpace.piece.score));
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                col = Convert.ToChar(position.position.Item1 - 1);
                row = position.position.Item2 - 1;
                for (; row >= 1 && col >= 'A'; row--, col--)
                {
                    // increasing Diagonal
                    BoardSpace moveSpace = boardState.getSpace(col, row);
                    if (moveSpace != null)
                    {

                        if (moveSpace.piece == null)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, 0));
                        }
                        else if (moveSpace.piece.color != color)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, moveSpace.piece.score));
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                col = Convert.ToChar(position.position.Item1 + 1);
                row = position.position.Item2 - 1;
                for (; row >= 1 && col <= 'H'; row--, col++)
                {
                    // increasing Diagonal
                    BoardSpace moveSpace = boardState.getSpace(col, row);
                    if (moveSpace != null)
                    {

                        if (moveSpace.piece == null)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, 0));
                        }
                        else if (moveSpace.piece.color != color)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, moveSpace.piece.score));
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                col = Convert.ToChar(position.position.Item1 - 1);
                row = position.position.Item2 + 1;
                for (; row <= 8 && col >= 'A'; row++, col--)
                {
                    // increasing Diagonal
                    BoardSpace moveSpace = boardState.getSpace(col, row);
                    if (moveSpace != null)
                    {

                        if (moveSpace.piece == null)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, 0));
                        }
                        else if (moveSpace.piece.color != color)
                        {
                            validMoveList.Add(new Move(position.piece, position, moveSpace, boardState, moveSpace.piece.score));
                            break;
                        }
                        else
                        {
                            break;
                        }
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

        public Queen(Chessman fromPiece)
        {
            if (fromPiece.GetType() == typeof(Queen))
            {
                Queen from = (Queen)fromPiece;
                score = from.score;
                position = null;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }
        }

        public Queen(Chessman fromPiece, BoardSpace initPos)
        {
            if (fromPiece.GetType() == typeof(Queen))
            {
                Queen from = (Queen)fromPiece;
                score = from.score;
                position = initPos;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }

        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            List<Move> validMovesList = new List<Move>();
            if (!boardState.isKingInCheck(color))
            {
                validMovesList.AddRange(Bishop.getMovesForBishop(boardState, color, position));
                validMovesList.AddRange(Rook.getMovesForRook(boardState, color, position));
            }

            return validMovesList;
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

        public King(Chessman fromPiece)
        {
            if (fromPiece.GetType() == typeof(King))
            {
                King from = (King)fromPiece;
                score = from.score;
                position = null;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
                canCastle = from.canCastle;
        }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }
        }

        public King(Chessman fromPiece, BoardSpace initPos)
        {
            if (fromPiece.GetType() == typeof(King))
            {
                King from = (King)fromPiece;
                score = from.score;
                position = initPos;
                this.color = from.color;
                whiteImage = from.whiteImage;
                blackImage = from.blackImage;
                canCastle = from.canCastle;
            }
            else
            {
                throw new Exception($"Cant Create Copy of Piece becasue it is the wrong type");
            }

        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            List<Move> validMoveList = new List<Move>();
            // Get Valid Moves

            BoardSpace toPos = null;


            // Then handle White Games not showing valid moves correctly
            // Normal Moves

            // First Move
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        toPos = boardState.getSpace(Convert.ToChar(position.position.Item1 + x), position.position.Item2 + y);

                        if (toPos != null)
                        {
                            if (toPos.piece == null || toPos.piece.color != color)
                            {
                                Move newMove = new Move(this, position, toPos, boardState, (toPos.piece == null) ? 0 : toPos.piece.score);
                                Board newBoardAfterMove = new Board(boardState);
                                newBoardAfterMove.updateBoardForMove(newMove);

                                if (!newBoardAfterMove.isKingInCheck(color)) // Check if New Move is in Check
                                {
                                    validMoveList.Add(newMove);
                                }
                                else
                                {
                                  
                                }
                            }
                        }
                    }
                }
            }

            // Amend Scores for Pieces that Are now attacking this piece
            // Handle Castle

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
            String rv = "k";
            if (color == ChessmanColor.black)
            {
                rv = rv.ToUpper();
            }
            return rv;
        }
    }
}
