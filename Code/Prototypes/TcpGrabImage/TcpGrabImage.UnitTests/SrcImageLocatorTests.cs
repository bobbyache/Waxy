using NUnit.Framework;

namespace CygX1.Waxy.Http.IntegrationTests
{
    [TestFixture]
    [Category("Image Locator")]
    class SrcImageLocatorTests
    {
        [Test]
        public void SrcImageLocator_GetFirstMatch_ReturnsCorrectHyperlink_OnPartialFakeLandingPage()
        {
            string regEx = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]*";
            string html = TxtFile.ReadText("FakeLandingPage_Partial.txt");

            string expectedSrc = @"https://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg&rnd=221650030";
            SrcImageLocator imageLocator = new SrcImageLocator(html, regEx);
            string hyperlink = imageLocator.GetFirstMatch();

            Assert.AreEqual(expectedSrc, hyperlink);
        }
    }
}
