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
		private static string xmlFault = "fault";

		private XmlRpcObject value = null;
		private XmlRpcFault fault = null;

		private string xml = null;

		/// <summary>
		/// Private constructor.
		/// </summary>
		/// <param name="xml">The XML string corresponding to this response.</param>
		private XmlRpcResponse(string xml)
		{
			// Set the XML string.
			this.xml = xml;
			
			// Parse the XML string to an XML document.
			XDocument document = XDocument.Parse(xml);
			XElement element;

			// Check the XML root element.
			if (document.Root.Name.LocalName != XmlRpcResponse.xmlMethodResponse) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcResponse.xmlMethodResponse, document.Root.Name.LocalName));

			// Parse the XML from the response parameter, if any.
			if(null != (element = document.Root.Element(XmlRpcResponse.xmlParams)))
			{
				try { this.value = (new XmlRpcParameters(element))[0].Value.Value; }
				catch (Exception exception) { throw new XmlRpcException(string.Format("Cannot parse the XML element. {0}", exception.Message), exception); }
			}

			// Parse the XML from the response fault, if any.
			if (null != (element = document.Root.Element(XmlRpcResponse.xmlFault)))
			{
				try { this.fault = new XmlRpcFault(element); }
				catch (Exception exception) { throw new XmlRpcException(string.Format("Cannot parse the XML element. {0}", exception.Message), exception); }
			}
		}

		/// <summary>
		/// Creates an XML RPC response.
		/// </summary>
		/// <param name="xml">The XML string representing the response.</param>
		/// <returns>The XML RPC response object.</returns>
		public static XmlRpcResponse Create(string xml)
		{
			XElement element = XDocument.Parse(xml).Root;

			// Check the XML element name.
			if (element.Name.LocalName != XmlRpcResponse.xmlMethodResponse) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcResponse.xmlMethodResponse, element.Name.LocalName));

			// Create and return a new response object.
			return new XmlRpcResponse(xml);
		}

		/// <summary>
		/// Returns the value of the current response, or <b>null</b> if no value exists.
		/// </summary>
		public XmlRpcObject Value { get { return this.value; } }

		/// <summary>
		/// Returns the fault of the current response, or <b>null</b> if no fault exists.
		/// </summary>
		public XmlRpcFault Fault { get { return this.fault; } }

		/// <summary>
		/// Returns the XML string corresponding to this response.
		/// </summary>
		public string Xml { get { return this.xml; } }
	}
}
