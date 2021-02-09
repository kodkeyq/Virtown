using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using VirtownShared.Global;

namespace VirtownShared
{
    public static class Constants
    {
        public const int Grid = 32;
        public const int GridH = Grid / 2;
        public const int GridQ = Grid / 4;
        public const int GridHQ = GridH + GridQ;

        public const int EntityMaxIsoSizeX = 10;
        public const int EntityMaxIsoSizeY = 10;
        public const int EntityMaxIsoSizeZ = 10;
        public const int EntityMaxAnimationIndex = 10;

        public const int AtlasMapsCount = 1000;

        public const int SpriteIndexMax = 100;

        public const string HostName = "127.0.0.1";
        public const int Port = 51000;

        public const int BufferSize = 262144;
        public const int ReadSizeLimit = 1024;

        public static readonly byte[] MagicBytes = new byte[4] { 255, 255, 255, 255 };

        public const bool Debug = true;

        public const int MaxOutcomingPackets = 20;
    }


}
