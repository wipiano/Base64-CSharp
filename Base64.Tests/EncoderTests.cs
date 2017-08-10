using System.Text;
using Xunit;

namespace Base64.Tests
{
    public class EncoderTests
    {
        [Fact]
        public void EncodeTest()
        {
            var source = Encoding.ASCII.GetBytes("ABCDEFG");
            var actual = Base64Encoder.Encode(source);
            Assert.Equal("QUJDREVGRw==", actual);
        }

        [Fact]
        public void EncodeStringTest()
        {
            var actual = Base64Encoder.Encode("ABCDEFG", Encoding.ASCII);
            Assert.Equal("QUJDREVGRw==", actual);
        }

        [Fact]
        public void DecodeTest()
        {
            var actual = Base64Encoder.Decode("QUJDREVGRw==");
            var expected = Encoding.ASCII.GetBytes("ABCDEFG");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeStringTest()
        {
            var actual = Base64Encoder.Decode("QUJDREVGRw==", Encoding.ASCII);
            Assert.Equal("ABCDEFG", actual);
        }
    }
}
