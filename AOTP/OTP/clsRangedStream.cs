using System.IO;
using System;

namespace AOTP
{
    /// <summary>
    /// Provides access to a range of another stream
    /// </summary>
    class RangedStream : Stream
    {
        private long start;
        private long length;
        private Stream st;

        /// <summary>
        /// returns true
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// returns true
        /// </summary>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// returns false
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// does nothing since readonly
        /// </summary>
        public override void Flush()
        {
            //NOOP
        }

        /// <summary>
        /// segment length
        /// </summary>
        public override long Length
        {
            get { return length; }
        }

        /// <summary>
        /// current position (in segment)
        /// </summary>
        public override long Position
        {
            get
            {
                return st.Position - start;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// initializes a new ranged stream
        /// </summary>
        /// <param name="BaseStream">Base stream. Must be readable, seekable. Is not closed on Dispose call!</param>
        /// <param name="Start">Start of range</param>
        /// <param name="Length">Length of range</param>
        public RangedStream(Stream BaseStream, long Start, long Length)
        {
            st = BaseStream;
            if (!st.CanRead)
            {
                throw new Exception("BaseStream is not readable");
            }
            if (!st.CanSeek)
            {
                throw new Exception("BaseStream cannot be seeked");
            }
            if (Start + Length > BaseStream.Length)
            {
                throw new Exception("Start and Length combination is too big");
            }

            start = Start;
            length = Length;
            BaseStream.Seek(Start, SeekOrigin.Begin);
        }

        /// <summary>
        /// reads from the ranged stream
        /// </summary>
        /// <param name="buffer">byte array to fill</param>
        /// <param name="offset">position in array to start</param>
        /// <param name="count">number of bytes to read</param>
        /// <returns>number of bytes read</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count > Length - Position)
            {
                count = (int)(Length - Position);
            }
            if (count > 0)
            {
                return st.Read(buffer, offset, count);
            }
            return 0;
        }

        /// <summary>
        /// seeks inside the range
        /// </summary>
        /// <param name="offset">seek offset</param>
        /// <param name="origin">offset origin</param>
        /// <returns>new position after seeking</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long temp = st.Position;
            if (st.Seek(offset, origin) - start > length)
            {
                st.Position = temp;
                throw new Exception("seek out of range");
            }
            return Position;
        }

        /// <summary>
        /// sets new segment length and seeks back if needed
        /// </summary>
        /// <param name="value">new length (>0)</param>
        public override void SetLength(long value)
        {
            if (value > 0)
            {
                if (start + value <= st.Length)
                {
                    length = value;
                    if (Position > length)
                    {
                        Position = length;
                    }
                }
                else
                {
                    throw new Exception("new length too big for base stream");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("new value must be bigger than 0");
            }
        }

        /// <summary>
        /// sets new segment start position and seeks to start
        /// </summary>
        /// <param name="value">start position (>=0)</param>
        public void SetStart(long value)
        {
            if (value >= 0)
            {
                if (length + value <= st.Length)
                {
                    start = value;
                    st.Seek(start, SeekOrigin.Begin);
                }
                else
                {
                    throw new Exception("new start point too big for base stream");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("negative values not supported");
            }
        }

        /// <summary>
        /// throws an exception.
        /// I should also delete your files because of CanWrite=false
        /// </summary>
        /// <param name="buffer">array to ignore</param>
        /// <param name="offset">offset to ignore</param>
        /// <param name="count">count to ignore</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Cannot write to a ranged stream");
        }
    }
}
