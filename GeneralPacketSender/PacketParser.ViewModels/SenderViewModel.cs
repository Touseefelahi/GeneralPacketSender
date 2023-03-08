using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PacketParser.Models;
using PacketSender.Core;
using System.Collections.ObjectModel;

namespace PacketParser.ViewModels
{
    public sealed partial class SenderViewModel : ObservableObject, ICloneable
    {
        private CancellationTokenSource cancelation;

        [ObservableProperty] private bool isOnRepeatSend;
        [ObservableProperty] private bool isSendingOnRepeat;
        [ObservableProperty] private PacketInfo packetInfo = null!;
        [ObservableProperty] private ObservableCollection<ParserInfo> parserList;
        [ObservableProperty] private ObservableCollection<ParsedData> result = null!;
        [ObservableProperty] private Transceiver transceiver;
        [ObservableProperty] private bool showSettings;
        [ObservableProperty] private uint timePeriod = 1000;
        [ObservableProperty] private Memory<byte> lastReply;
        [ObservableProperty] private bool isServer;
        [ObservableProperty] private bool isListening;
        [ObservableProperty] private ulong totalCount;

        private Communication.Core.IListener listener = null!;
        private ILogger Logger { get; } = null!;
        private IDialogService DialogService { get; } = null!;

        public SenderViewModel()
        {
            transceiver = new();
            parserList = new();
            cancelation = new();

            PacketInfo = new PacketInfo
            {
                Command = new Memory<byte>()
            };
            var service = Services.ServiceProvider.GetService(typeof(IDialogService));
            if (service is IDialogService dialogService)
            {
                DialogService = dialogService;
            }
            Logger = (ILogger)Services.ServiceProvider.GetService(typeof(ILogger));
        }

        public object Clone()
        {
            var cloned = (SenderViewModel)MemberwiseClone();
            cloned.PacketInfo = (PacketInfo)PacketInfo.Clone();
            cloned.transceiver = (Transceiver)Transceiver.Clone();
            cloned.ParserList = new ObservableCollection<ParserInfo>();

            foreach (var item in ParserList)
            {
                cloned.ParserList.Add((ParserInfo)item.Clone());
            }
            return cloned;
        }

        [RelayCommand]
        private async Task Send()
        {
            //This routine must be improved for better time period correction, we are waiting for the response and the Total Time period will
            //be TimePeriod + Time Taken by the server to respond
            if (IsOnRepeatSend)
            {
                IsSendingOnRepeat = true;
                cancelation = new();
                Logger.Info($"{Transceiver.CommunicationType} Repeat Sending Started @ {Transceiver.IP}:{Transceiver.Port}");
            }
            do
            {
                var reply = await Transceiver.SendAsync(PacketInfo.Command);
                TotalCount++;
                if (ParserList.Count > 0)
                {
                    try
                    {
                        Result = new ObservableCollection<ParsedData>(Parser.Parse(reply.ReplyData, ParserList));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
                LastReply = reply.ReplyData;
                if (IsOnRepeatSend || IsSendingOnRepeat)
                {
                    try
                    {
                        await Task.Delay((int)TimePeriod, cancelation.Token);
                    }
                    catch (Exception)
                    {
                        Logger.Info($"{Transceiver.CommunicationType} Repeat Sending Stopped @ {Transceiver.IP}:{Transceiver.Port}");
                    }
                }
            } while (IsSendingOnRepeat);
        }

        [RelayCommand]
        private void Stop()
        {
            IsSendingOnRepeat = false;
            cancelation.Cancel();
        }

        [RelayCommand]
        private void StartServer()
        {
            switch (Transceiver.CommunicationType)
            {
                case CommunicationType.Udp:
                    listener = new Communication.Core.ServerUdp
                    {
                        Port = Transceiver.Port
                    };
                    break;
                case CommunicationType.Tcp:
                    listener = new Communication.Core.ServerTcp()
                    {
                        Port = Transceiver.Port
                    };
                    break;
            }
            listener.DataEvent += DataFromServerIncoming;
            listener.StartListener();
            IsListening = listener.IsListening;
            if (IsListening is false)
            {
                Logger.Error($"Failed to start {Transceiver.CommunicationType} server @{Transceiver.Port} Port");
                DialogService.ShowDialog($"Failed to start {Transceiver.CommunicationType} server @{Transceiver.Port} Port",
                    "Error!");
                return;
            }
            Logger.Info($"{Transceiver.CommunicationType} Server @{Transceiver.Port} Port is running");
        }

        private void DataFromServerIncoming(object? sender, Communication.Core.IReply e)
        {
            if (ParserList.Count > 0)
            {
                try
                {
                    Result = new ObservableCollection<ParsedData>(Parser.Parse(e.RawBytes, ParserList));
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            TotalCount++;
            LastReply = e.RawBytes;
        }

        [RelayCommand]
        private void StopServer()
        {
            listener.StopListener();
            IsListening = listener.IsListening;
            Logger.Info($"{Transceiver.CommunicationType} Server @{Transceiver.Port} Port is stopped");
        }
    }
}