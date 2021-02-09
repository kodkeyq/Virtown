using System;
using System.Collections.Generic;
using System.Text;
using VirtownShared.Users;
using VirtownShared.Worlds;
using VirtownShared.Network.Packets;

namespace VirtownShared.Network
{
    public class PacketManager
    {
        private ClientManager _clientManager;
        private ServerManager _serverManager;
        private UserManager _userManager;
        private UserServerManager _userServerManager;
        private World _world;
        public PacketManager(ClientManager clientManager, UserManager userManager, World world )
        {
            if (clientManager == null || userManager == null || world == null) Logger.Error("PacketManager: detected null reference");
            _clientManager = clientManager;
            _userManager = userManager;
        }

        public PacketManager(ServerManager serverManager, UserServerManager userServerManager, World world)
        {
            if (serverManager == null || userServerManager == null || world == null) Logger.Error("PacketManager: detected null reference");
            _serverManager = serverManager;
            _userServerManager = userServerManager;
        }

        private bool CanUpdate(double lastTimeTick)
        {
            return true;
        }
        public void Tick(double lastTimeTick)
        {
            if (CanUpdate(lastTimeTick))
            {
                if (Constants.Client)
                {
                    TickClient();
                }
                else if (Constants.Server)
                {
                    TickServer();
                }
            }
        }

        private void TickServer()
        {
            for (int i = 0; i < _serverManager.Clients.Length; i++)
            {
                while (_serverManager.Clients[i]?.RawPacketManager.CanReadPacket == true)
                {
                    byte[] rawPacket = _serverManager.Clients[i]?.RawPacketManager.ReadIncomingPacket();

                    if (rawPacket != null)
                    {
                        PacketTypeEnum type = Packet.GetType(rawPacket);

                        Packet packet;

                        switch (type)
                        {
                            case PacketTypeEnum.Hello:
                                packet = new HelloPacket(rawPacket);
                                UserPacket packet2 = new UserPacket();
                                int userId = (packet as HelloPacket).Unpack(_userServerManager, i);
                                _serverManager.Clients[i]?.RawPacketManager.WriteOutcomingPacket(packet2.Pack(userId));
                                break;
                        }
                    }
                }
            }
        }

        private void TickClient()
        {
            while (_clientManager.RawPacketManager.CanReadPacket)
            {
                byte[] rawPacket = _clientManager.RawPacketManager.ReadIncomingPacket();

                PacketTypeEnum type = Packet.GetType(rawPacket);

                Packet packet;

                switch (type)
                {
                    case PacketTypeEnum.User:
                        packet = new UserPacket(rawPacket);
                        (packet as UserPacket).Unpack(_userManager);
                        break;
                }
            }
        }
    }
}
