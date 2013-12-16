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
//		private static readonly string nameItem = "Item";
		private static readonly string nameNull = "Null";

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
		public static T Deserialize<T>(this Stream stream, T instance = null) where T : class, new()
		{
			// Create an XML document from the stream.
			XDocument document = XDocument.Load(stream, LoadOptions.None);
			// Parse the root XML element and return the object.
			return document.Root.Deserialize(typeof(T), instance) as T;
		}

		// Private methods.

		/// <summary>
		/// Serializes the specified value as an XML element.
		/// </summary>
		/// <param name="obj">The value.</param>
		/// <param name="name">The XML element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement Serialize(this object obj, string name)
		{
			// Serialize the object as the current type.
			return obj.Serialize(obj.GetType(), name);
		}

		/// <summary>
		/// Serializes the specified value as an XML element, where the value is considered having the specified type.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="type">The type.</param>
		/// <returns>The XML element.</returns>
		private static XElement Serialize(this object value, Type type)
		{

		}

		/// <summary>
		/// Serializes the specified value as an XML element, where the value is considered having the specified type.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="type">The type.</param>
		/// <param name="name">The XML element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement Serialize(this object value, Type type, string name)
		{
			// If the value is null, return an empty element.
			if (null == value) return new XElement(name, new XAttribute(XmlSerialization.nameNull, true));

			// Serialize the object according to the underlying type.
			if (type.Equals(typeof(bool))) return ((bool)value).SerializeBoolean(name);
			if (type.Equals(typeof(int))) return ((int)value).SerializeInteger(name);
			if (type.Equals(typeof(double))) return ((double)value).SerializeDouble(name);
			if (type.Equals(typeof(DateTime))) return ((DateTime)value).SerializeDateTime(name);
			if (type.Equals(typeof(TimeSpan))) return ((TimeSpan)value).SerializeTimeSpan(name);
			if (type.Equals(typeof(string))) return (value as string).SerializeString(name);
			if (type.IsNullable()) return value.Serialize(type.GetNullable(), name);
			//if (type.IsAssignableToInterface(typeof(IEnumerable))) return (value as IEnumerable).SerializeEnumerable(name);
			return value.SerializeObject(name);
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
			XAttribute attr = element.Attributes(XmlSerialization.nameNull).FirstOrDefault();
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
			if (type.IsSubclassOf(typeof(Array))) return element.DeserializeArray(type);
			if (type.IsAssignableToGenericInterface(typeof(IList<>)) && type.IsAssignableToInterface(typeof(IList))) return element.DeserializeList(type, instance as IList);
			return element.DeserializeObject(type, instance);
		}

		/// <summary>
		/// Serialize the current enumerable to an XML element.
		/// </summary>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="name">The enumerable element name.</param>
		/// <param name="nameItem">The item element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeEnumerable(this IEnumerable enumerable, string name, string nameItem)
		{
			// Create a new element.
			XElement element = new XElement(name, new XAttribute(XmlSerialization.nameNull, false));
			// Serialize and add all items.
			foreach (object item in enumerable)
			{
				element.Add(item.Serialize(nameItem));
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
		private static IList DeserializeList(this XElement element, Type type, IList instance = null)
		{
			// Get the item type.
			Type itemType = type.GetGenericArguments()[0];
			// Create a list instance.
			IList list = instance == null ? Activator.CreateInstance(type) as IList : instance;
			// Deserialize all list items and add them to the list.
			foreach (XElement item in element.Elements(nameItem))
			{
				list.Add(item.Deserialize(itemType, null));
			}
			// Return the list.
			return list;
		}

		/// <summary>
		/// Serializes the specified boolean as an XML element.
		/// </summary>
		/// <param name="value">The boolean.</param>
		/// <param name="name">The element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeBoolean(this bool value, string name)
		{
			return new XElement(name, new XAttribute(XmlSerialization.nameNull, false), value.ToString());
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
		/// Serializes the specified integer as an XML element.
		/// </summary>
		/// <param name="value">The integer.</param>
		/// <param name="name">The element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeInteger(this int value, string name)
		{
			return new XElement(name, new XAttribute(XmlSerialization.nameNull, false), value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Deserializes the specified element as a integer value.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The integer value.</returns>
		private static int DeserializeInteger(this XElement element)
		{
			if (null == element.Value) throw new SerializationException("Cannot deserialize the element {0} into an integer because it does not have a value.".FormatWith(element.Name));
			return int.Parse(element.Value, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Serializes the specified double as an XML element.
		/// </summary>
		/// <param name="value">The double.</param>
		/// <param name="name">The element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeDouble(this double value, string name)
		{
			return new XElement(name, new XAttribute(XmlSerialization.nameNull, false), value.ToString("E20", CultureInfo.InvariantCulture));
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
		/// Serializes the specified date-time as an XML element.
		/// </summary>
		/// <param name="value">The date-time.</param>
		/// <param name="name">The element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeDateTime(this DateTime value, string name)
		{
			return new XElement(name, new XAttribute(XmlSerialization.nameNull, false), value.Ticks.ToString(CultureInfo.InvariantCulture));
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
		/// Serializes the specified time-span as an XML element.
		/// </summary>
		/// <param name="value">The time-span.</param>
		/// <param name="name">The element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeTimeSpan(this TimeSpan value, string name)
		{
			return new XElement(name, new XAttribute(XmlSerialization.nameNull, false), value.Ticks.ToString(CultureInfo.InvariantCulture));
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
		/// Serializes the specified string as an XML element.
		/// </summary>
		/// <param name="value">The string.</param>
		/// <param name="name">The element name.</param>
		/// <returns>The XML element.</returns>
		private static XElement SerializeString(this string value, string name)
		{
			return new XElement(name, new XAttribute(XmlSerialization.nameNull, false), value);
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
		/// <returns>The XML element.</returns>
		private static XElement SerializeObject(this object obj, string name)
		{
			// Create a new XML element with the specified name.
			XElement element = new XElement(name, new XAttribute(XmlSerialization.nameNull, false));
			// Get all instance properties for the current object type.
			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// For each property.
			foreach (PropertyInfo property in properties)
			{
				// If the property cannot be read and written, continue.
				if (!property.CanRead || !property.CanWrite) continue;

				// If the property does not have a public get method, continue.
				if (null == property.GetGetMethod(false)) continue;
				// If the property does not have a non-public set method, continue.
				if (null == property.GetSetMethod(true)) continue;

				// If the property is indexed, throw an exception.
				if (property.GetIndexParameters().Length > 0) throw new SerializationException("The property {0} of type {1} is indexed, but the serialization does not support indexed properties.".FormatWith(property.Name, obj.GetType().FullName));

				// Get the property value.
				object value = property.GetValue(obj, null);

				// Serialize the property using the property type.
				element.Add(value.Serialize(property.PropertyType, property.Name));
			}

			// Return the element.
			return element;
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
				// If the property cannot be read and written, continue.
				if (!property.CanRead || !property.CanWrite) continue;

				// If the property does not have a public get method, continue.
				if (null == property.GetGetMethod(false)) continue;
				// If the property does not have a non-public set method, continue.
				if (null == property.GetSetMethod(true)) continue;

				// If the property is indexed, throw an exception.
				if (property.GetIndexParameters().Length > 0) throw new SerializationException("The property {0} of type {1} is indexed, but the serialization does not support indexed properties.".FormatWith(property.Name, obj.GetType().FullName));

				// Get the XML element corresponding to this property.
				XElement el = element.Element(property.Name);
				if (null == el) throw new SerializationException("Cannot deserialize the property {0} of type {1} because a corresponing XML element was not found.".FormatWith(property.Name, type.FullName));

				// Deserialize the element into the property value.
				object value = el.Deserialize(property.PropertyType, null);

				// Set the value to the object.
				property.SetValue(obj, value, null);
			}

			// Return the object.
			return obj;
		}

		private XmlRootAttribute GetRootAttribute(Type type)
		{
			// Get the attributes.
			object[] attr = type.GetCustomAttributes(typeof(XmlRootAttribute), false);
			// Return the first attribute, if any or null otherwise.
			return attr.Length > 0 ? attr[0] : null;
		}
	}
}
