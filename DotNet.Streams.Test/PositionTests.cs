using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNet.Streams.Test
{
    public class PositionTests
    {
        [Theory]
        [ClassData(typeof(SingleToManyTestStreams))]
        public void CanReadLengthOfMergedStream(MergedStream mergedStream, string expectedData)
        {
            Assert.Equal(expectedData.Length, mergedStream.Length);
        }

        [Theory]
        [ClassData(typeof(SingleToManyTestStreams))]
        public async Task CanSeekMergedStream(MergedStream mergedStream, string expectedData)
        {
            // Seek to the end
            mergedStream.Seek(expectedData.Length, SeekOrigin.Begin);
            Assert.Equal(expectedData.Length, mergedStream.Position);

            // Seek to the beginning
            mergedStream.Seek(0, SeekOrigin.Begin);
            Assert.Equal(0, mergedStream.Position);

            // Seek to the end
            mergedStream.Seek(0, SeekOrigin.End);
            Assert.Equal(expectedData.Length, mergedStream.Position);

            // Seek to the middle
            mergedStream.Seek(expectedData.Length / 2, SeekOrigin.Begin);
            Assert.Equal(expectedData.Length / 2, mergedStream.Position);
        }
    }
}
