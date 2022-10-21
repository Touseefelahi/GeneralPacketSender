using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PacketSender.Core.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            var parser = new Parser();
            List<ParserInfo> parserInfos = new()
            {
                new ParserInfo() { DataType = DataType.Bool, StartIndex = 1 },
                new ParserInfo() { DataType = DataType.UInt16, StartIndex = 2 },
                new ParserInfo() { DataType = DataType.Float, StartIndex = 4 }
            };
            var result = parser.Parse(new byte[] { 0, 1, 88, 2, 0xcd, 0xcc, 0x35, 0x42, 8, 9, 10 }, parserInfos);
            var expectedResult = new List<object>() { true, (ushort)600, 45.45f };
            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}