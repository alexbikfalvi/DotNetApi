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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetApi.Web.Http
{
	/// <summary>
	/// A structure representing an HTTP header and value.
	/// </summary>
	public struct HttpHeader
	{
		private string name;
		private string value;

		/// <summary>
		/// Creates a new HTTP header value.
		/// </summary>
		/// <param name="name">The header name.</param>
		/// <param name="value">The header value.</param>
		public HttpHeader(string name, string value)
		{
			this.name = name;
			this.value = value;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the header name.
		/// </summary>
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		/// <summary>
		/// Gets or sets the header value.
		/// </summary>
		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		// Public methods.

		/// <summary>
		/// Compares the HTTP header with the given object.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the current object is equal to the object, or <b>false</b> otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (null == obj) return false;
			HttpHeader header = (HttpHeader)obj;
			if (null == (object)header) return false;
			return (this.name == header.name) && (this.value == header.value);
		}

		/// <summary>
		/// Returns the hash code of the current object.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			return this.name.GetHashCode() ^ this.value.GetHashCode();
		}

		/// <summary>
		/// Compares two HTTP headers.
		/// </summary>
		/// <param name="left">The left header.</param>
		/// <param name="right">The right header.</param>
		/// <returns><b>True</b> if the two headers are equal, or <b>false</b> otherwise.</returns>
		public static bool operator ==(HttpHeader left, HttpHeader right)
		{
			return (left.name == right.name) && (left.value == right.value);
		}

		/// <summary>
		/// Compares two HTTP headers.
		/// </summary>
		/// <param name="left">The left header.</param>
		/// <param name="right">The right header.</param>
		/// <returns><b>True</b> if the two headers are equal, or <b>false</b> otherwise.</returns>
		public static bool operator !=(HttpHeader left, HttpHeader right)
		{
			return !(left == right);
		}
	}
}
