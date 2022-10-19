namespace PacketSender.Core
{
    /// <summary>
    /// General Reply
    /// </summary>
    public struct Reply : IReply
    {
        public CommunicationStatus Status { get; set; }

        public Memory<byte> ReplyData { get; private set; }

        public void SetReply(Memory<byte> dataToSetForReply)
        {
            ReplyData = dataToSetForReply;
        }
    }
}
