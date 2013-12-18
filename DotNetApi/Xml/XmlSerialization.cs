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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DotNetApi.Xml
{
	/// <summary>
	/// A class used for XML object serialization.
	/// </summary>
	public static class XmlSerialization
	{
		private static readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
		private static readonly XName nil = XmlSerialization.xsi + "nil";

		/// <summary>
		/// Serializes the specified object in XML format to the specified stream.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="stream">The stream.</param>
		public static void Serialize(this object obj, Stream stream)
		{
			// Validate the arguments.
			if (null == obj) throw new ArgumentNullException("obj");
			if (null == stream) throw new ArgumentNullException("stream");
			// Create an XML document for the object.
			XDocument document = new XDocument(obj.Serialize(obj.GetType()));
			// Add the namespaces.
			document.Root.Add(new XAttribute(XNamespace.Xmlns + "xsi", XmlSerialization.xsi.ToString()));
			// Save the document to the specified stream.
			document.Save(stream, SaveOptions.None);
		}

		/// <summary>
		/// Deserializes the current object from an XML element.
		/// </summary>
		/// <typeparam name="T">The object type.</typeparam>
		/// <param name="stream">The XML stream to deserialize.</param>
		/// <param name="instance">An existing instance or <c>null</c>. If an existing instance is passed, the method overrides the object. Otherwise, the method creates a new object.</param>
		/// <returns>An instance of the deserialized object.</returns>
		public static T Deserialize<T>(this Stream stream, T instance = null) where T : class
		{
			// Create an XML document from the stream.
			XDocument document = XDocument.Load(stream, LoadOptions.None);
			// Parse the root XML element and return the object.
			return document.Root.Deserialize(typeof(T), instance) as T;
		}

		// Private methods.

		/// <summary>
		/// Serializes the specified value as an XML element, where the value is considered having the specified type.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="type">The type.</param>
		/// <returns>The XML element.</returns>
		private static XElement Serialize(this object value, Type type)
		{
			// Get the root XML attribute for the specified type.
			XmlRootAttribute attr = type.GetAttribute<XmlRootAttribute>();

			//  If the attribute is null, throw an exception.
			if (null == attr) throw new SerializationException("Cannot serialize an object of type {0} because it does not have a root XML element.".FormatWith(type.Name));

			// Else, serialize using the name from the root element.
			return value.SerializeAsElement(type, XmlSerialization.GetName(attr.ElementName, attr.Namespace), attr.IsNullable);
		}

		/// <summary>
		/// Serializes the specified value as an XML element, where the value is considered having the specified type.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="type">The type.</param>
		/// <param name="name">The XML element name.</param>
		/// <param name="isNullable">Indicates whether the value can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeAsElement(this object value, Type type, XName name, bool isNullable)
		{
			// Validate the value.
			if ((value == null) && (!isNullable)) throw new SerializationException("Cannot serialize an object of type {0} because the object value is null, but a null value is not allowed.".FormatWith(type.Name));

			// If the value is null, return an empty element.
			if (null == value) return XmlSerialization.CreateElement(name, true, true);

			// Serialize the object according to the underlying type.
			if (type.Equals(typeof(bool))) return ((bool)value).SerializeBooleanAsElement(name, isNullable);
			if (type.Equals(typeof(int))) return ((int)value).SerializeIntegerAsElement(name, isNullable);
			if (type.Equals(typeof(double))) return ((double)value).SerializeDoubleAsElement(name, isNullable);
			if (type.Equals(typeof(DateTime))) return ((DateTime)value).SerializeDateTimeAsElement(name, isNullable);
			if (type.Equals(typeof(TimeSpan))) return ((TimeSpan)value).SerializeTimeSpanAsElement(name, isNullable);
			if (type.Equals(typeof(string))) return (value as string).SerializeString(name, isNullable);
			if (type.IsNullable()) return value.SerializeAsElement(type.GetNullable(), name, isNullable);
			return value.SerializeObject(name, isNullable);
		}

		/// <summary>
		/// Serializes the specified value as an XML attribute, where the value is considered having the specified type.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="type">The type.</param>
		/// <param name="name">The XML attribute name.</param>
		/// <returns>The XML attribute.</returns>
		private static XAttribute SerializeAsAttribute(this object value, Type type, XName name)
		{
			// Check the type is a value type.
			if (!type.IsValueType) throw new SerializationException("Cannot serialize the reference type {0} as an XML attribute.".FormatWith(type.Name));

			// Serialize the object according to the underlying type.
			if (type.Equals(typeof(bool))) return ((bool)value).SerializeBooleanAsAttribute(name);
			if (type.Equals(typeof(int))) return ((int)value).SerializeIntegerAsAttribute(name);
			if (type.Equals(typeof(double))) return ((double)value).SerializeDoubleAsAttribute(name);
			if (type.Equals(typeof(DateTime))) return ((DateTime)value).SerializeDateTimeAsAttribute(name);
			if (type.Equals(typeof(TimeSpan))) return ((TimeSpan)value).SerializeTimeSpanAsAttribute(name);

			// Otherwise, throw an exception.
			throw new SerializationException("Cannot serialize the object of type {0} as an XML attribute.".FormatWith(type.Name));
		}

		/// <summary>
		/// Deserializes the specified XML element to an object, based on the given type.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="type">The type.</param>
		/// <param name="instance">An existing instance or <c>null</c>.</param>
		/// <returns>The object.</returns>
		private static object Deserialize(this XElement element, Type type, object instance = null)
		{
			// If the element corresponds to a null object, return null.
			XAttribute attr = element.Attributes(XmlSerialization.nil).FirstOrDefault();
			if (null == attr) throw new SerializationException("Cannot deserialize the element {0} of type {1} because it does not have a null attribute.".FormatWith(element.Name, type.FullName));
			if (bool.Parse(attr.Value)) return null;

			// Deserialize the element according to the underlying type.
			if (type.Equals(typeof(bool))) return element.DeserializeBoolean();
			if (type.Equals(typeof(int))) return element.DeserializeInteger();
			if (type.Equals(typeof(double))) return element.DeserializeDouble();
			if (type.Equals(typeof(DateTime))) return element.DeserializeDateTime();
			if (type.Equals(typeof(TimeSpan))) return element.DeserializeTimeSpan();
			if (type.Equals(typeof(string))) return element.DeserializeString();
			if (type.IsNullable()) return element.Deserialize(type.GetNullable(), instance);
			return element.DeserializeObject(type, instance);
		}

		/// <summary>
		/// Deserializes the specified XML attribute to an object, based on the given type.
		/// </summary>
		/// <param name="attribute">The attribute.</param>
		/// <param name="type">The type.</param>
		/// <returns>The object.</returns>
		private static object Deserialize(this XAttribute attribute, Type type)
		{
			// Deserialize the attribute according to the underlying type.
			if (type.Equals(typeof(bool))) return attribute.DeserializeBoolean();
			if (type.Equals(typeof(int))) return attribute.DeserializeInteger();
			if (type.Equals(typeof(double))) return attribute.DeserializeDouble();
			if (type.Equals(typeof(DateTime))) return attribute.DeserializeDateTime();
			if (type.Equals(typeof(TimeSpan))) return attribute.DeserializeTimeSpan();

			// Otherwise, throw an exception.
			throw new SerializationException("Cannot deserialize the XML attribute {0} to an object of type {1}.".FormatWith(attribute.Name, type.Name));
		}

		/// <summary>
		/// Serialize the current enumerable to an XML element.
		/// </summary>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="nameArray">The enumerable element name.</param>
		/// <param name="nameItem">The item element name.</param>
		/// <param name="isArrayNullable">Indicates whether the array can be null.</param>
		/// <param name="isItemNullable">Indicates whether the item can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeEnumerable(this IEnumerable enumerable, XName nameArray, XName nameItem, bool isArrayNullable, bool isItemNullable)
		{
			// Validate the enumerable.
			if ((enumerable == null) && (!isArrayNullable)) throw new SerializationException("Cannot serialize an enumerable because the object value is null, but a null value is not allowed.");

			// If the enumerable is null, return an empty element.
			if (null == enumerable) return XmlSerialization.CreateElement(nameArray, true, true);

			// Create a new element.
			XElement element = XmlSerialization.CreateElement(nameArray, isArrayNullable, false);
			
			// Serialize and add all items.
			foreach (object item in enumerable)
			{
				element.Add(item.SerializeAsElement(item.GetType(), nameItem, isItemNullable));
			}

			// Return the element.
			return element;
		}

		///// <summary>
		///// Deserializes the XML element into an array.
		///// </summary>
		///// <param name="element">The XML element.</param>
		///// <param name="type">The type.</param>
		///// <returns>An array object.</returns>
		//private static Array DeserializeArray(this XElement element, Type type)
		//{
		//	// Get the item type.
		//	Type itemType = type.GetElementType();
		//	// Get the XML elements.
		//	IEnumerable<XElement> elements = element.Elements(XmlSerialization.nameItem);
		//	// Create an array instance.
		//	Array array = Activator.CreateInstance(type, elements.Count()) as Array;
		//	// Deserialize all list items and add them to the list.
		//	int index = 0;
		//	foreach (XElement item in elements)
		//	{
		//		array.SetValue(item.Deserialize(itemType, null), index++);
		//	}
		//	// Return the array.
		//	return array;
		//}

		/// <summary>
		/// Deserializes the XML element into a generic list.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="type">The type.</param>
		/// <param name="instance">An existing instance or <c>null</c>.</param>
		/// <returns>A generic list object.</returns>
		//private static IList DeserializeList(this XElement element, Type type, IList instance = null)
		//{
		//	// Get the item type.
		//	Type itemType = type.GetGenericArguments()[0];
		//	// Create a list instance.
		//	IList list = instance == null ? Activator.CreateInstance(type) as IList : instance;
		//	// Deserialize all list items and add them to the list.
		//	foreach (XElement item in element.Elements(nameItem))
		//	{
		//		list.Add(item.Deserialize(itemType, null));
		//	}
		//	// Return the list.
		//	return list;
		//}

		/// <summary>
		/// Serializes the specified boolean value as an XML element.
		/// </summary>
		/// <param name="value">The boolean value.</param>
		/// <param name="name">The element name.</param>
		/// <param name="isNullable">Indicates whether the value can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeBooleanAsElement(this bool value, XName name, bool isNullable)
		{
			return XmlSerialization.CreateElement(name, value.ToString(), isNullable, false);
		}

		/// <summary>
		/// Serializes the specified boolean value as an XML element.
		/// </summary>
		/// <param name="value">The boolean value.</param>
		/// <param name="name">The attribute name.</param>
		/// <returns>The XML attribute.</returns>
		private static XAttribute SerializeBooleanAsAttribute(this bool value, XName name)
		{
			return new XAttribute(name, value.ToString());
		}

		/// <summary>
		/// Deserializes the specified element as a boolean value.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The boolean value.</returns>
		private static bool DeserializeBoolean(this XElement element)
		{
			if (null == element.Value) throw new SerializationException("Cannot deserialize the element {0} into a boolean because it does not have a value.".FormatWith(element.Name));
			return bool.Parse(element.Value);
		}

		/// <summary>
		/// Deserializes the specified attribute as a boolean value.
		/// </summary>
		/// <param name="attribute">The XML attribute.</param>
		/// <returns>The boolean value.</returns>
		private static bool DeserializeBoolean(this XAttribute attribute)
		{
			if (null == attribute.Value) throw new SerializationException("Cannot deserialize the attribute {0} into a boolean because it does not have a value.".FormatWith(attribute.Name));
			return bool.Parse(attribute.Value);
		}

		/// <summary>
		/// Serializes the specified integer value as an XML element.
		/// </summary>
		/// <param name="value">The integer value.</param>
		/// <param name="name">The element name.</param>
		/// <param name="isNullable">Indicates whether the value can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeIntegerAsElement(this int value, XName name, bool isNullable)
		{
			return XmlSerialization.CreateElement(name, value.ToString(CultureInfo.InvariantCulture), isNullable, false);
		}

		/// <summary>
		/// Serialize the specified integer value as an XML attribute.
		/// </summary>
		/// <param name="value">The integer value.</param>
		/// <param name="name">The attribute name.</param>
		/// <returns>The XML attribute.</returns>
		private static XAttribute SerializeIntegerAsAttribute(this int value, XName name)
		{
			return new XAttribute(name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Deserializes the specified element as an integer value.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The integer value.</returns>
		private static int DeserializeInteger(this XElement element)
		{
			if (null == element.Value) throw new SerializationException("Cannot deserialize the element {0} into an integer because it does not have a value.".FormatWith(element.Name));
			return int.Parse(element.Value, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Deserializes the specified attribute as an integer value.
		/// </summary>
		/// <param name="attribute">The XML attribute.</param>
		/// <returns>The integer value.</returns>
		private static int DeserializeInteger(this XAttribute attribute)
		{
			if (null == attribute.Value) throw new SerializationException("Cannot deserialize the attribute {0} into an integer because it does not have a value.".FormatWith(attribute.Name));
			return int.Parse(attribute.Value, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Serializes the specified double value as an XML element.
		/// </summary>
		/// <param name="value">The double value.</param>
		/// <param name="name">The element name.</param>
		/// <param name="isNullable">Indicates whether the value can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeDoubleAsElement(this double value, XName name, bool isNullable)
		{
			return XmlSerialization.CreateElement(name, value.ToString("E20", CultureInfo.InvariantCulture), isNullable, false);
		}

		/// <summary>
		/// Serializes the specified double value as an XML attribute.
		/// </summary>
		/// <param name="value">The double value.</param>
		/// <param name="name">The element name.</param>
		/// <returns>The XML attribute.</returns>
		private static XAttribute SerializeDoubleAsAttribute(this double value, XName name)
		{
			return new XAttribute(name, value.ToString("E20", CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Deserializes the specified element as a double value.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The double value.</returns>
		private static double DeserializeDouble(this XElement element)
		{
			if (null == element.Value) throw new SerializationException("Cannot deserialize the element {0} into a double because it does not have a value.".FormatWith(element.Name));
			return double.Parse(element.Value, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Deserializes the specified attribute as a double value.
		/// </summary>
		/// <param name="attribute">The XML attribute.</param>
		/// <returns>The double value.</returns>
		private static double DeserializeDouble(this XAttribute attribute)
		{
			if (null == attribute.Value) throw new SerializationException("Cannot deserialize the attribute {0} into a double because it does not have a value.".FormatWith(attribute.Name));
			return double.Parse(attribute.Value, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Serializes the specified date-time value as an XML element.
		/// </summary>
		/// <param name="value">The date-time value.</param>
		/// <param name="name">The element name.</param>
		/// <param name="isNullable">Indicates whether the value can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeDateTimeAsElement(this DateTime value, XName name, bool isNullable)
		{
			return XmlSerialization.CreateElement(name, value.Ticks.ToString(CultureInfo.InvariantCulture), isNullable, false);
		}

		/// <summary>
		/// Serializes the specified date-time value as an XML attribute.
		/// </summary>
		/// <param name="value">The date-time value.</param>
		/// <param name="name">The attribute name.</param>
		/// <returns>The XML element.</returns>
		private static XAttribute SerializeDateTimeAsAttribute(this DateTime value, XName name)
		{
			return new XAttribute(name, value.Ticks.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Deserializes the specified element as a date-time value.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The date-time value.</returns>
		private static DateTime DeserializeDateTime(this XElement element)
		{
			if (null == element.Value) throw new SerializationException("Cannot deserialize the element {0} into a date-time because it does not have a value.".FormatWith(element.Name));
			return new DateTime(long.Parse(element.Value, CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Deserializes the specified attribute as a date-time value.
		/// </summary>
		/// <param name="attribute">The XML attribute.</param>
		/// <returns>The date-time value.</returns>
		private static DateTime DeserializeDateTime(this XAttribute attribute)
		{
			if (null == attribute.Value) throw new SerializationException("Cannot deserialize the attribute {0} into a date-time because it does not have a value.".FormatWith(attribute.Name));
			return new DateTime(long.Parse(attribute.Value, CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Serializes the specified time-span value as an XML element.
		/// </summary>
		/// <param name="value">The time-span value.</param>
		/// <param name="name">The element name.</param>
		/// <param name="isNullable">Indicates whether the value can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeTimeSpanAsElement(this TimeSpan value, XName name, bool isNullable)
		{
			return XmlSerialization.CreateElement(name, value.Ticks.ToString(CultureInfo.InvariantCulture), isNullable, false);
		}

		/// <summary>
		/// Serializes the specified time-span value as an XML attribute.
		/// </summary>
		/// <param name="value">The time-span value.</param>
		/// <param name="name">The attribute name.</param>
		/// <returns>The XML element.</returns>
		private static XAttribute SerializeTimeSpanAsAttribute(this TimeSpan value, XName name)
		{
			return new XAttribute(name, value.Ticks.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Deserializes the specified element as a time-span value.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The time-span value.</returns>
		private static TimeSpan DeserializeTimeSpan(this XElement element)
		{
			if (null == element.Value) throw new SerializationException("Cannot deserialize the element {0} into a time-span because it does not have a value.".FormatWith(element.Name));
			return new TimeSpan(long.Parse(element.Value, CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Deserializes the specified attribute as a time-span value.
		/// </summary>
		/// <param name="attribute">The XML attribute.</param>
		/// <returns>The time-span value.</returns>
		private static TimeSpan DeserializeTimeSpan(this XAttribute attribute)
		{
			if (null == attribute.Value) throw new SerializationException("Cannot deserialize the attribute {0} into a time-span because it does not have a value.".FormatWith(attribute.Name));
			return new TimeSpan(long.Parse(attribute.Value, CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Serializes the specified string as an XML element.
		/// </summary>
		/// <param name="value">The string.</param>
		/// <param name="name">The element name.</param>
		/// <param name="isNullable">Indicates whether the value can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeString(this string value, XName name, bool isNullable)
		{
			return XmlSerialization.CreateElement(name, value, isNullable, false);
		}

		/// <summary>
		/// Deserializes the specified element as a string value.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The string value.</returns>
		private static String DeserializeString(this XElement element)
		{
			if (null == element.Value) throw new SerializationException("Cannot deserialize the element {0} into a date-time because it does not have a value.".FormatWith(element.Name));
			return element.Value;
		}


		/// <summary>
		/// Serializes the current object to an XML element with the specified name.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="name">The element name.</param>
		/// <param name="isNullable">Indicates whether the value can be null.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeObject(this object obj, XName name, bool isNullable)
		{
			// Create a new XML element with the specified name.
			XElement element = XmlSerialization.CreateElement(name, isNullable, false);
			// Get all instance properties for the current object type.
			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// For each property.
			foreach (PropertyInfo property in properties)
			{
				// If the property is indexed, throw an exception.
				if (property.GetIndexParameters().Length > 0) throw new SerializationException("The property {0} of type {1} is indexed, but the serialization does not support indexed properties.".FormatWith(property.Name, obj.GetType().FullName));

				// The XML child object.
				XObject child = null;

				// If the property has an element XML attribute
				if (property.GetAttribute<XmlElementAttribute>() != null)
				{
					// Serialize the property as an XML element.
					child = obj.SerializePropertyAsElement(property);
				}
				else if (property.GetAttribute<XmlAttributeAttribute>() != null)
				{
					// Serialize the property as an XML attribute.
					child = obj.SerializePropertyAsAttribute(property);
				}
				else if ((property.GetAttribute<XmlArrayAttribute>() != null) && (property.GetAttribute<XmlArrayItemAttribute>() != null))
				{
					// Serialize the property as an XML array.
					child = obj.SerializePropertyAsArray(property);
				}
				
				// If the property could not be serialized, skip.
				if (null == child) continue;

				// Else, add the XML object to the element.
				element.Add(child);
			}

			// Return the element.
			return element;
		}

		/// <summary>
		/// Serializes the object property as an XML element.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="property">The property.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializePropertyAsElement(this object obj, PropertyInfo property)
		{
			// Get the element attribute.
			XmlElementAttribute attr = property.GetAttribute<XmlElementAttribute>();

			// If the property cannot be read and written, return null.
			if (!property.CanRead || !property.CanWrite) return null;

			// If the property does not have a public get method, return null.
			if (null == property.GetGetMethod(false)) return null;
			// If the property does not have a non-public set method, return null.
			if (null == property.GetSetMethod(true)) return null;

			// Get the property value.
			object value = property.GetValue(obj, null);

			// Serialize the property as an attribute.
			return value.SerializeAsElement(property.PropertyType, XmlSerialization.GetName(attr.ElementName, attr.Namespace), attr.IsNullable);
		}

		/// <summary>
		/// Serializes the object property as an XML attribute.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="property">The property.</param>
		/// <returns>The XML attribute.</returns>
		private static XAttribute SerializePropertyAsAttribute(this object obj, PropertyInfo property)
		{
			// Get the attribute attribute.
			XmlAttributeAttribute attr = property.GetAttribute<XmlAttributeAttribute>();

			// If the property cannot be read and written, return null.
			if (!property.CanRead || !property.CanWrite) return null;

			// If the property does not have a public get method, return null.
			if (null == property.GetGetMethod(false)) return null;
			// If the property does not have a non-public set method, return null.
			if (null == property.GetSetMethod(true)) return null;

			// Get the property value.
			object value = property.GetValue(obj, null);

			// Serialize the property as an attribute.
			return value.SerializeAsAttribute(property.PropertyType, XmlSerialization.GetName(attr.AttributeName, attr.Namespace));
		}

		/// <summary>
		/// Serializes the object property as an XML array.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="property">The property.</param>
		/// <returns>The XML element corresponding to the array.</returns>
		private static XElement SerializePropertyAsArray(this object obj, PropertyInfo property)
		{
			// Get the array and array item attributes.
			XmlArrayAttribute attrArray = property.GetAttribute<XmlArrayAttribute>();
			XmlArrayItemAttribute attrItem = property.GetAttribute<XmlArrayItemAttribute>();

			// If the property cannot be read, return null.
			if (!property.CanRead) return null;

			// If the property does not have a public get method, return null.
			if (null == property.GetGetMethod(false)) return null;

			// If the property does not implement the IEnumerable interface, throw an exception.
			if (!property.PropertyType.IsAssignableToInterface(typeof(IEnumerable))) throw new SerializationException("Cannot serialize the property {0} of type {1} as an array because the property type {2} does not implement the IEnumerable interface.");

			// Get the property value.
			IEnumerable value = property.GetValue(obj, null) as IEnumerable;

			// Serialize the property as an attribute.
			return value.SerializeEnumerable(XmlSerialization.GetName(attrArray.ElementName, attrArray.Namespace), XmlSerialization.GetName(attrItem.ElementName, attrItem.Namespace), attrArray.IsNullable, attrItem.IsNullable);
		}

		/// <summary>
		/// Deserializes the XML element to an object.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="type">The type.</param>
		/// <param name="instance">An existing instance or <c>null</c>.</param>
		/// <returns>The object.</returns>
		private static object DeserializeObject(this XElement element, Type type, object instance = null)
		{
			// Create a new object instance.
			object obj = instance == null ? Activator.CreateInstance(type) : instance;

			// Get all instance properties for the current object type.
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// For each property.
			foreach (PropertyInfo property in properties)
			{
				// If the property is indexed, throw an exception.
				if (property.GetIndexParameters().Length > 0) throw new SerializationException("The property {0} of type {1} is indexed, but the serialization does not support indexed properties.".FormatWith(property.Name, obj.GetType().FullName));

				// If the property has an element XML attribute
				if (property.GetAttribute<XmlElementAttribute>() != null)
				{
					// Deserialize the property from an XML element.
					element.DeserializePropertyAsElement(obj, property);
				}
				else if (property.GetAttribute<XmlAttributeAttribute>() != null)
				{
					// Deserialize the property from an XML attribute.
					element.DeserializePropertyAsAttribute(obj, property);
				}
				else if ((property.GetAttribute<XmlArrayAttribute>() != null) && (property.GetAttribute<XmlArrayItemAttribute>() != null))
				{
					// Deserialize the property from an XML array.
					element.DeserializePropertyAsArray(obj, property);
				}


				//// If the property cannot be read and written, continue.
				//if (!property.CanRead || !property.CanWrite) continue;

				//// If the property does not have a public get method, continue.
				//if (null == property.GetGetMethod(false)) continue;
				//// If the property does not have a non-public set method, continue.
				//if (null == property.GetSetMethod(true)) continue;

				//// If the property is indexed, throw an exception.
				//if (property.GetIndexParameters().Length > 0) throw new SerializationException("The property {0} of type {1} is indexed, but the serialization does not support indexed properties.".FormatWith(property.Name, obj.GetType().FullName));

				//// Get the XML element corresponding to this property.
				//XElement el = element.Element(property.Name);
				//if (null == el) throw new SerializationException("Cannot deserialize the property {0} of type {1} because a corresponing XML element was not found.".FormatWith(property.Name, type.FullName));

				//// Deserialize the element into the property value.
				//object value = el.Deserialize(property.PropertyType, null);

				//// Set the value to the object.
				//property.SetValue(obj, value, null);
			}

			// Return the object.
			return obj;
		}

		/// <summary>
		/// Deserializes the object property from an XML element.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="property">The property.</param>
		/// <returns>The XML element.</returns>
		private static void DeserializePropertyAsElement(this XElement element, object obj, PropertyInfo property)
		{
			// Get the element attribute.
			XmlElementAttribute attr = property.GetAttribute<XmlElementAttribute>();

			// If the property cannot be read and written, throw an exception.
			if (!property.CanRead || !property.CanWrite) throw new SerializationException();

			// If the property does not have a public get method, return null.
			if (null == property.GetGetMethod(false)) throw new SerializationException();
			// If the property does not have a non-public set method, return null.
			if (null == property.GetSetMethod(true)) throw new SerializationException();

			// Get the property value.
			object value = property.GetValue(obj, null);

			// Serialize the property as an attribute.
			return value.SerializeAsElement(property.PropertyType, XmlSerialization.GetName(attr.ElementName, attr.Namespace), attr.IsNullable);
		}

		/// <summary>
		/// Gets the XML name for the specified local and namespace names.
		/// </summary>
		/// <param name="localName">The local name.</param>
		/// <param name="namespaceName">The namespace name.</param>
		/// <returns>The XML name.</returns>
		private static XName GetName(string localName, string namespaceName)
		{
			return namespaceName != null ? XName.Get(localName, namespaceName) : XName.Get(localName);
		}

		/// <summary>
		/// Gets the first attribute for the specified type.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="type">The type.</param>
		/// <returns>The attribute.</returns>
		private static T GetAttribute<T>(this Type type) where T : Attribute
		{
			// Get the attributes.
			object[] attr = type.GetCustomAttributes(typeof(T), false);
			// Return the first attribute, if any or null otherwise.
			return attr.Length > 0 ? attr[0] as T : null;
		}

		/// <summary>
		/// Gets the first attribute for the specified property.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="property">The property.</param>
		/// <returns>The attribute.</returns>
		private static T GetAttribute<T>(this PropertyInfo property) where T : Attribute
		{
			// Get the attributes.
			object[] attr = property.GetCustomAttributes(typeof(T), false);
			// Return the first attribute, if any or null otherwise.
			return attr.Length > 0 ? attr[0] as T : null;
		}

		/// <summary>
		/// Creates a new XML element from the specfied name.
		/// </summary>
		/// <param name="name">The name of the XML element.</param>
		/// <param name="isNullable">Indicates whether the element has a <b>xsi:nil</b> attribute.</param>
		/// <param name="isNull">Indicates the value of the <b>xsi:nil</b> attribute.</param>
		/// <returns>The XML element.</returns>
		private static XElement CreateElement(XName name, bool isNullable, bool isNull)
		{
			// Create the element.
			XElement element = new XElement(name);
			// If the element has a xsi:nil attribute.
			if (isNullable)
			{
				element.Add(new XAttribute(XmlSerialization.nil, isNull));
			}
			// Return the element.
			return element;
		}


		/// <summary>
		/// Creates a new XML element from the specfied name.
		/// </summary>
		/// <param name="name">The name of the XML element.</param>
		/// <param name="value">The element value.</param>
		/// <param name="isNullable">Indicates whether the element has a <b>xsi:nil</b> attribute.</param>
		/// <param name="isNull">Indicates the value of the <b>xsi:nil</b> attribute.</param>
		/// <returns>The XML element.</returns>
		private static XElement CreateElement(XName name, object value, bool isNullable, bool isNull)
		{
			// Create the element.
			XElement element = new XElement(name, value);
			// If the element has a xsi:nil attribute.
			if (isNullable)
			{
				element.Add(new XAttribute(XmlSerialization.nil, isNull));
			}
			// Return the element.
			return element;
		}
	}
}
