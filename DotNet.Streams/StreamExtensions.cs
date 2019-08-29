using System.IO;
using System.Linq;

namespace DotNet.Streams
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Append stream b onto the end of the current stream.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Stream Concat(this Stream self, params Stream[] streams)
        {
            return new MergedStream(new [] { self }.Concat(streams));
        }
    }
}