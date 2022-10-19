namespace PacketSender.Core
{
    /// <summary>
    /// Basic Serial transceiver model
    /// </summary>
    public sealed class SerialTransceiver : ISendable
    {
        public SerialTransceiver(string commPort, int baudRate)
        {

        }

        public SerialTransceiver()
        {

        }

        public async Task<IReply> SendAsync(ReadOnlyMemory<byte> dataToSend, bool isReplyRequired = true, CancellationTokenSource? cancellationToken = default)
        {
            Reply reply = new()
            {
                Status = CommunicationStatus.ConnectionTimeout
            };

            return reply;
        }
    }
}
