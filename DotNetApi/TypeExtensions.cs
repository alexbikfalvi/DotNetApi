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
using System.Linq;

namespace DotNetApi
{
	/// <summary>
	/// An class with type extension methods.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Checks whether the given type is nullable.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><b>True</b> if the type is nullable, <b>false</b> otherwise.</returns>
		public static bool IsNullable(this Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}

		/// <summary>
		/// Checks whether the given type is assignable from the specified type.
		/// </summary>
		/// <param name="type">The given type.</param>
		/// <param name="otherType">The other type.</param>
		/// <returns><b>True</b> if the type is assignable to the specified generic type, <b>false</b> otherwise.</returns>
		public static bool IsAssignableToInterface(this Type type, Type otherType)
		{
			// For all interfaces.
			foreach (Type iface in type.GetInterfaces())
				if (iface == otherType)
					return true;
			// Else, return false.
			return false;
		}

		/// <summary>
		/// Checks whether the given type is assignable from the specified type.
		/// </summary>
		/// <param name="type">The given type.</param>
		/// <param name="otherType">The other type.</param>
		/// <returns><b>True</b> if the type is assignable to the specified generic type, <b>false</b> otherwise.</returns>
		public static bool IsAssignableToGenericInterface(this Type type, Type otherType)
		{
			// If the generic type is not generic.
			if (!otherType.IsGenericType)
			{
				return otherType.IsAssignableFrom(type);
			}
			// Get the number of generic arguments.
			int count = otherType.GetGenericArguments().Count();
			// For all interfaces.
			foreach (Type iface in type.GetInterfaces())
				if (iface.IsGenericType)
					if ((iface.GetGenericTypeDefinition() == otherType) && (iface.GetGenericArguments().Count() == count))
						return true;
			// Else, return false.
			return false;
		}

		/// <summary>
		/// Returns the underlying type of a nullable type.
		/// </summary>
		/// <param name="type">The nullable type</param>
		/// <returns>The underlying type of a nullable type, or the specified type if the type is not nullable.</returns>
		public static Type GetNullable(this Type type)
		{
			return type.IsNullable() ? type.GetGenericArguments()[0] : type;
		}

		/// <summary>
		/// Returns the name for this type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The type name.</returns>
		public static string GetName(this Type type)
		{
			if (type.IsNullable()) return "*{0}".FormatWith(type.GetNullable().Name);
			else if (type.IsGenericType) return type.Name.Remove(type.Name.IndexOf('`'));
			else return type.Name;
		}
	}
}
