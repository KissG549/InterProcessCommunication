using InterProcessCommunication.InterProcessCommunication;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.Json;

namespace InterProcessCommunication
{
    internal class ClientImpl : IClient
    {
        private IPEndPoint mRemoteEP;
        private Socket mSocket;
        private string mMessage = string.Empty;

        public string Response() => mMessage;

        public ClientImpl()
        {
        }

        ~ClientImpl()
        {
            if (mSocket.Connected) { Disconnect(); }
        }

        public bool IsConnected
        {
            get { return mSocket.Connected;  }
        }

        public bool Connect(string pServerAddr, int pPort)
        {
            try 
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(pServerAddr);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                mRemoteEP = new IPEndPoint(ipAddress, pPort);

                // Create a TCP/IP socket.  
                mSocket = new Socket(
                    ipAddress.AddressFamily,
                    SocketType.Stream, 
                    ProtocolType.Tcp);

                // Connect to the remote endpoint.  

                mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                mSocket.Connect(mRemoteEP);

                Console.WriteLine(
                        "Connected to {0}",
                        mSocket.RemoteEndPoint.ToString());

                if(!mSocket.Connected)
                {
                    return false;
                }

            }
            catch (Exception e) 
            {
                Console.WriteLine("Can't connect to the server");
                return false;
            }

            return true;
        }

        public void Disconnect()
        {
            // Release the socket.  
            mSocket.Shutdown(SocketShutdown.Both);
            mSocket.Close();
            Console.WriteLine("Socket closed");
        }

        public void Receive()
        {
            SocketStateObject state = new SocketStateObject();
            
            state.mClientSocket = mSocket;
            
            while (mSocket.Connected)
            {
                Thread.Sleep(10);
                
                try
                {
                    if (mSocket.Available > 0)
                    {
                        state.mSb.Clear();

                        // Create the state object.  
                        ReadSocket(mSocket, state);
                    }
                }
                catch(System.Net.Sockets.SocketException e)
                {
                    Console.WriteLine("Socket Exception, quit");
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't receive data from the server: {0}");
                    return;
                }
            }
        }

        private void ReadSocket(Socket pSocket, SocketStateObject pStateObject)
        {
            try
            {
                int bytesRead = pSocket.Receive(pStateObject.mBuffer);
                
                if (bytesRead > 0)
                {
                    pStateObject.mSb.Append(
                           Encoding.ASCII.GetString(
                               pStateObject.mBuffer,
                               0,
                               bytesRead));
                    if ( pSocket.Available > 0)
                    {
                        ReadSocket(pSocket, pStateObject);
                    }
                }

                if (pStateObject.mSb.Length > 1)
                {
                    mMessage = pStateObject.mSb.ToString();

                    // Process with the incoming data here

                    Console.WriteLine(
                         "Arrived {0} bytes from socket.\n Data: {1}",
                         mMessage.Length,
                         mMessage);

                    // Process with the incoming data here
                    // Data exchange, decoding
                    DataEncoderImpl.Decapsulate(mMessage);
                }
            
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't receive data from the server.");

            }
        }
         public int Send(string pData)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(pData);

            try { 
                mSocket.BeginSend(
                    byteData,
                    0,
                    byteData.Length,
                    0,
                    new AsyncCallback(SendCallback),
                    mSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't send data to the server");
            }
            return byteData.Length;
        }

        public void SendCallback(IAsyncResult pAr)
        {
            try 
            {
                Socket client = (Socket)pAr.AsyncState;

                int bytesSent = client.EndSend(pAr);
                Console.WriteLine(
                    "Sent {0} bytes to server.", 
                    bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Can't send data to the server");
            }
        }

       
    }
}