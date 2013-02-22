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
	/// An XML RPC object.
	/// </summary>
	[Serializable]
	public abstract class XmlRpcObject : IDisposable
	{
		/// <summary>
		/// Create a new object from the specified value. If the value is an <code>XmlRpcObject</code>, the object is returned with
		/// no change. If the value is an <code>XElement</code>, the value is parsed according to the XML RPC rules. Otherwise, the
		/// value is tested in the following order <code>int</code>, <code>boolean</code>, <code>string</code>, <code>double</code>, 
		/// <code>dateTime</code>, <code>base64</code> for <code>byte[]</code>, <code>array</code> and <code>struct</code>.
		/// </summary>
		/// <param name="value">The object value.</param>
		/// <returns>The XML RPC object.</returns>
		public static XmlRpcObject Create(object value)
		{
			if (value.GetType() == typeof(XmlRpcObject)) return value as XmlRpcObject;
			else if (value.GetType() == typeof(XElement)) return XmlRpcObject.Create(value as XElement);
			else if (value.GetType() == typeof(int)) return new XmlRpcInt((int)value);
			else if (value.GetType() == typeof(bool)) return new XmlRpcBoolean((bool)value);
			else if (value.GetType() == typeof(string)) return new XmlRpcString(value as string);
			else if (value.GetType() == typeof(double)) return new XmlRpcDouble((double)value);
			else if (value.GetType() == typeof(DateTime)) return new XmlRpcDateTime((DateTime)value);
			else if (value.GetType() == typeof(byte[])) return new XmlRpcBase64(value as byte[]);
			else if (value.GetType().BaseType == typeof(Array)) return new XmlRpcArray(value as Array);
			else return new XmlRpcStruct(value);
		}

		/// <summary>
		/// Create a new object from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The XML RPC object.</returns>
		public static XmlRpcObject Create(XElement element)
		{
			if (element.Name == XmlRpcInt.XmlName) return new XmlRpcInt(element);
			else if (element.Name == XmlRpcBoolean.XmlName) return new XmlRpcBoolean(element);
			else if (element.Name == XmlRpcString.XmlName) return new XmlRpcString(element);
			else if (element.Name == XmlRpcDouble.XmlName) return new XmlRpcDouble(element);
			else if (element.Name == XmlRpcDateTime.XmlName) return new XmlRpcDateTime(element);
			else if (element.Name == XmlRpcBase64.XmlName) return new XmlRpcBase64(element);
			else if (element.Name == XmlRpcStruct.XmlName) return new XmlRpcStruct(element);
			else if (element.Name == XmlRpcArray.XmlName) return new XmlRpcArray(element);
			else throw new XmlRpcException(string.Format("Unknown XML element \'{0}\'.", element.Name));
		}

		/// <summary>
		/// Returns the XML element corresponding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public abstract XElement GetXml();

		/// <summary>
		/// Disposes the current object.
		/// </summary>
		public void Dispose() { }
	}
}
