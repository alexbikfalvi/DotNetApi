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
using System.Xml.Linq;

namespace MapApi
{
	/// <summary>
	/// A class representing a map part, that is a collection of map points.
	/// </summary>
	public class MapPart : IEnumerable<MapPoint>
	{
		private readonly List<MapPoint> points = new List<MapPoint>();

		/// <summary>
		/// Creates an empty map part.
		/// </summary>
		public MapPart()
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
		/// Adds a new map point to the map part.
		/// </summary>
		/// <param name="x">The X point.</param>
		/// <param name="y">The Y point.</param>
		public void Add(double x, double y)
		{
			this.points.Add(new MapPoint(x, y));
		}

		/// <summary>
		/// Creates an XML element for the current map object.
		/// </summary>
		/// <param name="name">The name of the XML element.</param>
		/// <returns>The XML element.</returns>
		public XElement ToXml(string name)
		{
			// Create the XML element.
			XElement element = new XElement(name);
			foreach (MapPoint point in this)
			{
				element.Add(point.ToXml("Point"));
			}
			// Return the element.
			return element;
		}
	}
}
