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
	public class XmlRpcBoolean : XmlRpcObject
	{
		private static string xmlName = "boolean";

		/// <summary>
		/// Creates a new boolean instance.
		/// </summary>
		/// <param name="value">The boolean value.</param>
		public XmlRpcBoolean(bool value)
		{
			this.Value = value;
		}

		/// <summary>
		/// Creates a new boolean instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		public XmlRpcBoolean(XElement element)
		{
			if(element.Name.LocalName != XmlRpcBoolean.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcBoolean.xmlName, element.Name.LocalName));

			switch (element.Value.ToLower())
			{
				case "0": this.Value = false; break;
				case "1": this.Value = true; break;
				case "false": this.Value = false; break;
				case "true": this.Value = true; break;
				default: throw new XmlRpcException(string.Format("Unknown boolean value \'{0}\'.", element.Value));
			}
		}

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcBoolean.xmlName; } }

		/// <summary>
		/// The object value;
		/// </summary>
		public bool Value { get; set; }

		/// <summary>
		/// Returns the XML element correspoding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			return new XElement(XmlRpcBoolean.xmlName, this.Value ? "true" : "false");
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
