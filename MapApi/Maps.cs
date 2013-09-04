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
using System.Reflection;
using System.Threading;

namespace MapApi
{
	/// <summary>
	/// A static class with all loaded maps.
	/// </summary>
	public static class Maps
	{
		private static Mutex mutex = new Mutex();
		private static ManualResetEvent load = new ManualResetEvent(false);
		private static Dictionary<string, Map> maps = new Dictionary<string, Map>();

		/// <summary>
		/// Loads all maps in the current resource file.
		/// </summary>
		static Maps()
		{
			// Load all maps on the thread pool.
			ThreadPool.QueueUserWorkItem((object state) =>
				{
					// Lock the mutex.
					Maps.mutex.WaitOne();
					try
					{
						// For all public and static properties. 
						foreach (PropertyInfo property in typeof(MapsBinary).GetProperties(BindingFlags.Public | BindingFlags.Static))
						{
							// If the property type is a byte array.
							if (property.PropertyType.Equals(typeof(byte[])))
							{
								// Get the property data.
								byte[] data = property.GetValue(null, null) as byte[];
								// Create a map from the specified data.
								Map map = Map.Read(data);
								// Add the map to the maps list.
								Maps.maps.Add(property.Name, map);
							}
						}
					}
					finally
					{
						// Change the load event state.
						Maps.load.Set();
						// Unlock the mutex.
						Maps.mutex.ReleaseMutex();
					}
				});
		}

		// Public methods.

		/// <summary>
		/// Gets the map with specified name.
		/// </summary>
		/// <param name="name">The map name.</param>
		/// <returns>The map.</returns>
		public static Map Get(string name)
		{
			// Wait for the maps to load.
			Maps.load.WaitOne();
			// Lock the mutex.
			Maps.mutex.WaitOne();
			try
			{
				// Return the map.
				return Maps.maps[name];
			}
			finally
			{
				// Unlock the mutex.
				Maps.mutex.ReleaseMutex();
			}
		}
	}
}
