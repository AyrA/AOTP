using System;
using System.Collections.Generic;
using System.Text;

namespace AOTP
{
    public interface iRNG
    {
        uint Next();

        int NextInt();

        byte NextByte();

        byte[] NextBytes(int Count);

        double NextDouble();
    }
}
