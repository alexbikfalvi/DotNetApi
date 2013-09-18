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
using System.Collections;
using System.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// A class with XML-RPC extension methods.
	/// </summary>
	public static class XmlRpcExtensions
	{
		/// <summary>
		/// Filters the XML-RPC array based on a predicate.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>A new XML-RPC array with the filtered elements.</returns>
		public static XmlRpcArray Where(this XmlRpcArray array, Func<XmlRpcValue, bool> predicate)
		{
			return new XmlRpcArray(array.Values.Where(predicate));
		}

		/// <summary>
		/// Determines whether the array contains the specified objec.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns><b>True</b> if the array contains the value, otherwise <b>false</b>.</returns>
		public static bool Contains(this XmlRpcArray array, object obj)
		{
			foreach (XmlRpcValue value in array.Values)
				if (value.Value.Equals(obj)) return true;
			return false;
		}
	}
}
