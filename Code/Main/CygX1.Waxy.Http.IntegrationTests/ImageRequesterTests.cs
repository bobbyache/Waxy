using CygX1.Waxy.Http;
using NUnit.Framework;
using System.Drawing;
using System.Drawing.Imaging;

namespace TcpGrabImage.UnitTests
{
    [TestFixture]
    [Category("Slow Integration Downloads")]
    public class ImageRequesterTests
    {
        [Test]
        public void ImageRequester_DownloadSimpleImage()
        {
            ImageRequester imageRequester = new ImageRequester();
            Image img = imageRequester.Fetch();
            img.Save(@"C:\Users\robertb\Documents\Work\test.png", ImageFormat.Png);
            Assert.IsNotNull(img);
        }
    }
}
