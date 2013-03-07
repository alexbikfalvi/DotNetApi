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
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// Creates a tool strip drop down calendar.
	/// </summary>
	public class ToolStripDropDownCalendar : ToolStripDropDown
	{
		private MonthCalendar calendar = new MonthCalendar();

		/// <summary>
		/// Creates a new class instance.
		/// </summary>
		public ToolStripDropDownCalendar()
		{

			// Add the list box to the drop down.
			this.Items.Add(new ToolStripControlHost(this.calendar));

			// Set the padding.
			this.Padding = new Padding(4, 2, 4, 0);
		}

		/// <summary>
		/// Gets the calendar control.
		/// </summary>
		public MonthCalendar Calendar { get { return this.calendar; } }
	}
}
