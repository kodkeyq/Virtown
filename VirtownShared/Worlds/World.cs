using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using VirtownShared.Entities;
using VirtownShared.Global;

namespace VirtownShared.Worlds
{
    public class World
    {
        public Cell[,] Cells;
        public Point IsoSize { get; private set; }

        public Entity[] EntityList = new Entity[Constants.MaxEntitiesCount];
        
        public World(Point isoSize)
        {
            Resize(isoSize);
        }





        public void Resize(Point isoSize) { IsoSize = isoSize;  Cells = new Cell[IsoSize.X, IsoSize.Y]; }
    }
}
