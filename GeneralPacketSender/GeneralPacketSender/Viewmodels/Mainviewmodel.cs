using PacketSender.Core;
using System.Threading.Tasks;
namespace GeneralPacketSender.Viewmodels
{
    public partial class Mainviewmodel : ObservableObject
    {
        [ObservableProperty]
        private string title = "General Packet Sender";
        public Mainviewmodel()
        {
            Task.Run(async () =>
            {
                UdpTransceiver udpTransceiver = new("192.168.250.105", 4454, 1);
                var bytes = new byte[] { 5, 4, 3, 2, 1 };
                var reply = await udpTransceiver.SendAsync(bytes, true);
            });
        }
    }
}
