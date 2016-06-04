using Mina.Core.Service;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Filter.Codec.Demux;
using Mina.Filter.Logging;
using Mina.Transport.Socket;
using System;
using System.Net;

namespace MyMinaTcpPingPongServer
{
    class MinaTCPServer
    {
        public TCPServerProtocolManager Manager { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="port"></param>
        public MinaTCPServer(Int32 port)
        {
            Manager = new TCPServerProtocolManager();
            StartMinaListener(port);
        }

        /// <summary>
        /// StartMinaListener
        /// </summary>
        private void StartMinaListener(Int32 port)
        {

            Manager.InitializeServer();

            if (Manager.Acceptor == null)
                throw new Exception("This should not happen!");

            Manager.Acceptor.ExceptionCaught += HandleException;
            Manager.Acceptor.SessionOpened += HandeleSessionOpened;
            Manager.Acceptor.SessionClosed += HandeleSessionClosed;
            Manager.Acceptor.SessionIdle += HandleIdle;
            Manager.Acceptor.MessageReceived += HandleReceived;

            Manager.Port = port;
            Manager.StartServer();
        }

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="data"></param>
        public void Send(Object data)
        {
            Manager.Send(data);
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
        void HandleException(Object sender, IoSessionExceptionEventArgs e)
        {
            Console.WriteLine(String.Format("Exception {0}",e.Exception.Message));
        }

        /// <summary>
        /// HandeleSessionOpened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandeleSessionOpened(Object sender, IoSessionEventArgs e)
        {
            Console.WriteLine(String.Format("SessionOpened {0}",e.Session.RemoteEndPoint));
        }

        /// <summary>
        /// HandeleSessionClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandeleSessionClosed(Object sender, IoSessionEventArgs e)
        {
            Console.WriteLine(String.Format("SessionClosed {0}", e.Session.RemoteEndPoint));
        }

        /// <summary>
        /// HandleIdle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandleIdle(Object sender, IoSessionIdleEventArgs e)
        {
            Console.WriteLine(String.Format("Idle {0}",e.Session.BothIdleCount));
        }

        /// <summary>
        /// HandleReceived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandleReceived(Object sender, IoSessionMessageEventArgs e)
        {
            var bytes = (byte[])e.Message;
            Console.WriteLine(String.Format("Received data => {0} | {1} |", ByteArrayToHexString(bytes), ByteArrayToAsciiString(bytes)));
        }
        public static string ByteArrayToHexString(byte[] buf)
        {
            System.Text.StringBuilder hex = new System.Text.StringBuilder(buf.Length * 2);
            foreach (byte b in buf)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }
        string ByteArrayToAsciiString(byte[] buf)
        {
            char[] carray = new char[buf.Length];
            char c;

            for(int i=0;i<buf.Length;i++)
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
