using System;
using System.Collections.Generic;
using System.Text;

namespace InterProcessCommunication.InterProcessCommunication
{
    internal interface IClient
    {
        abstract bool Connect(string pServerAddr);
        abstract bool Disconnect();
        abstract void AsyncWaitForMsg(); // Wait messages from the server
        abstract void AsyncWaitForInput();  // Wait messages from the user
        /*
            Properties
         */
        abstract int ConnectionId
        {
            get;
        }

        abstract bool IsConnected
        {
            get;
        }

        abstract string ServerAddr
        {
            get;
        }
    }
}