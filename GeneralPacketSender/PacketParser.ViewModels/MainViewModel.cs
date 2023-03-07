using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PacketSender.Core;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace PacketParser.ViewModels
{
    public sealed partial class MainViewModel : ObservableObject
    {
        private string fileName = "commandList.xml";

        [ObservableProperty] private SenderViewModel selectedSendable = null!;
        [ObservableProperty] private ParserInfo selectedParser = null!;
        [ObservableProperty] private ObservableCollection<SenderViewModel> sendables = null!;

        [ObservableProperty] private SenderViewModel defaultSendable = null!;
        [ObservableProperty] private bool showAddPanel;

        [ObservableProperty] private string title = "Packet Parser";
        [ObservableProperty] private bool isEditing;

        private XmlAttributeOverrides xOver = null!;
        private SenderViewModel temporarySendable = null!;
        public MainViewModel()
        {
            DefaultSendable = new SenderViewModel();
            SetXmlOverrides();
            LoadLastConfiguration();
        }

        [RelayCommand]
        private void AddNewControl()
        {
            sendables.Add((SenderViewModel)DefaultSendable.Clone());
            SaveXml();
        }

        [RelayCommand]
        private void Edit()
        {
            if (IsEditing) //Update
            {
                DefaultSendable = temporarySendable;
                SaveXml();
            }
            else //Edit Mode
            {
                temporarySendable = DefaultSendable;
                DefaultSendable = SelectedSendable;
            }
            IsEditing = !IsEditing;
        }


        [RelayCommand]
        private void ImportPackets()
        {

        }

        [RelayCommand]
        private void SaveXml()
        {
            XmlWriterSettings xmlWriterSettings = new() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(fileName, xmlWriterSettings);
            XmlSerializer serializer = new(typeof(ObservableCollection<SenderViewModel>), xOver);
            serializer.Serialize(writer, sendables);
        }

        [RelayCommand]
        private void AddParser()
        {
            SelectedSendable.ParserList.Add(new ParserInfo());
        }

        [RelayCommand]
        private void DeleteParser()
        {
            SelectedSendable.ParserList.Remove(SelectedParser);
        }

        private void LoadLastConfiguration()
        {
            try
            {
                using XmlReader reader = XmlReader.Create(fileName);
                XmlSerializer des = new(typeof(ObservableCollection<SenderViewModel>), xOver);
                sendables = (ObservableCollection<SenderViewModel>)des.Deserialize(reader);
            }
            catch (Exception)
            {
            }
            if (sendables is null)
            {
                sendables = new ObservableCollection<SenderViewModel>();
            }
        }
        private void SetXmlOverrides()
        {
            xOver = new();
            XmlAttributes attributes = new()
            {
                XmlIgnore = true
            };
            xOver.Add(typeof(SenderViewModel), nameof(SenderViewModel.Result), attributes);
            xOver.Add(typeof(SenderViewModel), nameof(SenderViewModel.IsSendingOnRepeat), attributes);
        }

        [RelayCommand]
        private void Send()
        {
            SelectedSendable.SendCommand.Execute(null);
        }

        [RelayCommand]
        private void Delete()
        {
            sendables.Remove(SelectedSendable);
            SaveXml();
        }
    }
}