using System;
using System.Collections.Generic;
using System.Text;
using VirtownShared.Users;

namespace VirtownShared.Network.Packets
{
    public class HelloPacket : Packet
    {
        public HelloPacket() : base(PacketTypeEnum.Hello) { }
        public HelloPacket(byte[] rawPacket) : base(rawPacket) { }

        public int Unpack(UserServerManager userServerManager, int clientIndex)
        {
            if (!userServerManager.Assigned(clientIndex))
            {
                userServerManager.SetUser(clientIndex);
            }
            return userServerManager.GetUserId(clientIndex);
        }
    }
}
