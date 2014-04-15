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
using System.Reflection;

namespace DotNetApi
{
	/// <summary>
	/// A class with enumeration extension methods.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Indicates whether the enumeration value has at least one attribute of the specified type.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="value">The enumeration value.</param>
		/// <returns><b>True</b> if the enumeratin value has an attribute of the specified type, <b>false</b> otherwise.</returns>
		public static bool HasAttribute<T>(this Enum value) where T : Attribute
		{
			Type type = value.GetType();
			MemberInfo[] member = type.GetMember(value.ToString());
			return member[0].GetCustomAttributes(typeof(T), false).Length > 0;
		}

		/// <summary>
		/// Gets the attribute of the specified enumeration value.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="value">The enumeration value.</param>
		/// <returns>The attribute.</returns>
		public static T GetAttribute<T>(this Enum value) where T : Attribute
		{
			Type type = value.GetType();
			MemberInfo[] member = type.GetMember(value.ToString());
			object[] attributes = member[0].GetCustomAttributes(typeof(T), false);
			return attributes[0] as T;
		}

		/// <summary>
		/// Indicates whether the enumeration value has at least one description attribute.
		/// </summary>
		/// <param name="value">The enumeration value.</param>
		/// <returns><b>True</b> if the enumeration has a description attribute, <b>false</b> otherwise.</returns>
		public static bool HasDescription(this Enum value)
		{
			return value.HasAttribute<DescriptionAttribute>();
		}

		/// <summary>
		/// Gets the value of the description attribute for the specified enumeration value.
		/// </summary>
		/// <param name="value">The enumeration value.</param>
		/// <returns>The description string.</returns>
		public static string GetDescription(this Enum value)
		{
			return value.GetAttribute<DescriptionAttribute>().Description;
		}
	}
}
