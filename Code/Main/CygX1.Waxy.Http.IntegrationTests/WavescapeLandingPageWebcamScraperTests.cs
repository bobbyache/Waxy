using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http.IntegrationTests
{
    [TestFixture]
    [Category("Slow Integration Downloads")]
    class WavescapeLandingPageWebcamScraperTests
    {
        [Test]
        public void WavescapeLandingPageWebcamScraper_Scrape_MuizenbergCorner()
        {
            string requestText = TxtFile.ReadText("HTTP_GET_BigBay.txt");
            string regEx = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)mbcorner.jpg&rnd=[0-9]*";
            WavescapeLandingPageWebcamScraper scraper = new WavescapeLandingPageWebcamScraper(requestText, regEx);
            string imageUrl = scraper.Scrape();
            Assert.IsTrue(imageUrl.Contains("rnd="));
            Assert.IsTrue(imageUrl.Contains("mbcorner.jpg"));
        }

        [Test]
        public void WavescapeLandingPageWebcamScraper_Scrape_Kommetjie()
        {
            string requestText = TxtFile.ReadText(@"Landing\WavescapeKommetjie.txt");
            string regEx = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)kom.jpg&rnd=[0-9]*";
            WavescapeLandingPageWebcamScraper scraper = new WavescapeLandingPageWebcamScraper(requestText, regEx);
            string imageUrl = scraper.Scrape();
            Assert.IsTrue(imageUrl.Contains("rnd="));
            Assert.IsTrue(imageUrl.Contains("kom.jpg"));
        }
    }
}
