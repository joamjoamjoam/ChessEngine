using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public enum ChessmanColor
    {
        white,
        black
    }
    public abstract class Chessman
    {
        public BoardSpace position;
        public int score = -1;
        public ChessmanColor color = ChessmanColor.white;

    

        public abstract void getAvailableMoves();
        public abstract bool isMoveValid(Board parentBoard, BoardSpace moveSpace);

    }

    public class Pawn : Chessman
    {

        public Pawn(BoardSpace initPos, ChessmanColor color)
        {
            score = 10;
            position = initPos;
            this.color = color;
        }

        public override void getAvailableMoves()
        {
            throw new NotImplementedException();
        }
        public override bool isMoveValid(Board parentBoard, BoardSpace moveSpace)
        {
            throw new NotImplementedException();
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

        public Knight(BoardSpace initPos, ChessmanColor color)
        {
            score = 100;
            position = initPos;
            this.color = color;
        }

        public override void getAvailableMoves()
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, BoardSpace moveSpace)
        {
            throw new NotImplementedException();
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
        public Rook(BoardSpace initPos, ChessmanColor color)
        {
            score = 500;
            position = initPos;
            this.color = color;
        }

        public override void getAvailableMoves()
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, BoardSpace moveSpace)
        {
            throw new NotImplementedException();
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

        public Bishop(BoardSpace initPos, ChessmanColor color)
        {
            score = 100;
            position = initPos;
            this.color = color;
        }

        public override void getAvailableMoves()
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, BoardSpace moveSpace)
        {
            throw new NotImplementedException();
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

        public Queen(BoardSpace initPos, ChessmanColor color)
        {
            score = 5000;
            position = initPos;
            this.color = color;
        }

        public override void getAvailableMoves()
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, BoardSpace moveSpace)
        {
            throw new NotImplementedException();
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

        public King(BoardSpace initPos, ChessmanColor color)
        {
            score = 1000000;
            position = initPos;
            this.color = color;
        }

        public override void getAvailableMoves()
        {
            throw new NotImplementedException();
        }

        public override bool isMoveValid(Board parentBoard, BoardSpace moveSpace)
        {
            throw new NotImplementedException();
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
