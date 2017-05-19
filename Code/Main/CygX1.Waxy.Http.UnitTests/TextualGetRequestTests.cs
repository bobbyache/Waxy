using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http.UnitTests
{
    [TestFixture]
    [Category("Fast Units")]
    public class TextualGetRequestTests
    {
        [Test]
        public void TextualGetRequest_ParseGetHeaderText_WithDirectLink_ToBigBay_IsParsedCorrectly()
        {
            string requestText = GetTextualHttpGetRequest(
                host: @"www.wavescape.co.za",
                requestUri: @"http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg&tmpl=component&rnd=614786193",
                refererUri: @"http://www.wavescape.co.za/tools/webcams/big-bay.html"
                );

            TextualGetRequest textualGetRequest = new TextualGetRequest(requestText);

            Assert.AreEqual("GET", textualGetRequest.Method);
            Assert.AreEqual("http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=bigbay.jpg&tmpl=component&rnd=614786193", textualGetRequest.RequestUri);
            Assert.AreEqual("HTTP/1.1", textualGetRequest.HttpVersion);
        }

        [Test]
        public void TextualGetRequest_ParseGetHeaderText_WithDirectLink_ToTheHoek_IsParsedCorrectly()
        {
            string requestText = GetTextualHttpGetRequest(
                host: @"www.wavescape.co.za",
                requestUri: @"http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=hoek.jpg&rnd=245430611",
                refererUri: @"http://www.brutube.co.za/tools/webcams/noordhoek.html"
                );
            // GET HTTP request specification.
            // https://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html
            TextualGetRequest textualGetRequest = new TextualGetRequest(requestText);

            Assert.AreEqual("GET", textualGetRequest.Method);
            Assert.AreEqual("http://www.wavescape.co.za/plugins/content/webcam/newfetch.php?pic=hoek.jpg&rnd=245430611", textualGetRequest.RequestUri);
            Assert.AreEqual("HTTP/1.1", textualGetRequest.HttpVersion);
        }

        [Test]
        public void TextualGetRequest_Parse_RequestUiPattern_Placeholder_ContainsSpaces_Throws_BadRequestUriPatternException()
        {
            string requestText = GetTextualHttpGetRequest(
                host: @"www.wavescape.co.za",
                requestUri: @"{{ https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]* }}",
                refererUri: @"http://www.brutube.co.za/tools/webcams/noordhoek.html"
                );

            Assert.Throws<Exceptions.BadRequestMethodLineException>(() => new TextualGetRequest(requestText));

            //// GET HTTP request specification.
            //// https://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html
            //TextualGetRequest textualGetRequest = new TextualGetRequest(requestText);

            //Assert.AreEqual("GET", textualGetRequest.Method);
            //Assert.AreEqual(@"{{ https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]* }}", textualGetRequest.RequestUri);
            //Assert.AreEqual("HTTP/1.1", textualGetRequest.HttpVersion);
        }

        [Test]
        // TextualGetRequest_Parse_RequestUiPattern_Placeholder_ContainsNoSpaces_ParsesCorrectly
        // TextualGetRequest_Parse_RequestUiPattern_Placeholder_ContainsSpaces_ParsesCorrectly
        public void TextualGetRequest_Parse_RequestUiPattern_Placeholder_ContainsNoSpaces_ParsesCorrectly()
        {
            string requestText = GetTextualHttpGetRequest(
                host: @"www.wavescape.co.za",
                requestUri: @"{{https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]*}}",
                refererUri: @"http://www.brutube.co.za/tools/webcams/noordhoek.html"
                );
            // GET HTTP request specification.
            // https://www.w3.org/Protocols/rfc2616/rfc2616-sec5.html
            TextualGetRequest textualGetRequest = new TextualGetRequest(requestText);

            Assert.AreEqual("GET", textualGetRequest.Method);
            Assert.AreEqual(@"{{https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]*}}", textualGetRequest.RequestUri);
            Assert.AreEqual("HTTP/1.1", textualGetRequest.HttpVersion);
        }

        [Test]
        public void TextualGetRequest_Parse_RequestUiPattern_Placeholder_NoPlaceholderPrefix_Throws_BadRequestUriPatternException()
        {
            string requestText = GetTextualHttpGetRequest(
                host: @"www.wavescape.co.za",
                requestUri: @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]*}}",
                refererUri: @"http://www.brutube.co.za/tools/webcams/noordhoek.html"
                );

            Assert.Throws<Exceptions.BadRequestUriPatternException>(() => new TextualGetRequest(requestText));
        }

        [Test]
        public void TextualGetRequest_Parse_RequestUiPattern_Placeholder_NoPlaceholderPostfix_Throws_BadRequestUriPatternException()
        {
            string requestText = GetTextualHttpGetRequest(
                host: @"www.wavescape.co.za",
                requestUri: @"{{https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)bigbay.jpg&rnd=[0-9]*",
                refererUri: @"http://www.brutube.co.za/tools/webcams/noordhoek.html"
                );

            Assert.Throws<Exceptions.BadRequestUriPatternException>(() => new TextualGetRequest(requestText));
        }

        [Test]
        public void TextualGetRequest_Parse_RequestUiPattern_Placeholder_NoPlaceholderBody_Throws_BadRequestMethodLineException()
        {
            string requestText = GetTextualHttpGetRequest(
                host: @"www.wavescape.co.za",
                requestUri: @"{{ }}",
                refererUri: @"http://www.brutube.co.za/tools/webcams/noordhoek.html"
                );

            Assert.Throws<Exceptions.BadRequestMethodLineException>(() => new TextualGetRequest(requestText));
        }

        [Test]
        public void TextualGetRequest_Parse_MethodLine_WithoutRequestUri_Throws_BadRequestMethodLineException()
        {
            string requestText = GetTextualHttpGetRequest(
                host: @"www.wavescape.co.za",
                requestUri: @"",
                refererUri: @"http://www.brutube.co.za/tools/webcams/noordhoek.html"
                );

            Assert.Throws<Exceptions.BadRequestMethodLineException>(() => new TextualGetRequest(requestText));
        }

        private string GetTextualHttpGetRequest(string host, string requestUri, string refererUri)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("GET {0} HTTP/1.1", requestUri));

            builder.AppendLine(string.Format("Host: {0}", host));
            builder.AppendLine(@"User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");
            builder.AppendLine(@"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            builder.AppendLine(string.Format("Referer: {0}", refererUri));
            builder.AppendLine(@"Accept-Encoding: gzip, deflate, sdch");
            builder.AppendLine(@"Accept-Language: en-US,en;q=0.8");
            builder.AppendLine(@""); // Empty line according to specification...

            return builder.ToString();
        }
    }
}
