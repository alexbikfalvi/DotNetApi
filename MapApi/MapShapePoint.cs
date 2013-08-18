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
	/// A class representing a point map shape.
	/// </summary>
	[Serializable]
	public sealed class MapShapePoint : MapShape
	{
		private MapPoint point;

		/// <summary>
		/// Creates a new point map shape.
		/// </summary>
		public MapShapePoint()
			: base(MapShapeType.Point)
		{
		}

		/// <summary>
		/// Creates a new point map shape.
		/// </summary>
		/// <param name="point">The map point.</param>
		public MapShapePoint(MapPoint point)
			: base(MapShapeType.Point)
		{
			this.point = point;
		}

		/// <summary>
		/// Creates a new point map shape.
		/// </summary>
		/// <param name="metadata">The map metadata.</param>
		/// <param name="point">The map point.</param>
		public MapShapePoint(MapMetadata metadata, MapPoint point)
			: base(MapShapeType.Point, metadata)
		{
			this.point = point;
		}

		// Public properties.

		/// <summary>
		/// Gets the point of the current shape.
		/// </summary>
		public MapPoint Point { get { return this.point; } set { this.point = value; } }
	}
}
