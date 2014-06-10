using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.IO.Ports;

namespace Marenco.Comms
{
    public class BlueSerial
    {
        static SerialPort serialPort;
        public BlueSerial(int baudRate = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            serialPort = new SerialPort(SerialPorts.COM1, baudRate, parity, dataBits, stopBits);
            serialPort.ReadTimeout = 1; // Set to 10ms. Default is -1?!
            serialPort.Open();
        }

        public void Print(byte[] junk)
        {
            serialPort.Write(junk, 0, junk.Length);
        }

        public void Print(UInt32 uintIn)
        {
            byte[] junk = new byte[4] { 
                    (byte)(uintIn & 0xFF), 
                    (byte)((uintIn >> 8) & 0xFF), 
                    (byte)((uintIn >> 16) & 0xFF), 
                    (byte)((uintIn >> 24) & 0xFF) };
            serialPort.Write(junk, 0, 4);
        }

        public void Print(UInt16 uintIn)
        {
            byte[] junk = new byte[2] { 
                    (byte)(uintIn & 0xFF), 
                    (byte)((uintIn >> 8) & 0xFF)};
            serialPort.Write(junk, 0, 2);
        }

        public void Print(long value)
        {
            byte[] junk = new byte[8] { 
                    (byte)(value & 0xFF), 
                    (byte)((value >> 8) & 0xFF), 
                    (byte)((value >> 16) & 0xFF), 
                    (byte)((value >> 24) & 0xFF),
                    (byte)((value >> 32) & 0xFF),
                    (byte)((value >> 40) & 0xFF),
                    (byte)((value >> 48) & 0xFF),
                    (byte)((value >> 56) & 0xFF)};
            serialPort.Write(junk, 0, 8);
        }
        public void Print(string theString)
        {
            byte[] junk = System.Text.Encoding.UTF8.GetBytes(theString);
            serialPort.Write(junk, 0, junk.Length);
        }
    }
}

