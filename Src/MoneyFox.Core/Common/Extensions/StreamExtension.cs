namespace MoneyFox.Core.Common.Extensions
{

    using System;
    using System.IO;

    public static class StreamExtension
    {
        /// <summary>
        ///     Reads the bytes of a stream and returns them in an array.
        /// </summary>
        /// <param name="stream">Stream to read.</param>
        /// <returns></returns>
        public static byte[] ReadToEnd(this Stream stream)
        {
            long originalPosition = 0;
            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                var readBuffer = new byte[4096];
                var totalBytesRead = 0;
                int bytesRead;
                while ((bytesRead = stream.Read(buffer: readBuffer, offset: totalBytesRead, count: readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;
                    if (totalBytesRead == readBuffer.Length)
                    {
                        var nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            var temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(
                                src: readBuffer,
                                srcOffset: 0,
                                dst: temp,
                                dstOffset: 0,
                                count: readBuffer.Length);

                            Buffer.SetByte(array: temp, index: totalBytesRead, value: (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                var buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(
                        src: readBuffer,
                        srcOffset: 0,
                        dst: buffer,
                        dstOffset: 0,
                        count: totalBytesRead);
                }

                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }

}
