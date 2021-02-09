using System;
using System.Collections.Generic;
using System.Text;
using VirtownShared.Global;

namespace VirtownShared.Network
{
    public class RawPacketManager
    {
        private byte[] _rawBuffer = new byte[Constants.BufferSize];
        private int _rawLength = 0;

        private static readonly byte[] _magicBytes = Constants.MagicBytes;
        private static readonly int _magicLength = Constants.MagicBytes.Length;
        private static readonly int _minLength = Constants.MagicBytes.Length + sizeof(int) + 1;

        private Queue<byte[]> _rawIncomingPackets = new Queue<byte[]>();
        private Queue<byte[]> _rawOutcomingPackets = new Queue<byte[]>();

        private const int MaxOutcomingPackets = Constants.MaxOutcomingPackets;

        public bool CanReadPacket { get { return _rawIncomingPackets.Count > 0; } }
        public bool CanWritePacket { get { return _rawIncomingPackets.Count < MaxOutcomingPackets; } }
        public bool CanProcessRead { get { return ReadBufferSize > 0; } }
        public bool CanProcessWrite { get { return _rawOutcomingPackets.Count > 0; } }

        public byte[] ReadIncomingPacket()
        {
            byte[] rawPacket = _rawIncomingPackets.Dequeue();
            byte[] unsignedRawPacket = new byte[rawPacket.Length - _magicLength - sizeof(int)];
            Buffer.BlockCopy(rawPacket, _magicLength + sizeof(int), unsignedRawPacket, 0, unsignedRawPacket.Length);
            return unsignedRawPacket;
        }

        public void WriteOutcomingPacket(byte[] rawPacket)
        {
            byte[] signedRawPacket = new byte[rawPacket.Length + _magicLength + sizeof(int)];
            Buffer.BlockCopy(_magicBytes, 0, signedRawPacket, 0, _magicBytes.Length);
            WriteLength(signedRawPacket, _magicLength, rawPacket.Length);
            _rawOutcomingPackets.Enqueue(signedRawPacket);
        }
        public int ReadBufferSize
        {
            get
            {
                int availableBufferSize = _rawBuffer.Length - _rawLength;
                return (availableBufferSize >= Constants.BufferSize) ? Constants.BufferSize : availableBufferSize;
            }
        }

        public void ProcessReadData(byte[] data, int dataLength)
        {
            Buffer.BlockCopy(data, 0, _rawBuffer, _rawLength, dataLength);
            _rawLength += dataLength;

            ProcessNewPackets();
        }

        public byte[] ProcessWriteData()
        {
            return _rawOutcomingPackets.Dequeue();
        }

        private void ProcessNewPackets()
        {
            int scanStart = 0;

            bool nextScan;
            do
            {
                nextScan = false;

                if (scanStart < (_rawLength - _minLength + 1))
                {
                    int packetHeaderIndex = FindPacketHeaderIndex(scanStart, _rawLength);
                    if (packetHeaderIndex != -1 && (packetHeaderIndex < (_rawLength - _minLength + 1)))
                    {
                        int packetLength = ReadLength(_rawBuffer, packetHeaderIndex + _magicLength);
                        if (packetHeaderIndex < (_rawLength - packetLength + 1))
                        {
                            ExtractPacket(packetHeaderIndex, packetLength);
                            scanStart = packetHeaderIndex + packetLength;
                            nextScan = true;
                        }
                    }
                }
            } while (nextScan);
            if (scanStart > 0)
            {
                int copyLength = _rawLength - scanStart;
                Buffer.BlockCopy(_rawBuffer, scanStart, _rawBuffer, 0, copyLength);
                _rawLength = copyLength;
            }
        }

        private void WriteLength(byte[] rawPacket, int index, int length)
        {
            rawPacket[index] = (byte)length;
            rawPacket[index + 1] = (byte)(length >> 8);
            rawPacket[index + 2] = (byte)(length >> 16);
            rawPacket[index + 3] = (byte)(length >> 24);
        }

        private int ReadLength(byte[] data, int index)
        {
            return data[0 + index] | (data[1 + index] << 8) | (data[2 + index] << 16) | (data[3 + index] << 24);
        }
        private void ExtractPacket(int packetHeaderIndex, int packetLength)
        {
            byte[] rawPacket = new byte[packetLength];
            Buffer.BlockCopy(_rawBuffer, packetHeaderIndex, rawPacket, 0, packetLength);
            _rawIncomingPackets.Enqueue(rawPacket);
        }

        private int FindPacketHeaderIndex(int scanStart, int scanEnd)
        {
            int loopEnd = scanEnd - _magicLength + 1;
            for (int i = scanStart; i < loopEnd; i++)
            {
                if (_rawBuffer[i] == _magicBytes[i] && CheckAllMagicBytes(i))
                {
                    return i;
                }
            }
            return -1;
        }

        private bool CheckAllMagicBytes(int index)
        {
            for (int i = 0; i <_magicLength; i++)
            {
                if (_rawBuffer[index + i] != _magicBytes[i]) return false;
            }
            return true;
        }

    }
}
