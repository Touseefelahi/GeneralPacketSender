using System.Net.Sockets;

namespace PacketSender.Core
{
    public sealed class UdpTransceiver : EthernetComm, ISendable
    {
        public UdpClient Client { get; }
        public UdpTransceiver(string iP, int port) : base(iP, port)
        {
            Client = new UdpClient(IP, Port);
        }

        public async Task<IReply> SendAsync(ReadOnlyMemory<byte> dataToSend, bool isReplyRequired = true, CancellationTokenSource? cancellationToken = default)
        {
            Reply reply = new()
            {
                Status = CommunicationStatus.ConnectionTimeout
            };
            var sentBytes = await Client.SendAsync(dataToSend);
            if (sentBytes == dataToSend.Length)
            {
                reply.Status = CommunicationStatus.Sent;
            }
            if (isReplyRequired)
            {
                try
                {
                    cancellationToken ??= new();
                    cancellationToken.CancelAfter(Timeout);
                    var result = await Client.ReceiveAsync(cancellationToken.Token);
                    reply.SetReply(result.Buffer);
                    reply.Status = CommunicationStatus.ReplyReceived;
                }
                catch (Exception ex)
                {
                    reply.Status = CommunicationStatus.SentButReceiveFailed;
                }
            }
            return reply;
        }
    }
}
