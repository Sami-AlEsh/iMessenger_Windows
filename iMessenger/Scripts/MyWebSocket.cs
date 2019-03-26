
/***************************************************************************************/
/*                                   WebSocket Socket                                  */
/***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using WebSocketSharp;
using System.IO;
using Newtonsoft.Json.Linq;

namespace iMessenger.Scripts
{
    static class MyWebSocket
    {
        public static bool connected { get; set; }
        public static string Uri { set; get; }
        public static WebSocket webSocket { set; get; }

        public static void Connect()
        {
            webSocket = new WebSocket("ws://192.168.43.56:3000");
            
            webSocket.OnMessage += (sender, e) => {
                Console.WriteLine("Server -> " + e.Data);
            };

            webSocket.Connect();
            connected = true;
            webSocket.OnOpen += (sender, e) =>
            {
                Console.WriteLine("WebSocket Connected Successfully !");
            };
        }
        public static void SendJson(string JsonData)
        {
            webSocket.Send(JsonData);
            Console.WriteLine("Json Message Sent !");
        }
        public static void SendBinary(byte[] file)
        {
            webSocket.Send(file);
            Console.WriteLine("Binary Message Sent !");
        }
    }
}