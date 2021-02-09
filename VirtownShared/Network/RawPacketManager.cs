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
        private static readonly int _minLength = Constants.MagicBytes.Length + 5;

        private Queue<byte[]> _rawIncomingPackets = new Queue<byte[]>();
        private Queue<byte[]> _rawOutcomingPackets = new Queue<byte[]>();

        private const int MaxOutcomingPackets = Constants.MaxOutcomingPackets;

        public bool CanReadPacket { get { return _rawIncomingPackets.Count > 0; } }
        public bool CanWritePacket { get { return _rawIncomingPackets.Count < MaxOutcomingPackets; } }
        public bool CanProcessRead { get { return ReadBufferSize > 0; } }
        public bool CanProcessWrite { get { return _rawOutcomingPackets.Count > 0; } }

        public byte[] ReadIncomingPacket()
        {
            return _rawIncomingPackets.Dequeue();
        }

        public void WriteOutcomingPacket(byte[] rawPacket)
        {
            _rawOutcomingPackets.Enqueue(rawPacket);
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
                        int packetLength = ByteConverter.ReadInt(_rawBuffer, packetHeaderIndex + _magicLength);
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
