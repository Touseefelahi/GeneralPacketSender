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
          
        }
    }
}
