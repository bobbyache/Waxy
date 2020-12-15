using CygX1.Waxy.Http;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TcpGrabImage.UnitTests
{
    [TestFixture]
    [Category("Slow Integration Downloads")]
    public class ImageRequesterTests
    {
        [Test]
        public void ImageRequester_DownloadSimpleImage()
        {
            SampleImageRequester imageRequester = new SampleImageRequester();
            Image img = imageRequester.Fetch();
            img.Save(@"C:\Users\robertb\Documents\Work\test.png", ImageFormat.Png);
            Assert.IsNotNull(img);
        }
    }

    public class SampleImageRequester
    {
        // http://stackoverflow.com/questions/1053052/a-generic-error-occurred-in-gdi-jpeg-image-to-memorystream
        // http://stackoverflow.com/questions/12022965/adding-http-headers-to-httpclient
        public Image Fetch()
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

            // Check this for other options: https://stackoverflow.com/questions/26958829/how-do-i-use-the-new-httpclient-from-windows-web-http-to-download-an-image
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
