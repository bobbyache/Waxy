using System;
using System.Net.Http;

namespace CygX1.Waxy.Http
{
    public class ImagePageScraper
    {
        private string regEx;
        private TextualGetRequest textualGetRequest = null;

        public ImagePageScraper(string getRequestText, string regEx)
        {
            this.textualGetRequest = new TextualGetRequest(getRequestText);
            this.regEx = regEx;
        }

        internal string Scrape()
        {
            /*
             * So how does this work?
             * 1. A request is made to the landing page.
             * 2. The HTML is retrieved for that page. 
             * 3. SrcImageLocator searches for the target image link in order to target the randomly generated image based on a pattern.
             * 4. Image is downloaded.
             * This is a very focused series of execution... for a specific target.
             * */

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

                SrcImageLocator srcImageLocator = new SrcImageLocator(textualGetRequest["Referer"], txt, this.regEx);
                src = srcImageLocator.GetFirstMatch();
            });
            task.Wait();

            return src;
        }
    }
}
