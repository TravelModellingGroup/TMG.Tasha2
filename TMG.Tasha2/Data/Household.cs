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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMG;

namespace TMG.Tasha2.Data
{
    /// <summary>
    /// Represents a single household
    /// </summary>
    public sealed class Household : IEnumerable<Person>
    {
        /// <summary>
        /// A unique identifier for this household.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// The index in the current zone system that this
        /// household resides in.
        /// </summary>
        public CategoryIndex HouseholdZone { get; private set; }

        /// <summary>
        /// The number of vehicles that are available to the household
        /// </summary>
        public int Vehicles { get; private set; }

        private Person[] _Persons;

        /// <summary>
        /// Get a referecne to the persons available in the household
        /// </summary>
        public ReadOnlySpan<Person> Persons => new ReadOnlySpan<Person>(_Persons);

        private int _NumberOfAdults = -1;

        /// <summary>
        /// Get the number of persons who are adults.
        /// </summary>
        public int NumberOfAdults => _NumberOfAdults >= 0 ? _NumberOfAdults
                    : (_NumberOfAdults = _Persons.Count(p => p.Adult));

        /// <summary>
        /// Get the number of persons who are children.
        /// </summary>
        public int NumberOfChildren => _Persons.Length - NumberOfAdults;

        public Household(int id, CategoryIndex householdZone, Person[] persons, int vehicles)
        {
            ID = id;
            HouseholdZone = householdZone;
            _Persons = persons;
            Vehicles = vehicles;
        }

        public IEnumerator<Person> GetEnumerator() => ((IEnumerable<Person>)_Persons).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Persons.GetEnumerator();
    }
}
