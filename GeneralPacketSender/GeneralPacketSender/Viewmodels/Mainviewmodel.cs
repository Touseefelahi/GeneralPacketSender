using PacketSender.Core;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
namespace GeneralPacketSender.Viewmodels
{
    public sealed partial class Mainviewmodel : ObservableObject
    {
        [ObservableProperty]
        private string title = "Advanced Packet Sender";
        public Mainviewmodel()
        {
            LoadLastConfiguration();
        }

        [ObservableProperty]
        ObservableCollection<SenderViewmodel> sendables;

        private void LoadLastConfiguration()
        {
            SelectedSendable = new SenderViewmodel() { CommunicationType = CommunicationType.Udp };
            Sendables = new();
            return;
            using XmlReader reader = XmlReader.Create(fileName);
            XmlSerializer des = new(typeof(ObservableCollection<SenderViewmodel>));
            sendables = (ObservableCollection<SenderViewmodel>)des.Deserialize(reader);
        }

        [ObservableProperty]
        SenderViewmodel selectedSendable;

        [ObservableProperty]
        private bool showAddPanel;

        string fileName = "commandList.json";
        [RelayCommand]
        private void AddNewControl()
        {
            sendables.Add((SenderViewmodel)SelectedSendable.Clone());
            return;
            XmlWriterSettings xmlWriterSettings = new() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(fileName, xmlWriterSettings);
            XmlSerializer serializer = new(typeof(ObservableCollection<SenderViewmodel>));
            serializer.Serialize(writer, sendables);
        }

        [RelayCommand]
        private void Send()
        {
            SelectedSendable.SendCommand.Execute(null);
        }


        [RelayCommand]
        private void AddParser()
        {
            SelectedSendable.ParserList.Add(new ParserInfo());
        }

    }
}
