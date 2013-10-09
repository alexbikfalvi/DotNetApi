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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// An XML RPC request.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcRequest
	{
		private static readonly string xmlMethodCall = "methodCall";
		private static readonly string xmlMethodName = "methodName";
		private static readonly string xmlParams = "params";
		private static readonly string xmlParam = "param";
		private static readonly string xmlValue = "value";

		/// <summary>
		/// Private constructor.
		/// </summary>
		private XmlRpcRequest() { }

		/// <summary>
		/// Creates a new XML RPC request.
		/// </summary>
		/// <param name="method">The XML RPC call method.</param>
		/// <param name="parameters">The call parameters.</param>
		/// <param name="bom">Specify whether to include the byte-mark-order into the XML byte array.</param>
		/// <returns>The XML document corresponding the request as a UTF-8 encoded byte array.</returns>
		public static byte[] Create(string method, object[] parameters, bool bom = false)
		{
			// Create the method call parameter.
			XElement elementMethodCall = new XElement(XmlRpcRequest.xmlMethodCall,
					new XElement(XmlRpcRequest.xmlMethodName, method)
					);
			// If the parameters argument is not null.
			if (parameters != null)
			{
				// Create a parameters element.
				XElement elementParams = new XElement(XmlRpcRequest.xmlParams);
				// Add all parameters to the parameters element.
				foreach (object parameter in parameters)
				{
					XElement elementParam = new XElement(XmlRpcRequest.xmlParam, new XElement(XmlRpcRequest.xmlValue, XmlRpcObject.Create(parameter).GetXml()));
					elementParams.Add(elementParam);
				}
				// Add the parameters element to the method call.
				elementMethodCall.Add(elementParams);
			}
			using(MemoryStream stream = new MemoryStream())
			{
				// Create a new XML document for the method.
				XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", null), elementMethodCall);
				// Create the encoding used to write the XML to the byte array.
				UTF8Encoding encoding = new UTF8Encoding(bom);
				// Create an XML writer using UTF-8 encoding to write the XML document.
				using (XmlWriter xmlWriter = new XmlTextWriter(stream, encoding))
				{
					// Save the XML document to the memory stream using the XML writer.
					document.Save(xmlWriter);
					// Flush the XML writer to the memory stream.
					xmlWriter.Flush();
					// Reset the memory stream to the begining.
					stream.Seek(0, SeekOrigin.Begin);
					// Create a binary reader to read the stream into a byte array.
					using (BinaryReader reader = new BinaryReader(stream))
					{
						// Read the stream into the byte array.
						return reader.ReadBytes((int)stream.Length);
					}
				}
			}
		}
	}
}
