namespace PacketSender.Core
{
    public interface ISendable : ICloneable
    {
        public int Timeout { get; set; }

        Task<IReply> SendAsync(ReadOnlyMemory<byte> dataToSend, bool isReplyRequired = true, CancellationTokenSource? cancellationToken = default);
    }
}