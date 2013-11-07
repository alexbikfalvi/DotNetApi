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
		private static readonly ThemeColorTable blueColorTable = new BlueThemeColorTable();

		/// <summary>
		/// Gets the default color table.
		/// </summary>
		public static ThemeColorTable DefaultColorTable { get { return ThemeColorTable.blueColorTable; } }
		/// <summary>
		/// Gets the blue color table.
		/// </summary>
		public static ThemeColorTable BlueColorTable { get { return ThemeColorTable.blueColorTable; } }

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
		/// <summary>
		/// Get the border color for a tool split container.
		/// </summary>
		public abstract Color ToolSplitContainerBorder { get; }
		/// <summary>
		/// Gets the title gradient begin color.
		/// </summary>
		public abstract Color PanelTitleGradientBegin { get; }
		/// <summary>
		/// Gets the title gradient end color.
		/// </summary>
		public abstract Color PanelTitleGradientEnd { get; }
		/// <summary>
		/// Gets the title text color.
		/// </summary>
		public abstract Color PanelTitleText { get; }
		/// <summary>
		/// Gets the selected title gradient begin color.
		/// </summary>
		public abstract Color PanelTitleSelectedGradientBegin { get; }
		/// <summary>
		/// Gets the selected title gradient end color.
		/// </summary>
		public abstract Color PanelTitleSelectedGradientEnd { get; }
		/// <summary>
		/// Gets the selected title text color.
		/// </summary>
		public abstract Color PanelTitleSelectedText { get; }
		/// <summary>
		/// Gets the normal background color of the status strip.
		/// </summary>
		public abstract Color StatusStripNormalBackground { get; }
		/// <summary>
		/// Gets the normal text color of the status strip.
		/// </summary>
		public abstract Color StatusStripNormalText { get; }
	}
}
