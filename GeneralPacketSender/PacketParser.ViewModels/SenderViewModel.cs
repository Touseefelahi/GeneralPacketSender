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

        public SenderViewModel()
        {
            transceiver = new();
            parserList = new();
            cancelation = new();

            PacketInfo = new PacketInfo
            {
                Command = new Memory<byte>()
                //new byte[] { 45, 78, 65 }
            };
        }

        public object Clone()
        {
            var cloned = new SenderViewModel
            {
                PacketInfo = (PacketInfo)PacketInfo.Clone(),
                transceiver = (Transceiver)Transceiver.Clone(),
                ParserList = new ObservableCollection<ParserInfo>()
            };
            foreach (var item in ParserList)
            {
                cloned.ParserList.Add((ParserInfo)item.Clone());
            }
            return cloned;
        }

        [ObservableProperty] private Memory<byte> lastReply;
        [RelayCommand]
        private async Task Send()
        {
            //This routine must be improved for better time period correction, we are waiting for the response and the Total Time period will
            //be TimePeriod + Time Taken by the server to respond
            if (IsOnRepeatSend)
            {
                IsSendingOnRepeat = true;
                cancelation = new();
            }
            do
            {
                var reply = await Transceiver.SendAsync(PacketInfo.Command);
                if (ParserList.Count > 0)
                {
                    try
                    {
                        Result = new ObservableCollection<ParsedData>(Parser.Parse(reply.ReplyData, ParserList));
                        LastReply = reply.ReplyData;
                    }
                    catch (Exception)
                    {
                        //Throw parser exception
                    }
                }
                if (IsOnRepeatSend || IsSendingOnRepeat)
                {
                    try
                    {
                        await Task.Delay((int)TimePeriod, cancelation.Token);
                    }
                    catch (Exception)
                    {
                        // It will be called on Cancelation/Stop
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
    }
}