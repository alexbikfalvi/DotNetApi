/* 
 * Copyright (C) 2012 Alex Bikfalvi
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
using System.Drawing;
using System.Windows.Forms;

namespace DotNetApi.Windows
{
	/// <summary>
	/// Formats a control to the default configuration.
	/// </summary>
	public static class Formatting
	{
		private static bool fontChange = false;
		private static FontFamily fontDefaultFamily = null;
		static readonly List<string> fontReplaceList = new List<string>(new string[] { "Microsoft Sans Serif", "Tahoma" });

		static Formatting()
		{
			// Check the major version of the operating system.
			if (Environment.OSVersion.Version.Major == 5)
			{
				// Windows 2000 (5.0), XP (5.1) and Server 2003 (5.2): the default font is Tahoma.
				Formatting.fontDefaultFamily = SystemFonts.DialogFont.FontFamily;
				Formatting.fontChange = true;
			}
			else if (Environment.OSVersion.Version.Major >= 6)
			{
				// Windows Vista and above: the default font is SegoeUI.
				Formatting.fontDefaultFamily = SystemFonts.MessageBoxFont.FontFamily;
				Formatting.fontChange = true;
			}
		}

		public static void SetFont(Control control)
		{
			// If the control is null, exit.
			if (null == control) return;
			// If the font cannot be changed, return.
			if (!Formatting.fontChange) return;
			// Suspend the control layout.
			control.SuspendLayout();
			// For all child controls.
			foreach (Control child in control.Controls)
			{
				// If the child font is in the replace list.
				if (Formatting.fontReplaceList.IndexOf(child.Font.Name) > -1)
				{
					Formatting.SetFont(child);
					child.Font = new Font(Formatting.fontDefaultFamily, child.Font.Size, child.Font.Style);
				}
			}
			// Resume the control layout.
			control.ResumeLayout();
		}
	}
}
