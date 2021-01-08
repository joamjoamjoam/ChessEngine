using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class AI
    {
        Game gameContext = null;

        public AI(Game context)
        {
            gameContext = context;
        }

        public Move getBestMove()
        {
            Move bestMove = null;

            bestMove = getBestMoveForBoard(gameContext.gameBoard, gameContext.color);

            return bestMove;
        }

        public static Move getBestMoveForBoard(Board gameBoard, ChessmanColor playerColor)
        {
            Move bestMove = null;

            List<Move> moveList = gameBoard.getAllAvailableMovesForPlayer(playerColor);
            Random rand = new Random();
            moveList.Sort(); // Sort by highest score of piece killed
            int highestScore = moveList[0].score;

            List<Move> moveChoiceList = new List<Move>();

            if (moveList.Count > 0)
            {
                if (gameBoard.isKingInCheck(playerColor))
                {
                    // Check if the King Has Valid Moves without Sacrafice
                    foreach (Move mv in moveList)
                    {
                        if (mv.piece.GetType() == typeof(King))
                        {
                            moveChoiceList.Add(mv);
                        }
                    }

                    if (moveChoiceList.Count == 0)
                    {
                        // Check if you can kill All Attackers

                        // No Valid King moves Sacrafice the Lowest Scoring Piece
                    }

                }
                else
                {

                    moveChoiceList = moveList.Where(c => c.score == highestScore).ToList();
                }

                if (moveChoiceList.Count > 0)
                {
                    bestMove = moveChoiceList[rand.Next(moveChoiceList.Count)];
                }
                else
                {
                    // No Valid Moves Stalemate
                }
                

            }

            return bestMove;
        }
    }

    public static class AIWeights
    {
        public static int pawnScore = 1;
        public static int bishopScore = 3;
        public static int knightScore = 3;
        public static int rookScore = 7;
        public static int queenScore = 9;
        public static int kingScore = 1000;
    }
}
