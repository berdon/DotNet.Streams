#!/usr/bin/env dotnet-script
#r "nuget: Berdon.DotNet.Streams, 0.0.1"
using System;
using System.IO;
using DotNet.Streams;

var stream1 = new MemoryStream(Encoding.UTF8.GetBytes("Hello"));
var stream2 = new MemoryStream(Encoding.UTF8.GetBytes(" "));
var stream3 = new MemoryStream(Encoding.UTF8.GetBytes("World"));
var mergedStream = stream1.Concat(stream2, stream3);
using (var streamReader = new StreamReader(mergedStream))
{
    Console.WriteLine(streamReader.ReadToEnd());
}