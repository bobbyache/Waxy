using CygX1.Waxy.Http;
using NUnit.Framework;
using System.Drawing;
using System.Drawing.Imaging;

namespace TcpGrabImage.UnitTests
{
    [TestFixture]
    public class ImageRequesterTests
    {
        [Test]
        [Category("Integration")]
        public void ImageRequester_DownloadSimpleImage()
        {
            ImageRequester imageRequester = new ImageRequester();
            Image img = imageRequester.Fetch();
            img.Save(@"C:\Users\robertb\Documents\Work\test.png", ImageFormat.Png);
            Assert.IsNotNull(img);
        }
    }
}
