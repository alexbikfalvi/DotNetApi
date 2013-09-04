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
using System.Drawing;

namespace DotNetApi.Drawing.Transforms
{
	/// <summary>
	/// A class representing a geometric transform between two points, one fixed and one mobile.
	/// </summary>
	public abstract class Transform
	{
		/// <summary>
		/// Creates a new transform with the default anchor point.
		/// </summary>
		public Transform()
		{
		}

		/// <summary>
		/// Creates a new transform with the specified anchor point.
		/// </summary>
		/// <param name="anchor">The anchor point.</param>
		public Transform(Point anchor)
		{
			this.Anchor = anchor;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the anchor point.
		/// </summary>
		public Point Anchor { get; set; }

		// Public methods.

		/// <summary>
		/// Transforms the specified point.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>The transformed point.</returns>
		public abstract Point GetAbsolute(Point point);
		
		/// <summary>
		/// Transforms the specified point relative to the current anchor.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>The transformed point.</returns>
		public abstract Point GetRelative(Point point);
	}
}
