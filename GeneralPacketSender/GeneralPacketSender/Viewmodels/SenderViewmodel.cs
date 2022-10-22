﻿using GeneralPacketSender.Models;
using PacketSender.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeneralPacketSender.Viewmodels
{
    [ObservableObject]
    public sealed partial class SenderViewmodel : ICloneable
    {
        [ObservableProperty]
        bool isRepeatSendOn;

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
        private bool showSettings;

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
            PacketInfo = new PacketInfo
            {
                Command = new Memory<byte>(new byte[] { 45, 78, 65 })
            };
        }

        [RelayCommand]
        private async Task Send()
        {
            var reply = await Sendable.SendAsync(PacketInfo.Command);
            if (ParserList.Count > 0)
            {
                Parser.Parse(reply.ReplyData, ParserList);
            }
        }

        private void SetSender()
        {
            switch (CommunicationType)
            {
                case CommunicationType.Udp:
                    Sendable = new UdpTransceiver("192.168.250.105", 4454);
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
