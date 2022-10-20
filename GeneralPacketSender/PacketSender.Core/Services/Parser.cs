using System;

namespace PacketSender.Core
{
    public sealed class Parser
    {
        public List<object> Parse(ReadOnlyMemory<byte> databytes, List<ParserInfo> parserInfoList)
        {
            if (parserInfoList is not null && databytes.Span.Length > 0 && parserInfoList.Count > 0)
            {

                List<object> listOfValues = new();


                foreach (var parser in parserInfoList)
                {

                }

                return new List<object>();
            }
            return null;
        }
    }
}
