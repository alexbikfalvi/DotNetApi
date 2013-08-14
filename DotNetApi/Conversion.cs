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

namespace DotNetApi
{
	/// <summary>
	/// A class used for data type conversions.
	/// </summary>
	public static class Conversion
	{
		/// <summary>
		/// Converts an array of bytes into an array of 32-bit integer values.
		/// </summary>
		/// <param name="bytes">The array of bytes.</param>
		/// <returns>The array of 32-bit integer values.</returns>
		public static Int32[] ToInt32Array(this byte[] bytes)
		{
			// Create the array.
			Int32[] array = new Int32[bytes.Length >> 2];
			// Copy the array elements.
			for (int index = 0; index < array.Length; index++)
			{
				array[index] = BitConverter.ToInt32(bytes, index << 2);
			}
			// Return the array.
			return array;
		}

		/// <summary>
		/// Converts an array of 32-bit integer values into an array of bytes.
		/// </summary>
		/// <param name="array">The array of 32-bit integer values.</param>
		/// <returns>The array of bytes.</returns>
		public static byte[] GetBytes(this Int32[] array)
		{
			// Create the array.
			byte[] bytes = new byte[array.Length << 2];
			// Copy the array elements.
			for (int index = 0; index < array.Length; index++)
			{
				Buffer.BlockCopy(BitConverter.GetBytes(array[index]), 0, bytes, index << 2, 4);
			}
			// Return the array.
			return bytes;
		}
	}
}
