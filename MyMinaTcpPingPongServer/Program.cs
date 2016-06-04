/******************************************************************************/
/*                                                                            */
/*   Program: MyMinaTcpPingPongServer                                         */
/*   Simple TCP Server using MINA for receiving and sending binary data       */
/*                                                                            */
/*   25.05.2016 0.0.0.0 uhwgmxorg Start                                       */
/*                                                                            */
/******************************************************************************/
using System;

namespace MyMinaTcpPingPongServer
{
    class Program
    {
        static int PORT = 4711;

        static MinaTCPServer _minaTCPServer;

        static void Main(string[] args)
        {
            ConsoleKeyInfo KeyInfo;
            char Key;
            byte[] data = {1,2,3,4,5,6,7,8,9,0};

            try
            {
                if (args.Length == 1)
                {
                    PORT = Convert.ToInt32(args[0]);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Input Error");
            }

            Console.WriteLine("Program MyMinaTcpPingPongServer [Port]");
            Console.WriteLine("press:");
            Console.WriteLine(" s for send");
            Console.WriteLine(" x for exit");

            Console.WriteLine(String.Format("TCP Server listen on port {0}", PORT));
            _minaTCPServer = new MinaTCPServer(PORT);
            do
            {
                KeyInfo = Console.ReadKey(true);
                Key = KeyInfo.KeyChar;
                if (Key == 's')
                    _minaTCPServer.Send(data);
            }
            while (Key != 'x');
        }
    }
}
