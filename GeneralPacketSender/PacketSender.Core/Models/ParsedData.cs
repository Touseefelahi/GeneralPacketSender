namespace PacketSender.Core
{
    /// <summary>
    /// Parsed data
    /// <paramref name="Name">Name of parameter</paramref>
    /// <paramref name="Value">Value of the parameter</paramref>
    /// </summary>
    public record ParsedData(string Name, object Value);

}
