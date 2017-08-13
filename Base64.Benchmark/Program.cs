using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Base64.Benchmark
{
    public class SystemConvertVsMyBase64Encoder
    {
        private const int N = 12345;

        private readonly IEnumerable<byte> _bytes;
        private readonly byte[] _byteArray;
        private readonly IEnumerable<char> _chars;
        private readonly char[] _charArray;
        private readonly string _str;
        private readonly Encoding Ascii = Encoding.ASCII;

        public SystemConvertVsMyBase64Encoder()
        {
            _byteArray = new byte[N];
            new Random().NextBytes(_byteArray);

            _bytes = _byteArray;

            _str = Encoding.ASCII.GetString(_byteArray);
            _chars = _str.AsEnumerable();
            _charArray = _str.ToCharArray();
        }

        [Benchmark]
        public string SerializeBytesSystem() => System.Convert.ToBase64String(_bytes.ToArray());

        [Benchmark]
        public string SerializeBytesMy() => new string(Base64Encoder.Encode(_bytes).ToArray());

        [Benchmark]
        public char[] SerializeBytesMy2() => Base64Encoder.Encode(_bytes).ToArray();

        [Benchmark]
        public string SerializeByteArraySystem() => System.Convert.ToBase64String(_byteArray);

        [Benchmark]
        public string SerializeByteArrayMy() => new string(Base64Encoder.Encode(_byteArray).ToArray());

        [Benchmark]
        public char[] SerializeByteArrayMy2() => Base64Encoder.Encode(_byteArray).ToArray();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SystemConvertVsMyBase64Encoder>();
        }
    }
}