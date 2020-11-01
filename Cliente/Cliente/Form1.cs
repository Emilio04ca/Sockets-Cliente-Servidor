using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Net.Sockets;
using System.IO;
namespace Cliente
{
   
    
    public partial class Form1 : Form
    { 
        //Este objeto es de la clase NetworkStream  el cual permite el flujo de infromacion 
        static private NetworkStream Fluinfo;
        //Este objeto es de la clase StreamWriter el cual nos permite escribir en ese flujo de informacion
        static private StreamWriter Escribir;
        //Este objeto es de la clase Streamreader el cual permite leer el flujo de informacion
        static private StreamReader Lectura;
        //Este objeto es de la clase TcpClient el caul hace posible conectar al flujo de informacion
        static private TcpClient cliente = new TcpClient();
        //una variavle que guarda el nombre del cliente pra los diferente proceso que realice apareza el nombre 
        static private string Nombrecliente = "unknown";

        private delegate void entreproceso(String s);
        private void AddItem(String s)
        {
            listBox1.Items.Add(s);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Enviar_Click(object sender, EventArgs e)
        { 
            Escribir.WriteLine(textBox1.Text);
            Escribir.Flush();
            textBox1.Clear();
        }

        void Listen()
        {
            while (cliente.Connected)
            {
                this.Invoke(new entreproceso(AddItem), Lectura.ReadLine());
            }
            {
                MessageBox.Show("No se ha podido conectar al servidor");
                Application.Exit();
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Nombrecliente = textBox2.Text;

            cliente.Connect("127.0.0.1", 8088);
            if (cliente.Connected)
            {
                Thread t = new Thread(Listen);

                Fluinfo = cliente.GetStream();
                Escribir = new StreamWriter(Fluinfo);
                Lectura = new StreamReader(Fluinfo);

                Escribir.WriteLine(Nombrecliente);
                Escribir.Flush();

                t.Start();
            }
            else
            {
                MessageBox.Show("Servidor no Disponible");
                Application.Exit();
            }

        }
    }
}
