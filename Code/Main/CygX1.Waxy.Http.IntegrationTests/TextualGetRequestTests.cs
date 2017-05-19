using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace CygX1.Waxy.Http.IntegrationTests
{

    /* *****************************************************************************************************************************************
    * Waxy - Regex to filter out hyperlinks or anchor tags or hyperlinks with image targets. 
    * Need this Regex as a setting in case image different. Then can get full link for http... 
    * This for pages that require some random code to stop linking from another site. Otherwise can just manually supply link. 
    * Then need template http request with key-value pairs for replacing params in the order declared in the file. A file for each http request. 
    * Wrap Http request up completely so can test as mock.
    *
    * Compare Images using some kind of Use sha1 to generate checksum
    * (you may need this to ensure that the image returned is not the "bad image" image sent back to you by
    * some sites when they're down or they reject your request.
    * https://www.youtube.com/watch?v=oJpZ5ygg4qQ
    * https://www.codeproject.com/Articles/38951/How-To-Hash-Data-Using-MD-and-SHA
    * 
    * Regular expression for http url (Source: http://stackoverflow.com/questions/3809401/what-is-a-good-regular-expression-to-match-a-url)
    * https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)
    * Regular expression for relative url?
    * [-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)
    * 
    * GET HTTP request specification.
    * https://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html
    ***************************************************************************************************************************************** */

    [TestFixture]
    [Category("Slow Integration Files")]
    public class TextualGetRequestTests
    {
        [Test]
        public void TextualGetRequest_GetHeaderValues_Via_IndexerProperties()
        {
            string requestText = TxtFile.ReadText("HTTP_GET_BigBay.txt");
            TextualGetRequest textualGetRequest = new TextualGetRequest(requestText);

            Assert.AreEqual("www.wavescape.co.za", textualGetRequest["Host"]);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36", textualGetRequest["User-Agent"]);
            Assert.AreEqual("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8", textualGetRequest["Accept"]);
            Assert.AreEqual("http://www.wavescape.co.za/tools/webcams/big-bay.html", textualGetRequest["Referer"]);
            Assert.AreEqual("gzip, deflate, sdch", textualGetRequest["Accept-Encoding"]);
            Assert.AreEqual("en-US,en;q=0.8", textualGetRequest["Accept-Language"]);
        }

        [Test]
        public void TextualGetRequest_IterateThroughHttpClientRequest_From_SampleBigBay_Get_Request()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();

            string requestText = TxtFile.ReadText("HTTP_GET_BigBay.txt");
            TextualGetRequest textualGetRequest = new TextualGetRequest(requestText);

            foreach (RequestHeader requestHeader in textualGetRequest.RequestHeaders)
                headers.Add(requestHeader.Key, requestHeader.Value);

            Assert.AreEqual("www.wavescape.co.za", headers["Host"]);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36", headers["User-Agent"]);
            Assert.AreEqual("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8", headers["Accept"]);
            Assert.AreEqual("http://www.wavescape.co.za/tools/webcams/big-bay.html", headers["Referer"]);
            Assert.AreEqual("gzip, deflate, sdch", headers["Accept-Encoding"]);
            Assert.AreEqual("en-US,en;q=0.8", headers["Accept-Language"]);
        }

        [Test]
        public void TextualGetRequest_ParseGetHeader_WithDirectLink_IsParsedCorrectly()
        {
            // No need to go scrape the link.
            string requestText = TxtFile.ReadText("HTTP_GET_BigBay.txt");
            TextualGetRequest textualGetRequest = new TextualGetRequest(requestText);

            Assert.AreEqual("GET", textualGetRequest.Method);
            Assert.AreEqual("http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg&tmpl=component&rnd=614786193", textualGetRequest.RequestUri);
            Assert.AreEqual("HTTP/1.1", textualGetRequest.HttpVersion);
        }
    }
}