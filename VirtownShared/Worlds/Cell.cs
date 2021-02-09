using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using VirtownShared.Entities;

namespace VirtownShared.Worlds
{
    public struct Cell
    {
        public Point FloorIndex;
        public Point DirtIndex;
        public Entity EntityReference;
    }
}
