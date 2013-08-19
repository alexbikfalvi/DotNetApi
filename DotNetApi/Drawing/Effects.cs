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
	/// A class with graphics effects extension methods.
	/// </summary>
	public static class Effects
	{
		/// <summary>
		/// Draws a shadow around the specified rectangle.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		/// <param name="shadow">The shadow.</param>
		public static void DrawShadow(this Graphics graphics, Shadow shadow, Rectangle rectangle)
		{
			// Call the effect drawing methods.
			shadow.Draw(graphics, rectangle);
		}
	}
}
