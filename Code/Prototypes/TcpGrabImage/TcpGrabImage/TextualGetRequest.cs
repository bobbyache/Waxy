using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygX1.Waxy.Http
{
    public class RequestHeader
    {
        public string Key { get; internal set; }
        public string Value { get; internal set; }
    }

    public class TextualGetRequest : IEnumerable<RequestHeader>
    {
        // How do I implement IEnumerable<T>
        // http://stackoverflow.com/questions/11296810/how-do-i-implement-ienumerablet
        private string filePath;

        public TextualGetRequest(string filePath)
        {
            this.filePath = filePath;
        }
        
        public IEnumerator<RequestHeader> GetEnumerator()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader streamReader = File.OpenText(filePath))
                {
                    string input = null;
                    while ((input = streamReader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(input) && input.Contains(":") && !input.Contains("GET"))
                        {
                            int index = input.IndexOf(':', 0);
                            string key = input.Substring(0, index);
                            string value = input.Substring(index + 1, input.Length - (index + 1));

                            RequestHeader requestHeader = new RequestHeader();
                            requestHeader.Key = key.Trim();
                            requestHeader.Value = value.Trim();
                            yield return requestHeader;
                        }
                    }
                }
                yield break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
