using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNet.Streams
{
    public class MergedStream : Stream
    {
        private IEnumerator<Stream> _streamIter;
        private readonly ICollection<Stream> _streams;
        private int _streamIndex = 0;
        private long _streamPosition = 0;
        private long _mergedPosition = 0;
        private bool _disposed = false;

        public MergedStream(IEnumerable<Stream> streams) {
            _streams = streams.ToList();
            _streamIter = streams.GetEnumerator();
        }

        public MergedStream(params Stream[] streams) : this((IEnumerable<Stream>) streams) { }

        public override bool CanRead => _streams.All(x => x.CanRead);

        public override bool CanSeek => _streams.All(x => x.CanSeek);

        public override bool CanWrite => false;

        public override long Length => _streams.Aggregate(0, (long a, Stream b) => a + b.Length);

        public override long Position {
            get => _mergedPosition;
            set => Seek(value, SeekOrigin.Begin);
        }

        public override void Flush() => throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_disposed) throw new ObjectDisposedException("underlying stream");
            if (!_streams.Any()) return 0;
            if (_streamIndex > _streams.Count) return 0;
            if (_mergedPosition > Length) return 0;
            if (offset > buffer.Length) throw new IndexOutOfRangeException();
            if (offset + count > buffer.Length) throw new IndexOutOfRangeException();

            int totalBytesRead = 0;
            while (count > 0 && _streamIter.MoveNext())
            {
                int bytesRead;
                while (count > 0 && (bytesRead = _streamIter.Current.Read(buffer, offset, count)) > 0)
                {
                    totalBytesRead += bytesRead;
                    _streamPosition += bytesRead;
                    _mergedPosition += bytesRead;

                    count -= bytesRead;
                    offset += bytesRead;
                }

                _streamIndex++;
                _streamPosition = 0;
            }

            return totalBytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (_disposed) throw new ObjectDisposedException("underlying stream");
            if (!CanSeek) throw new NotImplementedException();

            // Update offset based on origin
            var targetPosition = offset;
            if (origin == SeekOrigin.Current) targetPosition = _mergedPosition + offset;
            else if (origin == SeekOrigin.End) targetPosition = Length + offset;

            if (targetPosition == _mergedPosition) return 0;
            if (targetPosition > Length) throw new IndexOutOfRangeException($"{targetPosition} is greater than {Length}");
            if (targetPosition < 0) throw new IndexOutOfRangeException($"{targetPosition} cannot be less than zero");

            var oldPosition = _mergedPosition;
            var streamIndex = 0;

            // Locate the focus stream based on position
            var streamIter = _streams.GetEnumerator();
            Stream currenStream = null;
            var positionIndexWalker = targetPosition;
            while (streamIter.MoveNext())
            {
                currenStream = streamIter.Current;

                positionIndexWalker -= currenStream.Length;
                if (positionIndexWalker < 0) break;

                // If we pass over a stream seek to its end
                currenStream.Seek(0, SeekOrigin.End);
                streamIndex++;
            }

            // Update our index
            _streamIndex = streamIndex;
            _streamPosition = -positionIndexWalker;

            // Dispose the old stream iterator
            _streamIter.Dispose();
            _streamIter = streamIter;

            // Seek to the position in the actual stream
            currenStream.Seek(_streamPosition, SeekOrigin.Begin);

            // Update the merged position
            _mergedPosition = targetPosition;

            // Return the relative position change (should be equal to offset)
            return oldPosition - _mergedPosition;
        }

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        
        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing) {
                // Dispose the stream iterator
                _streamIter.Dispose();

                // Dispose underlying streams
                foreach (var stream in _streams)
                {
                    stream.Dispose();
                }
            }

            _disposed = true;
        }
    }
}