using HandyControl.Interactivity;
using PacketSender.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace GeneralPacketSender.Viewmodels
{
    public sealed partial class Mainviewmodel : ObservableObject
    {
        [ObservableProperty]
        private string title = "Advance Packet Sender";
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
            sendables.Add(new SenderViewmodel() { CommunicationType = CommunicationType.Udp });
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
    }
}
