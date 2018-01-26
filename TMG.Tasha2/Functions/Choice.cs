/*
    Copyright 2018 University of Toronto Transportation Research Institute

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
using System.Numerics;
using System.Text;

namespace TMG.Tasha2.Functions
{
    /// <summary>
    /// Contains a variety of methods to chose given a discrete choice set
    /// </summary>
    internal static class Choice
    {
        /// <summary>
        /// Randomly select an element from a probability vector.
        /// </summary>
        /// <param name="random">The random algorithm to use.</param>
        /// <param name="probabilities">The probability vector to chose from.</param>
        /// <returns>The index that was selected.</returns>
        public static int DiscreteChoiceFromProbabilities(TMGRandom random, float[] probabilities)
        {
            var pop = random.Pop();
            var acc = 0.0f;
            for (int i = 0; i < probabilities.Length; i++)
            {
                acc += probabilities[i];
                if (acc >= pop)
                {
                    return i;
                }
            }
            return probabilities.Length - 1;
        }

        /// <summary>
        /// Randomly select an element from a CDF vector.
        /// </summary>
        /// <param name="random">The random algorithm to use.</param>
        /// <param name="cdf">The CDF vector to chose from.</param>
        /// <returns>The index that was selected.</returns>
        public static int DiscreteChoiceFromCDF(TMGRandom random, float[] cdf)
        {
            var pop = random.Pop();
            // there is no need to check the last element
            for (int i = 0; i < cdf.Length - 1; i++)
            {
                if (pop > cdf[i])
                {
                    return i;
                }
            }
            return cdf.Length - 1;
        }
    }
}
