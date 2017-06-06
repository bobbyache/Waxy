using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http
{
    class ImageFormatResolver
    {
        // https://stackoverflow.com/questions/18543518/using-a-webclient-to-save-an-image-with-the-appropriate-extension
        //private HttpResponseMessage response;

        //public ImageFormatResolver(HttpResponseMessage response)
        //{
        //    this.response = response;
        //}

        public static string ResolveExtension(string contentType)
        {
            string extension = ".jpg";
            //string rawFileType = contentType; response.Content.Headers.ContentType.ToString(); // response.Headers.Where(h => h.Key == "ContentType").Single().Value.First();

            switch (contentType)
            {
                case "image/jpeg":
                    extension = ".jpg";
                    break;
                default:
                    extension = "." + contentType.Substring(contentType.IndexOf('/') + 1);
                    break;
            }

            return extension;
        }
    }
}
