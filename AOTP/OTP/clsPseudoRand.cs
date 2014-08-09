using System.IO;
using System;

namespace AOTP
{
    /// <summary>
    /// Provides stream access to System.Random
    /// </summary>
    public class PseudoRandStream : Stream
    {
        private long currPos=0;
        private long len = 0;
        private iRNG r;

        /// <summary>
        /// true
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// false
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// false
        /// </summary>
        public override bool CanTimeout
        {
            get { return false; }
        }

        /// <summary>
        /// false
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// returns given length
        /// </summary>
        public override long Length
        {
            get
            {
                return len;
            }
        }

        /// <summary>
        /// returns current position
        /// </summary>
        public override long Position
        {
            get
            {
                return currPos;
            }
            set
            {
                throw new Exception("This Property is readonly");
            }
        }

        /// <summary>
        /// ignored
        /// </summary>
        public override int ReadTimeout
        { get; set; }

        /// <summary>
        /// ignored
        /// </summary>
        public override int WriteTimeout
        { get; set; }

        /// <summary>
        /// creates new RNG stream with random seed
        /// </summary>
        /// <param name="length">length of stream</param>
        public PseudoRandStream(iRNG RandomGen, long length)
        {
            r = RandomGen;
            len = length;
        }

        /// <summary>
        /// creates new RNG stream with random seed and maximum length
        /// </summary>
        public PseudoRandStream(iRNG RandomGen)
        {
            r = RandomGen;
            len = long.MaxValue;
        }

        /// <summary>
        /// does nothing
        /// </summary>
        public override void Flush()
        {
            //Do nothing loop;
        }

        /// <summary>
        /// seeks, if SeekOrigin.Current given
        /// </summary>
        /// <param name="offset">offset to seek (>0)</param>
        /// <param name="origin">must be SeekOrigin.Current</param>
        /// <returns>new position</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin != SeekOrigin.Current)
            {
                throw new Exception("Can only seek from Current location forward");
            }
            else if (currPos + offset > len)
            {
                throw new Exception("cannot seek that much forward");
            }
            else if (offset > 0)
            {
                while (offset > 0)
                {
                    if (offset > int.MaxValue)
                    {
                        offset -= Read(new byte[int.MaxValue], 0, int.MaxValue);
                    }
                    else
                    {
                        offset -= Read(new byte[(int)offset], 0, (int)offset);
                    }
                }
                return currPos;
            }
            else
            {
                throw new Exception("Cannot seek backwards");
            }
        }

        /// <summary>
        /// throws an exception
        /// </summary>
        /// <param name="value">ignored</param>
        public override void SetLength(long value)
        {
            throw new Exception("Cannot change length");
        }

        /// <summary>
        /// reads random data
        /// </summary>
        /// <param name="buffer">buffer to fill</param>
        /// <param name="offset">offset to start</param>
        /// <param name="count">number of bytes to read</param>
        /// <returns>number of bytes read</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (offset >= 0 && count > 0)
            {
                if (offset + count <= buffer.Length)
                {
                    //make array not too big for stream
                    byte[] temp = r.NextBytes(count + currPos > len ? (int)(len - currPos) : count);
                    Array.Copy(temp, 0, buffer, offset, temp.Length);
                    currPos += temp.Length;
                    return temp.Length;
                }
                else
                {
                    throw new Exception("offset and count too bit for given buffer array");
                }
            }
            else if (count == 0)
            {
                return 0;
            }
            else
            {
                throw new Exception("You might want to verify, that offset and count are not negative");
            }
        }

        /// <summary>
        /// throws an exception
        /// </summary>
        /// <param name="buffer">ignored</param>
        /// <param name="offset">ignored</param>
        /// <param name="count">ignored</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("This Stream is readonly");
        }

        /// <summary>
        /// reads a single byte
        /// </summary>
        /// <returns>byte readed (or -1 if at end)</returns>
        public override int ReadByte()
        {
            if (currPos < len)
            {
                ++currPos;
                return r.NextByte();
            }
            else
            {
                return -1;
            }
        }
    }
}
