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
	/// The enumeration of map shape types according to ESRI standard.
	/// </summary>
	public enum MapShapeType
	{
		Null = 0,
		Point = 1,
		PolyLine = 3,
		Polygon = 5,
		MultiPoint = 8,
		PointZ = 11,
		PolyLineZ = 13,
		PolygonZ = 15,
		MultiPointZ = 18,
		PointM = 21,
		PolyLineM = 23,
		PolygonM = 25,
		MultiPointM = 28,
		MultiPatch = 31
	}

	/// <summary>
	/// A base class for all map shapes.
	/// </summary>
	[Serializable]
	[XmlInclude(typeof(MapShapePoint))]
	[XmlInclude(typeof(MapShapePolygon))]
	public abstract class MapShape
	{
		private MapShapeType type;
		private readonly MapMetadata metadata;

		/// <summary>
		/// Creates a new map shape of the specified type and with an empty metadata collection.
		/// </summary>
		/// <param name="type">The shape type.</param>
		public MapShape(MapShapeType type)
		{
			this.type = type;
			this.metadata = new MapMetadata();
		}

		/// <summary>
		/// Creates a new shape with the specified type and metadata.
		/// </summary>
		/// <param name="type">The shape type.</param>
		/// <param name="metadata">The shape metadata.</param>
		public MapShape(MapShapeType type, MapMetadata metadata)
		{
			this.type = type;
			this.metadata = metadata;
		}

		// Public properties.

		/// <summary>
		/// Gets the type of the current shape.
		/// </summary>
		[XmlAttribute("Type")]
		public MapShapeType Type { get { return this.type; } }
		/// <summary>
		/// Gets the metadata of the current shape.
		/// </summary>
		[XmlArray("Metadata")]
		[XmlArrayItem("Entry")]
		public MapMetadata Metadata { get { return this.metadata; } }
	}
}
