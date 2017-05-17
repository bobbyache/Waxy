using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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
    ***************************************************************************************************************************************** */

    [TestFixture]
    public class TextualGetRequestTests
    {
        //[Test]
        //public void CreateHttpClientRequestFrom_SampleBigBay_GET_Request()
        //{
        //    string filePath = "";

        //    RequestFile requestFile = new RequestFile(filePath);
        //    Assert.AreEqual("www.wavescape.co.za", requestFile["Host"]);
        //    Assert.AreEqual("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36", requestFile["User-Agent"]);
        //    Assert.AreEqual("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8", requestFile["Accept"]);
        //    Assert.AreEqual("http://www.wavescape.co.za/tools/webcams/big-bay.html", requestFile["Referer"]);
        //    Assert.AreEqual("gzip, deflate, sdch", requestFile["Accept -Encoding"]);
        //    Assert.AreEqual("en-US,en;q=0.8", requestFile["Accept-Language"]);
        //}

        [Test]
        [Category("Fast")]
        public void TextualGetRequest_IterateThroughHttpClientRequest_From_SampleBigBay_Get_Request()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();

            TextualGetRequest textualGetRequest = new TextualGetRequest(TxtFile.ResolvePath("HTTP_GET_BigBay.txt"));
            foreach (RequestHeader requestHeader in textualGetRequest)
                headers.Add(requestHeader.Key, requestHeader.Value);

            Assert.AreEqual("www.wavescape.co.za", headers["Host"]);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36", headers["User-Agent"]);
            Assert.AreEqual("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8", headers["Accept"]);
            Assert.AreEqual("http://www.wavescape.co.za/tools/webcams/big-bay.html", headers["Referer"]);
            Assert.AreEqual("gzip, deflate, sdch", headers["Accept-Encoding"]);
            Assert.AreEqual("en-US,en;q=0.8", headers["Accept-Language"]);
        }
    }
}