using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base64
{
    public static class Base64Encoder
    {
        /// <summary>
        /// base 64 の変換表
        /// </summary>
        private static readonly char[] Base64Chars = new[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
        };

        /// <summary>
        /// 逆引き用 dictionary
        /// </summary>
        private static readonly Dictionary<char, int> DecodeDictionary = Base64Chars
            .Select((v, i) => new KeyValuePair<char, int>(v, i))
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        /// <summary>
        /// 文字 encoding を指定して文字列を Base64 エンコードします
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IEnumerable<char> Encode(string source, Encoding encoding)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source == string.Empty)
            {
                return Enumerable.Empty<char>();
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return EncodeInner(encoding.GetBytes(source));
        }

        /// <summary>
        /// byte 列を Base64 エンコードします
        /// </summary>
        public static IEnumerable<char> Encode(IEnumerable<byte> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return EncodeInner(source);
        }

        /// <summary>
        /// 文字 encoding を指定して Base64 デコードします
        /// </summary>
        public static string Decode(IEnumerable<char> source, Encoding encoding)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return encoding.GetString(DecodeInner(source).ToArray());
        }

        /// <summary>
        /// Base64 デコードします
        /// </summary>
        public static IEnumerable<byte> Decode(IEnumerable<char> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return DecodeInner(source);
        }

        /// <summary>
        /// エンコード処理
        /// </summary>
        private static IEnumerable<char> EncodeInner(IEnumerable<byte> source)
        {
            // char の個数が 4 の倍数になるように、足りない分は '=' で補って変換をおこないます
            using (var enumerator = SplitBySixBit(source).Select(b => Base64Chars[b]).GetEnumerator())
            {
                bool hasNext = true;
                int count = 0;
                while (hasNext)
                {
                    if (hasNext = enumerator.MoveNext())
                    {
                        count++;
                        yield return enumerator.Current;
                    }

                    if (count == 4) count = 0;
                }

                if (count == 0)
                {
                    yield break;
                }

                for (var i = count; i < 4; i++)
                {
                    yield return '=';
                }
            }
        }

        /// <summary>
        /// byte 列を 6 ビットずつに分割します
        /// </summary>
        private static IEnumerable<byte> SplitBySixBit(IEnumerable<byte> bytes)
        {
            int current = 0;
            byte nextBitCount = 0;
            using (var enumerator = bytes.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        // あまった current を吐き出す処理
                        if (nextBitCount > 0)
                        {
                            // 6 ビットに満たない分は右側を 0 うめして返す
                            yield return (byte) (current << (6 - nextBitCount));
                        }
                        yield break;
                    }

                    // 3 回に 1 度、すでに 6 bit たまった状態になっているのでだしとく
                    if (nextBitCount == 6)
                    {
                        yield return (byte) current;
                        current = 0;
                        nextBitCount = 0;
                    }

                    // 下位ビットに追加
                    current = (current << 8) | enumerator.Current;
                    // 今回の yield return の対象にならないビット数
                    // 8 bit 増えて 6 bit へるので 2 ビット増加
                    nextBitCount += 2;

                    // 今回出力の対象にならない分は右シフトで落として return
                    yield return (byte)(current >> nextBitCount);

                    // 今回出力の対象にならなかったビットだけを立てる
                    current &= (1 << nextBitCount) - 1;
                }
            }
        }

        /// <summary>
        /// デコード処理
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static IEnumerable<byte> DecodeInner(IEnumerable<char> source)
        {
            // 6 bit 分割された IEnumerable<byte>
            // バイト列の長さが 6 の倍数にならなかった分は最後の数ビットに 0 が挿入されている可能性がある
            var separated = source.Where(c => c != '=').Select(c => DecodeDictionary[c]);

            using (var enumerator = separated.GetEnumerator())
            {
                // 8bit 分割して返す
                int current = 0;
                byte bitsCount = 0;
                bool hasNext = true;

                while (hasNext)
                {
                    if (!(hasNext = enumerator.MoveNext()))
                    {
                        yield break;
                    }

                    current = (current << 6) | enumerator.Current;
                    bitsCount += 6;

                    if (bitsCount >= 8)
                    {
                        bitsCount -= 8;
                        yield return (byte) (current >> bitsCount);
                    }
                }

                // 最後の 8 bit に満たないものは 0 埋めするために挿入されたはずなので返さない
            }

        }
    }
}
