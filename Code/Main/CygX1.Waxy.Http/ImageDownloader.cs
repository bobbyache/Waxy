using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http
{
    class ImageDownloader
    {
        private string imageUrl;
        private TextualGetRequest textualGetRequest = null;

        public ImageDownloader(string requestTemplateText, string imageUrl)
        {
            
            this.textualGetRequest = new TextualGetRequest(requestTemplateText);

            string pattern = textualGetRequest.RequestUri.Replace("{{", "").Replace("}}", "");
            SrcImageLocator imageLocator = new SrcImageLocator(textualGetRequest["Referer"], imageUrl, pattern);
            string checkImageUrl = imageLocator.GetFirstMatch();
            if (checkImageUrl != imageUrl)
                throw new Exceptions.BadImageUriException("Image URL does not match the image URL template pattern.");

            this.textualGetRequest.RequestUri = imageUrl;
            this.imageUrl = imageUrl;
        }

        public object ImageUrl { get { return this.imageUrl; } }

        public WebImage Download()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(this.textualGetRequest.RequestUri),
                Method = HttpMethod.Get,
                Version = this.textualGetRequest.HttpVersion
            };

            foreach (TextualGetRequest.RequestHeader header in this.textualGetRequest.RequestHeaders)
                request.Headers.Add(header.Key, header.Value);

            Image img = null;
            string extension = null;

            // Check this for other options: https://stackoverflow.com/questions/26958829/how-do-i-use-the-new-httpclient-from-windows-web-http-to-download-an-image
            var task = client.SendAsync(request)
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    extension = ImageFormatResolver.ResolveExtension(response.Content.Headers.ContentType.ToString());

                    Task<byte[]> taskByteArray = response.Content.ReadAsByteArrayAsync();
                    taskByteArray.Wait();

                    using (MemoryStream memoryStream = new MemoryStream(taskByteArray.Result))
                    {
                        img = Image.FromStream(memoryStream);
                    }
                });
            task.Wait();

            return new WebImage(img, extension, DateTime.Now);
        }
    }
}
