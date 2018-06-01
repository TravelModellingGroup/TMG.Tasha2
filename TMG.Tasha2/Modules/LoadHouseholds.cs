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
using System.Collections.Concurrent;
using System.Threading.Tasks;
using TMG.Utilities;

namespace TMG.Tasha2.Modules
{
    [Module(Name = "Load Households", Description = "Loads households from a stream containing CSV data.",
    DocumentationLink = "http://tmg.utoronto.ca/doc/2.0")]
    public sealed class LoadHouseholds : BaseFunction<IEnumerable<Household>>, IDisposable
    {
        [SubModule(Index = 0, Name = "Household Stream", Description = "The stream of CSV data to convert into households.", Required = true)]
        public IFunction<ReadStream> HouseholdStream;

        [SubModule(Index = 1, Name = "Load Persons", Description = "", Required = true)]
        public IFunction<int, Person[]> LoadPersons;

        [SubModule(Index = 2, Name = "Zone System", Description = "The zone system to link for home zones.", Required = true)]
        public IFunction<Categories> ZoneSystem;

        /// <summary>
        /// Used for moving data to the rest of the pipeline
        /// </summary>
        private BlockingCollection<Household> _stream;

        public override IEnumerable<Household> Invoke()
        {
            _stream?.Dispose(); // cleanup any previous streams
            _stream = new BlockingCollection<Household>(Environment.ProcessorCount * 10);
            Task.Run(() =>
            {
                var zones = ZoneSystem.Invoke();
                using (var reader = new CsvReader(HouseholdStream.Invoke()))
                {
                    // burn the header
                    reader.LoadLine();
                    while (reader.LoadLine(out var columns))
                    {
                        if (columns >= 6)
                        {
                            reader.Get(out int householdID, 0);
                            reader.Get(out int householdZone, 1);
                            reader.Get(out float expFactor, 2);
                            reader.Get(out int dwellingType, 3);
                            reader.Get(out int numberOfPersons, 4);
                            reader.Get(out int numberOfVehicles, 5);
                            _stream.Add(new Household(householdID, zones.GetFlatIndex(householdZone),
                                LoadPersons.Invoke(householdID), numberOfVehicles));
                        }
                    }
                }
                _stream.CompleteAdding();
            });
            return _stream.GetConsumingEnumerable();
        }

        private void Dispose(bool managed)
        {
            if (managed)
            {
                GC.SuppressFinalize(this);
            }
            _stream?.CompleteAdding();
            _stream?.Dispose();
            _stream = null;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~LoadHouseholds()
        {
            Dispose(false);
        }
    }
}
