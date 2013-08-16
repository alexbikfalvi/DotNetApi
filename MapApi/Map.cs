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
using System.Xml.Serialization;

namespace MapApi
{
	/// <summary>
	/// A class representing a map.
	/// </summary>
	[Serializable]
	[XmlRoot("Map")]
	public sealed class Map
	{
		private MapRectangle bounds;
		private MapShapeCollection shapes = new MapShapeCollection();

		/// <summary>
		/// Craetes a map with the default bounds.
		/// </summary>
		public Map()
		{
		}

		/// <summary>
		/// Creates a map with the specified bounds.
		/// </summary>
		/// <param name="bounds">The map bounds.</param>
		public Map(MapRectangle bounds)
		{
			this.bounds = bounds;
		}

		// Public properties.

		/// <summary>
		/// Gets the bounds of the current map.
		/// </summary>
		[XmlElement("Bounds")]
		public MapRectangle Bounds { get { return this.bounds; } set { this.bounds = value; } }
		/// <summary>
		/// Gets the collection of shapes for the current map.
		/// </summary>
		[XmlArray("Shapes")]
		[XmlArrayItem("Shape")]
		public MapShapeCollection Shapes { get { return this.shapes; } }
	}
}
