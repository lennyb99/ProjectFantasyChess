using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using UnityEngine;

public class StringCompresser
{
    public static byte[] CompressString(string text)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            using (var writer = new StreamWriter(gzipStream, Encoding.UTF8))
            {
                writer.Write(text);
            }
            return memoryStream.ToArray();
        }
    }

    public static string DecompressString(byte[] compressedData)
    {
        using (var memoryStream = new MemoryStream(compressedData))
        using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }
}
