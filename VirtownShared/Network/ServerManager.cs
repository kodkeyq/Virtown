using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using VirtownShared.Global;

namespace VirtownShared.Network
{
    public class ServerManager
    {
        public ClientServerManager[] Clients;
        private TcpListener _server;

        private readonly double _updateInterval = 20.0;
        private double _remaindTime = 0.0;
        private bool _started = false;
        private bool _listening = false;
        
        private bool CanUpdate(double lastTimeTick)
        {
            double sumTime = _remaindTime + lastTimeTick;
            int updatesCount = (int)(sumTime / _updateInterval);
            _remaindTime = sumTime - updatesCount * _updateInterval;
            return updatesCount > 0;
        }

        public ServerManager(int clientsCount)
        {
            try
            {
                Clients = new ClientServerManager[clientsCount];
                _server = new TcpListener(IPAddress.Parse(Constants.HostName), Constants.Port);
                _started = true;
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
        }
        public void Tick(double lastTimeTick)
        {
            if (_started)
            {
                if (CanUpdate(lastTimeTick)) Listen();

                for (int i = 0; i < Clients.Length; i++)
                {
                    Clients[i]?.Tick(lastTimeTick);
                }
            }
        }

        private void Listen()
        {
            if (!_listening)
            {
                try
                {
                    _server.BeginAcceptTcpClient(ListenCallback, null);
                }
                catch (Exception exception)
                {
                    Logger.Error(exception.Message);
                }
            }
        }

        private int NullClientIndex()
        {
            for (int i = 0; i < Clients.Length; i++)
            {
                if (Clients[i] == null) return i;
            }
            return -1;
        }

        private void ListenCallback(IAsyncResult result)
        {
            try
            {
                TcpClient client = _server.EndAcceptTcpClient(result);
                int nullClientIndex = NullClientIndex();
                if (nullClientIndex != -1)
                {
                    Clients[nullClientIndex] = new ClientServerManager(client);

                    Logger.Info("New clients connected with client index = " + nullClientIndex.ToString());
                }
                else
                {
                    Logger.Warn("No empty slots for new clients!");
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
            _listening = false;
        }
    }
}
