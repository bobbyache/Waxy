using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TcpGrabImage.UnitTests.Helpers;

namespace TcpGrabImage.UnitTests
{
    [TestFixture]
    public class WavescapeWebcamHttpRequestTests
    {
        [Test]
        public void WavescapeWebcamHttpRequest_ToString_ReturnsCorrectlyFilledOut_HttpRequest()
        {
            string requestTemplate = TxtFile.ReadText("Wavescape_Cam_LandingPage.txt");
            string finalRequest = TxtFile.ReadText("HTTP_GET_BigBay.txt");

            string referer = @"http://www.wavescape.co.za/tools/webcams/big-bay.html";
            string url = @"http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg";
            string randomNumber = "614786193";

            WavescapeWebcamHttpRequest httpRequest = new WavescapeWebcamHttpRequest(requestTemplate, referer, url, randomNumber);
            string fullText = httpRequest.ToString();

            Assert.That(fullText, Is.EqualTo(finalRequest.Trim()));
        }
    }
}
