using PacketSender.Core.Interfaces;

namespace PacketSender.Core
{
    /// <summary>
    /// Basic Serial transceiver model
    /// </summary>
    public sealed class SerialTransceiver : ISendable
    {
        /// <summary>
        /// Communication Port name
        /// </summary>
        public string? CommPort { get; set; }

        /// <summary>
        /// BaudRate
        /// </summary>
        public int BaudRate { get; set; }

        public SerialTransceiver(string commPort, int baudRate)
        {
            CommPort = commPort;
            BaudRate = baudRate;
            OpenCommuniactionPort();
        }

        private void OpenCommuniactionPort()
        {
            //Open the port
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
