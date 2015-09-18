using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

//This class represents the entire board of a Flood Control game.
//It is responsible for:
//-Storing GamePiece objects for each square on the board.
//-Passing update calls to individual game pieces.
//-Randomly assigning types to game pieces.
//-Setting and clearing the "water" flag for each game piece.
//-Determining which pieces need to be filled with water.
//-Returning lists of potenially scoring chains.

namespace Flood_Control
{
    class GameBoard
    {
        Random rand = new Random();

        public const int GameBoardWidth = 8;
        public const int GameBoardHeight = 10;

        private GamePiece[,] boardSquares = new GamePiece[GameBoardWidth, GameBoardHeight];

        private List<Vector2> WaterTracker = new List<Vector2>();

        //Create the Game Board
        public GameBoard()
        {
            ClearBoard();
        }

        //Clear the board, change every square to an empty piece
        public void ClearBoard()
        {
            for (int x = 0; x < GameBoardWidth; x++)
                for (int y = 0; y < GameBoardHeight; y++)
                    boardSquares[x,y] = new GamePiece("Empty");
        }

        //Rotate a game piece
        public void RotatePiece(int x, int y, bool clockwise)
        {
            boardSquares[x,y].RotatePiece(clockwise);
        }

        //Get the source rectangle for a piece's current sprite
        public Rectangle GetSourceRect(int x, int y)
        {
            return boardSquares[x,y].GetSourceRect();
        }

        //Get a piece's square type
        public string GetSquare(int x, int y)
        {
            return boardSquares[x,y].PieceType;
        }

        //Set a piece's square type
        public void SetSquare(int x, int y, string pieceName)
        {
            boardSquares[x,y].SetPiece(pieceName);
        }

        //Check if a certain side of a piece has a connector
        public bool HasConnector(int x, int y, string direction)
        {
            return boardSquares[x,y].HasConnector(direction);
        }

        //Randomly set a piece's type
        public void RandomPiece(int x, int y)
        {
            boardSquares[x,y].SetPiece(GamePiece.PieceTypes[rand.Next(0,
                GamePiece.MaxPlayablePieceIndex + 1)]);
        }

        //After a scoring chain has been removed, move the above pieces downward
        public void FillFromAbove(int x, int y)
        {
            int rowLookup = y - 1;
            
            while (rowLookup >= 0)
            {
                if (GetSquare(x, rowLookup) != "Empty")
                {
                    SetSquare(x, y, GetSquare(x, rowLookup));
                    SetSquare(x, rowLookup, "Empty");
                    rowLookup = -1;
                }
                rowLookup--;
            }
        }

        //Generate new pieces to fill empty squares
        public void GenerateNewPieces(bool dropSquares)
        {
            if (dropSquares)
            {
                for (int x =0; x < GameBoardWidth; x++)
                {
                    for (int y = GameBoardHeight - 1; y>= 0; y--)
                    {
                        if (GetSquare(x,y) == "Empty")
                            FillFromAbove(x,y);
                    }
                }
            }

            for (int y = 0; y < GameBoardHeight; y++)
                for (int x = 0; x < GameBoardWidth; x++)
            {
                if (GetSquare(x,y) == "Empty")
                    RandomPiece(x,y);
            }
        }

        //Reset the water
        public void ResetWater()
        {
            for (int y = 0; y < GameBoardHeight; y++)
                for (int x = 0; x < GameBoardWidth; x++)
                    boardSquares[x,y].RemoveSuffix("W");
        }

        //Fill a piece with water
        public void FillPiece(int x, int y)
        {
            boardSquares[x,y].AddSuffix("W");
        }

        //Determine which piece should be filled with water
        public void PropagateWater(int x, int y, string fromDirection)
        {
            if ((y >= 0) && (y < GameBoardHeight) && (x >= 0) && (x < GameBoardWidth))
            {
                if (boardSquares[x,y].HasConnector(fromDirection) && 
                    !boardSquares[x,y].Suffix.Contains("W"))
                {
                    FillPiece(x,y);
                    WaterTracker.Add(new Vector2(x,y));
                    foreach (string end in boardSquares[x,y].GetOtherEnds(fromDirection))
                        switch (end)
                        {
                            case "Left":
                                PropagateWater(x - 1, y, "Right");
                                break;
                            case "Right":
                                PropagateWater(x + 1, y, "Left");
                                break;
                            case "Top":
                                PropagateWater(x, y - 1, "Bottom");
                                break;
                            case "Bottom":
                                PropagateWater(x, y + 1, "Top");
                                break;
                        }
                }
            }
        }

        //Check if a scoring water chain has been made
        public List<Vector2> GetWaterChain(int y)
        {
            WaterTracker.Clear();
            PropagateWater(0, y, "Left");
            return WaterTracker;
        }
    }
}
