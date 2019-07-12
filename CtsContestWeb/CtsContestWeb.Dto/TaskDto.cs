using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;

namespace CtsContestWeb.Dto
{
    public class TaskDto : TaskDisplayDto
    {
        [JsonIgnore]
        public List<string> Inputs { get; set; }
        [JsonIgnore]
        public List<string> Outputs { get; set; }

        public string InputType { get; set; }
        public bool IsForDuel { get; set; }
    }

    public static class StringGZipCompressor
    {
        public static string DecompressString(this byte[] data)
        {
            // Read the last 4 bytes to get the length
            byte[] lengthBuffer = new byte[4];
            Array.Copy(data, data.Length - 4, lengthBuffer, 0, 4);
            int uncompressedSize = BitConverter.ToInt32(lengthBuffer, 0);

            var buffer = new byte[uncompressedSize];
            using (var ms = new MemoryStream(data))
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    gzip.Read(buffer, 0, uncompressedSize);
                }
            }
            return Encoding.Unicode.GetString(buffer);
        }
    }
}
