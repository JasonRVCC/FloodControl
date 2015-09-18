using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

//A falling game piece in a game of Flood Control

namespace Flood_Control
{
    class FallingPiece: GamePiece
    {
        public int verticalOffset;
        public static int fallRate = 5;

        public FallingPiece(string pieceType, int verticalOffset)
            : base(pieceType)
        {
            this.verticalOffset = verticalOffset;
        }

        public void UpdatePiece()
        {
            verticalOffset = (int)MathHelper.Max(0, verticalOffset - fallRate);
        }
    }
}
