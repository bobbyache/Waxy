using System; 
using System.Text.RegularExpressions;

namespace CygX1.Waxy.Http
{
    internal class SrcImageLocator
    {
        private string html;
        private string regularExpression;
        private string refererUrl;

        public SrcImageLocator(string refererUrl, string html, string regularExpression)
        {
            this.regularExpression = regularExpression;
            this.html = html;
            this.refererUrl = refererUrl;
        }

        internal string GetFirstMatch()
        {
            // https://stackoverflow.com/questions/2201171/determine-if-a-url-is-absolute-or-relative-from-vb
            // https://stackoverflow.com/questions/372865/path-combine-for-urls
            Match match = Regex.Match(html, regularExpression);
            string url = match.ToString();
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return url;
            else
            {
                return CreateFullUrlFrom(refererUrl, url);
            }
        }

        private string CreateFullUrlFrom(string refererUrl, string relativePath)
        {
            const string exceptionMessage = "Failed to combine the referer URL with the located relative path to the image";
            try
            {
                Uri result = null;
                if (Uri.TryCreate(new Uri(refererUrl), relativePath, out result))
                    return result.AbsoluteUri;
                else
                    throw new Exceptions.AbsoluteUrlCombineFailureException($"{ exceptionMessage }." );
            }
            catch (UriFormatException uriEx)
            {
                throw new Exceptions.AbsoluteUrlCombineFailureException($"{ exceptionMessage }. The referer url appears to be malformed.", uriEx);
            }
            catch (Exception ex)
            {
                throw new Exceptions.AbsoluteUrlCombineFailureException($"{ exceptionMessage }. The specific reason for the failure is unknown.", ex);
            }
        }
    }
}