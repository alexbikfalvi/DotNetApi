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
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// An XML RPC structure.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcStruct : XmlRpcObject
	{
		private static string xmlName = "struct";

		private Dictionary<string, XmlRpcMember> members = new Dictionary<string,XmlRpcMember>();

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
		public XmlRpcStruct(XElement element)
		{
			if (element.Name.LocalName != XmlRpcStruct.xmlName) throw new XmlRpcException(string.Format("Invalid \'{0}\' XML element name \'{1}\'.", XmlRpcStruct.xmlName, element.Name.LocalName));
			foreach (XElement child in element.Elements(XmlRpcMember.XmlName))
			{
				// Create a new member for each public property.
				XmlRpcMember member = new XmlRpcMember(child);
				// Add the member to the struct list.
				this.members.Add(member.Name, member);
			}
			
		}

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
	}
}