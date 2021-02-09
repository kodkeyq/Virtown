using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace VirtownShared.Global
{
    public static class ByteConverter
    {
        public static int ReadInt(byte[] data, int index, out int value)
        {
            value = data[0 + index] | (data[1 + index] << 8) | (data[2 + index] << 16) | (data[3 + index] << 24);
            return 4;
        }

        public static int WriteInt(byte[] data, int index, int value)
        {
            data[index] = (byte)value;
            data[index + 1] = (byte)(value >> 8);
            data[index + 2] = (byte)(value >> 16);
            data[index + 3] = (byte)(value >> 24);
            return 4;
        } 
    }
}
