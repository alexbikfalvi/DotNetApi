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
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// An XML RPC boolean object.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcMember : XmlRpcObject
	{
		private static string xmlName = "member";
		private static string xmlNameName = "name";
		private static string xmlNameValue = "value";

		/// <summary>
		/// Creates a new struct member instance from the specified name and value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public XmlRpcMember(string name, object value)
		{
			this.Name = name;
			this.Value = new XmlRpcValue(value);
		}

		/// <summary>
		/// Creates a new struct member instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="format">The format.</param>
		public XmlRpcMember(XElement element, IFormatProvider format)
		{
			if (element.Name.LocalName != XmlRpcMember.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcMember.xmlName, element.Name.LocalName));
			this.Name = element.Element(XmlRpcMember.xmlNameName).Value;
			this.Value = new XmlRpcValue(element.Element(XmlRpcMember.xmlNameValue) as XElement, format);
		}

		// Public properties.

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcMember.xmlName; } }

		/// <summary>
		/// Returns the structure name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Returns the structure value.
		/// </summary>
		public XmlRpcValue Value { get; private set; }

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
			if (obj is XmlRpcMember) return this.Equals(obj as XmlRpcMember);
			return false;
		}

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public bool Equals(XmlRpcMember obj)
		{
			return this.Name.Equals(obj.Name) && this.Value.Equals(obj.Value);
		}

		/// <summary>
		/// Returns the hash code for the current object.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			return this.Name.GetHashCode() ^ this.Value.GetHashCode();
		}

		/// <summary>
		/// Returns the XML element correspoding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			return new XElement(
				XmlRpcMember.xmlName,
				new XElement(XmlRpcMember.xmlNameName, this.Name),
				this.Value.GetXml()
				);
		}

		/// <summary>
		/// Returns the value corresponding to this object.
		/// </summary>
		/// <returns>The object value.</returns>
		public override object GetValue()
		{
			return this.Value;
		}
	}
}
