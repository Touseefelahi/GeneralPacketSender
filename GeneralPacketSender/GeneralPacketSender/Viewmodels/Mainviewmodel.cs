using PacketSender.Core;
using System.Collections.ObjectModel;

namespace GeneralPacketSender.Viewmodels
{
    public sealed partial class Mainviewmodel : ObservableObject
    {
        [ObservableProperty]
        private string title = "General Packet Sender";
        public Mainviewmodel()
        {
            LoadLastConfiguration();
        }

        [ObservableProperty]
        ObservableCollection<object> sendables;

        private void LoadLastConfiguration()
        {
            sendables = new() { new UdpTransceiver(), new TcpTransceiver() };
        }

        [RelayCommand]
        private void AddNewControl()
        {
            sendables.Add(new UdpTransceiver());
        }
    }
}
