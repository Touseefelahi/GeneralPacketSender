namespace PacketSender.Core
{
    public interface ISendable
    {
        Task<IReply> SendAsync(ReadOnlyMemory<byte> dataToSend, bool isReplyRequired = true, CancellationTokenSource? cancellationToken = default);
    }
}
