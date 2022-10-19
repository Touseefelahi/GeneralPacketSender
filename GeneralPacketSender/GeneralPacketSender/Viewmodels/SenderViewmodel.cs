using GeneralPacketSender.Models;
using PacketSender.Core;
using System.Threading.Tasks;

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
                    Sendable = new UdpTransceiver();
                    break;
                case CommunicationType.Tcp:
                    Sendable = new TcpTransceiver();
                    break;
                case CommunicationType.Serial:
                    Sendable = new SerialTransceiver();
                    break;
            }
        }
    }
}
