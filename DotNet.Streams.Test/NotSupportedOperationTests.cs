using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNet.Streams.Test
{
    public class NotSupportedTests
    {
        [Fact]
        public void WriteThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => new MergedStream().Write(null, 0, 0));
        }

        [Fact]
        public void SetLengthThrowsNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => new MergedStream().SetLength(0));
        }
    }
}
