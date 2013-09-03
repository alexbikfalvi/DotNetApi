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
using System.ComponentModel;
using System.Drawing;
using MapApi;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a map item.
	/// </summary>
	public abstract class MapItem : Component
	{
		/// <summary>
		/// Creates a new map item instance.
		/// </summary>
		public MapItem()
		{
		}

		// Abstract methods.

		/// <summary>
		/// Updates the map item geometric characteristics to the specified map bounds and scale.
		/// </summary>
		/// <param name="bounds">The map bounds.</param>
		/// <param name="scale">The map scale.</param>
		internal abstract void Update(MapRectangle bounds, MapScale scale);

		/// <summary>
		/// Draws the item on the specified graphics object.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		/// <param name="brush">The brush.</param>
		/// <param name="pen">The pen.</param>
		internal abstract void Draw(Graphics graphics, Brush brush, Pen pen);
	}
}
