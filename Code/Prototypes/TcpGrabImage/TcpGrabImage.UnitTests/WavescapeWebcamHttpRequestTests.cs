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
using TcpGrabImage.UnitTests.Helpers;
using System.Collections;

namespace TcpGrabImage.UnitTests
{

    /*
* Waxy - Regex to filter out hyperlinks or anchor tags or hyperlinks with image targets. 
* Need this Regex as a setting in case image different. Then can get full link for http... 
* This for pages that require some random code to stop linking from another site. Otherwise can just manually supply link. 
* Then need template http request with key-value pairs for replacing params in the order declared in the file. A file for each http request. 
* Wrap Http request up completely so can test as mock.
*/

    /*
* Compare Images using some kind of Use sha1 to generate checksum
* (you may need this to ensure that the image returned is not the "bad image" image sent back to you by
* some sites when they're down or they reject your request.
* https://www.youtube.com/watch?v=oJpZ5ygg4qQ
* https://www.codeproject.com/Articles/38951/How-To-Hash-Data-Using-MD-and-SHA
* */

    [TestFixture]
    public class WavescapeWebcamHttpRequestTests
    {
        [Test]
        [Category("Integration")]
        public void GetImageFromHyperlink()
        {
            SimpleWebCam simpleWebCam = new SimpleWebCam();
            Image img = simpleWebCam.Fetch();
            img.Save(@"C:\Users\robertb\Documents\Work\test.png", ImageFormat.Png);
            Assert.IsNotNull(img);
        }

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
        public void IterateThroughHttpClientRequest_From_SampleBigBay_Get_Request()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();

            //RequestFile requestFile = new RequestFile(TxtFile.ReadText("HTTP_GET_BigBay.txt"));
            RequestFile requestFile = new RequestFile(TxtFile.ResolvePath("HTTP_GET_BigBay.txt"));
            foreach (RequestHeader requestHeader in requestFile)
                headers.Add(requestHeader.Key, requestHeader.Value);

            Assert.AreEqual("www.wavescape.co.za", headers["Host"]);
            Assert.AreEqual("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36", headers["User-Agent"]);
            Assert.AreEqual("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8", headers["Accept"]);
            Assert.AreEqual("http://www.wavescape.co.za/tools/webcams/big-bay.html", headers["Referer"]);
            Assert.AreEqual("gzip, deflate, sdch", headers["Accept-Encoding"]);
            Assert.AreEqual("en-US,en;q=0.8", headers["Accept-Language"]);
        }
    }

    internal class RequestFile : IEnumerable<RequestHeader>
    {
        // How do I implement IEnumerable<T>
        // http://stackoverflow.com/questions/11296810/how-do-i-implement-ienumerablet
        private string filePath;

        public RequestFile(string filePath)
        {
            this.filePath = filePath;
        }

        public IEnumerator<RequestHeader> GetEnumerator()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader streamReader = File.OpenText(filePath))
                {
                    string input = null;
                    while ((input = streamReader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(input) && input.Contains(":") && !input.Contains("GET"))
                        {
                            int index = input.IndexOf(':', 0);
                            string key = input.Substring(0, index);
                            string value = input.Substring(index + 1, input.Length - (index + 1));

                            RequestHeader requestHeader = new RequestHeader();
                            requestHeader.Key = key.Trim();
                            requestHeader.Value = value.Trim();
                            yield return requestHeader;
                        }
                    }
                }
                yield break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    internal class RequestHeader
    {
        public string Key { get; internal set; }
        public string Value { get; internal set; }
    }

    internal class SimpleWebCam
    {
        // http://stackoverflow.com/questions/1053052/a-generic-error-occurred-in-gdi-jpeg-image-to-memorystream
        // http://stackoverflow.com/questions/12022965/adding-http-headers-to-httpclient
        internal Image Fetch()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://www.bouml.fr/doc/figs/diagramstereotypes.png"),
                Method = HttpMethod.Get,
                Version = new Version("1.1")
            };

            request.Headers.Host = "www.bouml.fr";
            request.Headers.Connection.Add("keep-alive");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");
            request.Headers.Add("Accept", "image/webp,image/*,*/*;q=0.8");
            request.Headers.Add("Referer", "http://www.bouml.fr/doc/statediagram.html");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");

            Image img = null;

            var task = client.SendAsync(request)
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    Task<byte[]> taskByteArray = response.Content.ReadAsByteArrayAsync();
                    taskByteArray.Wait();

                    using (MemoryStream memoryStream = new MemoryStream(taskByteArray.Result))
                    {
                        img = Image.FromStream(memoryStream);
                    }
                });
            task.Wait();

            return img;
        }
    }
}



// http://www.surf-forecast.com/breaks/Bayof-Plenty_1/photos/5658
//[Test]
//public void WavescapeWebcamHttpRequest_ToString_ReturnsCorrectlyFilledOut_HttpRequest()
//{
//    string requestTemplate = TxtFile.ReadText("Wavescape_Cam_LandingPage.txt");
//    string finalRequest = TxtFile.ReadText("HTTP_GET_BigBay.txt");

//    string referer = @"http://www.wavescape.co.za/tools/webcams/big-bay.html";
//    string url = @"http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg";
//    string randomNumber = "614786193";

//    WavescapeWebcamHttpRequest httpRequest = new WavescapeWebcamHttpRequest(requestTemplate, referer, url, randomNumber);
//    string fullText = httpRequest.ToString();

//    Assert.That(fullText, Is.EqualTo(finalRequest.Trim()));
//}
