using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;
using MathWorks.MATLAB.NET.Arrays;


namespace marencoHosts
{
    public class Serial2Matlab
    {
        private static double old1 =  1;
        private static double old2 = 2;
        private static byte[] bytesIn = new byte[4];
        static SerialPort serialPort;

//        public Serial2Matlab(string port, int baudRate = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        public Serial2Matlab()
        {
            serialPort = new SerialPort("COM43", 115200, Parity.None, 8, StopBits.One);
            serialPort.ReadTimeout = 2;
            serialPort.Open();
            serialPort.DiscardInBuffer();
        }
        
        public void readSerial(out double[] strainsOut)
        {
            strainsOut = new double[2] { old1, old2 };

            if (serialPort.BytesToRead != 0)
            {
                //
                //  Read the bytes
                //
                while (serialPort.BytesToRead > 3)
                {
                    serialPort.Read(bytesIn, 0, 4);
                }
                //
                //  Decode the bytes.
                //
                strainsOut[0] = (double)(bytesIn[0] + (Int32)(bytesIn[1] << 8));
                strainsOut[1] = (double)(bytesIn[2] + (Int32)(bytesIn[3] << 8));
                old1 = strainsOut[0];
                old2 = strainsOut[1];
            }
            else
            {
            }
        }
    }
}



