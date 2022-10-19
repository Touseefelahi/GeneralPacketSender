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
        ObservableCollection<SenderViewmodel> sendables;

        private void LoadLastConfiguration()
        {
            sendables = new()
            {
                new SenderViewmodel() { CommunicationType = CommunicationType.Udp },
                new SenderViewmodel() { CommunicationType = CommunicationType.Tcp },
                new SenderViewmodel() { CommunicationType = CommunicationType.Serial }
            };
        }

        [RelayCommand]
        private void AddNewControl()
        {
            sendables.Add(new SenderViewmodel() { CommunicationType = CommunicationType.Udp });
        }
    }
}
