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

namespace DotNetApi.Windows.Themes
{
	/// <summary>
	/// A class representing the theme settings.
	/// </summary>
	public abstract class ThemeSettings
	{
		private static readonly BlueThemeSettings blueSettings = new BlueThemeSettings();

		/// <summary>
		/// Gets the default theme settings.
		/// </summary>
		public static ThemeSettings Default { get { return ThemeSettings.blueSettings; } }
		/// <summary>
		/// Gets the blue theme settings.
		/// </summary>
		public static ThemeSettings Blue { get { return ThemeSettings.blueSettings; } }

		/// <summary>
		/// Gets the theme color table.
		/// </summary>
		public abstract ThemeColorTable ColorTable { get; }
		/// <summary>
		/// Gets the panel title height.
		/// </summary>
		public abstract int PanelTitleHeight { get; }
		/// <summary>
		/// Gets the panel title font size.
		/// </summary>
		public abstract float PanelTitleFontSize { get; }
		/// <summary>
		/// Gets the panel title font style.
		/// </summary>
		public abstract FontStyle PanelTitleFontStyle { get; }
	}
}
