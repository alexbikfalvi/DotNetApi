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
	/// A class representing a polygon map shape.
	/// </summary>
	public class MapShapePolygon : MapShape
	{
		private readonly MapRectangle bounds;
		private readonly MapPartCollection parts = new MapPartCollection();

		/// <summary>
		/// Creates a new polygon map shape.
		/// </summary>
		/// <param name="bounds">The shape bounds.</param>
		public MapShapePolygon(MapRectangle bounds)
			: base(MapShapeType.Polygon)
		{
			this.bounds = bounds;
		}

		/// <summary>
		/// Creates a new polygon map shape.
		/// </summary>
		/// <param name="metadata">The map metadata.</param>
		/// <param name="bounds">The shape bounds.</param>
		public MapShapePolygon(MapMetadata metadata, MapRectangle bounds)
			: base(MapShapeType.Polygon, metadata)
		{
			this.bounds = bounds;
		}

		// Public properties.

		/// <summary>
		/// Gets the point of the current shape.
		/// </summary>
		public MapPartCollection Parts { get { return this.parts; } }

		// Public methods.

		/// <summary>
		/// Creates an XML element for the current map object.
		/// </summary>
		/// <param name="name">The name of the XML element.</param>
		/// <returns>The XML element.</returns>
		public override System.Xml.Linq.XElement ToXml(string name)
		{
			// Get the XML element from the base class method.
			XElement element = base.ToXml(name);
			// Add the polygon data.
			element.Add(this.bounds.ToXml("Bounds"));
			element.Add(this.parts.ToXml("Parts"));
			// Return the XML element.
			return element;
		}
	}
}
