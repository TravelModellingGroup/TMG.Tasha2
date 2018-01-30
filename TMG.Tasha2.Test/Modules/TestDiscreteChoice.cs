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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TMG.Tasha2.Modules;
using TMG.Utilities;
using static TMG.Tasha2.Test.Utilities.Helper;

namespace TMG.Tasha2.Test
{
    [TestClass]
    public class TestDiscreteChoice
    {
        [TestMethod]
        public void DiscreteChoiceFromProbabilities()
        {
            var r = new TMGRandom(12345);
            var choiceModel = new DiscreteChoiceFromProbabilities()
            {
                Name = "Choice Model"
            };
            float[] data = Enumerable.Repeat(0f, 100).ToArray();
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = r.Pop();
            }
            var total = VectorHelper.Sum(data, 0, data.Length);
            VectorHelper.Multiply(data, data, 1.0f / total);
            for (int i = 0; i < 100; i++)
            {
                var result = choiceModel.Invoke((r, data));
                if(result < 0)
                {
                    Assert.Fail("Generated a number less than zero!");
                }
                if(result >= data.Length)
                {
                    Assert.Fail("Generated a number greater than the number of choices!");
                }
            }
        }

        [TestMethod]
        public void DiscreteChoiceFromCDF()
        {
            var r = new TMGRandom(12345);
            var choiceModel = new DiscreteChoiceFromCDF()
            {
                Name = "Choice Model"
            };
            float[] data = Enumerable.Repeat(0f, 100).ToArray();
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = r.Pop();
            }
            var invSum = 1.0f / VectorHelper.Sum(data, 0, data.Length);
            data[0] *= invSum; 
            for (int i = 1; i < data.Length; i++)
            {
                data[i] = (data[i] * invSum) + data[i - 1];
            }
            for (int i = 0; i < 100; i++)
            {
                var result = choiceModel.Invoke((r, data));
                if (result < 0)
                {
                    Assert.Fail("Generated a number less than zero!");
                }
                if (result >= data.Length)
                {
                    Assert.Fail("Generated a number greater than the number of choices!");
                }
            }
        }
    }
}
