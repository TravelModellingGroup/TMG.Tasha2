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
using TMG.Tasha2.Data;
using XTMF2;

namespace TMG.Tasha2.Modules
{
    [Module(Name = "Basic Household Pipe", Description = "Chains together a loader of households with the scheduler and mode choice.",
        DocumentationLink = "http://tmg.utoronto.ca/doc/2.0")]
    public class ExecuteBasicHouseholdPipeline : BaseAction
    {
        [SubModule(Index = 0, Name = "Household Loader", Required = true)]
        public IFunction<IEnumerable<Household>> HouseholdLoader;

        [SubModule(Index = 1, Name = "Scheduler", Required = false)]
        public IFunction<IEnumerable<(TMGRandom, Household)>, IEnumerable<(TMGRandom, Household)>> Scheduler;

        [SubModule(Index = 2, Name = "ModeChoice", Required = false)]
        public IFunction<IEnumerable<(TMGRandom, Household)>, IEnumerable<(TMGRandom, Household)>> ModeChoice;

        [Parameter(Index = 3, Name = "RandomSeed", Required = true, DefaultValue = "12345")]
        public IFunction<int> RandomSeed;

        /// <summary>
        /// Create a new enumeration that combines households with the random number generator to use during the main algorithm.
        /// </summary>
        /// <param name="households">The stream of households to process</param>
        /// <returns></returns>
        private IEnumerable<(TMGRandom,Household)> CombineWithRandomSeeds(IEnumerable<Household> households)
        {
            var seedBase = RandomSeed.Invoke();
            foreach (var hhld in households)
            {
                yield return (new TMGRandom(hhld.ID * seedBase), hhld);
            }
        }

        /// <summary>
        /// Constructs a pipe depending on the optional submodules for the module.
        /// </summary>
        /// <returns>A pipe that contains all of the contained submodules</returns>
        private IEnumerable<(TMGRandom, Household)> GetPipe()
        {
            if (ModeChoice != null && Scheduler != null)
            {
                return ModeChoice.Invoke(Scheduler.Invoke(CombineWithRandomSeeds(HouseholdLoader.Invoke())));
            }
            if(ModeChoice != null)
            {
                return ModeChoice.Invoke(CombineWithRandomSeeds(HouseholdLoader.Invoke()));
            }
            if(Scheduler != null)
            {
                return Scheduler.Invoke(CombineWithRandomSeeds(HouseholdLoader.Invoke()));
            }
            return CombineWithRandomSeeds(HouseholdLoader.Invoke());
        }

        public override void Invoke()
        {
            using (var pipe = GetPipe().GetEnumerator())
            {
                // keep moving through the pipe to execute all households
                while (pipe.MoveNext()) ;
            }
        }
    }
}
