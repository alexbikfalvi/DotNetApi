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
	/// A class with array extensions methods.
	/// </summary>
	public static class ArrayExtensions
	{
		/// <summary>
		/// Sets all values in the array with the specified value.
		/// </summary>
		/// <typeparam name="T">The array type.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="value">The value.</param>
		public static void Set<T>(this T[] array, T value)
		{
			for (int index = 0; index < array.Length; index++)
			{
				array[index] = value;
			}
		}

		/// <summary>
		/// Sets all values in the array at the given range with the specified value.
		/// </summary>
		/// <typeparam name="T">The array type.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="value">The value.</param>
		/// <param name="start">The start index.</param>
		/// <param name="length">The number of elements to set.</param>
		public static void Set<T>(this T[] array, T value, int start, int length)
		{
			for (int index = start, count = 0; (index < array.Length) && (count < length); index++, count++)
			{
				array[index] = value;
			}
		}
	}
}
