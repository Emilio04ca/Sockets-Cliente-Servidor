    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperWebSocket;
namespace SocketConsola
{
    class Program
    {
        private static WebSocketServer ServidorSocket;
        static void Main(string[] args)
        {
            ServidorSocket = new WebSocketServer();
            int puerto = 8088;
            ServidorSocket.Setup(puerto);
            ServidorSocket.NewSessionConnected += servidorSocket_NuevaSesionconectada;
            ServidorSocket.NewMessageReceived += servidorSocket_NuevoMensajeRecibido;
            ServidorSocket.NewDataReceived += servidorSocket_NuevoDatosRecibidos;
            ServidorSocket.SessionClosed += servidorSocket_Sessionclosed;
            ServidorSocket.Start();
            Console.WriteLine("inicio el servidor en el " + puerto + " presiona una tecla para finalizarlo");
            Console.ReadKey();
            ServidorSocket.Stop();
        }
         
        private static void servidorSocket_Sessionclosed(WebSocketSession session, CloseReason value)
        {
            Console.WriteLine("Sesion cerrda");
        }

        private static void servidorSocket_NuevoDatosRecibidos(WebSocketSession session, byte[] value)
        {
            Console.WriteLine("Nuevos datos");
        }

        private static void servidorSocket_NuevoMensajeRecibido(WebSocketSession session, string value)
        {
            Console.WriteLine("inicio el servidor en el " + value);
            if (value == "Hola serviodor")
            {
                session.Send("Hola cliente");
            }
        }

        private static void servidorSocket_NuevaSesionconectada(WebSocketSession session)
        {
            Console.WriteLine("Sesion conectada");
        }
    }
}
