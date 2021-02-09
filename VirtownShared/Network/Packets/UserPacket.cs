using System;
using System.Collections.Generic;
using System.Text;
using VirtownShared.Users;

namespace VirtownShared.Network.Packets
{
    public class UserPacket : Packet
    {
        public UserPacket() : base(PacketTypeEnum.User) { }
        public UserPacket(byte[] rawPacket) : base(rawPacket) { }

        public void Unpack(UserManager userManager)
        {
            if (userManager?.Assigned == false) userManager.SetUserId(ReadInt());
        }

        public byte[] Pack(int userId)
        {
            WriteInt(userId);
            return Pack();
        }
    }
}
