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
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// An XML RPC string object.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcString : XmlRpcObject
	{
		private static readonly string xmlName = "string";

		/// <summary>
		/// Creates a new string instance.
		/// </summary>
		/// <param name="value">The string value.</param>
		public XmlRpcString(string value)
		{
			this.Value = value;
		}

		/// <summary>
		/// Creates a new boolean instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		public XmlRpcString(XElement element)
		{
			if (element.Name.LocalName != XmlRpcString.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcString.xmlName, element.Name.LocalName));
			// Set the value to the unescaped value of the XML element.
			this.Value = XmlRpcString.Unescape(element.Value);
		}

		// Public properties.

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcString.xmlName; } }

		/// <summary>
		/// The original string value (unescaped).
		/// </summary>
		public string Value { get; private set; }

		// Public methods.

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (null == obj) return false;
			if (object.ReferenceEquals(this, obj)) return true;
			if (obj is XmlRpcString) return this.Equals(obj as XmlRpcString);
			if (obj is string) return this.Equals((string)obj);
			return false;
		}

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public bool Equals(XmlRpcString obj)
		{
			return this.Value == obj.Value;
		}

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public bool Equals(string obj)
		{
			return this.Value == obj;
		}

		/// <summary>
		/// Returns the hash code for the current object.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		/// <summary>
		/// Returns the XML element correspoding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			// Return an XML element with the escaped version of the string.
			return new XElement(XmlRpcString.xmlName, XmlRpcString.Escape(this.Value));
		}

		/// <summary>
		/// Returns the value corresponding to this object.
		/// </summary>
		/// <returns>The object value.</returns>
		public override object GetValue()
		{
			return this.Value;
		}

		/// <summary>
		/// Escapes the string value. 
		/// </summary>
		/// <param name="value">The string value to escape.</param>
		/// <returns>The escaped string value.</returns>
		private static string Escape(string value)
		{
			// Replace the "&" and "<" in the unescaped version of the argument string.
			string str = Regex.Replace(XmlRpcString.Unescape(value), "&", "&amp;");
			return Regex.Replace(str, "<", "&lt;");
		}

		/// <summary>
		/// Unescapes the string value.
		/// </summary>
		/// <param name="value">The string value to unescape.</param>
		/// <returns>The unescaped string value.</returns>
		private static string Unescape(string value)
		{
			// Replace the "&amp;" and "&lt;" in the argument string.
			string str = Regex.Replace(value, "&amp;", "&");
			return Regex.Replace(str, "&lt;", "<");
		}
	}
}
