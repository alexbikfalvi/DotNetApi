/* 
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
	/// A class representing a collection of map points.
	/// </summary>
	[Serializable]
	public class MapPointCollection : IEnumerable<MapPoint>
	{
		private readonly List<MapPoint> points = new List<MapPoint>();

		/// <summary>
		/// Creates an empty map point collection.
		/// </summary>
		public MapPointCollection()
		{
		}

		// Public properties.

		/// <summary>
		/// Gets the number of points in the collection.
		/// </summary>
		public int Count { get { return this.points.Count; } }

		// Public methods.

		/// <summary>
		/// Returns the generic enumerator for the points collection.
		/// </summary>
		/// <returns>The generic enumerator.</returns>
		public IEnumerator<MapPoint> GetEnumerator()
		{
			return this.points.GetEnumerator();
		}

		/// <summary>
		/// Returns the enumerator for the points in the collection.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Adds a new empty map point to the collection.
		/// </summary>
		public void Add(MapPoint point)
		{
			this.points.Add(point);
		}

		/// <summary>
		/// Adds a new map point to the map part.
		/// </summary>
		/// <param name="x">The X point.</param>
		/// <param name="y">The Y point.</param>
		public void Add(double x, double y)
		{
			this.points.Add(new MapPoint(x, y));
		}
	}
}
