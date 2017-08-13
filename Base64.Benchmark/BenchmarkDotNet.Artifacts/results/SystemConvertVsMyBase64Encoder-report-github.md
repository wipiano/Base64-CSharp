``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-6700 CPU 3.40GHz (Skylake), ProcessorCount=8
Frequency=3328132 Hz, Resolution=300.4689 ns, Timer=TSC
.NET Core SDK=1.0.4
  [Host]     : .NET Core 1.1.2 (Framework 4.6.25211.01), 64bit RyuJIT
  DefaultJob : .NET Core 1.1.2 (Framework 4.6.25211.01), 64bit RyuJIT


```
 |                   Method |      Mean |     Error |    StdDev |    Median |
 |------------------------- |----------:|----------:|----------:|----------:|
 |     SerializeBytesSystem |  17.12 us | 0.1537 us | 0.1362 us |  17.11 us |
 |         SerializeBytesMy | 233.44 us | 1.6558 us | 1.4678 us | 233.70 us |
 |        SerializeBytesMy2 | 233.72 us | 4.6887 us | 7.8337 us | 230.74 us |
 | SerializeByteArraySystem |  16.84 us | 0.3581 us | 0.9802 us |  16.27 us |
 |     SerializeByteArrayMy | 236.19 us | 3.2851 us | 2.5648 us | 235.14 us |
 |    SerializeByteArrayMy2 | 229.51 us | 0.8330 us | 0.6956 us | 229.43 us |
