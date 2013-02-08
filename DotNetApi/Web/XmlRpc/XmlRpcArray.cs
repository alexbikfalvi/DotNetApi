﻿/* 
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
		private static string xmlNameValue = "value";

		private XmlRpcObject[] values;

		/// <summary>
		/// Creates a new array instance from the specified object array.
		/// </summary>
		/// <param name="values">The object array.</param>
		public XmlRpcArray(object[] values)
		{
			this.values = new XmlRpcObject[values.Length];

			for (int index = 0; index < values.Length; index++)
			{
				this.values[index] = XmlRpcObject.Create(values[index]);
			}
		}

		/// <summary>
		/// Creates a new array instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		public XmlRpcArray(XElement element)
		{
			if (element.Name.LocalName != XmlRpcArray.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcArray.xmlName, element.Name.LocalName));
			IEnumerable<XElement> elements = element.Element(XmlRpcArray.xmlNameData).Elements(XmlRpcArray.xmlNameValue);
			this.values = new XmlRpcObject[elements.
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
		public XmlRpcObject this[int index] { get { return this.values[index]; } }

		/// <summary>
		/// Returns the array values.
		/// </summary>
		public XmlRpcObject[] Values { get { return this.values; } }

		/// <summary>
		/// Returns the XML element corresponding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			return null;
		}
	}
}