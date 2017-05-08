using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpGrabImage
{
    public class ImageGrabber
    {
        public static async Task<Image> HttpRequestAsync(string request)
        {
            Image result = null;
            using (TcpClient tcpClient = new TcpClient())
            {
                //tcpClient.SendTimeout = 500;
                //tcpClient.ReceiveTimeout = 1000;

                await tcpClient.ConnectAsync("www.wavescape.co.za", 80);
                //tcpClient.Connect()
                if (tcpClient.Connected)
                {
                    using (NetworkStream stream = tcpClient.GetStream())
                    {
                        // Send request headers
                        var builder = new StringBuilder();
                        builder.Append(request);
                        builder.AppendLine();
                        var header = Encoding.ASCII.GetBytes(builder.ToString());
                        await stream.WriteAsync(header, 0, header.Length);

                        // Send payload data if you are POST request
                        //await stream.WriteAsync(data, 0, data.Length);

                        // receive data
                        using (var memory = new MemoryStream())
                        {
                            await stream.CopyToAsync(memory);
                            memory.Position = 0;
                            var data = memory.ToArray();

                            var index = BinaryMatch(data, Encoding.ASCII.GetBytes("\r\n\r\n")) + 4;
                            var headers = Encoding.ASCII.GetString(data, 0, index);
                            memory.Position = index;

                            if (headers.IndexOf("Content-Encoding: gzip") > 0)
                            {
                                using (GZipStream decompressionStream = new GZipStream(memory, CompressionMode.Decompress))
                                using (var decompressedMemory = new MemoryStream())
                                {
                                    decompressionStream.CopyTo(decompressedMemory);
                                    decompressedMemory.Position = 0;
                                    result = Image.FromStream(decompressedMemory);
                                }
                            }
                            else
                            {
                                result = Image.FromStream(memory);
                            }
                        }
                    }
                }
                return result;
            }
        }

        private static int BinaryMatch(byte[] input, byte[] pattern)
        {
            int sLen = input.Length - pattern.Length + 1;
            for (int i = 0; i < sLen; ++i)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; ++j)
                {
                    if (input[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
