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

		/// <summary>
		/// Private constructor.
		/// </summary>
		/// <param name="xml">The XML string corresponding to this response.</param>
		/// <param name="format">The format.</param>
		private XmlRpcResponse(string xml, IFormatProvider format)
		{
			// Set the XML string.
			this.Xml = xml;
			
			// Parse the XML string to an XML document.
			XDocument document = XDocument.Parse(xml);
			XElement element;

			// Check the XML root element.
			if (document.Root.Name.LocalName != XmlRpcResponse.xmlMethodResponse) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcResponse.xmlMethodResponse, document.Root.Name.LocalName));

			// Parse the XML from the response parameter, if any.
			if(null != (element = document.Root.Element(XmlRpcResponse.xmlParams)))
			{
				try { this.Value = (new XmlRpcParameters(element, format))[0].Value.Value; }
				catch (Exception exception) { throw new XmlRpcException(string.Format("Cannot parse the XML element. {0}", exception.Message), exception); }
			}

			// Parse the XML from the response fault, if any.
			if (null != (element = document.Root.Element(XmlRpcResponse.xmlFault)))
			{
				try { this.Fault = new XmlRpcFault(element, format); }
				catch (Exception exception) { throw new XmlRpcException(string.Format("Cannot parse the XML element. {0}", exception.Message), exception); }
			}
		}

		/// <summary>
		/// Creates an XML RPC response.
		/// </summary>
		/// <param name="xml">The XML string representing the response.</param>
		/// <param name="format">The format.</param>
		/// <returns>The XML RPC response object.</returns>
		public static XmlRpcResponse Create(string xml, IFormatProvider format)
		{
			XElement element = XDocument.Parse(xml).Root;

			// Check the XML element name.
			if (element.Name.LocalName != XmlRpcResponse.xmlMethodResponse) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcResponse.xmlMethodResponse, element.Name.LocalName));

			// Create and return a new response object.
			return new XmlRpcResponse(xml, format);
		}

		/// <summary>
		/// Returns the value of the current response, or <b>null</b> if no value exists.
		/// </summary>
		public XmlRpcObject Value { get; private set; }

		/// <summary>
		/// Returns the fault of the current response, or <b>null</b> if no fault exists.
		/// </summary>
		public XmlRpcFault Fault { get; private set; }

		/// <summary>
		/// Returns the XML string corresponding to this response.
		/// </summary>
		public string Xml { get; private set; }
	}
}
