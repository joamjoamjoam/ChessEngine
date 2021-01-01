using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class UCIParserContext
    {
        Game currentGame = null;

        public UCIParserContext(Game gameforContext)
        {
            currentGame = gameforContext;
        }

        public bool executeUCIOperation(String uciOp)
        {
            bool rv = false;

            List<String> command = uciOp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            int fieldNum = 1;
            switch (command[0])
            {
                case "position":
                    // update Board Position
                    Board board = null;

                    // Initial Postion FEN or startpos
                    if (command[fieldNum] == "startpos")
                    {
                        board = new Board();
                        fieldNum++;
                    }
                    else
                    {
                        board = new Board(command[fieldNum++]);
                    }
                    currentGame.gameBoard = board;

                    fieldNum++; // moves



                    for (; fieldNum < command.Count; fieldNum++)
                    {
                        board.updateBoardForMove(command[fieldNum]);
                    }

                    

                    break;
            }


            return rv;
        }
    }
}
