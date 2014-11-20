using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using marencoHosts;

namespace ConsoleApplication1
{
    class Program
    {
        public static Serial2Matlab a = new Serial2Matlab();
        public static double[] strains = new double[2] { 0, 0 };

        static void Main(string[] args)
        {
            while (true)
            {
                Thread.Sleep(50);
                a.readSerial(out strains);
                Console.WriteLine(strains[0].ToString() + "   " + strains[1].ToString());
            }

        }

    }
}
