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
using System.ComponentModel;
using System.Xml.Serialization;

namespace MapApi
{
	/// <summary>
	/// A structure representing map bounds.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public struct MapRectangle
	{
		/// <summary>
		/// Creates a new map rectangle structure with the specified boundaries.
		/// </summary>
		/// <param name="left">The left boundary.</param>
		/// <param name="top">The top boundary.</param>
		/// <param name="right">The right boundary.</param>
		/// <param name="bottom">The bottom boundary.</param>
		public MapRectangle(double left, double top, double right, double bottom)
			: this()
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the left boundary.
		/// </summary>
		[XmlAttribute("Left")]
		public double Left { get; set; }
		/// <summary>
		/// Gets or sets the top boundary.
		/// </summary>
		[XmlAttribute("Top")]
		public double Top { get; set; }
		/// <summary>
		/// Gets or sets the right boundary.
		/// </summary>
		[XmlAttribute("Right")]
		public double Right { get; set; }
		/// <summary>
		/// Gets or sets the bottom boundary.
		/// </summary>
		[XmlAttribute("Bottom")]
		public double Bottom { get; set; }
		/// <summary>
		/// Gets the bounds width.
		/// </summary>
		public double Width { get { return Math.Abs(this.Left - this.Right); } }
		/// <summary>
		/// Gets the bounds height.
		/// </summary>
		public double Height { get { return Math.Abs(this.Top - this.Bottom); } }
	}
}
