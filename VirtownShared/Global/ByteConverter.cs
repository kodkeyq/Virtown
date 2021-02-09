using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace VirtownShared.Global
{
    public static class ByteConverter
    {
        public static int ReadInt(byte[] data, int index)
        {
            return data[0 + index] | (data[1 + index] >> 8) | (data[2 + index] >> 16) | (data[3 + index] >> 24);
        }

        public static void WriteInt()
        {

        }
    }
}
