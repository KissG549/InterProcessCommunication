using InterProcessCommunication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

/*
    Based on: 
        https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
 */

namespace InterProcessCommunication
{
    class SocketStateObject
    {
        /* buffer size */
        public const int mBufferSize = 1024;

        /* buffer */
        public byte[] mBuffer = new byte[mBufferSize];

        /* data string */
        public StringBuilder mSb = new StringBuilder();

        /* client socket */
        public Socket mClientSocket = null;
    }

    class ServerImpl
    {
        private Socket mConnectedClient;

        ~ServerImpl()
        {
             if(mConnectedClient.Connected )
             {
                  mConnectedClient.Shutdown(SocketShutdown.Both);
                  mConnectedClient.Close();
             }
        }
        public void Listening(string pIPAddress, int pPortNumber)
        {
            /*
                Set default binding address
             */
            if( pIPAddress.Length < 1 )
            {
                pIPAddress = "127.0.0.1";
            }
            /*
                Attach the program to a local socket
             */
            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());

            IPAddress ipAddress = IPAddress.Loopback;

            if (Uri.CheckHostName(pIPAddress) == UriHostNameType.Dns)
            {
                var hostAddressArr = Dns.GetHostEntry(pIPAddress);

                if (hostAddressArr.AddressList.Length > 0)
                {
                    ipAddress = hostAddressArr.AddressList[0];
                }
                else 
                {
                    Console.WriteLine("Can't resolve hostname to IP, {0}", pIPAddress);
                }
            }
            else
            {
                ipAddress = IPAddress.Parse(pIPAddress);
            }

            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, pPortNumber);

            Console.WriteLine("Creating TCP socket on {0}:{1}", ipAddress, pPortNumber);

            Socket listener = 
                new Socket(
                    ipAddress.AddressFamily, 
                    SocketType.Stream, 
                    ProtocolType.Tcp);

            /* Bind the socket */
            try {

                listener.Bind(localEndPoint);
                listener.Listen(100);
                
                Console.WriteLine("Listening on {0} : {1}", ipAddress.ToString(), pPortNumber);
                Console.WriteLine("Waiting for new connection...");

                if(mConnectedClient != null && mConnectedClient.Connected)
                {
                    mConnectedClient.Shutdown(SocketShutdown.Both);
                    mConnectedClient.Close();
                    mConnectedClient.Dispose();
                }
                mConnectedClient = listener.Accept();

                Console.WriteLine(
                        "Accepted connection from {0}",
                        ((IPEndPoint)mConnectedClient.RemoteEndPoint).Address);
                
            } 
            catch(Exception e)
            {
                Console.WriteLine(
                    "Can't bind the socket: {0}",
                    e.ToString());
            }
        }

        public void Receive()
        {
            SocketStateObject state = new SocketStateObject();
            state.mClientSocket = mConnectedClient;

            while (true)
            {
                if ( mConnectedClient == null || mConnectedClient.Available <= 0)
                {
                    System.Threading.Thread.Sleep(10);
                    continue;
                }
                try
                {
                    state.mSb.Clear();
                    ReadSocket(mConnectedClient, state);
                }
                 catch (Exception e)
                {
                    Console.WriteLine(
                        "Can't receive data from the server: {0}",
                        e.ToString());
                    return;
                }
            }
        }
        private void ReadSocket(Socket pSocket, SocketStateObject pStateObject)
        {
            string message = string.Empty;

            if (pSocket == null)
            {
                return;
            }
            // Read data from the socket
            int bytesRead = 0;
            try
            {
                bytesRead = pSocket.Receive(pStateObject.mBuffer);
            }
            catch(Exception e)
            {
                Console.WriteLine("Can't read data from remote end ");
                pSocket.Close();
                bytesRead = 0;
                return;
            }

            if( bytesRead > 0 )
            {
                pStateObject.mSb.Append(
                    Encoding.ASCII.GetString(
                        pStateObject.mBuffer, 
                        0, 
                        bytesRead));

                if (pSocket.Available > 0)
                {
                    ReadSocket(pSocket, pStateObject);
                }

                message = pStateObject.mSb.ToString();


                Console.WriteLine(
                    "Arrived {0} bytes from socket.\n Data: {1}", 
                    message.Length, 
                    message);

                // Process with the incoming data here
                // Data exchange, decoding
                DataEncoderImpl.Decapsulate(message);
            }
        }

        public int Send( string pData)
        {
            
            byte[] byteData = Encoding.ASCII.GetBytes(pData);
            if (mConnectedClient.Connected)
            {
                mConnectedClient.BeginSend(
                    byteData,
                    0,
                    byteData.Length,
                    0,
                    new AsyncCallback(SendCallBack),
                    mConnectedClient);
            }
            return byteData.Length;
        }

        public void SendCallBack(IAsyncResult pAr)
        {
            try
            {
                Socket handler = (Socket)pAr.AsyncState;

                int byteSent = handler.EndSend(pAr);
                Console.WriteLine("Sent {0} bytes to client.", byteSent);

            }
            catch( Exception e)
            {
                Console.WriteLine("Can't send data to the client.");
            }
        }

    }
}
