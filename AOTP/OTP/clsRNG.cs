using System;
using System.Security.Cryptography;

namespace AOTP
{
    /// <summary>
    /// Custom random number generator
    /// </summary>
    public class RNG : iRNG
    {
        private uint z;
        private uint w;

        public static uint RandomSeed
        {
            get
            {
                var RNG=RandomNumberGenerator.Create();
                byte[] b = new byte[4];
                RNG.GetBytes(b);
                uint rv=BitConverter.ToUInt32(b, 0);
                return rv == 0 ? 1 : rv;
            }
        }

        /// <summary>
        /// returns the seed used to initialize the generator
        /// </summary>
        public uint Seed
        { get; private set; }

        /// <summary>
        /// initializes a new random seed generator
        /// </summary>
        /// <param name="seed">seed for generator (0 not allowed)</param>
        public RNG(uint seed)
        {
            Seed = z = w = seed == 0 ? 1 : seed;
        }

        /// <summary>
        /// initializes a new RNG with time as seed
        /// </summary>
        public RNG()
        {
            Seed = z = w = RandomSeed;
        }

        /// <summary>
        /// resets the generator to the given seed to reproduce the same numbers again
        /// </summary>
        public void Reseed()
        {
            Reseed(Seed);
        }

        /// <summary>
        /// reseeds the generator with the given seed
        /// </summary>
        /// <param name="seed">new seed >0</param>
        public void Reseed(uint seed)
        {
            Seed = z = w = seed == 0 ? 1 : seed;
        }

        /// <summary>
        /// returns next unsigned integer
        /// </summary>
        /// <returns>unsigned integer</returns>
        public uint Next()
        {
            z = 36969 * (z & 65535) + (z >> 16);
            w = 18000 * (w & 65535) + (w >> 16);
            return (z << 16) + w;
        }

        /// <summary>
        /// returns new integer value
        /// </summary>
        /// <returns>integer value</returns>
        public int NextInt()
        {
            return (int)(int.MinValue + Next());
        }

        /// <summary>
        /// returns a byte
        /// </summary>
        /// <returns>byte</returns>
        public byte NextByte()
        {
            return (byte)(Next() % 256);
        }

        /// <summary>
        /// fills an array with random bytes
        /// </summary>
        /// <param name="Count">number of bytes</param>
        /// <returns>byte array of given size</returns>
        public byte[] NextBytes(int Count)
        {
            byte[] retVal = new byte[Count];
            for (int i = 0; i < Count; i++)
            {
                retVal[i] = NextByte();
            }
            return retVal;
        }

        /// <summary>
        /// returns a double number (0 - 1)
        /// </summary>
        /// <returns>double</returns>
        public double NextDouble()
        {
            return (Next() + 1.0) * 2.328306435454494e-10;
        }
    }
}
