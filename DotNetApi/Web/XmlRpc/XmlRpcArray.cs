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
using System.Linq;
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// An XML RPC array.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcArray : XmlRpcObject
	{
		private static readonly string xmlName = "array";
		private static readonly string xmlNameData = "data";

		/// <summary>
		/// Creates a new array instance from the specified object array.
		/// </summary>
		/// <param name="values">The object array.</param>
		public XmlRpcArray(Array values)
		{
			this.Values = new XmlRpcValue[values.Length];

			int index = 0;
			foreach (object value in values)
			{
				this.Values[index++] = new XmlRpcValue(value);
			}
		}

		/// <summary>
		/// Creates a new array instance from the specified values array.
		/// </summary>
		/// <param name="values">The values array.</param>
		public XmlRpcArray(IEnumerable<XmlRpcValue> values)
		{
			this.Values = new XmlRpcValue[values.Count()];

			int index = 0;
			foreach (XmlRpcValue value in values)
			{
				this.Values[index++] = value;
			}
		}

		/// <summary>
		/// Creates a new array instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="format">The format.</param>
		public XmlRpcArray(XElement element, IFormatProvider format)
		{
			if (element.Name.LocalName != XmlRpcArray.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcArray.xmlName, element.Name.LocalName));
			// Get the sub-elements of the "data" element.
			IEnumerable<XElement> elements = element.Element(XmlRpcArray.xmlNameData).Elements();
			// Allocate the values array.
			this.Values = new XmlRpcValue[elements.Count()];
			// Add the objects to the values array.
			uint index = 0;
			foreach (XElement el in elements)
			{
				this.Values[index++] = new XmlRpcValue(el, format);
			}
		}

		// Public properties.

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcArray.xmlName; } }

		/// <summary>
		/// Returns the array length.
		/// </summary>
		public int Length { get { return this.Values.Length; } }

		/// <summary>
		/// Returns the value at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The value.</returns>
		public XmlRpcObject this[int index] { get { return this.Values[index].Value; } }

		/// <summary>
		/// Returns the array values.
		/// </summary>
		public XmlRpcValue[] Values { get; private set; }

		// Public methods.

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public override bool Equals(object obj)
		{
			if (null == obj) return false;
			if (object.ReferenceEquals(this, obj)) return true;
			if (obj is XmlRpcArray) return this.Equals(obj as XmlRpcArray);
			return false;
		}

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public bool Equals(XmlRpcArray obj)
		{
			return this.Values.SequenceEqual(obj.Values);
		}

		/// <summary>
		/// Returns the hash code for the current object.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int code = 0;
			foreach (XmlRpcValue value in this.Values) code ^= value.GetHashCode();
			return code;
		}

		/// <summary>
		/// Returns the XML element corresponding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			XElement element = new XElement(XmlRpcArray.xmlNameData);
			foreach (XmlRpcValue value in this.Values)
			{
				element.Add(value.GetXml());
			}
			return new XElement(XmlRpcArray.xmlName, element);
		}

		/// <summary>
		/// Returns the value corresponding to this object.
		/// </summary>
		/// <returns>The object value.</returns>
		public override object GetValue()
		{
			return this.Values;
		}

		/// <summary>
		/// Returns the array of the specified type.
		/// </summary>
		/// <typeparam name="T">The array type.</typeparam>
		/// <returns>The array.</returns>
		public T[] GetArray<T>()
		{
			T[] array = new T[this.Values.Length];
			for (int index = 0; index < this.Values.Length; index++ )
			{
				array[index] = (T)this.Values[index].Value.GetValue();
			}
			return array;
		}
	}
}