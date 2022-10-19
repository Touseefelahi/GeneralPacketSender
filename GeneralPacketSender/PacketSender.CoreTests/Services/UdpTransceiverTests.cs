using Xunit;
using PacketSender.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketSender.Core.Tests
{
    public class UdpTransceiverTests
    {
        [Fact()]
        public async Task SendAsyncTest()
        {
            UdpTransceiver udpTransceiver = new("192.168.250.105", 4454, 500);
            var bytes = new byte[] { 5, 4, 3, 2, 1 };
            var reply = await udpTransceiver.SendAsync(bytes, true);
        }
    }
}