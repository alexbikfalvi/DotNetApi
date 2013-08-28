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
	/// A structure representing map size.
	/// </summary>
	[Serializable]
	public struct MapSize
	{
		/// <summary>
		/// Creates a new map rectangle structure with the specified boundaries.
		/// </summary>
		/// <param name="width">The size width.</param>
		/// <param name="height">The size height.</param>
		public MapSize(double width, double height)
		{
			this.Width = width;
			this.Height = height;
		}

		// Public fields.

		/// <summary>
		/// Gets or sets the size width.
		/// </summary>
		[XmlAttribute("Width")]
		public double Width;
		/// <summary>
		/// Gets or sets the size height.
		/// </summary>
		[XmlAttribute("Height")]
		public double Height;
	}
}
