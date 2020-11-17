using System;
using System.Collections.Generic;
using System.Text;

namespace InterProcessCommunication.InterProcessCommunication
{
    internal interface IClient
    {
        bool connect(string pServerAddr);
        bool disconnect();
        void asyncWaitForMsg(); // Wait messages from the server
        void asyncWaitForInput();  // Wait messages from the user
        /*
            Properties
         */
        int ConnectionId
        {
            get;
        }

        bool IsConnected
        {
            get;
        }

        string ServerAddr
        {
            get;
        }
    }
}