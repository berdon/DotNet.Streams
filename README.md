# DotNet.Streams
.Net Stream utilities

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
> dotnet script Samples/HelloWorld.cs
Hello World
```