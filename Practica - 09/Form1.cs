using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practica___09
{
    public partial class Form1 : Form
    {
        delegate void setTextDelegate(string val);
        public SerialPort ArduinoPort { get; }
        public Form1()
        {
            InitializeComponent();
            ArduinoPort = new SerialPort();
            ArduinoPort.PortName = "COM3";
            ArduinoPort.BaudRate = 9600;
            ArduinoPort.DataBits = 8;
            ArduinoPort.ReadTimeout = 500;
            ArduinoPort.WriteTimeout = 500;
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);

            this.btnConectar.Click += btnConectar_Click;
        }

        void DataRecievedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string dato = ArduinoPort.ReadLine();
            EscribirTxt(dato);
        }

        void EscribirTxt(string dato)
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new setTextDelegate(EscribirTxt), dato);
                }
                catch { }
            }
            else lbTemp.Text = dato;
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            btnDesconectar.Enabled = true;
            try
            {
                if (!ArduinoPort.IsOpen)
                    ArduinoPort.Open();
                if (int.TryParse(tbLimTemp.Text, out int temperatureLimit))
                {
                    string limitString = temperatureLimit.ToString();
                    ArduinoPort.Write(limitString);
                }
                else
                {
                    MessageBox.Show("Ingresa un valor númerico válido en el TextBox del límite de la temperatura.");
                }

                lbConexion.Text = "Conexión Ok";
                lbConexion.ForeColor = Color.Lime;
            }
            catch
            {
                MessageBox.Show("Configure el puerto de comunicación correcto o Desconéctate");
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            btnConectar.Enabled = true;
            btnConectar.Enabled = false;
            if (ArduinoPort.IsOpen)
                ArduinoPort.Close();
            lbConexion.Text = "Desconectado";
            lbConexion.ForeColor = Color.Red;
            lbTemp.Text = "00.0";
        }
    }
}
