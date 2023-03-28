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
        private string defaultFileName = "defaultCommandList.xml";

        [ObservableProperty] private string version;
        [ObservableProperty] private string title = "Packet Parser";
        [ObservableProperty] private SenderViewModel selectedSendable = null!;
        [ObservableProperty] private ParserInfo selectedParser = null!;
        [ObservableProperty] private ObservableCollection<SenderViewModel> sendables = null!;
        [ObservableProperty] private SenderViewModel defaultSendable = null!;
        [ObservableProperty] private bool showAddPanel;
        [ObservableProperty] private bool isEditing;
        [ObservableProperty] private ILogger logger = null!;

        private XmlAttributeOverrides xOver = null!;
        private SenderViewModel temporarySendable = null!;

        public MainViewModel(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Services.SetProvider(serviceProvider);
            Logger = (ILogger)ServiceProvider.GetService(typeof(ILogger));
            SetXmlOverrides();
            LoadConfiguration(fileName);
            LoadDefaultSendable();
            SelectedSendable = DefaultSendable;
        }

        public IServiceProvider ServiceProvider { get; }

        private void LoadDefaultSendable()
        {
            try
            {
                using XmlReader reader = XmlReader.Create(defaultFileName);
                XmlSerializer des = new(typeof(SenderViewModel), xOver);
                DefaultSendable = (SenderViewModel)des.Deserialize(reader);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            DefaultSendable ??= new();
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
        private async Task ImportPackets()
        {
            var dialogService = (IDialogService)ServiceProvider.GetService(typeof(IDialogService));
            var fileName = await dialogService.OpenFileDialog();
            LoadConfiguration(fileName);
        }

        [RelayCommand]
        private void SaveXml()
        {
            try
            {
                XmlWriterSettings xmlWriterSettings = new() { Indent = true };
                using XmlWriter writer = XmlWriter.Create(fileName, xmlWriterSettings);
                XmlSerializer serializer = new(typeof(ObservableCollection<SenderViewModel>), xOver);
                serializer.Serialize(writer, sendables);
                Logger.Info($"Command list saved");

                using XmlWriter writer2 = XmlWriter.Create(defaultFileName, xmlWriterSettings);
                XmlSerializer serializer2 = new(typeof(SenderViewModel), xOver);
                serializer2.Serialize(writer2, DefaultSendable);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        [RelayCommand]
        private void AddParser()
        {
            if (SelectedSendable != null)
            {
                SelectedSendable.ParserList.Add(new ParserInfo());
            }
        }

        [RelayCommand]
        private void DeleteParser()
        {
            if (SelectedParser != null && SelectedSendable != null)
            {
                SelectedSendable.ParserList.Remove(SelectedParser);
            }
        }

        private void LoadConfiguration(string fileName)
        {
            try
            {
                using XmlReader reader = XmlReader.Create(fileName);
                XmlSerializer des = new(typeof(ObservableCollection<SenderViewModel>), xOver);
                Sendables = (ObservableCollection<SenderViewModel>)des.Deserialize(reader);
                Logger.Info($"Configuration Loaded {Path.GetFileName(fileName)}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            sendables ??= new();
        }

        private void SetXmlOverrides()
        {
            xOver = new();
            XmlAttributes attributes = new()
            {
                XmlIgnore = true
            };
            xOver.Add(typeof(SenderViewModel), nameof(SenderViewModel.Result), attributes);
            xOver.Add(typeof(SenderViewModel), nameof(SenderViewModel.Results), attributes);
            xOver.Add(typeof(SenderViewModel), nameof(SenderViewModel.IsListening), attributes);
            xOver.Add(typeof(SenderViewModel), nameof(SenderViewModel.IsSendingOnRepeat), attributes);
        }

        [RelayCommand]
        private void Send()
        {
            DefaultSendable.SendCommand.Execute(null);
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedSendable != null && sendables.Contains(SelectedSendable))
            {
                Logger.Info($"Removing {SelectedSendable.PacketInfo.CommandName} Control");
                sendables.Remove(SelectedSendable);
                SaveXml();
            }
        }
    }
}