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
	/// An XML RPC integer object.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcFault
	{
		private static string xmlFault = "fault";
		private static string xmlValue = "value";
		private static string xmlFaultCode = "faultCode";
		private static string xmlFaultString = "faultString";

		private int faultCode;
		private string faultString;

		/// <summary>
		/// Creates a new XML RPC fault instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		public XmlRpcFault(XElement element)
		{
			if (element.Name.LocalName != XmlRpcFault.xmlFault) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcFault.xmlFault, element.Name.LocalName));
			using (XmlRpcStruct structFault = new XmlRpcStruct(element.Element(XmlRpcFault.xmlValue).FirstNode as XElement))
			{
				this.faultCode = (structFault[XmlRpcFault.xmlFaultCode].Value.Value as XmlRpcInt).Value;
				this.faultString = (structFault[XmlRpcFault.xmlFaultString].Value.Value as XmlRpcString).Value;
			}
		}

		/// <summary>
		/// Returns the fault code.
		/// </summary>
		public int FaultCode { get { return this.faultCode; } }

		/// <summary>
		/// Returns the fault string.
		/// </summary>
		public string FaultString { get { return this.faultString; } }
	}
}
