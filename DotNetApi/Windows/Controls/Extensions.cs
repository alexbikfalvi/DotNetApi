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
using System.Linq;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class with extension methods for windows forms controls.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Returns the first element in the list view items collection that matches the specified predicate.
		/// </summary>
		/// <param name="items">The collection of list view items.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The list view item found, or the default value otherwise.</returns>
		public static ListViewItem FirstOrDefault(this ListView.ListViewItemCollection items, Func<ListViewItem, bool> predicate)
		{
			foreach (ListViewItem item in items)
			{
				if (predicate(item)) return item;
			}
			return default(ListViewItem);
		}

		/// <summary>
		/// Returns an array with the checked list view items.
		/// </summary>
		/// <param name="items">The collection of list view items.</param>
		/// <returns>The array with the checked list view items.</returns>
		public static IList<ListViewItem> Checked(this ListView.ListViewItemCollection items)
		{
			// Create a list with the checked list view items.
			List<ListViewItem> list = new List<ListViewItem>();
			// Add the checked list view items.
			foreach (ListViewItem item in items)
			{
				if (item.Checked) list.Add(item);
			}
			// Return the list.
			return list;
		}
	}
}