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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// An XML RPC structure.
	/// </summary>
	[Serializable]
	public class XmlRpcStruct : XmlRpcObject
	{
		private static readonly string xmlName = "struct";

		private readonly Dictionary<string, XmlRpcMember> members = new Dictionary<string, XmlRpcMember>();

		/// <summary>
		/// Creates a new empty structure.
		/// </summary>
		protected XmlRpcStruct() { }

		/// <summary>
		/// Creates a new struct instance from the specified class object. Only public properties are added to the struct as members.
		/// </summary>
		/// <param name="element">The object.</param>
		public XmlRpcStruct(object obj)
		{
			foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
			{
				if (propertyInfo.CanRead)
				{
					// The member name.
					string name = propertyInfo.Name;
					// Get the display name attribute of the property.
					object[] displayNames = propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
					// If the attributes set is not null.
					if (displayNames != null)
					{
						if (displayNames.Length != 0)
						{
							name = (displayNames[0] as DisplayNameAttribute).DisplayName;
						}
					}
					// Check there is no other member with the same name.
					if (this.members.ContainsKey(name)) throw new XmlRpcException(string.Format("A structure field with the name \'{0}\' already exists.", name));
					// Create a new member for each public property.
					XmlRpcMember member = new XmlRpcMember(
						name,
						propertyInfo.GetGetMethod().Invoke(obj, null));
					// Add the member to the struct list.
					this.members.Add(name, member);
				}
			}
		}

		/// <summary>
		/// Creates a new struct instance from the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="format">The format.</param>
		public XmlRpcStruct(XElement element, IFormatProvider format)
		{
			if (element.Name.LocalName != XmlRpcStruct.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcStruct.xmlName, element.Name.LocalName));
			foreach (XElement child in element.Elements(XmlRpcMember.XmlName))
			{
				// Create a new member for each public property.
				XmlRpcMember member = new XmlRpcMember(child, format);
				// Add the member to the struct list.
				this.members.Add(member.Name, member);
			}		
		}

		/// <summary>
		/// Creates a new struct instance with a single member.
		/// </summary>
		/// <param name="name">The member name.</param>
		/// <param name="value">The member value.</param>
		public XmlRpcStruct(string name, object value)
		{
			// Create a new member with the specified name.
			XmlRpcMember member = new XmlRpcMember(name, value);
			// Add the member to the struct list.
			this.members.Add(name, member);
		}

		// Public properties.

		/// <summary>
		/// Returns the XML name.
		/// </summary>
		public static string XmlName { get { return XmlRpcStruct.xmlName; } }

		/// <summary>
		/// Returns the member for the specified name
		/// </summary>
		/// <param name="name">The member name.</param>
		/// <returns>The structure member.</returns>
		public XmlRpcMember this[string name] { get { return this.members[name]; } }

		/// <summary>
		/// Returns the structure members.
		/// </summary>
		public ICollection<KeyValuePair<string, XmlRpcMember>> Members { get { return this.members; } }

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
			if (obj is XmlRpcStruct) return this.Equals(obj as XmlRpcStruct);
			return false;
		}

		/// <summary>
		/// Compares this object with the specified argument.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two objects are equal, <b>false</b> otherwise.</returns>
		public bool Equals(XmlRpcStruct obj)
		{
			return this.members.SequenceEqual(obj.members);
		}

		/// <summary>
		/// Returns the hash code for the current object.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			int code = 0;
			foreach (KeyValuePair<string, XmlRpcMember> member in this.members) code ^= member.Key.GetHashCode() ^ member.Value.GetHashCode();
			return code;
		}

		/// <summary>
		/// Returns the XML element corresponding to this object.
		/// </summary>
		/// <returns>The XML element.</returns>
		public override XElement GetXml()
		{
			XElement element = new XElement(XmlRpcStruct.xmlName);
			foreach (KeyValuePair<string, XmlRpcMember> member in this.members)
			{
				element.Add(member.Value.GetXml());
			}
			return element;
		}

		/// <summary>
		/// Returns the value corresponding to this object.
		/// </summary>
		/// <returns>The object value.</returns>
		public override object GetValue()
		{
			return this.members;
		}

		/// <summary>
		/// Adds a new structure member.
		/// </summary>
		/// <param name="name">The structure member name.</param>
		/// <param name="value">The structure member value.</param>
		protected void Add(string name, string value)
		{
			this.members.Add(name, new XmlRpcMember(name, value));
		}
	}
}