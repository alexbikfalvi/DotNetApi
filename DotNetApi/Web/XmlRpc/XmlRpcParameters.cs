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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// A list of XML RPC parameters.
	/// </summary>
	[Serializable]
	public class XmlRpcParameters : XmlRpcObject
	{
		private static string xmlName = "params";

		private XmlRpcParameter[] parameters;

		/// <summary>
		/// Creates a new instance of XML RPC parameters.
		/// </summary>
		/// <param name="parameters">The XML RPC parameters.</param>
		public XmlRpcParameters(object[] parameters)
		{
			this.parameters = new XmlRpcParameter[parameters.Length];
			int index = 0;
			foreach (object parameter in parameters)
			{
				this.parameters[index++] = new XmlRpcParameter(parameter);
			}
		}

		/// <summary>
		/// Creates a new instance of XML RPC parameters from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="format">The format.</param>
		public XmlRpcParameters(XElement element, IFormatProvider format)
		{
			if (element.Name.LocalName != XmlRpcParameters.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcParameters.xmlName, element.Name.LocalName));
			// Create the value of each XML RPC parameter from the inner XML elements.
			IEnumerable<XElement> elements = element.Elements();
			// Allocated the parameters list.
			this.parameters = new XmlRpcParameter[Enumerable.Count<XElement>(elements)];
			// Add the parameters.
			int index = 0;
			foreach (XElement el in elements)
			{
				this.parameters[index++] = new XmlRpcParameter(el, format);
			}
		}

		// Public properties.

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcParameters.xmlName; } }

		/// <summary>
		/// Returns the XML RPC parameter at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The XML RPC parameter.</returns>
		public XmlRpcParameter this[int index] { get { return this.parameters[index]; } }

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
			if (obj is XmlRpcParameters) return this.Equals(obj as XmlRpcParameters);
			return false;
		}

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public bool Equals(XmlRpcParameters obj)
		{
			return this.parameters.SequenceEqual(obj.parameters);
		}

		/// <summary>
		/// Returns the hash code for the current object.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int code = 0;
			foreach (XmlRpcParameter parameter in this.parameters) code ^= parameter.GetHashCode();
			return code;
		}

		/// <summary>
		/// Returns the XML element correspoding to this parameters list.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			XElement element = new XElement(XmlRpcParameters.xmlName);

			foreach (XmlRpcParameter parameter in this.parameters)
			{
				element.Add(parameter.GetXml());
			}

			return element;
		}

		/// <summary>
		/// Returns the value corresponding to this object.
		/// </summary>
		/// <returns>The object value.</returns>
		public override object GetValue()
		{
			return this.parameters;
		}
	}
}
