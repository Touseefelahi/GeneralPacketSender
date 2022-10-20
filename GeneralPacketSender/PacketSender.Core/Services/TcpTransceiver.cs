using System.Net.Sockets;
using PacketSender.Core.Interfaces;

namespace PacketSender.Core
{
    public sealed class TcpTransceiver : EthernetComm, ISendable
    {
        public UdpClient Client { get; }
        public TcpTransceiver(string iP, int port) : base(iP, port)
        {
            Client = new UdpClient(IP, Port);
        }
        public TcpTransceiver()
        {

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
