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
using System.Text;
using XTMF2;
using TMG.Tasha2.Data;
using TMG.Tasha2.Functions;

namespace TMG.Tasha2.Modules
{
    [Module(Name = "Tasha1Scheduler", Description = "The TASHA/1 scheduler converted for XTMF2.",
        DocumentationLink = "http://tmg.utoronto.ca/doc/2.0")]
    public class Tasha1Scheduler : BaseFunction<(TMGRandom, Household), (TMGRandom, Household)>
    {
        public override (TMGRandom, Household) Invoke((TMGRandom, Household) context)
        {
            return context;
        }
    }
}
