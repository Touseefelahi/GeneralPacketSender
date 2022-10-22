using System.Net;
using System.Net.Sockets;

namespace PacketSender.Core
{
    /// <summary>
    /// Basic UDP transceiver model
    /// </summary>
    public sealed class UdpTransceiver : EthernetComm, ISendable
    {
        public UdpClient Client { get; }
        public UdpTransceiver(string ip, int port) : base(ip, port)
        {
            Client = new UdpClient();
        }

        public UdpTransceiver()
        {

        }

        public async Task<IReply> SendAsync(ReadOnlyMemory<byte> dataToSend, bool isReplyRequired = true, CancellationTokenSource? cancellationToken = default)
        {
            Reply reply = new()
            {
                Status = CommunicationStatus.ConnectionTimeout
            };
            cancellationToken ??= new();
            var sentBytes = await Client.SendAsync(dataToSend, new IPEndPoint(IPAddress.Parse(IP), Port), cancellationToken.Token);
            if (sentBytes == dataToSend.Length)
            {
                reply.Status = CommunicationStatus.Sent;
            }
            if (isReplyRequired)
            {
                try
                {
                    cancellationToken.CancelAfter(Timeout);
                    var result = await Client.ReceiveAsync(cancellationToken.Token);
                    reply.SetReply(result.Buffer);
                    reply.Status = CommunicationStatus.ReplyReceived;
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    reply.Status = CommunicationStatus.SentButReceiveFailed;
                }
            }
            return reply;
        }



        public object Clone()
        {
            var cloned = this.MemberwiseClone();
            return cloned;
        }
    }
}
