using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNet.Streams.Test
{
    public static class TestData
    {
        public const string SampleText = "Some sample string here";
        public static Stream GenerateStream(string data = SampleText) => new MemoryStream(Encoding.UTF8.GetBytes(data));
    }

    public class SingleToManyTestStreams : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            for (var i = 0; i < 10; i++)
            {
                yield return new object[] {
                    // i merged streams
                    new MergedStream(new object[i].Select(x => TestData.GenerateStream())),
                    // i concatted sample data
                    new object[i].Select(x => TestData.SampleText).Aggregate("", (a, b) => a + b) };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}