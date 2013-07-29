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
using System.Linq;
using System.Xml.Linq;

namespace DotNetApi.Xml
{
	/// <summary>
	/// A class with filtering extension methods for XML documents.
	/// </summary>
	public static class XmlFilter
	{
		/// <summary>
		/// Checks whether the element has the specified name.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="prefix">The namespace prefix.</param>
		/// <param name="localName">The local name.</param>
		/// <returns><b>True</b> if the element has the same name, or <b>false</b> otherwise.</returns>
		public static bool HasName(this XElement element, string prefix, string localName)
		{
			return (element.Name.LocalName == localName) && (element.Name.Namespace == element.GetNamespaceOfPrefixEx(prefix));
		}

		/// <summary>
		/// Returns the attribute of the element with the specified local name.
		/// </summary>
		/// <param name="element">The parent element.</param>
		/// <param name="prefix">The namespace prefix</param>
		/// <param name="localName">The local name.</param>
		/// <returns>The attribute with the specified local name, or null if no attribute matches the name.</returns>
		public static XAttribute Attribute(this XElement element, string prefix, string localName)
		{
			IEnumerator<XAttribute> enumerator = element.Attributes().Where(e =>
				prefix != null ?
				(e.Name.LocalName == localName) && (e.Name.Namespace == element.GetNamespaceOfPrefix(prefix)) :
				e.Name.LocalName == localName).GetEnumerator();
			return enumerator.MoveNext() ? enumerator.Current : null;
		}

		/// <summary>
		/// Returns the first child element with the specified local name.
		/// </summary>
		/// <param name="element">The parent element.</param>
		/// <param name="prefix">The namespace prefix</param>
		/// <param name="localName">The local name.</param>
		/// <returns>The first element with the specified local name, or null if no element matches the name.</returns>
		public static XElement Element(this XElement element, string prefix, string localName)
		{
			IEnumerator<XElement> enumerator = element.Elements().Where(e =>
				prefix != null ?
				(e.Name.LocalName == localName) && (e.Name.Namespace == e.GetNamespaceOfPrefix(prefix)) :
				e.Name.LocalName == localName).GetEnumerator();
			return enumerator.MoveNext() ? enumerator.Current : null;
		}

		/// <summary>
		/// Returns all child elements with the specified local name.
		/// </summary>
		/// <param name="element">The source.</param>
		/// <param name="prefix">The namespace prefix</param>
		/// <param name="localName">The local name.</param>
		/// <returns>The elements with the specified local name.</returns>
		public static IEnumerable<XElement> Elements(this XElement element, string prefix, string localName)
		{
			return element.Elements().Where(e =>
				prefix != null ?
				(e.Name.LocalName == localName) && (e.Name.Namespace == e.GetNamespaceOfPrefix(prefix)) :
				e.Name.LocalName == localName);
		}

		public static XNamespace GetNamespaceOfPrefixEx(this XElement element, string prefix)
		{
			return prefix != null ? element.GetNamespaceOfPrefix(prefix) : element.GetDefaultNamespace();
		}
	}
}
