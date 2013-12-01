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
	/// A class representing a blue theme settings.
	/// </summary>
	public sealed class BlueThemeSettings : ThemeSettings
	{
		/// <summary>
		/// Gets the theme color table.
		/// </summary>
		public override ThemeColorTable ColorTable { get { return ThemeColorTable.BlueColorTable; } }
		/// <summary>
		/// Gets the panel title height.
		/// </summary>
		public override int PanelTitleHeight { get { return (int)(2.34 * this.PanelTitleFontSize); } }
		/// <summary>
		/// Gets the panel title font size.
		/// </summary>
		public override float PanelTitleFontSize { get { return Window.DefaultFont.Size; } }
		/// <summary>
		/// Gets the panel title font style.
		/// </summary>
		public override FontStyle PanelTitleFontStyle { get { return FontStyle.Bold; } }
	}
}
