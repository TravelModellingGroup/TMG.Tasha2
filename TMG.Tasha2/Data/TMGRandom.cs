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
    /// <summary>
    /// 
    /// </summary>
    public sealed class TMGRandom
    {
        private const int N = 624;
        private const int M = 397;
        private const uint UPPER_MASK = 0x80000000U;
        private const uint LOWER_MASK = 0x7fffffffU;
        private const float InvMaxUIntAsFloat = 1.0f / uint.MaxValue;
        private uint[] _mt = new uint[N];
        private uint _mti;

        public TMGRandom(int seed)
        {
            _mt[0] = (uint)seed;
            for (int i = 1; i < _mt.Length; i++)
            {
                // See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. 
                // In the previous versions, MSBs of the seed affect   
                // only MSBs of the array mt[].                        
                _mt[i] =
                (uint)(1812433253U * (_mt[i - 1] ^ (_mt[i - 1] >> 30)) + i);
            }
            _mti = 0;
        }

        public float Pop()
        {
            if (_mti >= N)
            {
                /* generate N words at one time */
                UpdateRandomData();
            }
            return _mt[_mti++] * InvMaxUIntAsFloat;
        }

        /// <summary>
        /// Get a normally distributed number mean zero, std 1.
        /// </summary>
        /// <returns></returns>
        public float PopNormal()
        {
            /* Ratio method (Kinderman-Monahan); see Knuth v2, 3rd ed, p130.
             * K+M, ACM Trans Math Software 3 (1977) 257-260.
             *
             * [Added by Charles Karney] This is an implementation of Leva's
             * modifications to the original K+M method; see:
             * J. L. Leva, ACM Trans Math Software 18 (1992) 449-453 and 454-455. */
            float u, v, x, y, q;
            const float s = 0.449871f;    /* Constants from Leva */
            const float t = -0.386595f;
            const float a = 0.19600f;
            const float b = 0.25472f;
            const float r1 = 0.27597f;
            const float r2 = 0.27846f;

            do                            /* This loop is executed 1.369 times on average  */
            {
                /* Generate a point P = (u, v) uniform in a rectangle enclosing
                   the K+M region v^2 <= - 4 u^2 log(u). */

                /* u in (0, 1] to avoid singularity at u = 0 */
                u = 1 - Pop();

                /* v is in the asymmetric interval [-0.5, 0.5).  However v = -0.5
                   is rejected in the last part of the while clause.  The
                   resulting normal deviate is strictly symmetric about 0
                   (provided that v is symmetric once v = -0.5 is excluded). */
                v = Pop() - 0.5f;

                /* Constant 1.7156 > sqrt(8/e) (for accuracy); but not by too
                   much (for efficiency). */
                v *= 1.7156f;

                /* Compute Leva's quadratic form Q */
                x = u - s;
                y = Math.Abs(v) - t;
                q = x * x + y * (a * y - b * x);

                /* Accept P if Q < r1 (Leva) */
                /* Reject P if Q > r2 (Leva) */
                /* Accept if v^2 <= -4 u^2 log(u) (K+M) */
                /* This final test is executed 0.012 times on average. */
            }
            //TODO: Replace this with a MathF.Log when it is available
            while (q >= r1 && (q > r2 || v * v > -4 * u * u * Math.Log(u)));
            // Return slope
            return (v / u);
        }

        private void UpdateRandomData()
        {
            int kk;
            for (kk = 0; kk < N - M; kk++)
            {
                var y = (_mt[kk] & UPPER_MASK) | (_mt[kk + 1] & LOWER_MASK);
                y = _mt[kk + M] ^ (y >> 1) ^ ((y & 0x1U) > 0 ? 0x9908b0dfU : 0U);
                _mt[kk] = y;
            }
            for (; kk < N - 1; kk++)
            {
                var y = (_mt[kk] & UPPER_MASK) | (_mt[kk + 1] & LOWER_MASK);
                y = _mt[kk + (M - N)] ^ (y >> 1) ^ ((y & 0x1U) > 0 ? 0x9908b0dfU : 0U);
                _mt[kk] = y;
            }
            var y2 = (_mt[N - 1] & UPPER_MASK) | (_mt[0] & LOWER_MASK);
            _mt[N - 1] = _mt[M - 1] ^ (y2 >> 1) ^ ((y2 & 0x1U) > 0 ? 0x9908b0dfU : 0U);
            for (int i = 0; i < N; i++)
            {
                var y = _mt[i];
                y ^= (y >> 11);
                y ^= (y << 7) & 0x9d2c5680U;
                y ^= (y << 15) & 0xefc60000U;
                y ^= (y >> 18);
                _mt[i] = y;
            }
            _mti = 0;
        }
    }
}
