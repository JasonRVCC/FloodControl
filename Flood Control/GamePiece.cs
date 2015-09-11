using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

//This class represents an individual pipe on the Flood Control game board.
//It is responsible for:
//-Identifying which sides of the piece have connectors
//-Wheither or not the piece is filled with water.
//-Automaticly handling rotation.
//-Returning connector info to other pieces.
//-Providing the sprite sheet location for the piece's current sprite. 

namespace Flood_Control
{
    class GamePiece
    {
        public static string[] PieceTypes =
        {
            "Left,Right","Top,Bottom","Left,Top","Top,Right",
            "Right,Bottom","Bottom,Left","Empty"
        };

        public const int PieceHeight = 40;
        public const int PieceWidth = 40;

        public const int MaxPlayablePieceIndex = 5;
        public const int EmptyPieceIndex = 6;

        private const int textureOffsetX = 1;
        private const int textureOffsetY = 1;
        private const int texturePaddingX = 1;
        private const int texturePaddingY = 1;

        private string pieceType = "";
        private string pieceSuffix = "";

        //Create a new piece with a type and suffix
        public GamePiece(string type, string suffix)
        {
            pieceType = type;
            pieceSuffix = suffix;
        }

        //Create a new piece with just a type
        public GamePiece(string type)
        {
            pieceType = type;
            pieceSuffix = "";
        }

        //Get Piece Type
        public string PieceType
        {
            get {return pieceType;}
        }

        //Get Suffix
        public string Suffix
        {
            get {return pieceSuffix;}
        }

        //Update the game piece
        //With type and suffix
        public void SetPiece(string type, string suffix)
        {
            pieceType = type;
            pieceSuffix = suffix;
        }

        //With type only
        public void SetPiece(string type)
        {
            SetPiece(type, "");
        }

        //Add a suffix
        public void AddSuffix(string suffix)
        {
            if(!pieceSuffix.Contains(suffix))
                pieceSuffix += suffix;
        }

        //Remove the suffix (suffix must be passed)
        public void RemoveSuffix(string suffix)
        {
            pieceSuffix = pieceSuffix.Replace(suffix, "");
        }

        //Rotate the game piece
        public void RotatePiece(bool clockwise)
        {
            switch (pieceType)
            {
                case "Left,Right":
                    pieceType = "Top,Bottom";
                    break;
                case "Top,Bottom":
                    pieceType = "Left,Right";
                    break;
                case "Left,Top":
                    if(clockwise)
                        pieceType = "Top,Right";
                    else
                        pieceType = "Bottom,Left";
                    break;
                case "Top,Right":
                    if(clockwise)
                        pieceType = "Right,Bottom";
                    else
                        pieceType = "Left,Top";
                    break;
                case "Right,Bottom":
                    if(clockwise)
                        pieceType = "Bottom,Left";
                    else
                        pieceType = "Top,Right";
                    break;
                case "Bottom,Left":
                    if(clockwise)
                        pieceType = "Left,Top";
                    else
                        pieceType = "Right,Bottom";
                    break;
                case "Empty":
                    break;
            }
        }

        //Get connector locations
        public string[] GetOtherEnds(string startingEnd)
        {
            List<string> opposites = new List<string>();

            foreach (string end in pieceType.Split(','))
            {
                if (end != startingEnd)
                    opposites.Add(end);
            }
            return opposites.ToArray();
        }

        //Check if a certain side of the piece has a connector 
        public bool HasConnector(string direction)
        {
            return pieceType.Contains(direction);
        }

        //Get the source rectangle for the piece's current sprite
        public Rectangle GetSourceRect()
        {
            int x = textureOffsetX;
            int y = textureOffsetY;

            if (pieceSuffix.Contains("W"))
                x += PieceWidth + texturePaddingX;

            y += (Array.IndexOf(PieceTypes, pieceType) * (PieceHeight + texturePaddingY));

            return new Rectangle(x, y, PieceWidth, PieceHeight);
        }
    }
}
