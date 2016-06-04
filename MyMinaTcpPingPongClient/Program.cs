/******************************************************************************/
/*                                                                            */
/*   Program: MyMinaTcpPingPongClient                                         */
/*   Simple TCP Client using MINA for receiving and sending binary data       */
/*                                                                            */
/*   25.05.2016 0.0.0.0 uhwgmxorg Start                                       */
/*                                                                            */
/******************************************************************************/
using System;
using System.Net;

namespace MyMinaTcpPingPongClient
{
    class Program
    {
        static String IP_ADDRESS = "127.0.0.1";
        static int PORT = 4711;

        static MinaTCPClient _minaTCPClient;

        static void Main(string[] args)
        {
            ConsoleKeyInfo KeyInfo;
            char Key;
            byte[] data = {0,9,8,7,6,5,4,3,2,1};

            try
            {
                if (args.Length == 2)
                {
                    IP_ADDRESS = args[0];
                    PORT = Convert.ToInt32(args[1]);
                }
                if (args.Length == 1)
                {
                    IP_ADDRESS = args[0];
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Input Error");
            }

            Console.WriteLine("Program MyMinaTcpPingPongClient [IP] [Port]");
            Console.WriteLine("press:");
            Console.WriteLine(" c for connect");
            Console.WriteLine(" s for send");
            Console.WriteLine(" d for disconnect");
            Console.WriteLine(" x for exit");

            Console.WriteLine(String.Format("TCP Client press c to connect to {0} {1}",IP_ADDRESS,PORT));
            _minaTCPClient = new MinaTCPClient(IPAddress.Parse(IP_ADDRESS), PORT, false);
            do
            {
                KeyInfo = Console.ReadKey(true);
                Key = KeyInfo.KeyChar;
                if (Key == 'c')
                    _minaTCPClient.OpenMinaSocket();
                if (Key == 's')
                    _minaTCPClient.Send(data);
                if (Key == 'd')
                    _minaTCPClient.CloseMinaSocket();
            }
            while (Key != 'x');
        }
    }
}
