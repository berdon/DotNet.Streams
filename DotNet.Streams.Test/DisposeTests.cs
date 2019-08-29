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

        [Fact]
        public void UnderlyingStreamsAreNotDisposedIfFlagSet()
        {
            var mergedStream = new ThrowOnDisposeStream().Concat(false, new ThrowOnDisposeStream());
            mergedStream.Dispose();
        }

        [Fact]
        public void UnderlyingStreamsDisposed()
        {
            var stream1 = new ThrowOnDisposeStream();
            var stream2 = new ThrowOnDisposeStream();
            var mergedStream = stream1.Concat(stream2);
            Assert.Throws<Exception>(() => mergedStream.Dispose());
        }

        public class ThrowOnDisposeStream : Stream
        {
            public override bool CanRead => throw new NotImplementedException();

            public override bool CanSeek => throw new NotImplementedException();

            public override bool CanWrite => throw new NotImplementedException();

            public override long Length => throw new NotImplementedException();

            public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            protected override void Dispose(bool disposing)
            {
                throw new Exception();
            }
        }
    }
}