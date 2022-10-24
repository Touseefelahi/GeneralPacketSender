using GeneralPacketSender.Models;
using PacketSender.Core;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeneralPacketSender.Viewmodels
{
    public sealed partial class SenderViewmodel : ObservableObject, ICloneable
    {
        [ObservableProperty]
        bool isOnRepeatSend;

        public CommunicationType CommunicationType
        {
            get => communicationType;
            set
            {
                communicationType = value;
                SetSender();
            }
        }

        [ObservableProperty]
        uint timePeriod = 1000;

        [ObservableProperty]
        private bool showSettings;


        [ObservableProperty]
        private bool isSendingOnRepeat;

        [ObservableProperty]
        [XmlIgnore]
        private ISendable sendable;

        [ObservableProperty]
        PacketInfo packetInfo;

        private CommunicationType communicationType;

        [ObservableProperty]
        private ObservableCollection<ParserInfo> parserList;
        public SenderViewmodel()
        {
            parserList = new();
            cancelation = new();
            PacketInfo = new PacketInfo
            {
                Command = new Memory<byte>(new byte[] { 45, 78, 65 })
            };
        }

        [ObservableProperty]
        private ObservableCollection<ParsedData> result;


        [RelayCommand]
        private async Task Send()
        {
            //This routine must be improved for better time period correction, we are waiting for the response and the Total Time period will 
            //be TimePeriod + Time Taken by the server to respond 
            if (IsOnRepeatSend)
            {
                IsSendingOnRepeat = true;
            }
            do
            {
                var reply = await Sendable.SendAsync(PacketInfo.Command);
                if (ParserList.Count > 0)
                {
                    try
                    {
                        Result = new ObservableCollection<ParsedData>(Parser.Parse(reply.ReplyData, ParserList));
                    }
                    catch (Exception)
                    {
                    }
                }
                if (IsOnRepeatSend)
                {
                    try
                    {
                        await Task.Delay((int)TimePeriod, cancelation.Token);
                    }
                    catch (Exception) // It will be called on Cancelation/Stop
                    {

                    }
                }
            } while (IsSendingOnRepeat);
        }

        CancellationTokenSource cancelation;

        [RelayCommand]
        private void Stop()
        {
            IsSendingOnRepeat = false;
            cancelation.Cancel();
        }

        private void SetSender()
        {
            switch (CommunicationType)
            {
                case CommunicationType.Udp:
                    Sendable = new UdpTransceiver("192.168.8.103", 4454);
                    ParserList = new();
                    ParserList.Add(new ParserInfo(DataType.Bool, "Status", 1));
                    ParserList.Add(new ParserInfo(DataType.UInt16, "LRF", 2));
                    ParserList.Add(new ParserInfo(DataType.Float, "GDOP", 4));
                    break;
                case CommunicationType.Tcp:
                    Sendable = new TcpTransceiver("172.20.10.5", 3030);
                    break;
                case CommunicationType.Serial:
                    Sendable = new SerialTransceiver();
                    break;
            }
        }

        public object Clone()
        {
            var cloned = (SenderViewmodel)MemberwiseClone();
            cloned.PacketInfo = (PacketInfo)PacketInfo.Clone();
            cloned.sendable = (ISendable)Sendable.Clone();
            return cloned;

        }
    }
}
