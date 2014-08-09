using System.IO;
using System;
using System.Text;

namespace AOTP
{
    public delegate void xorProgressHandler(int Percentage);

    /// <summary>
    /// provides OTP and header functions
    /// </summary>
    public static class OTP
    {
        /// <summary>
        /// Provides progress during xor operation
        /// </summary>
        public static event xorProgressHandler xorProgress;

        /// <summary>
        /// Header properties
        /// </summary>
        public struct HeaderProps
        {
            /// <summary>
            /// File name (without path)
            /// </summary>
            public string FileName;
            /// <summary>
            /// File size (or -1 to indicate error)
            /// </summary>
            public long FileLength;
        }

        /// <summary>
        /// 10 MB buffer const
        /// </summary>
        private const int BUFFER = 1024 * 1024 * 10;

        /// <summary>
        /// initializing static stuff
        /// </summary>
        static OTP()
        {
            xorProgress += new xorProgressHandler(OTP_xorProgress);
        }

        /// <summary>
        /// does nothing
        /// </summary>
        /// <param name="Percentage">0-100</param>
        private static void OTP_xorProgress(int Percentage)
        {
            //NOOP
        }

        /// <summary>
        /// returns a random unsigned integer derived from time and date
        /// </summary>
        public static uint RandomSeed
        {
            get
            {
                return (uint)(DateTime.Now.Ticks % (uint.MaxValue - 1)) + 1;
            }
        }

        /// <summary>
        /// generates a new key for AOTP
        /// </summary>
        /// <param name="Size">size of the biggest file + header</param>
        /// <param name="Seed">seed for the generator, see <see cref="RandomSeed"/>RandomSeed</param>
        /// <param name="Output">Stream to write the key to. Any writeable stream supported</param>
        public static void generateKey(long Size, uint Seed, Stream Output)
        {
            PseudoRandStream PRS = new PseudoRandStream(new CryptoRNG(), Size);
            byte[] buffer=new byte[BUFFER];
            int readed = 0;

            xorProgress(0);

            do
            {
                Output.Write(buffer, 0, readed = PRS.Read(buffer, 0, BUFFER));
                xorProgress(getPerc(PRS.Position, Size));
            } while (readed > 0);
            xorProgress(100);
        }

