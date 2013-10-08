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
	/// A structure representing a pair of data entries.
	/// </summary>
	/// <typeparam name="TFirst">The first type.</typeparam>
	/// <typeparam name="TSecond">The second type.</typeparam>
	public struct Pair<TFirst, TSecond>
	{
		/// <summary>
		/// Creates a new pair instance.
		/// </summary>
		/// <param name="first">The first value.</param>
		/// <param name="second">The second value.</param>
		public Pair(TFirst first, TSecond second)
		{
			this.First = first;
			this.Second = second;
		}

		/// <summary>
		/// The first value.
		/// </summary>
		public TFirst First;
		/// <summary>
		/// The second value.
		/// </summary>
		public TSecond Second;
	}

	/// <summary>
	/// A structure representing a triplet of data entries.
	/// </summary>
	/// <typeparam name="TFirst">The first type.</typeparam>
	/// <typeparam name="TSecond">The second type.</typeparam>
	/// <typeparam name="TThird">The third type.</typeparam>
	public struct Triple<TFirst, TSecond, TThird>
	{
		/// <summary>
		/// Creates a new pair instance.
		/// </summary>
		/// <param name="first">The first value.</param>
		/// <param name="second">The second value.</param>
		/// <param name="third">The third value.</param>
		public Triple(TFirst first, TSecond second, TThird third)
		{
			this.First = first;
			this.Second = second;
			this.Third = third;
		}

		/// <summary>
		/// The first value.
		/// </summary>
		public TFirst First;
		/// <summary>
		/// The second value.
		/// </summary>
		public TSecond Second;
		/// <summary>
		/// The third value.
		/// </summary>
		public TThird Third;
	}
}
