using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace VirtownShared.Worlds
{
    public struct Cell
    {

    }
    public class World
    {
        public Cell[,] Cells;

        public void Resize(Point newIsoSize)
        {
            Cells = new Cell[newIsoSize.X, newIsoSize.Y];
        }
    }
}
