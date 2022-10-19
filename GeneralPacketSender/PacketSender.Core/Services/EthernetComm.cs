using System.Threading;

namespace PacketSender.Core
{
    public abstract class EthernetComm
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; } = 1000;
        public Exception LastException { get; protected set; }
        public EthernetComm(string iP, int port)
        {
            IP = iP ?? throw new ArgumentNullException(nameof(iP));
            Port = port;
        }

        public EthernetComm()
        {

        }
    }
}
