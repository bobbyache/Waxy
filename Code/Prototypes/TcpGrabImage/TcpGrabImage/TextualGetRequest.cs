using CygX1.Waxy.Http.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            string key = requestLine.Substring(0, index);
            string value = requestLine.Substring(index + 1, requestLine.Length - (index + 1));

            this.Key = key.Trim();
            this.Value = value.Trim();
        }

        public string Key { get; internal set; }
        public string Value { get; internal set; }
    }

    public class TextualGetRequest
    {
        // How do I implement IEnumerable<T>
        // http://stackoverflow.com/questions/11296810/how-do-i-implement-ienumerablet

        private string templateText;
        private string[] requestLines;

        public TextualGetRequest(string templateText)
        {
            this.templateText = templateText;
        }

        public string this [string key]
        {
            get
            {
                return RequestHeaders.Where(r => r.Key == key).SingleOrDefault().Value;
            }
        }

        public IEnumerable<RequestHeader> RequestHeaders
        {
            get
            {
                if (requestLines == null)
                    requestLines = ProcessTemplateText(templateText);

                IEnumerable<RequestHeader> requestHeaders = requestLines.Skip(1)
                                             .Where(s => ValidHeaderLine(s))
                                             .Select(s => new RequestHeader(s));

                return requestHeaders;
            }
        }

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

        private bool ValidHeaderLine(string requestLine)
        {
            return !string.IsNullOrWhiteSpace(requestLine) && requestLine.Contains(":") && !requestLine.Contains("GET");
        }
    }
}
