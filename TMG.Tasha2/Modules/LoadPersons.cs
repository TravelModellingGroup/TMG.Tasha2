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
using TMG.Utilities;
using XTMF2;

namespace TMG.Tasha2.Modules
{
    [Module(Name = "Load Persons", Description = "Loads persons from a stream containing CSV data.",
        DocumentationLink = "http://tmg.utoronto.ca/doc/2.0")]
    public class LoadPersons : ObjectStream<int, Person[]>, IDisposable
    {
        [SubModule(Index = 0, Name = "Person Stream", Description = "The stream of CSV data to convert into persons.", Required = true)]
        public IFunction<ReadStream> PersonStream;

        [SubModule(Index = 1, Name = "Load Trips", Description = "", Required = false)]
        public ObjectStream<int, TripChain[]> LoadTrips;

        [SubModule(Index = 2, Name = "Zone System", Description = "The zone system to link for home zones.", Required = true)]
        public IFunction<Categories> ZoneSystem;

        private CsvReader _readStream;
        private bool _alreadyLoaded = false;

        public override void Reset()
        {
            LoadTrips?.Reset();
            _readStream?.Dispose();
            _readStream = null;
        }

        public override Person[] Invoke(int householdId)
        {
            var stream = _readStream;
            if(stream == null)
            {
                stream = new CsvReader(PersonStream?.Invoke());
                // burn the file header
                stream.LoadLine();
                _alreadyLoaded = false;
            }
            if(!_alreadyLoaded)
            {
                stream.LoadLine();
            }
            return null;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _readStream?.Dispose();
                    _readStream = null;
                }
                disposedValue = true;
            }
        }

        ~LoadPersons() {
          // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
          Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
