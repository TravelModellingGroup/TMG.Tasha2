﻿/*
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
using XTMF2;

namespace TMG.Tasha2.Modules
{
    [Module(Name = "Discrete Choice", Description = "Returns a random index from a probability vector as a discrete choice.",
        DocumentationLink = "http://tmg.utoronto.ca/doc/2.0")]
    public sealed class DiscreteChoice : BaseFunction<(TMGRandom, float[]), int>
    {
        public override int Invoke((TMGRandom, float[]) context)
        {
            var (r, data) = context;
            var pop = r.Pop();
            var acc = 0.0f;
            for (int i = 0; i < data.Length; i++)
            {
                acc += data[i];
                if(acc >= pop)
                {
                    return i;
                }
            }
            return data.Length - 1;
        }
    }
}
