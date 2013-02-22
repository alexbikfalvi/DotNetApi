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
	/// An XML RPC array.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcArray : XmlRpcObject
	{
		private static string xmlName = "array";
		private static string xmlNameData = "data";

		private XmlRpcValue[] values;

		/// <summary>
		/// Creates a new array instance from the specified object array.
		/// </summary>
		/// <param name="values">The object array.</param>
		public XmlRpcArray(Array values)
		{
			this.values = new XmlRpcValue[values.Length];

			int index = 0;
			foreach (object value in values)
			{
				this.values[index++] = new XmlRpcValue(value);
			}
		}

		/// <summary>
		/// Creates a new array instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		public XmlRpcArray(XElement element)
		{
			if (element.Name.LocalName != XmlRpcArray.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcArray.xmlName, element.Name.LocalName));
			// Get the sub-elements of the "data" element.
			IEnumerable<XElement> elements = element.Element(XmlRpcArray.xmlNameData).Elements();
			// Allocate the values array.
			this.values = new XmlRpcValue[Enumerable.Count<XElement>(elements)];
			// Add the objects to the values array.
			uint index = 0;
			foreach (XElement el in elements)
			{
				this.values[index++] = new XmlRpcValue(el);
			}
		}

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcArray.xmlName; } }

		/// <summary>
		/// Returns the array length.
		/// </summary>
		public int Length { get { return this.values.Length; } }

		/// <summary>
		/// Returns the value at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The value.</returns>
		public XmlRpcObject this[int index] { get { return this.values[index].Value; } }

		/// <summary>
		/// Returns the array values.
		/// </summary>
		public XmlRpcValue[] Values { get { return this.values; } }

		/// <summary>
		/// Returns the XML element corresponding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			XElement element = new XElement(XmlRpcArray.xmlNameData);
			foreach (XmlRpcValue value in this.values)
			{
				element.Add(value.GetXml());
			}
			return new XElement(XmlRpcArray.xmlName, element);
		}
	}
}