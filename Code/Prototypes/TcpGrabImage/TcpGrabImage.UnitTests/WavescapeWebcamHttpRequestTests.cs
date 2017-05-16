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


namespace TcpGrabImage.UnitTests
{

    /*
* Waxy - Regex to filter out hyperlinks or anchor tags or hyperlinks with image targets. 
* Need this Regex as a setting in case image different. Then can get full link for http... 
* This for pages that require some random code to stop linking from another site. Otherwise can just manually supply link. 
* Then need template http request with key-value pairs for replacing params in the order declared in the file. A file for each http request. 
* Wrap Http request up completely so can test as mock.
*/

    [TestFixture]
    public class WavescapeWebcamHttpRequestTests
    {
        [Test]
        public void GetImageFromHyperlink()
        {
            SimpleWebCam simpleWebCam = new SimpleWebCam();
            Image img = simpleWebCam.Fetch();
            img.Save(@"C:\Users\robertb\Documents\Work\test.png", ImageFormat.Png);
            Assert.IsNotNull(img);
        }

        /*
         * Compare Images using some kind of Use sha1 to generate checksum
         * (you may need this to ensure that the image returned is not the "bad image" image sent back to you by
         * some sites when they're down or they reject your request.
         * https://www.youtube.com/watch?v=oJpZ5ygg4qQ
         * https://www.codeproject.com/Articles/38951/How-To-Hash-Data-Using-MD-and-SHA
         * */
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
