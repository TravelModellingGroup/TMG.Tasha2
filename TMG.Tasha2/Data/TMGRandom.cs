/*
    Copyright 2017 University of Toronto Transportation Research Institute

    This file is part of TMG.Tasha2.

    TMG.Tasha2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    TMG.Tasha2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with TMG.Tasha2.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace TMG.Tasha2
{
    public sealed class TMGRandom
    {
        private const int N = 624;
        private const int M = 397;
        private const uint UPPER_MASK = 0x80000000U;
        private const uint LOWER_MASK = 0x7fffffffU;
        private const float InvMaxUIntAsFloat = 1.0f / uint.MaxValue;
        uint[] mt = new uint[N];
        uint mti;
        public TMGRandom(int seed)
        {
            mt[0] = (uint)seed;
            for (int i = 1; i < mt.Length; i++)
            {
                // See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. 
                // In the previous versions, MSBs of the seed affect   
                // only MSBs of the array mt[].                        
                mt[i] =
                (uint)(1812433253U * (mt[i - 1] ^ (mt[i - 1] >> 30)) + i);
            }
            mti = 0;
        }

        public float Pop()
        {
            if (mti >= N)
            {
                /* generate N words at one time */
                UpdateRandomData();
            }
            return mt[mti++] * InvMaxUIntAsFloat;
        }

        private void UpdateRandomData()
        {
            int kk;
            for (kk = 0; kk < N - M; kk++)
            {
                var y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                y = mt[kk + M] ^ (y >> 1) ^ ((y & 0x1U) > 0 ? 0x9908b0dfU : 0U);
                mt[kk] = y;
            }
            for (; kk < N - 1; kk++)
            {
                var y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                y = mt[kk + (M - N)] ^ (y >> 1) ^ ((y & 0x1U) > 0 ? 0x9908b0dfU : 0U);
                mt[kk] = y;
            }
            var y2 = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
            mt[N - 1] = mt[M - 1] ^ (y2 >> 1) ^ ((y2 & 0x1U) > 0 ? 0x9908b0dfU : 0U);
            for (int i = 0; i < N; i++)
            {
                var y = mt[i];
                y ^= (y >> 11);
                y ^= (y << 7) & 0x9d2c5680U;
                y ^= (y << 15) & 0xefc60000U;
                y ^= (y >> 18);
                mt[i] = y;
            }
            mti = 0;
        }
    }
}
