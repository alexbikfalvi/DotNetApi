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

namespace DotNetApi.Drawing.Imaging
{
	/// <summary>
	/// A class representing a bidimesional kernel used for image processing.
	/// </summary>
	public abstract class Kernel2D
	{
		protected int size;

		/// <summary>
		/// Creates bidimesional kernel instance of the specified size.
		/// </summary>
		/// <param name="size">The size.</param>
		public Kernel2D(int size)
		{
			// Validate the kernel size.
			if (size % 2 != 1) throw new ArgumentException("The kernel size must be an odd number.", "size");
			// Set the kernel size.
			this.size = size;
		}

		// Public properties.

		/// <summary>
		/// Gets the kernel value at the specified point, where (0, 0) is the the kernel center.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <returns>The kernel value.</returns>
		public abstract double this[int x, int y] { get; }

		/// <summary>
		/// Gets the size of the current kernel.
		/// </summary>
		public int Size { get { return this.size; } }
	}
}
