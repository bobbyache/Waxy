using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http.UnitTests
{
    [TestFixture]
    [Category("Fast Units")]
    class RequestHeaderTests
    {
        [Test]
        public void RequestHeader_ParseHeaderFromRequesHeaderLine()
        {
            string requestHeaderLine = " Host: www.wavescape.co.za ";
            TextualGetRequest.RequestHeader requestHeader = new TextualGetRequest.RequestHeader(requestHeaderLine);
            Assert.AreEqual("Host", requestHeader.Key, "RequestHeader.Key is expected to be 'Host'");
            Assert.AreEqual("www.wavescape.co.za", requestHeader.Value, "RequestHeader.Value is expected to be 'www.wavescape.co.za'");
        }

        [Test]
        public void RequestHeader_ParseEmptyHeader_ReturnsEmptyValueText()
        {
            string requestHeaderLine = "Host: ";
            TextualGetRequest.RequestHeader requestHeader = new TextualGetRequest.RequestHeader(requestHeaderLine);
            Assert.AreEqual("Host", requestHeader.Key, "RequestHeader.Key is expected to be 'Host'");
            Assert.AreEqual("", requestHeader.Value, "RequestHeader.Value is expected to be ''");
        }

        [Test]
        public void RequestHeader_ParseInvalidEmptyHeader_WithNoColon_Throws_InvalidHttpRequestHeader()
        {
            string requestHeaderLine = "invalid header";
            Assert.Throws<Exceptions.InvalidHttpRequestHeader>(() => new TextualGetRequest.RequestHeader(requestHeaderLine));
        }
    }
}
