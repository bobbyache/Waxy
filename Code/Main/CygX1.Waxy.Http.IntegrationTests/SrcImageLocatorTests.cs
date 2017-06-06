using NUnit.Framework;

namespace CygX1.Waxy.Http.IntegrationTests
{
    [TestFixture]
    [Category("Slow Integration Files")]
    class SrcImageLocatorTests
    {
        // Example Url/Images
        // http://sat.greatstock.co.za/SearchResults.aspx?tmid=5&uid=201766173852894590
        // http://sat.greatstock.co.za/preview.aspx?ci=29&uid=201766173852894590

        // Images with the appropriate extension
        // https://stackoverflow.com/questions/18543518/using-a-webclient-to-save-an-image-with-the-appropriate-extension

        // https://stackoverflow.com/questions/24177403/net-webclient-downloaddata-get-file-type
        //HttpClient client = new HttpClient();
        //var response = await client.GetAsync("https://encrypted-tbn2.gstatic.com/images?q=tbn%3aANd9GcTw4P3HxyHR8wumE3lY3TOlGworijj2U2DawhY9wnmcPKnbmGHg");
        //var filetype = response.Content.Headers.ContentType.MediaType;
        //var imageArray = await response.Content.ReadAsByteArrayAsync();

        // Do HttpClient and HttpClientHandler have to be disposed?
        // https://stackoverflow.com/questions/15705092/do-httpclient-and-httpclienthandler-have-to-be-disposed

        // Is HttpClient safe to use concurrently?
        // https://stackoverflow.com/questions/11178220/is-httpclient-safe-to-use-concurrently

        // What is the overhead of creating a new HttpClient per call in a WebAPI client?
        // https://stackoverflow.com/questions/22560971/what-is-the-overhead-of-creating-a-new-httpclient-per-call-in-a-webapi-client


        [Test]
        public void SrcImageLocator_GetFirstMatch_ReturnsCorrectHyperlink_OnPartialFakeLandingPage()
        {
            string regEx = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]*";
            string html = TxtFile.ReadText(@"Files\ScrapeHtml\BigBay_LandingPage_Partial.txt");

            string expectedSrc = @"https://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg&rnd=221650030";
            SrcImageLocator imageLocator = new SrcImageLocator("", html, regEx);
            string hyperlink = imageLocator.GetFirstMatch();

            Assert.AreEqual(expectedSrc, hyperlink);
        }

        [Test]
        public void SrcImageLocator_GetFirstMatch_ReturnsCorrectHyperlink_OnFullFakeLandingPage()
        {
            string regEx = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]*";
            string html = TxtFile.ReadText(@"Files\ScrapeHtml\BigBay_LandingPage_Full.txt");

            string expectedSrc = @"https://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg&rnd=221650030";
            SrcImageLocator imageLocator = new SrcImageLocator("", html, regEx);
            string hyperlink = imageLocator.GetFirstMatch();

            Assert.AreEqual(expectedSrc, hyperlink);
        }

        [Test]
        public void Src_ImageLocator_GetFirstMatch_ReturnsCorrectHyperlink_FromRelativeUrl_UsingReferer()
        {
            string refererUrl = @"http://sat.greatstock.co.za/preview.aspx?ci=29&uid=201766173852894590";
            string regEx = @"comps/[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
            string html = "<img id=\"imgAsset\" src=\"comps/SAT-000-1179G.jpg\" />";

            SrcImageLocator imageLocator = new SrcImageLocator(refererUrl, html, regEx);
            string hyperlink = imageLocator.GetFirstMatch();

            Assert.AreEqual(@"http://sat.greatstock.co.za/comps/SAT-000-1179G.jpg", hyperlink);
        }

        [Test]
        public void Src_ImageLocator_GetFirstMatch_ReturnsCorrectHyperlink_AsFullUrl_WithoutUsingReferer()
        {
            string refererUrl = @"http://sat.greatstock.co.za/preview.aspx?ci=29&uid=201766173852894590";
            string regEx = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
            string html = "<img id=\"imgAsset\" src=\"http://www.hullabaloo.co.za/run/externalimage.jpg\" />";

            SrcImageLocator imageLocator = new SrcImageLocator(refererUrl, html, regEx);
            string hyperlink = imageLocator.GetFirstMatch();

            Assert.AreEqual(@"http://www.hullabaloo.co.za/run/externalimage.jpg", hyperlink);
        }

        [Test]
        public void Src_ImageLocator_GetFirstMatch_ReturnsCorrectHyperlink_IfFullUrlPassed_AndNotARegularExpression()
        {
            string refererUrl = @"http://sat.greatstock.co.za/preview.aspx?ci=29&uid=201766173852894590";
            string fullUrl = @"http://www.hullabaloo.co.za/run/externalimage.jpg";
            string html = "<img id=\"imgAsset\" src=\"http://www.hullabaloo.co.za/run/externalimage.jpg\" />";

            SrcImageLocator imageLocator = new SrcImageLocator(refererUrl, html, fullUrl);
            string hyperlink = imageLocator.GetFirstMatch();

            Assert.AreEqual(@"http://www.hullabaloo.co.za/run/externalimage.jpg", hyperlink);
        }

        [Test]
        public void Src_ImageLocator_GetFirstMatch_ReturnsCorrectHyperlink_IfRelativeUrlPassed_AndNotARegularExpression()
        {
            string refererUrl = @"http://sat.greatstock.co.za/preview.aspx?ci=29&uid=201766173852894590";
            string regEx = @"comps/SAT-000-1179G.jpg";
            string html = "<img id=\"imgAsset\" src=\"comps/SAT-000-1179G.jpg\" />";

            SrcImageLocator imageLocator = new SrcImageLocator(refererUrl, html, regEx);
            string hyperlink = imageLocator.GetFirstMatch();

            Assert.AreEqual(@"http://sat.greatstock.co.za/comps/SAT-000-1179G.jpg", hyperlink);
        }

        [Test]
        public void Src_ImageLocator_GetFirstMatch_CannotCombineToFormAbsoluteUri_Throws_AbsoluteUrlCombineFailureException()
        {
            string refererUrl = @"preview.aspx?ci=29&uid=201766173852894590";
            string regEx = @"comps/SAT-000-1179G.jpg";
            string html = "<img id=\"imgAsset\" src=\"comps/SAT-000-1179G.jpg\" />";

            SrcImageLocator imageLocator = new SrcImageLocator(refererUrl, html, regEx);

            Assert.Throws<Exceptions.AbsoluteUrlCombineFailureException>(() => imageLocator.GetFirstMatch());
        }
    }
}
