using System;
using System.Collections.Generic;
using System.Text;
using VirtownShared.Global;
namespace VirtownShared.Users
{
    public class UserManager
    {
        public int UserId { get; private set; } = -1;

        public UserManager(int userId)
        {
            UserId = userId;
        }

        public UserManager()
        {

        }

        public void SetUserId(int userId)
        {
            UserId = userId;
        }

        public bool Assigned { get { return UserId != -1; } }
    }
}
