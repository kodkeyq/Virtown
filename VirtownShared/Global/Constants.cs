﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace VirtownShared.Global
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

        public const int SpriteIndexMax = 30;

        public const string HostName = "127.0.0.1";
        public const int Port = 51000;

        public const int BufferSize = 262144;
        public const int ReadSizeLimit = 1024;

        public static readonly byte[] MagicBytes = new byte[4] { 255, 255, 255, 255 };

        public const bool Debug = true;

        public const int MaxOutcomingPackets = 20;

        public const int MaxUsersCount = 65536;
        public const int MaxPacketSize = 65536;

        public const bool Client = true;
        public const bool Server = !Client;

        public const int MaxEntitiesCount = 1000;
        public const int MaxPersonsCount = 100;
    }


}
