using System.Text;
using System.Xml.Serialization;

namespace GeneralPacketSender.Models
{
    /// <summary>
    /// Basic packet information
    /// </summary>
    public sealed class PacketInfo : ICloneable
    {
        private Memory<byte> command;
        private string commandAsString = null!;

        [XmlIgnore]
        public Memory<byte> Command
        {
            get => command;
            set
            {
                command = value;
                SetCommandString(command);
            }
        }

        public string CommandAsString
        {
            get => commandAsString;
            set
            {
                commandAsString = value;
                SetMemoryByte(commandAsString);
            }
        }

        public string? CommandName { get; set; }

        /// <summary>
        /// It will create the deep copy of this object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var cloned = (PacketInfo)MemberwiseClone();
            cloned.Command = new Memory<byte>(Command.Span.ToArray());
            return cloned;
        }

        private void SetCommandString(Memory<byte> memory)
        {
            try
            {
                StringBuilder sb = new();

                for (int i = 0; i < memory.Length; i++)
                {
                    sb.Append(memory.Span[i].ToString("X2")).Append(" ");
                }

                if (sb.Length > 2)
                    sb.Remove(sb.Length - 1, 1);
                commandAsString = sb.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void SetMemoryByte(string commandAsString)
        {
            try
            {
                commandAsString = commandAsString.Replace(" ", "");
                if (commandAsString.Length > 0 && commandAsString.Length % 2 != 0)
                {
                    //Adding 0 to second last hex value
                    var lastChar = commandAsString[commandAsString.Length - 1];
                    var firstSegment = commandAsString.Substring(0, commandAsString.Length - 1);
                    commandAsString = firstSegment + "0" + lastChar;
                }
                command = new Memory<byte>(Convert.FromHexString(commandAsString));
            }
            catch (Exception)
            {
            }
        }
    }
}