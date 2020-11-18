using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace InterProcessCommunication
{
    interface IServer
    {
        abstract void Listening(string pIPAddress, int pPortNumber);
        abstract void Send(string data);

        abstract void SendCallBack(IAsyncResult pAr);
    }
}
