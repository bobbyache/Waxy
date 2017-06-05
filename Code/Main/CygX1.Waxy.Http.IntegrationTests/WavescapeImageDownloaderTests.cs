using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http.IntegrationTests
{
    [TestFixture]
    
    class WavescapeImageDownloaderTests
    {
        [Test]
        [Category("Slow Integration Files")]
        public void WavescapeImageDownloader_WhenInitialized_WithRequestUrlToPatternMismatch_Throws_BadImageUriException()
        {
            string imageUrl = @"http://www.testdomain.co.za/plugins/content/webcam/newfetch.php?pic=muizies.jpg&rnd=614786193";
            string requestTemplateText = TxtFile.ReadText(@"Files\HttpRequests\SrcTarget\WavescapeBigBay.txt"); ;

            Assert.Throws<Exceptions.BadImageUriException>(() => new WavescapeImageDownloader(requestTemplateText, imageUrl));
        }

        [Test]
        [Category("Slow Integration Files")]
        public void WavescapeImageDownloader_WhenInitialized_WithRequestUrlToPatternMatch_InitializesSuccessfully()
        {
            string imageUrl = @"http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg&rnd=614786193";
            string requestTemplateText = TxtFile.ReadText(@"Files\HttpRequests\SrcTarget\WavescapeBigBay.txt"); ;

            WavescapeImageDownloader imageDownloader = new WavescapeImageDownloader(requestTemplateText, imageUrl);
            Assert.AreEqual(imageUrl, imageDownloader.ImageUrl);
        }

        [Test]
        [Category("Sensitive Slow Integration Downloads")]
        public void WavescapeImageDownloader_WhenDownloaded_RetrievesValidImage()
        {
            string scrapeRequestText = TxtFile.ReadText(@"Files\HttpRequests\Landing\WavescapeKommetjie.txt");
            string imageFetchRequestText = TxtFile.ReadText(@"Files\HttpRequests\SrcTarget\WavescapeKommetjie.txt");
            string regEx = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)kom.jpg&rnd=[0-9]*";

            WavescapeLandingPageWebcamScraper scraper = new WavescapeLandingPageWebcamScraper(scrapeRequestText, regEx);
            string imageUrl = scraper.Scrape();

            WavescapeImageDownloader downloader = new WavescapeImageDownloader(imageFetchRequestText, imageUrl);
            Image image = downloader.Download();

            Bitmap bitmap = new Bitmap(image);

            bitmap.Save(@"C:\Users\robertb\Documents\Work\test.jpg", ImageFormat.Jpeg);

            Assert.IsNotNull(bitmap);
        }
    }
}
