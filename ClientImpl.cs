using InterProcessCommunication.InterProcessCommunication;
using System;

namespace InterProcessCommunication
{
    internal class ClientImpl : IClient
    {
        private int mConnectionId;
        private bool mIsConnected;
        private string mServerAddr;

        public ClientImpl()
        {
            mConnectionId = 0;
            mIsConnected  = false;
            mServerAddr = "";
        }

        ~ClientImpl()
        {
            if (mIsConnected) { disconnect(); }
        }

        public int ConnectionId
        {
            get { return mConnectionId;  }
        }

        public bool IsConnected
        {
            get { return mIsConnected;  }
        }

        public string ServerAddr
        {
            get { return mServerAddr; }
        }

        public void asyncWaitForMsg()
        {
            Console.WriteLine("asyncWaitForMsg from the other end");
        }

        public void asyncWaitForInput()
        {
            Console.WriteLine("asyncWaitForInput from the user ");
        }

        public bool connect(string pServerAddr)
        {
            Console.WriteLine("Connecting to server: ", pServerAddr);
            return true;
        }

        public bool disconnect()
        {
            Console.WriteLine("Disconnecting");
            return true;
        }
    }
}