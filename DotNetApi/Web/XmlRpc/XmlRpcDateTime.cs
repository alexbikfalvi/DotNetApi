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
	public sealed class XmlRpcDateTime : XmlRpcObject
	{
		private static string xmlName = "dateTime.iso8601";

		/// <summary>
		/// Creates a new boolean instance.
		/// </summary>
		/// <param name="value">The boolean value.</param>
		public XmlRpcDateTime(DateTime value)
		{
			this.Value = value;
		}

		/// <summary>
		/// Creates a new boolean instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		public XmlRpcDateTime(XElement element)
		{
			if (element.Name.LocalName != XmlRpcDateTime.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcDateTime.xmlName, element.Name.LocalName));
			this.Value = DateTime.Parse(element.Value);
		}

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcDateTime.xmlName; } }

		/// <summary>
		/// The object value;
		/// </summary>
		public DateTime Value { get; private set; }

		/// <summary>
		/// Returns the XML element correspoding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			return new XElement(XmlRpcDateTime.xmlName, this.Value);
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
