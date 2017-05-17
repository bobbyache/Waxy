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
     * Decouple file retrieval from TextualGetRequest.
     * 
     * Rather go:
     *  textualGetRequest.RequestHeaders.GetRequestHeaders();
     *  textualGetRequest.RequestHeaders["Host"]
     * 
     * */
    public class RequestHeader
    {
        public RequestHeader(string requestLine)
        {
            if(!string.IsNullOrWhiteSpace(requestLine) || requestLine.Contains(":") || !requestLine.Contains("GET"))
                throw new InvalidHttpRequestHeader("Invalid request header. Request header string cannot be parsed.");

            int index = requestLine.IndexOf(':', 0);

            if (index < 0)
                throw new InvalidHttpRequestHeader("Invalid request header. Request header string cannot be parsed.");

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

        private string filePath;
        private string[] requestLines;

        public TextualGetRequest(string filePath)
        {
            this.filePath = filePath;
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
                    requestLines = ReadRequestFile(filePath);

                IEnumerable<RequestHeader> requestHeaders = requestLines.Skip(1)
                                             .Where(s => ValidHeaderLine(s))
                                             .Select(s => new RequestHeader(s));

                return requestHeaders;
            }
        }

        private string[] ReadRequestFile(string filePath)
        {
            List<string> requestLineList = new List<string>();

            if (File.Exists(filePath))
            {
                using (StreamReader streamReader = File.OpenText(filePath))
                {
                    string input = null;
                    while ((input = streamReader.ReadLine()) != null)
                    {
                        requestLineList.Add(input);
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
