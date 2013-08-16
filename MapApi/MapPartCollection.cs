﻿/* 
 * Copyright (C) 2013 Alex Bikfalvi
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace MapApi
{
	/// <summary>
	/// A class representing a collection of map parts.
	/// </summary>
	[Serializable]
	public class MapPartCollection : IEnumerable<MapPart>
	{
		private readonly List<MapPart> parts = new List<MapPart>();

		/// <summary>
		/// Creates an empty map part collection.
		/// </summary>
		public MapPartCollection()
		{
		}

		// Public properties.

		/// <summary>
		/// Gets the number of points in the collection.
		/// </summary>
		public int Count { get { return this.parts.Count; } }

		// Public methods.

		/// <summary>
		/// Returns the generic enumerator for the parts collection.
		/// </summary>
		/// <returns>The generic enumerator.</returns>
		public IEnumerator<MapPart> GetEnumerator()
		{
			return this.parts.GetEnumerator();
		}

		/// <summary>
		/// Returns the enumerator for the parts in the collection.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Adds a new empty map part to the collection.
		/// </summary>
		public void Add(MapPart part)
		{
			this.parts.Add(part);
		}
	}
}
