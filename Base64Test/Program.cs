using System;
using System.Linq;
using System.Text;
using Base64;

namespace Base64Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // "ABCDEFG" -> "QUJDREVGRw=="
            var encoded = Base64Encoder.Encode("ABCDEFG", Encoding.ASCII).ToArray();

            // 4 文字ずつ区切って出してみる
            var formatted = string.Join(string.Empty, encoded.Select((v, i) => ((i + 1) % 4 == 0) ? $"{v} " : $"{v}"));
            Console.WriteLine(formatted);

            Console.WriteLine(Base64Encoder.Decode(encoded, Encoding.ASCII));
            Console.Read();
        }
    }
}