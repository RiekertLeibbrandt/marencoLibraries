using System;
using Microsoft.SPOT;
using System.Net;
using System.Net.Sockets;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Marenco.Comms
{

    class SimulinkUDP
    {
        public static Microsoft.SPOT.Net.NetworkInformation.NetworkInterface NI = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0];

        public static Socket send = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public static IPAddress sendToAddress = IPAddress.Parse("192.168.60.231");  // 1.201  60.236
        public static IPEndPoint sendingEndPoint = new IPEndPoint(sendToAddress, 49001);

        public static Socket receive = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public static IPAddress receiveAddress = IPAddress.Parse("192.168.60.248");
        public static IPEndPoint remoteEndPoint = new IPEndPoint(receiveAddress, 49002);
    }
}
