namespace PacketSender.Core
{
    /// <summary>
    /// This is general reply data for any type of communication
    /// </summary>
    public interface IReply
    {
        /// <summary>
        /// Communication Status
        /// </summary>
        CommunicationStatus Status { get; }

        /// <summary>
        /// Reply data
        /// </summary>
        Memory<byte> ReplyData { get; }

        /// <summary>
        /// Set reply
        /// </summary>
        /// <param name="dataToSetForReply"></param>
        void SetReply(Memory<byte> dataToSetForReply);

        /// <summary>
        /// Initializes new memory of given length
        /// </summary>
        /// <param name="length"></param>
        void InitializeMemory(int length);
    }
}