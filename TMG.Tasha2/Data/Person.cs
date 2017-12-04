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
using System.Text;

namespace TMG.Tasha2.Data
{
    /// <summary>
    /// Represents a person in the Tasha2 logic.
    /// </summary>
    public sealed class Person : IEnumerable<TripChain>
    {
        /// <summary>
        /// The age of the person
        /// </summary>
        public readonly int Age;

        /// <summary>
        /// Is this person over 18?
        /// </summary>
        public bool Adult => Age >= 18;

        /// <summary>
        /// Is this person under the age of 11
        /// </summary>
        public bool Child => Age < 11;

        /// <summary>
        /// Is this person between [16, 19]
        /// </summary>
        public bool YoungAdult => Age >= 16 & Age <= 19;

        /// <summary>
        /// Is this person older than 11, but not yet 16?
        /// </summary>
        public bool Youth => Age >= 11 & Age <= 15;

        /// <summary>
        /// Is this person Female?
        /// </summary>
        public bool Female { get; private set; }

        /// <summary>
        /// An index into the Occupation Categories
        /// </summary>
        public int Occupation { get; private set; }

        /// <summary>
        /// An index into the Zone System category where this person is employed.
        /// -1 if they do not have an employment zone.
        /// </summary>
        public int EmploymentZone { get; set; } = -1;

        /// <summary>
        /// An index into the Zone System category where this person goes to school.
        /// -1 if they do not have a school zone.
        /// </summary>
        public int SchoolZone { get; set; } = -1;

        /// <summary>
        /// An index into the Employment Statuses
        /// </summary>
        public int EmploymentStatus { get; private set; }

        /// <summary>
        /// Does this person have a driver's license?
        /// </summary>
        public readonly bool DriversLicense;

        private TripChain[] _TripChains;

        public ReadOnlySpan<TripChain> TripChains => new ReadOnlySpan<TripChain>(_TripChains);

        /// <summary>
        /// Create a new person with the given attributes.
        /// </summary>
        /// <param name="age">The age of the person</param>
        /// <param name="female">True if they are female</param>
        /// <param name="dLic">True if they have a driver's license</param>
        /// <param name="employmentStatus">The index into the employment status categories that represents this person</param>
        /// <param name="occupation">The index into the occupation categories that represents this person</param>
        public Person(int age, bool female, bool dLic, int employmentStatus, int occupation)
        {
            Age = age;
            Female = female;
            DriversLicense = dLic;
            EmploymentStatus = employmentStatus;
            Occupation = occupation;
        }

        /// <summary>
        /// Assign trip chains to this individual
        /// </summary>
        /// <param name="chains">The trip chains to assign</param>
        public void SetTrips(TripChain[] chains)
        {
            _TripChains = chains ?? throw new ArgumentNullException(nameof(chains));
        }

        public IEnumerator<TripChain> GetEnumerator() => ((IEnumerable<TripChain>)_TripChains).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _TripChains.GetEnumerator();
    }
}
