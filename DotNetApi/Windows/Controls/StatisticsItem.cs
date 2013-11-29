/* 
 * Copyright (C) 2012-2013 Alex Bikfalvi
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

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A statistics list box item.
	/// </summary>
	[DesignTimeVisible(false)]
	public sealed class StatisticsItem : Component
	{
		private string text;

		/// <summary>
		/// Creates a new statistics item instance.
		/// </summary>
		public StatisticsItem()
		{
		}

		/// <summary>
		/// Creates a new statistics item with the specified text.
		/// </summary>
		/// <param name="text">The item text</param>
		public StatisticsItem(string text)
		{
			this.text = text;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the item text.
		/// </summary>
		public string Text
		{
			get { return this.text; }
			//set { this.OnTextSet(value); }
		}

		// Private methods.

		//public 
	}
}
