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
using System.Drawing;
using System.Windows.Forms;

namespace DotNetApi.Windows.Themes
{
	/// <summary>
	/// The base class for theme color tables.
	/// </summary>
	public abstract class ThemeColorTable : ProfessionalColorTable
	{
		/// <summary>
		/// Gets the border color for a notification box.
		/// </summary>
		public abstract Color NotificationBoxBorder { get; }
		/// <summary>
		/// Gets the background color for a notification box.
		/// </summary>
		public abstract Color NotificationBoxBackground { get; }
		/// <summary>
		/// Gets the title color for a notification box.
		/// </summary>
		public abstract Color NotificationBoxTitle { get; }
		/// <summary>
		/// Gets the title text color for a notification box.
		/// </summary>
		public abstract Color NotificationBoxTitleText { get; }
	}
}
