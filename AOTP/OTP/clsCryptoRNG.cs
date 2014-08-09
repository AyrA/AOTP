using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace AOTP
{
    public class CryptoRNG : iRNG
    {
        RandomNumberGenerator RNG;

        public CryptoRNG()
        {
            RNG = RandomNumberGenerator.Create();
        }

        /// <summary>
        /// returns next unsigned integer
        /// </summary>
        /// <returns>unsigned integer</returns>
        public uint Next()
        {
            byte[] b = new byte[4];
            RNG.GetBytes(b);
            return BitConverter.ToUInt32(b,0);
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
            RNG.GetBytes(retVal);
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
