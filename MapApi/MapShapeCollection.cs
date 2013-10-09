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
	/// A class representing a collection of map shapes.
	/// </summary>
	[Serializable]
	public sealed class MapShapeCollection : IEnumerable<MapShape>
	{
		private readonly List<MapShape> shapes = new List<MapShape>();

		/// <summary>
		/// Creates an empty map part collection.
		/// </summary>
		public MapShapeCollection()
		{
		}

		// Public properties.

		/// <summary>
		/// Gets the number of shapes in the collection.
		/// </summary>
		public int Count { get { return this.shapes.Count; } }

		// Public methods.

		/// <summary>
		/// Returns the generic enumerator for the shape collection.
		/// </summary>
		/// <returns>The generic enumerator.</returns>
		public IEnumerator<MapShape> GetEnumerator()
		{
			return this.shapes.GetEnumerator();
		}

		/// <summary>
		/// Returns the enumerator for the shapes in the collection.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Adds a new shape to the collection.
		/// </summary>
		public void Add(MapShape shape)
		{
			// Add the map part to the collection.
			this.shapes.Add(shape);
		}
	}
}
