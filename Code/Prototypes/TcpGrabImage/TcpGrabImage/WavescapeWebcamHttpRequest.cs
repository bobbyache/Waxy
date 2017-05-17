//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TcpGrabImage
//{
//    public class CygX1.Waxy.Http
//    {
//        private string requestTemplate;

//        public WavescapeWebcamHttpRequest(string requestTemplate, string referer, string url, string randomNumber)
//        {
//            this.requestTemplate = requestTemplate;
//            this.Referer = referer;
//            this.Url = url;
//            this.RandomNumber = randomNumber;
//        }

//        public string Referer { get; }
//        public string RandomNumber { get; }
//        public string Url { get; }

//        public override string ToString()
//        {
//            return string.Format(requestTemplate, this.Url, this.RandomNumber, this.Referer).Trim();
//        }
//    }
//}
