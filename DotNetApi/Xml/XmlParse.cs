/* 
 * Copyright (C) 2012-2013 Alex Bikfalvi
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
using System.Globalization;

namespace DotNetApi.Xml
{
	/// <summary>
	/// An extension XML class used for string parsing.
	/// </summary>
	public static class XmlParse
	{
		/// <summary>
		/// Parses the specified value string to a boolean.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <returns>The boolean value.</returns>
		public static bool ToBoolean(this string value)
		{
			return bool.Parse(value);
		}

		/// <summary>
		/// Parses the specified value string to a signed integer.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <returns>The signed integer value.</returns>
		public static int ToInt(this string value)
		{
			return int.Parse(value);
		}

		/// <summary>
		/// Parses the specified value string to a signed integer.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <param name="defaultValue">The default value, if the string cannot be parsed.</param>
		/// <returns>The signed integer value.</returns>
		public static int ToInt(this string value, int defaultValue)
		{
			int result;
			return int.TryParse(value, out result) ? result : defaultValue;
		}

		/// <summary>
		/// Parses the specified value string to a signed integer, using the specified style for an invariant culture.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <param name="style">The number style.</param>
		/// <param name="defaultValue">The default value, if the string cannot be parsed.</param>
		/// <returns>The signed integer value.</returns>
		public static int ToInt(this string value, NumberStyles style, int defaultValue)
		{
			int result;
			return int.TryParse(value, style, NumberFormatInfo.InvariantInfo, out result) ? result : defaultValue;
		}

		/// <summary>
		/// Parses the specified value string to an unsigned integer.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <returns>The unsigned integer value.</returns>
		public static uint ToUint(this string value)
		{
			return uint.Parse(value);
		}

		/// <summary>
		/// Parses the specified value string to a signed 64-bit integer.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <returns>The signed 64-bit integer value.</returns>
		public static Int64 ToInt64(this string value)
		{
			return Int64.Parse(value);
		}

		/// <summary>
		/// Parses the specified value string to a double floating-point number.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <returns>The double value.</returns>
		public static double ToDouble(this string value)
		{
			return double.Parse(value);
		}

		/// <summary>
		/// Parses the specified value string to a double floating-point number.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <param name="defaultValue">The default value, if the string cannot be parsed.</param>
		/// <returns>The double value.</returns>
		public static double ToDouble(this string value, double defaultValue)
		{
			double result;
			return double.TryParse(value, out result) ? result : defaultValue;
		}

		/// <summary>
		/// Parses the specified value string to a decimal number.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <returns>The decimal value.</returns>
		public static decimal ToDecimal(this string value)
		{
			return decimal.Parse(value);
		}

		/// <summary>
		/// Parses the specified value string to a decimal number.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <param name="defaultValue">The default value, if the string cannot be parsed.</param>
		/// <returns>The decimal value.</returns>
		public static decimal ToDecimal(this string value, decimal defaultValue)
		{
			decimal result;
			return decimal.TryParse(value, out result) ? result : defaultValue;
		}

		/// <summary>
		/// Parses the specified value string to a date-time.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <returns>The date-time value.</returns>
		public static DateTime ToDateTime(this string value)
		{
			return DateTime.Parse(value);
		}

		/// <summary>
		/// Parses the specified value string to a date-time.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <param name="defaultValue">The default value, if the string cannot be parsed.</param>
		/// <returns>The date-time value.</returns>
		public static DateTime ToDateTime(this string value, DateTime defaultValue)
		{
			DateTime result;
			return DateTime.TryParse(value, out result) ? result : defaultValue;
		}

		/// <summary>
		/// Parses the specified value string to a time-span.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <returns>The time-span value.</returns>
		public static TimeSpan ToTimeSpan(this string value)
		{
			return TimeSpan.Parse(value);
		}

		/// <summary>
		/// Parses the specified value string to a time-span.
		/// </summary>
		/// <param name="value">The value string to parse.</param>
		/// <param name="defaultValue">The default value, if the string cannot be parsed.</param>
		/// <returns>The time-span value.</returns>
		public static TimeSpan ToTimeSpan(this string value, TimeSpan defaultValue)
		{
			TimeSpan result;
			return TimeSpan.TryParse(value, out result) ? result : defaultValue;
		}

		/// <summary>
		/// Converts the specified string to a URI.
		/// </summary>
		/// <param name="value">The string to convert.</param>
		/// <returns>The URI.</returns>
		public static Uri ToUri(this string value)
		{
			return new Uri(value);
		}
	}
}
