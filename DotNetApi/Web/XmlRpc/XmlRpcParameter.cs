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
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// A class that represents an XML RPC parameter.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcParameter : XmlRpcObject
	{
		private static string xmlName = "param";

		private XmlRpcValue value = null;

		/// <summary>
		/// Creates a new XML RPC parameter for the specified value.
		/// </summary>
		/// <param name="value">The parameter value.</param>
		public XmlRpcParameter(object value)
		{
			this.value = new XmlRpcValue(value);
		}

		/// <summary>
		/// Creates a new XML RPC parameter from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		public XmlRpcParameter(XElement element)
		{
			if (element.Name.LocalName != XmlRpcParameter.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcParameter.xmlName, element.Name.LocalName));
			// Create the value XML RPC object from the inner XML element.
			this.value = new XmlRpcValue(element.FirstNode as XElement);
		}

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcParameter.xmlName; } }

		/// <summary>
		/// Returns the XML RPC value object corresponding to this parameter.
		/// </summary>
		public XmlRpcValue Value { get { return this.value; } }

		/// <summary>
		/// Returns the XML element correspoding to this parameter.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			return new XElement(XmlRpcParameter.xmlName, this.value.GetXml());
		}
	}
}
