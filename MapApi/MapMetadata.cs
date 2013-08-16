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
using System.Xml.Serialization;
using DotNetApi;

namespace MapApi
{
	/// <summary>
	/// A class representing a map metdata collection.
	/// </summary>
	[Serializable]
	public class MapMetadata : IEnumerable<MapMetadataEntry>
	{
		private readonly Dictionary<string, MapMetadataEntry> metadata = new Dictionary<string, MapMetadataEntry>();

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
			get { return this.metadata[name].Value; }
			set { this.metadata[name] = new MapMetadataEntry(name, value); }
		}

		// Public methods.

		/// <summary>
		/// Returns the generic enumerator for the metadata collection.
		/// </summary>
		/// <returns>The generic enumerator.</returns>
		public IEnumerator<MapMetadataEntry> GetEnumerator()
		{
			return this.metadata.Values.GetEnumerator();
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
		/// Adds the specified object to the metadata.
		/// </summary>
		/// <param name="obj">The object to add.</param>
		public void Add(MapMetadataEntry entry)
		{
			this.metadata.Add(entry.Name, entry);
		}
	}
}
