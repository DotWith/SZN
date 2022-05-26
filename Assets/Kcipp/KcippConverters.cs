using System;
using System.IO;
using System.IO.Compression;

namespace Kcipp
{
    public class KcippConverters
    {
        public static byte[] ToByteArray(float[] floatArray)
        {
            int len = floatArray.Length * 4;
            byte[] byteArray = new byte[len];
            int pos = 0;
            foreach (float f in floatArray)
            {
                byte[] data = BitConverter.GetBytes(f);
                Array.Copy(data, 0, byteArray, pos, 4);
                pos += 4;
            }
            return byteArray;
        }

        public static float[] ToFloatArray(byte[] byteArray)
        {
            int len = byteArray.Length / 4;
            float[] floatArray = new float[len];
            for (int i = 0; i < byteArray.Length; i += 4)
            {
                floatArray[i / 4] = BitConverter.ToSingle(byteArray, i);
            }
            return floatArray;
        }

        public static byte[] Compress(byte[] uncompressed)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(uncompressed, 0, uncompressed.Length);
                return compressedStream.ToArray();
            }
        }

        public static byte[] Decompress(byte[] compressed)
        {
            using (var compressedStream = new MemoryStream(compressed))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }
}
