using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using VirtownShared.Global;

namespace VirtownShared.Network
{
    public class ClientManager
    {
        protected TcpClient _client;
        protected NetworkStream _networkStream;
        public RawPacketManager RawPacketManager = new RawPacketManager();
        protected bool _connected = false;
        protected bool _reading = false;
        protected bool _writing = false;
        private bool _connecting = false;

        protected byte[] _readBuffer;

        protected double _remaindTime = 0.0;
        private double _connectRetryInterval = 300.0;
        protected double _updateInterval = 20.0;

        protected bool CanUpdate(double lastTimeTick)
        {
            double sumTime = _remaindTime + lastTimeTick;
            int updatesCount = (int)(sumTime / _updateInterval);
            _remaindTime = sumTime - updatesCount * _updateInterval;
            return updatesCount > 0;
        }

        private bool CanConnect(double lastTimeTick)
        {
            double sumTime = _remaindTime + lastTimeTick;
            int updatesCount = (int)(sumTime / _connectRetryInterval);
            _remaindTime = sumTime - updatesCount * _connectRetryInterval;
            return updatesCount > 0;
        }

        public virtual void Tick(double lastTimeTick)
        {
            if (_connected && CanUpdate(lastTimeTick))
            {
                Update();
            }
            else if (!_connected && !_connecting && CanConnect(lastTimeTick))
            {
                Connect();
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_connected)
                {
                    _networkStream.Close();
                    _client.Close();
                    _connected = false;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
        }
        
        protected void Update()
        {
            if (!_reading)
            {
                Read();
            }
            if (!_writing)
            {
                Write();
            }
        }
        private void Connect()
        {
            try
            {
                _client = new TcpClient();
                _client.BeginConnect(Constants.HostName, Constants.Port, ConnectCallback, null);
                _connecting = true;
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
        }

        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                _client.EndConnect(result);
                _client.NoDelay = true;
                _networkStream = _client.GetStream();
                _connected = true;

            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
            _connecting = false;
        }

        protected void Read()
        {
            try
            {
                if (RawPacketManager.CanProcessRead && _networkStream.CanRead && _networkStream.DataAvailable)
                {
                    _readBuffer = new byte[RawPacketManager.ReadBufferSize];
                    _networkStream.BeginRead(_readBuffer, 0, _readBuffer.Length, ReadCallback, null);
                    _reading = true;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
        }

        protected void ReadCallback(IAsyncResult result)
        {
            try
            {
                int readLength = _networkStream.EndRead(result);

                RawPacketManager.ProcessReadData(_readBuffer, readLength);

                if (Constants.Debug)
                {
                    Logger.Info("Bytes read from network stream: " + readLength.ToString());
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
            _reading = false;
        }

        protected void Write()
        {
            try
            {
                if (RawPacketManager.CanProcessWrite)
                {
                    byte[] writeBuffer = RawPacketManager.ProcessWriteData();
                    _networkStream.BeginWrite(writeBuffer, 0, writeBuffer.Length, WriteCallback, null);
                    _writing = true;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
        }

        protected void WriteCallback(IAsyncResult result)
        {
            try
            {
                _networkStream.EndWrite(result);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
            _writing = false;
        }
    }
}
