using System.IO.Ports;
using System.Threading;

namespace Core
{
    public class PortReader
    {
        public string PortName = "COM8";
        public int Frequency = 115200;

        public delegate void DataReceivedEvent(string a);
        public event DataReceivedEvent DataReceived;

        private SerialPort _serialPort;

        private bool _isReading;

        private Thread _thread;

        public static string[] GetAccessiblePortNames()
        {
            return SerialPort.GetPortNames();
        }

        public PortReader(string portName)
        {
            PortName = portName;
            DataReceived += EmptyFunction;
        }

        public PortReader(string portName, int frequency)
        {
            PortName = portName;
            Frequency = frequency;
            DataReceived += EmptyFunction;
        }

        public string Read()
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    return _serialPort.ReadLine();
                }
                catch (System.Exception) { }
            }
            return null;
        }

        public void WriteChar(char v)
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Write("" + v);
                }
                catch (System.Exception) { }
            }
        }


        public void Begin()
        {
            _serialPort = new SerialPort(PortName, Frequency);
            _serialPort.Open();
            _serialPort.ReadTimeout = 50;
            _thread = new Thread(Reading);
            _isReading = true;
            _thread.Start();
        }

        public void End()
        {
            _isReading = false;
            _serialPort.Close();
            _serialPort = null;
        }

        private void EmptyFunction(string a) { }

        private void Reading()
        {
            while (_isReading)
            {
                string a = Read();
                if (a != null)
                {
                    DataReceived(a);
                }
            }
        }
    }
}