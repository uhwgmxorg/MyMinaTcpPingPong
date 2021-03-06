﻿using Mina.Core.Future;
using Mina.Core.Service;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Filter.Codec.TextLine;
using Mina.Filter.Logging;
using Mina.Transport.Socket;
using System;
using System.Net;

namespace MyMinaTcpPingPongClient
{
    class MinaTCPClient : IDisposable
    {
        public TCPClientProtocolManager Manager { get; set; }
        public Boolean Connected { get; set; }

        public IPAddress ServerIpAddress { get; set; }
        public int Port { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverIPAddress"></param>
        /// <param name="port"></param>
        public MinaTCPClient(IPAddress serverIPAddress, Int32 port,bool autoConnect)
        {
            Connected = false;

            ServerIpAddress = serverIPAddress;
            Port = port;
            Manager = new TCPClientProtocolManager();
            if(autoConnect)
                OpenMinaSocket();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MinaTCPClient()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            CloseMinaSocket();
        }

        /// <summary>
        /// OpenMinaSocket
        /// </summary>
        public void OpenMinaSocket()
        {
            if (Connected)
                return;

            Manager.InitializeClient();

            if (Manager.Connector == null)
                throw new Exception("This should not happen!");

            Manager.Connector.ExceptionCaught += HandleException;
            Manager.Connector.SessionOpened += HandeleSessionOpened;
            Manager.Connector.SessionClosed += HandeleSessionClosed;
            Manager.Connector.SessionIdle += HandleIdle;
            Manager.Connector.MessageReceived += HandleReceived;

            Manager.ServerIpAddress = ServerIpAddress;
            Manager.Port = Port;
            Manager.ConnectToServer();
        }

        /// <summary>
        /// Send
        /// </summary>
        public void Send(Object data)
        {
            Manager.Send(data);
        }

        /// <summary>
        /// CloseMinaSocket
        /// </summary>
        public void CloseMinaSocket()
        {
            if (!Connected)
                return;

            Manager.Session.Close(true);
        }

        /******************************/
        /*          Events            */
        /******************************/
        #region Events

        /// <summary>
        /// HandleException
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleException(Object sender, IoSessionExceptionEventArgs e)
        {
            Console.WriteLine(String.Format("Exception {0}", e.Exception.Message));
        }

        /// <summary>
        /// HandeleSessionOpened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandeleSessionOpened(Object sender, IoSessionEventArgs e)
        {
            Connected = true;
            Console.WriteLine(String.Format("SessionOpened {0}", e.Session.RemoteEndPoint));
        }

        /// <summary>
        /// HandeleSessionClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandeleSessionClosed(Object sender, IoSessionEventArgs e)
        {
            Connected = false;
            Console.WriteLine(String.Format("SessionClosed {0}", e.Session.RemoteEndPoint));
        }

        /// <summary>
        /// HandleIdle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleIdle(Object sender, IoSessionIdleEventArgs e)
        {
            Console.WriteLine(String.Format("Idle {0}", e.Session.BothIdleCount));
        }

        /// <summary>
        /// HandleReceived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleReceived(Object sender, IoSessionMessageEventArgs e)
        {
            var bytes = (byte[])e.Message;
            Console.WriteLine(String.Format("Received data => {0} | {1} |", ByteArrayToHexString(bytes), ByteArrayToAsciiString(bytes)));
        }
        private string ByteArrayToHexString(byte[] buf)
        {
            System.Text.StringBuilder hex = new System.Text.StringBuilder(buf.Length * 2);
            foreach (byte b in buf)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }
        private string ByteArrayToAsciiString(byte[] buf)
        {
            char[] carray = new char[buf.Length];
            char c;

            for (int i = 0; i < buf.Length; i++)
            {
                if (33 <= buf[i] && buf[i] <= 127)
                    c = (char)buf[i];
                else
                    c = '.';
                carray[i] = c;
            }

            return new String(carray);
        }

        #endregion
    }
}
