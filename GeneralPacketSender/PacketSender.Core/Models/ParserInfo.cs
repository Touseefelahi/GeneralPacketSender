namespace PacketSender.Core
{
    /// <summary>
    /// Parser Info
    /// </summary>
    public sealed class ParserInfo
    {
        /// <summary>
        /// Type of value
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// Name of value
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Start Index in the reply packet
        /// </summary>
        public int StartIndex { get; set; }


        public ParserInfo()
        {

        }

        public ParserInfo(DataType dataType, string name, int startIndex)
        {
            DataType = dataType;
            Name = name;
            StartIndex = startIndex;
        }
    }
}
