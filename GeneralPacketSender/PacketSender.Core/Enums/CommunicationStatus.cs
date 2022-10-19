namespace PacketSender.Core
{
    /// <summary>
    /// General Communication Status
    /// </summary>
    public enum CommunicationStatus
    {
        /// <summary>
        /// Failed to connect within the specified time
        /// </summary>
        ConnectionTimeout,

        /// <summary>
        /// Couldn't send any bytes 
        /// </summary>
        FailedToSend,

        /// <summary>
        /// Data sent
        /// </summary>
        Sent,

        /// <summary>
        /// Reply received
        /// </summary>
        ReplyReceived,

        /// <summary>
        /// Sent but failed to receive the reply
        /// </summary>
        SentButReceiveFailed,

    }
}
