# Base64 for C#
C# 用の Base64 エンコーダ
勉強用に作ったもの．性能評価などはしてない．

## Encode
### string
任意の文字コードを指定して文字列をエンコード

```csharp
IEnumerable<char> encoded = Base64Encoder.Encode("ABCDEFG", Encoding.ASCII);
```

### bytes
byte[] をエンコードする場合

```csharp
byte[] source = new byte {0x10, 0x11, 0x12};
IEnumerable<char> encoded = Base64Encoder.Encode(source);
```

## Decode
### string
任意の文字コードを指定してデコード

```csharp
IEnumerable<char> encoded = // ...
string decoded = Base64Encoder.Decode(encoded, Encoding.ASCII);
```

### bytes

```csharp
IEnumerable<char> encoded = // ...
IEnumerable<byte> decoded = Base64Encoder.Decode(encoded);
```