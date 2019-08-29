using System;
using System.IO;
using Xunit;

namespace DotNet.Streams.Test
{
    public class DisposeTests
    {
        [Fact]
        public void CanDispose()
        {
            var stream = new MergedStream();
            stream.Dispose();
        }

        [Fact]
        public void ReadAfterDisposeThrowsObjectDisposedExcpetion()
        {
            var stream = new MergedStream();
            stream.Dispose();
            Assert.Throws<ObjectDisposedException>(() => stream.Read(null, 0, 0));
        }

        [Fact]
        public void SeekAfterDisposeThrowsObjectDisposedExcpetion()
        {
            var stream = new MergedStream();
            stream.Dispose();
            Assert.Throws<ObjectDisposedException>(() => stream.Seek(0, SeekOrigin.Begin));
        }
    }
}