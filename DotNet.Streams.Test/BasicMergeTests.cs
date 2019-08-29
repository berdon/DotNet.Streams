using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNet.Streams.Test
{
    public class BasicMergeTests
    {
        [Theory]
        [ClassData(typeof(SingleToManyTestStreams))]
        public async Task CanReadToEndOneMergesStream(MergedStream mergedStream, string expectedData)
        {
            using var streamReader = new StreamReader(mergedStream);
            var data = await streamReader.ReadToEndAsync();
            Assert.Equal(expectedData, data);
        }

        [Theory]
        [ClassData(typeof(SingleToManyTestStreams))]
        public async Task CanReadMergedStreamsIncrementally(MergedStream mergedStream, string expectedData)
        {
            using var streamReader = new StreamReader(mergedStream);

            var totalData = new StringBuilder();
            var totalBytesRead = 0;

            while (!streamReader.EndOfStream)
            {
                var buffer = new char[10];
                var bytesRead = await streamReader.ReadAsync(buffer, 0, buffer.Length);
                totalBytesRead += bytesRead;
                totalData.Append(buffer, 0, bytesRead);

                Assert.Equal(expectedData.Substring(0, totalBytesRead), totalData.ToString());
            }
        }
    }
}
