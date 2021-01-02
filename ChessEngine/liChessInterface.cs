﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ChessEngine
{
    


    class ChessEngine
    {
    }

    public class Board
    {
        BoardSpace[,] boardState = new BoardSpace[8, 8]; //Column, Row


        public Board(String fen = "")
        {
            // A1 is Bottom Left Corner, H8 is top Right Corner
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Char tmp = (Char)('A' + col);
                    int effectiveRow = 8 - row;
                    boardState[row, col] = new BoardSpace(tmp, effectiveRow);

                    if (fen != "")
                    {
                        // Process FEN
                        boardState[row, col].generatePieceFromFen(fen);
                    }
                    else
                    {
                        if (effectiveRow == 2 || effectiveRow == 7)
                        {
                            boardState[row, col].piece = new Pawn(boardState[row, col], (effectiveRow == 2) ? ChessmanColor.white : ChessmanColor.black);
                        }
                        else if (effectiveRow == 1 || effectiveRow == 8)
                        {
                            // Setup White BackRow
                            switch (tmp)
                            {
                                case 'A':
                                    boardState[row, col].piece = new Rook(boardState[row, col], ((effectiveRow == 1) ? ChessmanColor.white : ChessmanColor.black));
                                    break;
                                case 'B':
                                    boardState[row, col].piece = new Knight(boardState[row, col], ((effectiveRow == 1) ? ChessmanColor.white : ChessmanColor.black));
                                    break;
                                case 'C':
                                    boardState[row, col].piece = new Bishop(boardState[row, col], ((effectiveRow == 1) ? ChessmanColor.white : ChessmanColor.black));
                                    break;
                                case 'D':
                                    boardState[row, col].piece = new Queen(boardState[row, col], ((effectiveRow == 1) ? ChessmanColor.white : ChessmanColor.black));
                                    break;
                                case 'E':
                                    boardState[row, col].piece = new King(boardState[row, col], ((effectiveRow == 1) ? ChessmanColor.white : ChessmanColor.black));
                                    break;
                                case 'F':
                                    boardState[row, col].piece = new Bishop(boardState[row, col], ((effectiveRow == 1) ? ChessmanColor.white : ChessmanColor.black));
                                    break;
                                case 'G':
                                    boardState[row, col].piece = new Knight(boardState[row, col], ((effectiveRow == 1) ? ChessmanColor.white : ChessmanColor.black));
                                    break;
                                case 'H':
                                    boardState[row, col].piece = new Rook(boardState[row, col], ((effectiveRow == 1) ? ChessmanColor.white : ChessmanColor.black));
                                    break;
                            }
                        }
                    }
                }

                    
                    
            }
        }

        public Board(List<String> uciMovesList) : this()
        {

            foreach (String move in uciMovesList)
            {
                if (!updateBoardForMove(move))
                {
                    throw new Exception($"Move '{move}' is Invalid.");
                }
            }
        }


        public bool updateBoardForMove(String move)
        {
            bool moveValid = false;
            bool kingInCheck = false;
            String fromPos = move.Substring(0, 2).ToUpper();
            String toPos = move.Substring(2, 2).ToUpper();

            try
            {
                BoardSpace fromSpace = getSpace(fromPos[0], int.Parse(new String(new char[] { fromPos[1] })));
                BoardSpace toSpace = getSpace(toPos[0], int.Parse(new String(new char[] { toPos[1] })));

                if (fromSpace.piece != null)
                {
                    Chessman fromPiece = fromSpace.piece;

                    if (true) // Do check if move is valid for piece type
                    {
                        if (fromPiece.GetType() == typeof(King) && ((King)fromPiece).canCastle && ((fromPiece.color == ChessmanColor.white && fromSpace.position.Item1 == 'E' && fromSpace.position.Item2 == 1 && (toSpace.position.Item1 == 'G' && toSpace.position.Item2 == 1) || (toSpace.position.Item1 == 'C' && toSpace.position.Item2 == 1)) || (fromPiece.color == ChessmanColor.black && fromSpace.position.Item1 == 'E' && fromSpace.position.Item2 == 8 && (toSpace.position.Item1 == 'G' && toSpace.position.Item2 == 8) || (toSpace.position.Item1 == 'C' && toSpace.position.Item2 == 8))))
                        {
                            King kingPiece = ((King)fromPiece);

                            //Attempting a castle VAlidate it
                            BoardSpace rookSpace = null;

                            if (kingPiece.color == ChessmanColor.white && (toSpace.position.Item1 == 'G' && toSpace.position.Item2 == 1))
                            {
                                // White King Side Castle
                                rookSpace = getSpace('H', 1);

                                if (rookSpace.piece.GetType() == typeof(Rook) && ((Rook)rookSpace.piece).canCastle) // Was the King Side Rook moved before now
                                {
                                    if (getSpace('F', 1).piece == null && getSpace('G', 1).piece == null) // Are all the spaces between the Rook and Knight Clear
                                    {

                                        if (true) // Are any Spaces the King Must Move Under Attack. (E1(Check),F1, G1) (Skipping for now until we can check this)
                                        {
                                            movePieceOnBoard(fromSpace, toSpace); // Move King
                                            movePieceOnBoard(rookSpace, getSpace('F', 1)); // Move Rook
                                            moveValid = true;
                                        }

                                    }

                                }
                            }
                            else if (kingPiece.color == ChessmanColor.white && (toSpace.position.Item1 == 'C' && toSpace.position.Item2 == 1))
                            {
                                // White Queen Side Castle
                                rookSpace = getSpace('A', 1);

                                if (rookSpace.piece.GetType() == typeof(Rook) && ((Rook)rookSpace.piece).canCastle) // Was the King Side Rook moved before now
                                {
                                    if (getSpace('B', 1).piece == null && getSpace('C', 1).piece == null && getSpace('D', 1).piece == null) // Are all the spaces between the Rook and Knight Clear
                                    {

                                        if (true) // Are any Spaces the King Must Move Under Attack. (E1(Check), D1, C1) (Skipping for now until we can check this)
                                        {
                                            movePieceOnBoard(fromSpace, toSpace); // Move King
                                            movePieceOnBoard(rookSpace, getSpace('D', 1)); // Move Rook
                                            moveValid = true;
                                        }

                                    }

                                }
                            }
                            else if (kingPiece.color == ChessmanColor.black && (toSpace.position.Item1 == 'G' && toSpace.position.Item2 == 8))
                            {
                                // White King Side Castle
                                rookSpace = getSpace('H', 8);

                                if (rookSpace.piece.GetType() == typeof(Rook) && ((Rook)rookSpace.piece).canCastle) // Was the King Side Rook moved before now
                                {
                                    if (getSpace('F', 8).piece == null && getSpace('G', 8).piece == null) // Are all the spaces between the Rook and Knight Clear
                                    {

                                        if (true) // Are any Spaces the King Must Move Under Attack. (E8(Check),F8, G8) (Skipping for now until we can check this)
                                        {
                                            movePieceOnBoard(fromSpace, toSpace); // Move King
                                            movePieceOnBoard(rookSpace, getSpace('F', 8)); // Move Rook
                                            moveValid = true;
                                        }

                                    }

                                }
                            }
                            else if (kingPiece.color == ChessmanColor.black && (toSpace.position.Item1 == 'C' && toSpace.position.Item2 == 8))
                            {
                                // White Queen Side Castle
                                rookSpace = getSpace('A', 8);

                                if (rookSpace.piece.GetType() == typeof(Rook) && ((Rook)rookSpace.piece).canCastle) // Was the King Side Rook moved before now
                                {
                                    if (getSpace('B', 8).piece == null && getSpace('C', 8).piece == null && getSpace('D', 8).piece == null) // Are all the spaces between the Rook and Knight Clear
                                    {
                                        if (true) // Are any Spaces the King Must Move Under Attack. (E1(Check), D1, C1) (Skipping for now until we can check this)
                                        {
                                            movePieceOnBoard(fromSpace, toSpace); // Move King
                                            movePieceOnBoard(rookSpace, getSpace('D', 8)); // Move Rook
                                            moveValid = true;

                                            
                                        }

                                    }

                                }
                            }

                            if (moveValid)
                            {
                                // Grab All King and Rooks for the color and disable castling
                                foreach (BoardSpace s in boardState)
                                {
                                    if (s.piece != null && s.piece.color == fromPiece.color)
                                    {
                                        if (s.piece.GetType() == typeof(Rook))
                                        {
                                            ((Rook)s.piece).canCastle = false;
                                        }
                                        else if (s.piece.GetType() == typeof(King))
                                        {
                                            ((King)s.piece).canCastle = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            movePieceOnBoard(fromSpace, toSpace);
                            moveValid = true;
                        }
                    }

                }
                else
                {
                    // There should always be a piece in the from Space
                }
            }
            catch
            {

            }




            return moveValid;
        }

        private Chessman movePieceOnBoard(BoardSpace fromSpace, BoardSpace toSpace)
        {
            // This is kust updating the data model. Validation Should occur prior to this
            // This function Assumes that the moves are valid.

            Chessman fromPiece = fromSpace.piece;
            Chessman killedPiece = null;

            if (toSpace.piece != null)
            {
                // We killed piece remove it.
                killedPiece = toSpace.piece;

            }
            fromPiece.position = toSpace;
            toSpace.piece = fromPiece;
            fromSpace.piece = null;

            if (toSpace.piece.GetType() == typeof(King))
            {
                King kp = (King)toSpace.piece;
                if (kp.canCastle)
                {
                    kp.canCastle = false;
                }
            }
            else if (toSpace.piece.GetType() == typeof(Rook))
            {
                Rook rp = (Rook)toSpace.piece;
                if (rp.canCastle)
                {
                    rp.canCastle = false;
                }
            }

            return killedPiece;
        }


        public BoardSpace getSpace(char col, int row)
        {
            BoardSpace rv = null;
            try
            {
                if (col >= 'A' && col <= 'H' && row >= 1 && row <= 8)
                {
                    rv = boardState[BoardSpace.getIndexForBoardRow(row), BoardSpace.getIndexForBoardColumn(col)];
                }
            }
            catch
            {

            }


            return rv;
        }


        public String printBoard(String color)
        {
            String boardStr = "";
            
            if (color.ToLower() == "black")
            {
                boardStr = "    | H | G | F | E | D | C | B | A |".ToLower() + Environment.NewLine;
                for (int row = 7; row >= 0; row--)
                {
                    boardStr += $"{8-row}  |";
                    for (int col = 7; col >= 0; col--)
                    {
                        boardStr += $" {boardState[row, col].ToString()} |";
                    }
                    boardStr += Environment.NewLine;
                }
            }
            else if (color.ToLower() == "white")
            {
                boardStr = "    | A | B | C | D | E | F | G | H |".ToLower() + Environment.NewLine;
                for (int row = 0; row < 8; row++)
                {
                    boardStr += $"{8 - row}  |";
                    for (int col = 0; col < 8; col++)
                    {
                        boardStr += $" {boardState[row, col].ToString()} |";
                    }
                    boardStr += Environment.NewLine;
                }
            }

            return boardStr;
        }

    };

    public class BoardSpace
    {
        public readonly Tuple<Char, int> position;
        public Chessman piece = null;

        public BoardSpace(Char column, int row)
        {
            if (column >= 'A' && column <= 'H' && row >= 1 && row <= 8)
            {
                // Valid Board Space
                position = new Tuple<char, int>(column, row);
            }
            else
            {
                throw new InvalidDataException($"{column}{row} is an invalid space designation.");
            }
        }

        public override String ToString()
        {
            //return $"{position.Item1}{position.Item2}";
            String rv = "";
            if (piece != null)
            {
                rv = piece.ToString();
            }
            else
            {
                rv = "-";
            }

            return rv;
        }

        public static int getIndexForBoardRow(int row)
        {
            return 8 - row;
        }

        public static int getIndexForBoardColumn(Char Col)
        {
            return Col - 'A';
        }

        public Chessman generatePieceFromFen(String fen)
        {
            Chessman rv = null;

            List<String> fenBoard = fen.Split(new char[] { '/'}).ToList();
            fenBoard.Reverse();


            // replace Empty Squares with _
            if (Regex.IsMatch(fenBoard[position.Item2 - 1], "[0-9]"))
            {
                foreach (Match m in Regex.Matches(fenBoard[position.Item2 - 1], "[0-9]"))
                {
                    int emptySpaces = int.Parse(m.Value);

                    fenBoard[position.Item2 - 1] = Regex.Replace(fenBoard[position.Item2 - 1], $"{m.Value}", string.Concat(Enumerable.Repeat("_", emptySpaces)));
                }
            }

            try
            {
                for (int i = 0; i <= 7; i++)
                {
                    int emptySpaces = 0;
                    Char nextFenRep = fenBoard[position.Item2-1][i];

                        // Is a piece
                    if (position.Item1 == ('A' + i))
                    {
                        if (nextFenRep == '_')
                        {
                            piece = null;
                        }
                        else
                        {
                            // Setup White BackRow
                            switch (Char.ToUpper(nextFenRep))
                            {
                                case 'K':
                                    piece = new King(this, ((Char.IsLower(nextFenRep)) ? ChessmanColor.black : ChessmanColor.white));
                                    break;
                                case 'R':
                                    piece = new Rook(this, ((Char.IsLower(nextFenRep)) ? ChessmanColor.black : ChessmanColor.white));
                                    break;
                                case 'P':
                                    piece = new Pawn(this, ((Char.IsLower(nextFenRep)) ? ChessmanColor.black : ChessmanColor.white));
                                    break;
                                case 'B':
                                    piece = new Bishop(this, ((Char.IsLower(nextFenRep)) ? ChessmanColor.black : ChessmanColor.white));
                                    break;
                                case 'N':
                                    piece = new Knight(this, ((Char.IsLower(nextFenRep)) ? ChessmanColor.black : ChessmanColor.white));
                                    break;
                                case 'Q':
                                    piece = new Queen(this, ((Char.IsLower(nextFenRep)) ? ChessmanColor.black : ChessmanColor.white));
                                    break;
                            }
                        }
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            catch
            {
                
            }

            return rv;
        }
    };

    public class Game
    {
        public readonly String fullId = "";
        public readonly String gameId = "";
        public readonly String color = "";
        public readonly String lastMove = "";
        public readonly String opponentName = "";
        public readonly String opponentID = "";
        public Board gameBoard = null;
        //public readonly String opponentLevel = "";
        public Uri url = null;
        public UCIParserContext uciParser = null;
        public String authToken = "";
        public WebClient client = new WebClient();
        public Task gameStreamTask = null;

        // New Event for When a new Move is recieved. either UCI or from lichess
        public delegate void GameBoardUpdated();
        public event GameBoardUpdated BoardUpdated;

        public Game(JObject json, String authToken)
        {
            if (json != null)
            {
                fullId = (String)json["fullId"];
                gameId = (String)json["gameId"];
                lastMove = (String)json["lastMove"];
                color = (String)json["color"];

                opponentName = (String)((JObject)json["opponent"])["username"];
                opponentID = (String)((JObject)json["opponent"])["id"];

                uciParser = new UCIParserContext(this);
                this.authToken = authToken;

                // generate Starting Board
                try
                {
                    String fenStr = ((String)json["fen"]).ToString();
                    gameBoard = new Board(fenStr);

                }
                catch
                {
                    gameBoard = new Board();
                }
            }
        }

        public void startGameStream()
        {
            if (gameStreamTask == null)
            {
                gameStreamTask = new Task(new Action(gameStateStreamHandler));
                gameStreamTask.Start();
            }
        }

        public void stopGameStream()
        {
            // add cancellation token here
        }

        private void gameStateStreamHandler()
        {
            client.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {authToken}");
            client.OpenReadCompleted += (sender, e) => {
                StreamReader reader = new StreamReader(e.Result);
                while (!reader.EndOfStream)
                {
                    String tmp = reader.ReadLine();
                    if (tmp != "")
                    {
                        Debug.WriteLine(tmp);
                        JObject json = JObject.Parse(tmp);
                        String msgType = (String)json["type"];
                        try
                        {
                            if (msgType == "gameState")
                            {
                                this.uciParser.executeUCIOperation($"position startpos moves {(String)json["moves"]}");
                            }
                        }
                        catch
                        {

                        }


                        // Fire Event for UI to refresh Display
                        if (BoardUpdated != null)
                        {
                            BoardUpdated.Invoke();
                        }
                    }
                    
                }

                reader.Close();

            };
            client.OpenReadAsync(new Uri(Helper.apiBaseEndpoint + $"/bot/game/stream/{fullId}"));
        }


        public Board getBoard()
        {
            return gameBoard;
        }

        public bool makeMove(String move)
        {
            bool validMove = false;
            HttpStatusCode res = HttpStatusCode.Unused;
            String response = Account.sendWebAPIRequest("POST", Helper.apiBaseEndpoint + $"/bot/game/{fullId}/move/{move.ToLower().ToString()}", "", "application/json", out res, authToken);

            if (res == HttpStatusCode.OK)
            {
                validMove = true;
            }


            return validMove;
        }

        public override string ToString()
        {
            return $"Game ({fullId}) against {opponentName} Playing {color}: Last Move {lastMove}";
        }
    };

    public class Account
    {
        public enum AccountType
        {
            USER,
            BOT
        }



        public List<Game> currentGames = new List<Game>();
        public readonly AccountType acctType = AccountType.USER;
        public readonly String username = "";
        public readonly String id = "";
        public bool online = false;
        public String authToken = "";

        private readonly String title = "";

        private Account()
        {

        }


        public Account(String authToken)
        {
            this.authToken = authToken;
            HttpStatusCode res = HttpStatusCode.Unused;
            String response = sendWebAPIRequest("GET", Helper.apiBaseEndpoint+"/account", "", "application/json", out res, authToken);
            
            if (res == HttpStatusCode.OK)
            {
                JObject obj = JObject.Parse(response);

                if (obj != null)
                {
                    username = (String)obj["username"];
                    id = (String)obj["id"];
                    online = (bool)obj["online"];

                    title = (String)obj["title"];

                    if (title == "BOT")
                    {
                        acctType = AccountType.BOT;
                    }


                    getGames();
                }
                else
                {
                    throw new Exception($"Error Serializing Response for Account");
                }


            }
            else
            {
                throw new Exception($"No Account with authToken '{authToken}' exists on Lichess.org");
            }

        }

        public bool getGames()
        {
            // Get Current games
            bool rv = false;
            HttpStatusCode res = HttpStatusCode.Unused;
            String response = sendWebAPIRequest("GET", Helper.apiBaseEndpoint + "/account/playing", "", "application/json", out res, authToken);

            if (res == HttpStatusCode.OK)
            {
                JObject gamesObj = JObject.Parse(response);

                if (gamesObj != null)
                {

                    currentGames.Clear();
                    foreach (JObject gameObj in ((JArray)gamesObj["nowPlaying"]))
                    {
                        Game tmp = new Game(gameObj, authToken);
                        if (tmp != null)
                        {
                            currentGames.Add(tmp);
                        }

                    }
                    rv = true;
                }
                else
                {
                    throw new Exception($"Error Fetching Games");
                }


            }
            else
            {
                throw new Exception($"Error fetching Games List {response}");
            }

            return rv;
        }

        public Account(Account fromAccount)
        {
            username = fromAccount.username;
            id = fromAccount.id;
            online = fromAccount.online;
            acctType = fromAccount.acctType;
            title = fromAccount.title;
            currentGames = fromAccount.currentGames;
        }

        public static String sendWebAPIRequest(String reqType, String endPoint, String requestBody, String contentType, out HttpStatusCode result, String authToken = "")
        {
            String respBody = "";

            result = HttpStatusCode.Unused;
            try
            {
                WebRequest req = WebRequest.Create(endPoint);
                req.Timeout = 5000;
                req.Method = reqType;
                if (authToken != "")
                {
                    req.Headers["Authorization"] = "Bearer " + authToken;
                }

                UTF8Encoding encoding = new UTF8Encoding();
                req.ContentType = contentType;
                req.Headers.Add("Accept-Encoding", "utf-8");

                if (reqType == "POST")
                {

                    string postData = requestBody;

                    byte[] serializedJson = encoding.GetBytes(postData);

                    // Set the content type of the data being posted.
                    //req.ContentType = "application/json";

                    // Set the content length of the string being posted.
                    req.ContentLength = serializedJson.Length;

                    Stream newStream = req.GetRequestStream();

                    newStream.Write(serializedJson, 0, serializedJson.Length);

                }



                //req.Credentials = new NetworkCredential("username", "password");
                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                if (resp != null && resp.GetResponseStream() != null)
                {
                    result = resp.StatusCode;
                    Stream s = resp.GetResponseStream();

                    if (resp.CharacterSet.ToLower() == "utf-8" || resp.CharacterSet.ToLower() == "")
                    {
                        StreamReader rd = new StreamReader(s, Encoding.UTF8);

                        respBody = rd.ReadToEnd();
                        resp.Close();
                    }

                }

                result = resp.StatusCode;
            }
            catch (WebException e)
            {
                String errorMessage = "";
                result = HttpStatusCode.Unused;
                if (e.Response != null && e.Response.GetResponseStream() != null)
                {
                    HttpWebResponse res = (HttpWebResponse)e.Response;
                    result = res.StatusCode;
                    Stream s = res.GetResponseStream();

                    if (res.CharacterSet.ToLower() == "utf-8")
                    {
                        StreamReader rd = new StreamReader(s, Encoding.UTF8);

                        errorMessage = rd.ReadToEnd();

                        respBody = $"{e.Message} Error Sending Web Request - {errorMessage}";



                        e.Response.Close();
                    }

                }
            }

            return respBody;
        }
    };

    public static class Helper
    {
        public static string apiBaseEndpoint = "https://lichess.org/api";
    }
    
}