using GeneralPacketSender.Models;
using PacketSender.Core;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeneralPacketSender.Viewmodels
{
    [ObservableObject]
    public sealed partial class SenderViewmodel
    {
        [ObservableProperty]
        bool isRepeatSendOn;

        public CommunicationType CommunicationType
        {
            get => communicationType;
            set
            {
                communicationType = value;
                SetSender();
            }
        }

        [ObservableProperty]
        private bool showSettings;



        [ObservableProperty]
        [XmlIgnore]
        private ISendable sendable;

        [ObservableProperty]
        PacketInfo packetInfo;

        private CommunicationType communicationType;

        public SenderViewmodel()
        {
            PacketInfo = new PacketInfo
            {
                Command = new Memory<byte>(new byte[] { 45, 78, 65 })
            };
        }

        [RelayCommand]
        private async Task Send()
        {
            await Sendable.SendAsync(PacketInfo.Command);
        }

        private void SetSender()
        {
            switch (CommunicationType)
            {
                case CommunicationType.Udp:
                    Sendable = new UdpTransceiver("192.168.250.105", 4454);
                    break;
                case CommunicationType.Tcp:
                    Sendable = new TcpTransceiver("172.20.10.5", 3030);
                    break;
                case CommunicationType.Serial:
                    Sendable = new SerialTransceiver();
                    break;
            }
        }
    }
}
