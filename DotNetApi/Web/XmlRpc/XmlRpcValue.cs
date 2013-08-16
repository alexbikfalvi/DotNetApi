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
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// A class that represents an XML RPC value.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcValue : XmlRpcObject
	{
		private static string xmlName = "value";

		/// <summary>
		/// Creates a new XML RPC parameter for the specified value.
		/// </summary>
		/// <param name="value">The parameter value.</param>
		public XmlRpcValue(object value)
		{
			this.Value = XmlRpcObject.Create(value);
		}

		/// <summary>
		/// Creates a new XML-RPC value object from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		public XmlRpcValue(XElement element)
		{
			if (element.Name.LocalName != XmlRpcValue.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcValue.xmlName, element.Name.LocalName));
			// Create the value XML RPC object from the inner XML element.
			this.Value = XmlRpcObject.Create(element.Elements().FirstOrDefault());
		}

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcValue.xmlName; } }

		/// <summary>
		/// Gets the XML RPC object corresponding to this value.
		/// </summary>
		public XmlRpcObject Value { get; private set; }

		/// <summary>
		/// Returns the XML element correspoding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			return new XElement(XmlRpcValue.xmlName, this.Value.GetXml());
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
