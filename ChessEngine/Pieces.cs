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
        public abstract bool isMoveValid(Board parentBoard, Move move);
        public abstract Image getImage();

    }

    public class Pawn : Chessman
    {
        private Image whiteImage;
        private Image blackImage;

        public Pawn(BoardSpace initPos, ChessmanColor color)
        {
            score = 10;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhitePawn;
            blackImage = Properties.Resources.BlackPawn;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }
        public override bool isMoveValid(Board parentBoard, Move move)
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
            score = 100;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteKnight;
            blackImage = Properties.Resources.BlackKnight;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, Move move)
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
            score = 500;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteRook;
            blackImage = Properties.Resources.BlackRook;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, Move move)
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
            score = 100;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteBishop;
            blackImage = Properties.Resources.BlackBishop;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, Move move)
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
            score = 5000;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteQueen;
            blackImage = Properties.Resources.BlackQueen;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, Move move)
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
            score = 1000000;
            position = initPos;
            this.color = color;
            whiteImage = Properties.Resources.WhiteKing;
            blackImage = Properties.Resources.BlackKing;
        }

        public override List<Move> getAvailableMoves(Board boardState)
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, Move move)
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
