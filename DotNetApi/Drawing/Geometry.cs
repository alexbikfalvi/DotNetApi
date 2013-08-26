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

namespace DotNetApi.Drawing
{
	/// <summary>
	/// A class with useful geometric methods.
	/// </summary>
	public static class Geometry
	{
		/// <summary>
		/// Returns the rectangle region encompassing the two rectangles.
		/// </summary>
		/// <param name="rectangle1">The first rectangle.</param>
		/// <param name="rectangle2">The second rectangle.</param>
		/// <returns>The merged rectangle region.</returns>
		public static Rectangle Merge(Rectangle rectangle1, Rectangle rectangle2)
		{
			int left = rectangle1.X < rectangle2.X ? rectangle1.X : rectangle2.X;
			int top = rectangle1.Y < rectangle2.Y ? rectangle1.Y : rectangle2.Y;
			int right = rectangle1.Right > rectangle2.Right ? rectangle1.Right : rectangle2.Right;
			int bottom = rectangle1.Bottom > rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

			return new Rectangle(left, top, right - left, bottom - top);
		}
	}
}
