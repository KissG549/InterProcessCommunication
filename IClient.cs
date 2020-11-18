using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace InterProcessCommunication.InterProcessCommunication
{
    internal interface IClient
    {
        abstract bool Connect(string pServerAddr, int pPort);
        abstract void Disconnect();

        abstract void ConnectCallback(IAsyncResult pAr);

        abstract void Receive(Socket pClient);

        abstract void ReceiveCallback(IAsyncResult pAr);

        abstract void Send(Socket pClient, string pData);

        abstract void SendCallback(IAsyncResult pAr);
        /*
            Properties
         */
        abstract bool IsConnected
        {
            get;
        }

    }
}