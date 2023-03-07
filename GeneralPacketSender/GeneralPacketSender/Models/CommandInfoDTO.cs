using PacketSender.Core;

namespace GeneralPacketSender.Models
{
    public sealed record CommandInfoDTO(string Name, byte[] CommandBytes, CommunicationType CommunicationType,
        DataType DataType, ParserInfo[] ParserList);
}