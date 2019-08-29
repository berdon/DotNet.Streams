# DotNet.Streams
[![Build Status](https://img.shields.io/azure-devops/build/berdon/DotNet.Streams/4/release)](https://img.shields.io/azure-devops/build/berdon/DotNet.Streams/4/release)
[![Nuget](https://img.shields.io/nuget/v/Berdon.DotNet.Streams)](https://www.nuget.org/packages/Berdon.DotNet.Streams/)

.NET Stream utilities


```C#
var stream1 = new MemoryStream(Encoding.UTF8.GetBytes("Hello"));
var stream2 = new MemoryStream(Encoding.UTF8.GetBytes(" "));
var stream3 = new MemoryStream(Encoding.UTF8.GetBytes("World"));
var mergedStream = stream1.Concat(stream2, stream3);
using (var streamReader = new StreamReader(mergedStream))
{
    Console.WriteLine(streamReader.ReadToEnd());
}
```

```bash
➜ dotnet script Samples/HelloWorld.csx --verbosity e 
Hello World
```