using System.Net.Sockets;

namespace PacketSender.Core
{
    public class Transceiver : ISendable
    {
        public Transceiver()
        {
            IP = "";
            Port = 0;
            CommunicationType = CommunicationType.Udp;
        }
        public Transceiver(string ip, int port, CommunicationType communicationType)
        {
            IP = ip;
            Port = port;
            CommunicationType = communicationType;
        }

        public int Timeout { get; set; } = 1000;
        public string IP { get; set; }
        public int Port { get; set; }
        public CommunicationType CommunicationType { get; set; }

        public async Task<IReply> SendAsync(ReadOnlyMemory<byte> dataToSend, bool isReplyRequired = true, CancellationTokenSource? cancellationToken = default)
        {
            Reply reply = new()
            {
                Status = CommunicationStatus.ConnectionTimeout
            };
            try
            {
                using var client = GetSocket();
                await client.ConnectAsync(IP, Port);
                var sentBytes = await client.SendAsync(dataToSend, SocketFlags.None);
                if (sentBytes != dataToSend.Length)
                {
                    return reply;
                }

                reply.Status = CommunicationStatus.Sent;
                if (isReplyRequired)
                {
                    cancellationToken ??= new();
                    cancellationToken.CancelAfter(Timeout);
                    var length = client.Available;
                    while (cancellationToken.IsCancellationRequested is false && length == 0)
                    {
                        length = client.Available;
                        await Task.Delay(1);
                    }
                    reply.InitializeMemory(length);
                    await client.ReceiveAsync(reply.ReplyData, SocketFlags.None, cancellationToken.Token);
                    reply.Status = CommunicationStatus.ReplyReceived;
                }
            }
            catch (Exception)
            {
                if (reply.Status == CommunicationStatus.Sent)
                {
                    reply.Status = CommunicationStatus.SentButReceiveFailed;
                }
            }
            return reply;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Socket GetSocket()
        {
            Socket client;
            switch (CommunicationType)
            {
                case CommunicationType.Udp:
                    client = new Socket(SocketType.Dgram, ProtocolType.Udp);
                    break;

                case CommunicationType.Tcp:
                    client = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    break;

                default:
                    throw new NotImplementedException("Only UDP and TCP are supported");
            }
            return client;
        }
    }
}