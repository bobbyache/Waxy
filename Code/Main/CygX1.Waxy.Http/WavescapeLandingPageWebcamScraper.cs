using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http
{
    public class WavescapeLandingPageWebcamScraper
    {
        private string regEx;
        private TextualGetRequest textualGetRequest = null;

        public WavescapeLandingPageWebcamScraper(string getRequestText, string regEx)
        {
            this.textualGetRequest = new TextualGetRequest(getRequestText);
            this.regEx = regEx;
        }

        internal string Scrape()
        {
            // decompress what is returned... this might not be needed... investigate what should happen if we don't need it.
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

            var client = new HttpClient(handler);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(this.textualGetRequest.RequestUri),
                Method = HttpMethod.Get,
                Version = this.textualGetRequest.HttpVersion
            };

            foreach (TextualGetRequest.RequestHeader header in this.textualGetRequest.RequestHeaders)
                request.Headers.Add(header.Key, header.Value);

            string src = null;
            var task = client.SendAsync(request)
            .ContinueWith((responseTask) =>
            {
                var response = responseTask.Result;
                string txt = response.Content.ReadAsStringAsync().Result;

                SrcImageLocator srcImageLocator = new SrcImageLocator(txt, this.regEx);
                src = srcImageLocator.GetFirstMatch();
            });
            task.Wait();

            return src;
        }
    }
}
