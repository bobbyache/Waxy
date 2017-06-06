using NUnit.Framework;
using System.Drawing;
using System.IO;

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

            Assert.Throws<Exceptions.BadImageUriException>(() => new ImageDownloader(requestTemplateText, imageUrl));
        }

        [Test]
        [Category("Slow Integration Files")]
        public void WavescapeImageDownloader_WhenInitialized_WithRequestUrlToPatternMatch_InitializesSuccessfully()
        {
            string imageUrl = @"http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg&rnd=614786193";
            string requestTemplateText = TxtFile.ReadText(@"Files\HttpRequests\SrcTarget\WavescapeBigBay.txt"); ;

            ImageDownloader imageDownloader = new ImageDownloader(requestTemplateText, imageUrl);
            Assert.AreEqual(imageUrl, imageDownloader.ImageUrl);
        }

        [Test]
        [Category("Sensitive Slow Integration Downloads")]
        public void WavescapeImageDownloader_WhenDownloaded_RetrievesValidImage()
        {
            string scrapeRequestText = TxtFile.ReadText(@"Files\HttpRequests\Landing\WavescapeKommetjie.txt");
            string imageFetchRequestText = TxtFile.ReadText(@"Files\HttpRequests\SrcTarget\WavescapeKommetjie.txt");
            string regEx = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)kom.jpg&rnd=[0-9]*";

            ImagePageScraper scraper = new ImagePageScraper(scrapeRequestText, regEx);
            string imageUrl = scraper.Scrape();

            ImageDownloader imageDownloader = new ImageDownloader(imageFetchRequestText, imageUrl);
            WebImage webImage = imageDownloader.Download();

            Bitmap bitmap = new Bitmap(webImage.Image);
            bitmap.Save(Path.Combine(@"C:\Users\robertb\Documents\Work", webImage.FileName));

            Assert.IsNotNull(bitmap);
        }

        [Test]
        [Category("Slow Integration Downloads")]
        public void GenericImageDownloader_WhenDownloaded_RetrievesValidImageExtension_UsingContentType()
        {
            string scrapeRequestText = TxtFile.ReadText(@"Files\HttpRequests\Landing\SampleImageLandingPage.txt");
            string imageFetchRequestText = TxtFile.ReadText(@"Files\HttpRequests\SrcTarget\SampleImageSrc.txt");
            string regEx = @"comps/[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";

            ImagePageScraper scraper = new ImagePageScraper(scrapeRequestText, regEx);
            string imageUrl = scraper.Scrape();

            ImageDownloader imageDownloader = new ImageDownloader(imageFetchRequestText, imageUrl);
            WebImage webImage = imageDownloader.Download();

            Bitmap bitmap = new Bitmap(webImage.Image);
            bitmap.Save(Path.Combine(@"C:\Users\robertb\Documents\Work", webImage.FileName));

            Assert.IsNotNull(bitmap);
        }
    }
}
