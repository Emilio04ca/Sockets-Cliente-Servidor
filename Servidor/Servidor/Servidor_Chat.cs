using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace Servidor
{
    class Servidor_Chat
    {
        //TcpListener--------> Espera la conexion del Cliente.
        private TcpListener Servidor;
        //TcpClient Proporciona la Conexion entre el Servidor y el Cliente.
        private TcpClient cliente = new TcpClient();
        //permite la conexion 
        private IPEndPoint Ipconexion = new IPEndPoint(IPAddress.Any, 8088);
        //uniliza clases de flujo, para la escritura y lectura.
        private List<Connection> flujos = new List<Connection>();

        Connection conexion;


        private struct Connection
        {
            //NetworkStream Se encarga de enviar mensajes atravez de los sockets.
            public NetworkStream Flujoinfo;
            //escritura
            public StreamWriter FluEscribir;
            //lectura
            public StreamReader FluLectura;

            public string NombreCliente;
        }

        public Servidor_Chat()
        {
            Console.WriteLine("Esperando conexion de los clientes");
            Servidor = new TcpListener(Ipconexion);
            Servidor.Start();

            while (true)
            {
                cliente = Servidor.AcceptTcpClient();

                conexion = new Connection();
                conexion.Flujoinfo = cliente.GetStream();
                conexion.FluLectura = new StreamReader(conexion.Flujoinfo);
                conexion.FluEscribir = new StreamWriter(conexion.Flujoinfo);

                conexion.NombreCliente = conexion.FluLectura.ReadLine();

                flujos.Add(conexion);
                Console.WriteLine(conexion.NombreCliente + " se a conectado.");



                Thread t = new Thread(Escuchar_conexion);

                t.Start();
            }
        }


        void Escuchar_conexion()
        {
            Connection ReuComponentes = conexion;

            do
            {
                try
                {
                    string FluCliente = ReuComponentes.FluLectura.ReadLine();
                    Console.WriteLine(ReuComponentes.NombreCliente + ": " + FluCliente);
                    foreach (Connection c in flujos)
                    {
                        try
                        {
                            c.FluEscribir.WriteLine(ReuComponentes.NombreCliente + ": " + FluCliente);
                            c.FluEscribir.Flush();
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                    flujos.Remove(ReuComponentes);
                    Console.WriteLine(conexion.NombreCliente + " se a desconectado.");
                    break;
                }
            } while (true);
        }

    }
}