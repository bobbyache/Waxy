using System.Text.RegularExpressions;

namespace CygX1.Waxy.Http
{
    internal class SrcImageLocator
    {
        private string html;
        private string regularExpression;

        public SrcImageLocator(string html, string regularExpression)
        {
            this.regularExpression = regularExpression;
            this.html = html;
        }

        internal string GetFirstMatch()
        {
            Match match = Regex.Match(html, regularExpression);
            return match.ToString();
        }
    }
}