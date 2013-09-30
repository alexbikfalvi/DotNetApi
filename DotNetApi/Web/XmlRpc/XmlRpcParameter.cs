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
	/// A class that represents an XML RPC parameter.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcParameter : XmlRpcObject
	{
		private const string xmlName = "param";

		/// <summary>
		/// Creates a new XML RPC parameter for the specified value.
		/// </summary>
		/// <param name="value">The parameter value.</param>
		public XmlRpcParameter(object value)
		{
			this.Value = new XmlRpcValue(value);
		}

		/// <summary>
		/// Creates a new XML RPC parameter from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="format">The format.</param>
		public XmlRpcParameter(XElement element, IFormatProvider format)
		{
			if (element.Name.LocalName != XmlRpcParameter.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcParameter.xmlName, element.Name.LocalName));
			// Create the value XML RPC object from the inner XML element.
			this.Value = new XmlRpcValue(element.FirstNode as XElement, format);
		}

		// Public properties.

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcParameter.xmlName; } }

		/// <summary>
		/// Returns the XML RPC value object corresponding to this parameter.
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
			if (obj is XmlRpcParameter) return this.Equals(obj as XmlRpcParameter);
			return false;
		}

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public bool Equals(XmlRpcParameter obj)
		{
			return this.Value.Equals(obj.Value);
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
		/// Returns the XML element correspoding to this parameter.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			return new XElement(XmlRpcParameter.xmlName, this.Value.GetXml());
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
