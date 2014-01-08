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
using System.Text;

namespace DotNetApi
{
	/// <summary>
	/// A class used for data formatting.
	/// </summary>
	public static class FormatExtensions
	{
		/// <summary>
		/// Converts the specified object into an extended string.
		/// </summary>
		/// <param name="value">The object.</param>
		/// <returns>The string.</returns>
		public static string ToExtendedString(this object value)
		{
			if (value is byte[]) return (value as byte[]).ToExtendedString();
			else return value.ToString();
		}

		/// <summary>
		/// Converts the specified object array into their extended string representation.
		/// </summary>
		/// <param name="value">The object array.</param>
		/// <returns>An array with the extended string representation.</returns>
		public static string[] ToExtendedString(this object[] value)
		{
			string[] str = new string[value.Length];
			for (int index = 0; index < value.Length; index++)
				str[index] = value[index] != null ? value[index].ToExtendedString() : null;
			return str;
		}

		/// <summary>
		/// Converts the specified byte array into an extended string.
		/// </summary>
		/// <param name="value">The byte array.</param>
		/// <returns>The string.</returns>
		public static string ToExtendedString(this byte[] value)
		{
			StringBuilder builder = new StringBuilder();
			foreach (byte b in value)
			{
				builder.AppendFormat(CultureInfo.InvariantCulture, "{0:X2} ", b);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Converts the specified array into an extended string.
		/// </summary>
		/// <param name="value">The array.</param>
		/// <returns>The string.</returns>
		public static string ToExtendedString(this Array array)
		{
			StringBuilder builder = new StringBuilder();
			foreach (object obj in array)
			{
				builder.AppendFormat(CultureInfo.InvariantCulture, "{0} ", obj.ToString());
			}
			return builder.ToString();
		}

		/// <summary>
		/// Formats the specified string using the list of arguments and an invariant culture.
		/// </summary>
		/// <param name="format">The string to format.</param>
		/// <param name="args">The list of arguments.</param>
		public static string FormatWith(this string format, params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}

		/// <summary>
		/// Returns the plural suffix for the specified value for an invariant culture.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The plural suffix.</returns>
		public static string PluralSuffix(this byte value)
		{
			return value == 1 ? string.Empty : "s";
		}

		/// <summary>
		/// Returns the plural suffix for the specified value for an invariant culture.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The plural suffix.</returns>
		public static string PluralSuffix(this int value)
		{
			return value == 1 ? string.Empty : "s";
		}

		/// <summary>
		/// Converts the specified longitude to a string.
		/// </summary>
		/// <param name="longitude">The longitude.</param>
		/// <returns>The display string.</returns>
		public static string LongitudeToString(this double longitude)
		{
			double absoluteLongitude = Math.Abs(longitude);

			double degrees = Math.Floor(absoluteLongitude);
			double minutes = (absoluteLongitude - degrees) * 60.0;
			double seconds = (minutes - Math.Floor(minutes)) * 60.0;
			double tenths = (seconds - Math.Floor(seconds)) * 10.0;

			minutes = Math.Floor(minutes);
			seconds = Math.Floor(seconds);
			tenths = Math.Floor(tenths);

			char hemisphere = Math.Sign(longitude) > 0 ? 'E' : Math.Sign(longitude) < 0 ? 'W' : ' ';

			if (tenths != 0.0) return string.Format("{0}° {1}ʹ {2}.{3}ʺ {4}", degrees, minutes, seconds, tenths, hemisphere);
			else if (seconds != 0.0) return string.Format("{0}° {1}ʹ {2}ʺ {3}", degrees, minutes, seconds, hemisphere);
			else if (minutes != 0.0) return string.Format("{0}° {1}ʹ {2}", degrees, minutes, hemisphere);
			else return string.Format("{0}° {1}", degrees, hemisphere);
		}

		/// <summary>
		/// Converts the specified latitude to a string.
		/// </summary>
		/// <param name="latitude">The latitude.</param>
		/// <returns>The display string.</returns>
		public static string LatitudeToString(this double latitude)
		{
			double absoluteLatitude = Math.Abs(latitude);

			double degrees = Math.Floor(absoluteLatitude);
			double minutes = (absoluteLatitude - degrees) * 60.0;
			double seconds = (minutes - Math.Floor(minutes)) * 60.0;
			double tenths = (seconds - Math.Floor(seconds)) * 10.0;

			minutes = Math.Floor(minutes);
			seconds = Math.Floor(seconds);
			tenths = Math.Floor(tenths);

			char hemisphere = Math.Sign(latitude) > 0 ? 'N' : Math.Sign(latitude) < 0 ? 'S' : ' ';

			if (tenths != 0.0) return string.Format("{0}° {1}ʹ {2}.{3}ʺ {4}", degrees, minutes, seconds, tenths, hemisphere);
			else if (seconds != 0.0) return string.Format("{0}° {1}ʹ {2}ʺ {3}", degrees, minutes, seconds, hemisphere);
			else if (minutes != 0.0) return string.Format("{0}° {1}ʹ {2}", degrees, minutes, hemisphere);
			else return string.Format("{0}° {1}", degrees, hemisphere);
		}

		/// <summary>
		/// Converts the specified bitrate to string.
		/// </summary>
		/// <param name="bitrate">The bitrate.</param>
		/// <returns>The string.</returns>
		public static string BitRateToString(this long bitrate)
		{
			if (bitrate > 1000000000000L)
				return "{0:G3} Tb/s".FormatWith((double)bitrate / 1000000000000L);
			else if (bitrate > 1000000000L)
				return "{0:G3} Gb/s".FormatWith((double)bitrate / 1000000000L);
			else if (bitrate > 1000000L)
				return "{0:G3} Mb/s".FormatWith((double)bitrate / 1000000L);
			else if (bitrate > 1000L)
				return "{0:G3} kb/s".FormatWith((double)bitrate / 1000L);
			else
				return "{0} b/s".FormatWith(bitrate);
		}
	}
}
