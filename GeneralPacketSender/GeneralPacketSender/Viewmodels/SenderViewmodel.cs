using GeneralPacketSender.Models;
using PacketSender.Core;

namespace GeneralPacketSender.Viewmodels
{
    [ObservableObject]
    public sealed partial class SenderViewmodel
    {
        [ObservableProperty]
        bool isRepeatSendOn;

        [ObservableProperty]
        CommunicationType communicationType;

        [ObservableProperty]
        private object sendable;

        [ObservableProperty]
        PacketInfo packetInfo;

        public SenderViewmodel()
        {

        }
    }
}
