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
	/// A structure representing a map point.
	/// </summary>
	[Serializable]
	public struct MapPoint
	{
		/// <summary>
		/// Creates a new map point instance.
		/// </summary>
		/// <param name="x">The longitude.</param>
		/// <param name="y">The latitude.</param>
		public MapPoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Public fields.

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		[XmlAttribute("X")]
		public double X;
		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		[XmlAttribute("Y")]
		public double Y;

		// Public methods.

		/// <summary>
		/// Compares two map point for equality.
		/// </summary>
		/// <param name="point1">The left map point.</param>
		/// <param name="point2">The right map point.</param>
		/// <returns><b>True</b> if the two map points are equal, <b>false</b> otherwise.</returns>
		public static bool operator ==(MapPoint point1, MapPoint point2)
		{
			return (point1.X == point2.X) && (point1.Y == point2.Y);
		}

		/// <summary>
		/// Compares two map point for equality.
		/// </summary>
		/// <param name="point1">The left map point.</param>
		/// <param name="point2">The right map point.</param>
		/// <returns><b>True</b> if the two map points are different, <b>true</b> otherwise.</returns>
		public static bool operator !=(MapPoint point1, MapPoint point2)
		{
			return (point1.X != point2.X) || (point1.Y != point2.Y);
		}

		/// <summary>
		/// Compares the current and specified map points for equality.s
		/// </summary>
		/// <param name="obj">The map point to compare.</param>
		/// <returns><b>True</b> if the two map points are equal, <b>false</b> otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (null == obj) return false;
			if (!(obj is MapPoint)) return false;
			return this == ((MapPoint)obj);
		}

		/// <summary>
		/// Returns the hash code of the current map point.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}
	}
}
