using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using VirtownShared.Global;

namespace VirtownShared.Entities
{
    public struct EntityData
    {
        public readonly string Name;
        public readonly Point IsoSize;
        public readonly int IsoSizeZ;
        public readonly Rectangle[,,,] Map;
        public readonly EntityTypeEnum Type;

        public EntityData(string name, Point isoSize, int isoSizeZ, Rectangle[,,,] map, EntityTypeEnum type)
        {
            Name = name;
            IsoSize = isoSize;
            IsoSizeZ = isoSizeZ;
            Map = map;
            Type = type;
        }
    }
}