        /// <summary>
        /// generates a header for the specified file properties
        /// </summary>
        /// <param name="Name">file name (no path please)</param>
        /// <param name="Length">length of file in bytes</param>
        /// <returns>byte array containing full header information</returns>
        public static byte[] generateHeader(string Name,long Length)
        {
            using (MemoryStream MS = new MemoryStream())
            {
                //Header Format:
                //<File length:8><Name Length:4><Name:<Name Length>><0:4><Header Length:4>

                //header length includes itself: name Length + 20

                //File Format:
                //<Encrypted content><padding><Header>

                //Together with <File length> and <Header length> the padding can
                //be cut out of the stream and does not needs to be decoded at all

                //Length argument is long (8 bytes = 64 bit)
                MS.Write(BitConverter.GetBytes(Length), 0, 8);
                //length of Name is int (4 bytes = 32 bit)
                MS.Write(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(Name)), 0, 4);
                //following by name of specified length
                MS.Write(Encoding.UTF8.GetBytes(Name), 0, Encoding.UTF8.GetByteCount(Name));
                //following a 0 for "no more headers"
                MS.Write(new byte[] { 0, 0, 0, 0 }, 0, 4);
                //Length argument is long (8 bytes = 64 bit)
                MS.Write(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(Name) + 20), 0, 4);
                return MS.ToArray();
            }
        }

        /// <summary>
        /// Decrypts a header from a file
        /// </summary>
        /// <param name="InFile">File with header</param>
        /// <param name="InKey">Key to decrypt</param>
        /// <returns>HeaderProps. File size is -1 in case of errors</returns>
        public static HeaderProps decryptHeader(Stream InFile, Stream InKey)
        {
            HeaderProps HP = new HeaderProps();
            try
            {
                byte[] bufKey, bufFile;
                int i;

                bufKey = new byte[4];
                bufFile = new byte[4];

                InFile.Seek(-4, SeekOrigin.End);
                InKey.Seek(-4, SeekOrigin.End);

                InFile.Read(bufFile, 0, 4);
                InKey.Read(bufKey, 0, 4);

                for (i = 0; i < 4; i++)
                {
                    bufFile[i] ^= bufKey[i];
                }

                int HeaderLength = BitConverter.ToInt32(bufFile, 0);

                bufKey = new byte[HeaderLength];
                bufFile = new byte[HeaderLength];

                InFile.Seek(-HeaderLength, SeekOrigin.End);
                InKey.Seek(-HeaderLength, SeekOrigin.End);

                InFile.Read(bufFile, 0, HeaderLength);
                InKey.Read(bufKey, 0, HeaderLength);

                for (i = 0; i < HeaderLength; i++)
                {
                    bufFile[i] ^= bufKey[i];
                }

                //complete header decoded by now

                HP.FileLength = BitConverter.ToInt64(bufFile, 0);

                if (HP.FileLength <= InFile.Length - HeaderLength)
                {
                    HP.FileName = Encoding.UTF8.GetString(bufFile, 12, BitConverter.ToInt32(bufFile, 8));
                }
                else
                {
                    throw new Exception("Decoded File length is too big. Key probably invalid");
                }
            }
            catch
            {
                HP.FileLength = -1;
                HP.FileName = null;
            }
            return HP;
        }

        /// <summary>
        /// encrypting a file, adding headers and padding to match key length
        /// </summary>
        /// <param name="Filename">Full file name of readable file</param>
        /// <param name="Key">Key stream</param>
        /// <param name="Output">Output Stream</param>
        public static void xorFile(string Filename, Stream Key, Stream Output)
        {
            using (Stream InFile = File.OpenRead(Filename))
            {
                byte[] Header = generateHeader(new FileInfo(Filename).Name, InFile.Length);

                //encrypt File with key
                xor(InFile, Key, Output);

                //check if file too small for key
                if (InFile.Length + Header.Length < Key.Length)
                {
                    //expand file and add encrypted crap
                    using (PseudoRandStream PRS = new PseudoRandStream(new CryptoRNG(), Key.Length - InFile.Length - Header.Length))
                    {
                        xor(PRS, Key, Output);
                    }
                }

                //encrypt header
                using (MemoryStream MS = new MemoryStream(Header))
                {
                    xor(MS, Key, Output);
                }
            }
        }

        /// <summary>
        /// applies xor to streams
        /// </summary>
        /// <param name="InputA">input A</param>
        /// <param name="InputB">input B</param>
        /// <param name="Output">xor output</param>
        public static void xor(Stream SourceFile, Stream Key, Stream Output)
        {
            int readed = 0;

            int lastPerc = 0;

            byte[] SourceBuffer = new byte[BUFFER];
            byte[] KeyBuffer = new byte[BUFFER];

            if (SourceFile.Length - SourceFile.Position > Key.Length - Key.Position)
            {
                throw new Exception("Source file size bigger than key size; aborting");
            }

            xorProgress(0);

            do
            {
                readed = SourceFile.Read(SourceBuffer, 0, SourceBuffer.Length);

                if (readed > 0)
                {
                    Key.Read(KeyBuffer, 0, readed);
                    for (int i = 0; i < readed; i++)
                    {
                        SourceBuffer[i] ^= KeyBuffer[i];
                    }
                    Output.Write(SourceBuffer, 0, readed);
                    if (getPerc(SourceFile.Position, SourceFile.Length) != lastPerc)
                    {
                        xorProgress(lastPerc = getPerc(SourceFile.Position, SourceFile.Length));
                    }
                }
            } while (readed > 0);

            xorProgress(100);
        }

        /// <summary>
        /// calculates percentage of progress vs maximum value
        /// </summary>
        /// <param name="progress">progess (0-max)</param>
        /// <param name="max">maximum possible progress</param>
        /// <returns>integer (0-100)</returns>
        private static int getPerc(long progress, long max)
        {
            //shrink numbers to make maths easier
            while (progress > 1000 || max > 1000)
            {
                progress /= 10;
                max /= 10;
            }
            return max == 0 ? 0 : (int)(progress * 100 / max);
        }
    }
}
