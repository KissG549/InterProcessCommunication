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

        abstract void Receive();
 
        abstract void Send(string pData);

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