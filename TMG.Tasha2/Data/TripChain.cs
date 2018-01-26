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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMG.Tasha2.Data
{
    public sealed class TripChain : IEnumerable<Trip>
    {
        private Trip[] _trips;

        public ReadOnlySpan<Trip> Trips => new ReadOnlySpan<Trip>(_trips);

        public Time StartTime => _trips[0].StartTime;

        public Time EndTime => _trips.Last().ActivityStartTime;

        public TripChain(Trip[] trips)
        {
            _trips = trips;
        }

        public IEnumerator<Trip> GetEnumerator() => ((IEnumerable<Trip>)_trips).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _trips.GetEnumerator();

        /// <summary>
        /// Attached values for the trip.
        /// </summary>
        /// <param name="propertyName">The name of the property to access.</param>
        /// <returns>The attached property, or null if it doesn't exist.</returns>
        public object this[string propertyName]
        {
            get
            {
                _attachedProperties.TryGetValue(propertyName, out var ret);
                return ret;
            }
            set => _attachedProperties[propertyName] = value;
        }

        private ConcurrentDictionary<string, object> _attachedProperties = new ConcurrentDictionary<string, object>();
    }
}
