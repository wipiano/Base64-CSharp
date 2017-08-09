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
            // "ABCDEFG"
            var bytes = Encoding.ASCII.GetBytes("ABCDEFG");
            string encoded = string.Join(string.Empty, Base64Encoder.Encode(bytes));
            // "QUJDREVGRw=="
            Console.WriteLine(encoded);
            // "ABCDEFG"
            Console.WriteLine(Encoding.ASCII.GetString(Base64Encoder.Decode(encoded).ToArray()));
            Console.Read();
        }
    }
}