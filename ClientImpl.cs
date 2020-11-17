using InterProcessCommunication.InterProcessCommunication;
using System;

namespace InterProcessCommunication
{
    internal class ClientImpl : IClient
    {
        private readonly int mConnectionId;
        private readonly bool mIsConnected;
        private readonly string mServerAddr;

        public ClientImpl()
        {
            mConnectionId = 0;
            mIsConnected  = false;
            mServerAddr = "";
        }

        ~ClientImpl()
        {
            if (mIsConnected) { Disconnect(); }
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

        public void AsyncWaitForMsg()
        {
            Console.WriteLine("asyncWaitForMsg from the other end");
        }

        public void AsyncWaitForInput()
        {
            Console.WriteLine("asyncWaitForInput from the user ");
        }

        public bool Connect(string pServerAddr)
        {
            Console.WriteLine("Connecting to server: ", pServerAddr);
            return true;
        }

        public bool Disconnect()
        {
            Console.WriteLine("Disconnecting");
            return true;
        }

    }
}