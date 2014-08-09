using System;
using System.Collections.Generic;
using System.Text;

namespace AOTP
{
    public class DefaultRNG : iRNG
    {
        Random R;

        public int Seed
        { get; private set; }

        public DefaultRNG()
        {
            Seed = (int)RNG.RandomSeed;
            R = new Random(Seed);
        }

        public DefaultRNG(int seed)
        {
            Seed = seed;
            R = new Random(Seed);
        }

        public uint Next()
        {
            return BitConverter.ToUInt32(NextBytes(4),0);
        }

        public int NextInt()
        {
            return BitConverter.ToInt32(NextBytes(4),0);
        }

        public byte NextByte()
        {
            return (byte)R.Next(256);
        }

        public byte[] NextBytes(int Count)
        {
            byte[] b = new byte[Count];
            R.NextBytes(b);
            return b;
        }

        public double NextDouble()
        {
            return R.NextDouble();
        }
    }
}
