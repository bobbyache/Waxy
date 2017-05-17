//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;

//namespace CygX1.Waxy.Http
//{

//    public class ImageGrabber
//    {




//        public void Test()
//        {
//            IEnumerable<string> lines = File.ReadLines("WaveScape_LandingPage_BigBay.txt");
//            IEnumerable<string> httpHeaders = lines.Skip(1);

//            WebClient webClient = new WebClient();
//            Dictionary<string, string> dictionary = new Dictionary<string, string>();

//            foreach (string line in httpHeaders)
//            {
//                string[] keyVals = line.Split(new char[] { ':' }, StringSplitOptions.None);
//                webClient.Headers.Add(keyVals[0].Trim(), keyVals[1].Trim());
//                dictionary.Add(keyVals[0].Trim(), keyVals[1].Trim());
//            }


//            //WebClient webClient = new WebClient();
//            //webClient.Headers.Add(HttpRequestHeader.Host, "www.wavescape.co.za");
//            //webClient.Headers.Add(HttpRequestHeader.Connection, "keep-alive");
//            //webClient.Headers.Add("Upgrade-Insecure-Requests", "1");
//            //webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");
//            //webClient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
//            //webClient.Headers.Add(HttpRequestHeader.Referer, "http://www.wavescape.co.za/tools/webcams/muizenberg-2.html");
//            //webClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch");
//            //webClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");


//        }

//        //public static async Task<Image> HttpRequestAsync(string request)
//        //{
//        //    Image result = null;
//        //    using (TcpClient tcpClient = new TcpClient())
//        //    {
//        //        //tcpClient.SendTimeout = 500;
//        //        //tcpClient.ReceiveTimeout = 1000;

//        //        await tcpClient.ConnectAsync("www.wavescape.co.za", 80);
//        //        //tcpClient.Connect()
//        //        if (tcpClient.Connected)
//        //        {
//        //            using (NetworkStream stream = tcpClient.GetStream())
//        //            {
//        //                // Send request headers
//        //                var builder = new StringBuilder();
//        //                builder.Append(request);
//        //                builder.AppendLine();
//        //                var header = Encoding.ASCII.GetBytes(builder.ToString());
//        //                await stream.WriteAsync(header, 0, header.Length);

//        //                // Send payload data if you are POST request
//        //                //await stream.WriteAsync(data, 0, data.Length);

//        //                // receive data
//        //                using (var memory = new MemoryStream())
//        //                {
//        //                    await stream.CopyToAsync(memory);
//        //                    memory.Position = 0;
//        //                    var data = memory.ToArray();

//        //                    var index = BinaryMatch(data, Encoding.ASCII.GetBytes("\r\n\r\n")) + 4;
//        //                    var headers = Encoding.ASCII.GetString(data, 0, index);
//        //                    memory.Position = index;

//        //                    if (headers.IndexOf("Content-Encoding: gzip") > 0)
//        //                    {
//        //                        using (GZipStream decompressionStream = new GZipStream(memory, CompressionMode.Decompress))
//        //                        using (var decompressedMemory = new MemoryStream())
//        //                        {
//        //                            decompressionStream.CopyTo(decompressedMemory);
//        //                            decompressedMemory.Position = 0;
//        //                            result = Image.FromStream(decompressedMemory);
//        //                        }
//        //                    }
//        //                    else
//        //                    {
//        //                        result = Image.FromStream(memory);
//        //                    }
//        //                }
//        //            }
//        //        }
//        //        return result;
//        //    }
//        //}

//        //private static int BinaryMatch(byte[] input, byte[] pattern)
//        //{
//        //    int sLen = input.Length - pattern.Length + 1;
//        //    for (int i = 0; i < sLen; ++i)
//        //    {
//        //        bool match = true;
//        //        for (int j = 0; j < pattern.Length; ++j)
//        //        {
//        //            if (input[i + j] != pattern[j])
//        //            {
//        //                match = false;
//        //                break;
//        //            }
//        //        }
//        //        if (match)
//        //        {
//        //            return i;
//        //        }
//        //    }
//        //    return -1;
//        //}
//    }
//}



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
