using System;
using System.Collections.Generic;
using System.Text;
using VirtownShared.Global;

namespace VirtownShared.Users
{
    public class UserServerManager
    {
        private int _userCounter = 0;
        private int[] _clientId = new int[Constants.MaxUsersCount];
        private int[] _userId = new int[Constants.MaxUsersCount];

        public UserServerManager() { for (int i = 0; i < _clientId.Length; i++) { _clientId[i] = -1; _userId[i] = -1; } }

        public void SetUser(int clientId) 
        {
            _clientId[_userCounter] = clientId;
            _userId[clientId] = _userCounter;
            _userCounter++;
        }

        public int GetClientId(int userId) { return _clientId[userId]; }

        public int GetUserId(int clientId) { return _userId[clientId]; }

        public bool Assigned(int clientId) { return _userId[clientId] != -1; }
    }
}
