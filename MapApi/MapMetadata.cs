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
using System.Xml.Linq;

namespace MapApi
{
	/// <summary>
	/// A class representing a map metdata collection.
	/// </summary>
	public class MapMetadata : IEnumerable<KeyValuePair<string, string>>
	{
		private readonly Dictionary<string, string> metadata = new Dictionary<string, string>();

		/// <summary>
		/// Creates an empty metadata collection.
		/// </summary>
		public MapMetadata()
		{
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the value of the specified metadata entry.
		/// </summary>
		/// <param name="name">The entry name.</param>
		/// <returns>The entry value.</returns>
		public string this[string name]
		{
			get { return this.metadata[name]; }
			set { this.metadata[name] = value; }
		}

		// Public methods.

		/// <summary>
		/// Returns the generic enumerator for the metadata collection.
		/// </summary>
		/// <returns>The generic enumerator.</returns>
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return this.metadata.GetEnumerator();
		}

		/// <summary>
		/// Returns the enumerator for the metadata collection.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Creates an XML element for the current map object.
		/// </summary>
		/// <param name="name">The name of the XML element.</param>
		/// <returns>The XML element.</returns>
		public XElement ToXml(string name)
		{
			// Create the XML element.
			XElement element = new XElement(name);
			// Add the metadata values.
			foreach (KeyValuePair<string, string> pair in this)
			{
				element.Add(new XElement("Entry",
					new XAttribute("Name", pair.Key),
					new XAttribute("Value", pair.Value)
					));
			}
			// Return the XML element.
			return element;
		}
	}
}
