using PacketParser.ViewModels;
using PacketSender.Core;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace GeneralPacketSender.Viewmodels
{
    public sealed partial class Mainviewmodel : ObservableObject
    {
        private string fileName = "commandList.json";

        [ObservableProperty]
        private SenderViewModel selectedSendable = null!;

        [ObservableProperty]
        private ObservableCollection<SenderViewModel> sendables = null!;

        [ObservableProperty]
        private bool showAddPanel;

        [ObservableProperty]
        private string title = "Advanced Packet Sender";

        public Mainviewmodel()
        {
            LoadLastConfiguration();
        }

        [RelayCommand]
        private void AddNewControl()
        {
            sendables.Add((SenderViewModel)SelectedSendable.Clone());
            return;
            XmlWriterSettings xmlWriterSettings = new() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(fileName, xmlWriterSettings);
            XmlSerializer serializer = new(typeof(ObservableCollection<SenderViewModel>));
            serializer.Serialize(writer, sendables);
        }

        [RelayCommand]
        private void AddParser()
        {
            SelectedSendable.ParserList.Add(new ParserInfo());
        }

        private void LoadLastConfiguration()
        {
            SelectedSendable = new SenderViewModel() { CommunicationType = CommunicationType.Udp };
            Sendables = new();
            return;
            using XmlReader reader = XmlReader.Create(fileName);
            XmlSerializer des = new(typeof(ObservableCollection<SenderViewModel>));
            sendables = (ObservableCollection<SenderViewModel>)des.Deserialize(reader);
        }

        [RelayCommand]
        private void Send()
        {
            SelectedSendable.SendCommand.Execute(null);
        }
    }
}