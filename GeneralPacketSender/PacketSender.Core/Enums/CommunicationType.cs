namespace PacketSender.Core
{
    /// <summary>
    /// Communication Type 
    /// </summary>
    public enum CommunicationType
    {
        /// <summary>
        /// UDP communication- User datagram protocol
        /// </summary>
        Udp,

        /// <summary>
        /// TCP communication- Transmission control protocol
        /// </summary>
        Tcp,

        /// <summary>
        /// Serial communication- Simple Rs232
        /// </summary>
        Serial
    }
}
