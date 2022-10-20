namespace PacketSender.Core
{
    /// <summary>
    /// Parser Info
    /// </summary>
    public struct ParserInfo
    {
        /// <summary>
        /// Type of value
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Name of value
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Start Index in the reply packet
        /// </summary>
        public int StartIndex { get; set; }
    }
}
