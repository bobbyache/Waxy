using CygX1.Waxy.Http.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CygX1.Waxy.Http
{
    /*
     * Make RequestHeader a value object.
     * 
     * Rather go:
     *  textualGetRequest.RequestHeaders.GetRequestHeaders();
     *  textualGetRequest.RequestHeaders["Host"]
     * 
     * Consider decorator actual image request, landing page - referer request.
     * Task with OnCompleted event returns a task... create example and show works with decorator.
     * */
    public class RequestHeader
    {
        internal RequestHeader(string requestLine)
        {
            if(string.IsNullOrWhiteSpace(requestLine) || !requestLine.Contains(":") || requestLine.Contains("GET"))
                throw new InvalidHttpRequestHeader("Invalid request header. Request header string cannot be parsed.");

            int index = requestLine.IndexOf(':', 0);

            this.key = requestLine.Substring(0, index).Trim();
            this.value = requestLine.Substring(index + 1, requestLine.Length - (index + 1)).Trim();
        }

        private readonly string key;
        private readonly string value;

        public string Key { get { return key; } }
        public string Value { get { return value; } }
    }

    public class TextualGetRequest
    {
        // How do I implement IEnumerable<T>
        // http://stackoverflow.com/questions/11296810/how-do-i-implement-ienumerablet

        private string templateText;
        private string requestLine; // this is where you'll do the GET http://... HTTP/1.1
        private IEnumerable<RequestHeader> requestHeaders;

        public TextualGetRequest(string templateText)
        {
            this.templateText = templateText;
            string[] requestHeaderLines = ProcessTemplateText(templateText);
            requestHeaders = CreateRequestHeaders(requestHeaderLines);
        }

        public string this [string key]
        {
            get { return RequestHeaders.Where(r => r.Key == key).SingleOrDefault().Value; }
        }

        public IEnumerable<RequestHeader> RequestHeaders { get { return requestHeaders; } }

        public string Method { get; internal set; }
        public string RequestUri { get; internal set; }
        public string HttpVersion { get; internal set; }

        private string[] ProcessTemplateText(string templateText)
        {
            List<string> requestLineList = new List<string>();

            if (!string.IsNullOrWhiteSpace(templateText))
            {
                using (StringReader stringReader = new StringReader(templateText))
                {
                    string line = null;
                    while ((line = stringReader.ReadLine()) != null)
                    {
                        string currentLine = line.Trim();
                        if (!string.IsNullOrEmpty(currentLine))
                            requestLineList.Add(currentLine);
                    }
                }
                return requestLineList.ToArray();
            }
            return null;
        }

        private IEnumerable<RequestHeader> CreateRequestHeaders(string[] requestHeaderLines)
        {
            return requestHeaderLines.Skip(1)
                             .Where(s => ValidHeaderLine(s))
                             .Select(s => new RequestHeader(s));
        }

        private bool ValidHeaderLine(string requestLine)
        {
            return !string.IsNullOrWhiteSpace(requestLine) && requestLine.Contains(":") && !requestLine.Contains("GET");
        }
    }
}
