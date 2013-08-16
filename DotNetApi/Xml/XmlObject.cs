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
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using DotNetApi;

namespace DotNetApi.Xml
{
	/// <summary>
	/// Represents the base class of an XML object used for serialization.
	/// </summary>
	public abstract class XmlObject
	{

		// Public methods.

		/// <summary>
		/// Serializes the current object to an XML element, using the class XML root attribute as name.
		/// </summary>
		/// <returns>The XML element corresponding to the serialization of this object.</returns>
		public virtual XElement ToXml()
		{
			// Get the object type.
			Type type = this.GetType();
			// Get the XML root attribute for the current object.
			XmlRootAttribute xmlRootAttribute = type.GetCustomAttributes(typeof(XmlRootAttribute), false).FirstOrDefault() as XmlRootAttribute;
			// If the class does not have a root attribute, throw an exception.
			if (null == xmlRootAttribute) throw new XmlException("Cannot create an XML element for the object of type {0} because the root XML attribute is missing.".FormatWith(type));
			// Create an XML element with the name specified in the root attribute.
			return this.ToXml(XmlObject.GetXmlName(
				xmlRootAttribute.ElementName,
				xmlRootAttribute.Namespace));
		}

		/// <summary>
		/// Serializes the current object to an XML element, using the specified name.
		/// </summary>
		/// <param name="name">The name of the XML element.</param>
		/// <returns>The XML element corresponding to the serialization of this object.</returns>
		public virtual XElement ToXml(XName name)
		{
			// Check the element name is not null.
			name.ValidateNotNull("name");
			// Create a new XML element.
			XElement element = new XElement(name);

			// Get the object type.
			Type type = this.GetType();

			// Add all public and instance fields to the element.
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				// If the field has an XML ignore attribute, continue to the next field.
				if (fieldInfo.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).FirstOrDefault() != null) continue;
				// If the field is read-only, continue to the next field.
				if (fieldInfo.IsInitOnly) continue;				
				// Serialize the field, and add it to the XML element.
				element.Add(this.SerializeField(fieldInfo));
			}

			// Return the XML element.
			return element;
		}

		// Protected methods.

		/// <summary>
		/// Creates the XML name for the specified local and namespace names, where the namespace name can be <b>null</b>.
		/// </summary>
		/// <param name="localName">The local name.</param>
		/// <param name="namespaceName">The namespace name.</param>
		/// <returns>The XML name.</returns>
		protected static XName GetXmlName(string localName, string namespaceName)
		{
			// Check the local name is not null.
			localName.ValidateNotNull("localName");
			// Return the XML name.
			return namespaceName != null ? XName.Get(localName, namespaceName) : XName.Get(localName);
		}

		// Private methods.

		/// <summary>
		/// Serializes the field into an XML object.
		/// </summary>
		/// <param name="fieldInfo">The field to serialize.</param>
		/// <returns>The XML object, either an element or an attribute.</returns>
		private XObject SerializeField(FieldInfo fieldInfo)
		{
			// Get the XML element attribute.
			XmlElementAttribute xmlElementAttribute = fieldInfo.GetCustomAttributes(typeof(XmlElementAttribute), false).FirstOrDefault() as XmlElementAttribute;
			// Get the XML attribute attribute.
			XmlAttributeAttribute xmlAttributeAttribute = fieldInfo.GetCustomAttributes(typeof(XmlAttributeAttribute), false).FirstOrDefault() as XmlAttributeAttribute;

			// If the field type is a value type, use the string serialization.
			if (fieldInfo.FieldType.IsValueType)
			{
				// If the field does not have an XML element and attribute attribute, throw an exception.
				if ((null == xmlElementAttribute) && (null == xmlAttributeAttribute)) throw new XmlException("The field {0} for the object of type {1} cannot be serialized because it does not have an XML element or attribute attribute.".FormatWith(fieldInfo.Name, this.GetType()));
				// Get the field value as an object.
				object value = fieldInfo.GetValue(this);
				// If the field has an XML element attribute.
				if (null != xmlElementAttribute)
				{
					// Serialize the field as an XML element.
					return new XElement(XmlObject.GetXmlName(xmlElementAttribute.ElementName, xmlElementAttribute.Namespace), value);
				}
				else
				{
					// Serialize the field as an XML attribute.
					return new XAttribute(XmlObject.GetXmlName(xmlElementAttribute.ElementName, xmlElementAttribute.Namespace), value);
				}
			}
			// If the field type is derived from the XmlObject class, use the object serialization.
			if (fieldInfo.FieldType.IsSubclassOf(typeof(XmlObject)))
			{
				// If the field does not have an XML element attribute, throw an exception.
				if (null == xmlElementAttribute) throw new XmlException("The field {0} for the object of type {1} cannot be serialized because it does not have an XML element attribute.".FormatWith(fieldInfo.Name, this.GetType()));
				// Get the field value as an XML object.
				XmlObject value = fieldInfo.GetValue(this) as XmlObject;
				// Return the object serialized.
				return value.ToXml(XmlObject.GetXmlName(xmlElementAttribute.ElementName, xmlElementAttribute.Namespace));
			}
			// If the field cannot be serialized throw an exception.
			throw new XmlException("The field {0} for the object of type {1} cannot be serialized.".FormatWith(fieldInfo.Name, this.GetType()));
		}
	}
}
