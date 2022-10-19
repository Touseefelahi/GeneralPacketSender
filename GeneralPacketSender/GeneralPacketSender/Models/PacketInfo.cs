namespace GeneralPacketSender.Models
{
    /// <summary>
    /// Basic packet information
    /// </summary>
    public sealed class PacketInfo
    {
        public Memory<byte> Command { get; set; }
        public string? CommandName { get; set; }
    }
}
