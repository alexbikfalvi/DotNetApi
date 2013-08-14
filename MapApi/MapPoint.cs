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

namespace MapApi
{
	/// <summary>
	/// A structure representing a map point.
	/// </summary>
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
		public double X;
		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		public double Y;
	}
}
