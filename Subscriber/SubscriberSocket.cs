using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Common;


namespace Subscriber
{
    class SubscriberSocket
    {
        private Socket _socket;
        private string _topic;

        public SubscriberSocket(string topic)
        {
            _topic = topic;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallBack, null);
            Console.WriteLine("connection aiting");
        }
        private void ConnectedCallBack(IAsyncResult asyncResult)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Sub connected ");
                Subscribe();
                StartRecieve();
            }
            else
            {
                Console.WriteLine("Error cant connect ");
            }

           

        }

        private void Subscribe()
        {
            var data = Encoding.UTF8.GetBytes("subscribe#" + _topic);
            Send(data);
        }

        private void StartRecieve()
        {
            ConnectionInfo connection = new ConnectionInfo();
            connection.Socket = _socket;
            _socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, RecieveCallBack,
                connection);
        }

        private void RecieveCallBack(IAsyncResult asyncResult)
        {
            ConnectionInfo connectionInfo = asyncResult.AsyncState as ConnectionInfo;
            try
            {
                SocketError response;
                int buffSize = _socket.EndReceive(asyncResult, out response);
                if (response == SocketError.Success)
                {
                    byte[] payloadBytes = new byte[buffSize];
                    Array.Copy(connectionInfo.Data, payloadBytes, payloadBytes.Length);
                    PayloadHandler.Handle(payloadBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"CAnt recieve data from broker {e.Message}");

            }
            finally
            {
                try
                {
                    connectionInfo.Socket.BeginReceive(connectionInfo.Data, 0, connectionInfo.Data.Length,
                        SocketFlags.None, RecieveCallBack, connectionInfo);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    connectionInfo.Socket.Close();
                }
            }
        }

        private void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not send dataa {e.Message}");
                
            }
        }
    }
}
