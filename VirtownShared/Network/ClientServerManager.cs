using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using VirtownShared.Global;

namespace VirtownShared.Network
{
    public class ClientServerManager : ClientManager
    {
        public ClientServerManager(TcpClient client)
        {
            try
            {
                _client = client;
                _networkStream = _client.GetStream();
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
        }
        public override void Tick(double lastTimeTick)
        {
            if (CanUpdate(lastTimeTick))
            {
                Update();
            }
        }
    }
}
