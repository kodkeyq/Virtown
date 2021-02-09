using System;
using System.Collections.Generic;
using System.Text;
using VirtownShared.Global;

namespace VirtownShared.Network.Packets
{
    public abstract class Packet : IPacket
    {
        private byte[] _data = new byte[Constants.MaxPacketSize];
        private int _position = 0;
        public static PacketTypeEnum GetType(byte[] rawPacket)
        {
            return (PacketTypeEnum)rawPacket[0];
        }

        protected Packet(PacketTypeEnum type)
        {
            _data[0] = (byte)type;
            _position++;
        }

        public Packet(byte[] rawPacket)
        {
            Buffer.BlockCopy(rawPacket, 0, _data, 0, rawPacket.Length);
            _position++;
        }
        protected int ReadInt()
        {
            int value = _data[0 + _position] | (_data[1 + _position] << 8) | (_data[2 + _position] << 16) | (_data[3 + _position] << 24);
            _position += 4;
            return value;
        }
        protected void WriteInt(int value)
        {
            _data[_position] = (byte)value;
            _data[_position + 1] = (byte)(value >> 8);
            _data[_position + 2] = (byte)(value >> 16);
            _data[_position + 3] = (byte)(value >> 24);
            _position += 4;
        }

        protected byte[] Pack()
        {
            byte[] data = new byte[_position];
            Buffer.BlockCopy(_data, 0, data, 0, _position);
            return data;
        }
    }
}
