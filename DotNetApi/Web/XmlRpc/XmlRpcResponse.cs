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
	/// An XML RPC response.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcResponse
	{
		private static string xmlMethodResponse = "methodResponse";
		private static string xmlParams = "params";
		private static string xmlParam = "param";
		private static string xmlValue = "value";
		private static string xmlFault = "fault";

		private XmlRpcObject value = null;
		private XmlRpcFault fault = null;

		/// <summary>
		/// Private constructor.
		/// </summary>
		private XmlRpcResponse() { }

		/// <summary>
		/// Creates an XML RPC response.
		/// </summary>
		/// <param name="xml">The XML string representing the response.</param>
		/// <returns>The XML RPC response object.</returns>
		public static XmlRpcResponse Create(string xml)
		{
			XElement element = XDocument.Parse(xml).Root;

			if (element.Name.LocalName != XmlRpcResponse.xmlMethodResponse) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcResponse.xmlMethodResponse, element.Name.LocalName));

			XmlRpcResponse response = new XmlRpcResponse();

			return response;
		}
	}
}
