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

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// An interface representing the core functionality of a side control.
	/// </summary>
	public interface ISideControl
	{
		/// <summary>
		/// Initializes the current side control.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Shows the current side control and activates the control content.
		/// </summary>
		void ShowSideControl();

		/// <summary>
		/// Hides the current side control and deactivates the control content.
		/// </summary>
		void HideSideControl();

		/// <summary>
		/// Indicates whether the control has a selectable item.
		/// </summary>
		/// <returns><b>True</b> if the control has a selectable item, <b>false</b> otherwise.</returns>
		bool HasSelected();

		/// <summary>
		/// Returns the indices of the selected item.
		/// </summary>
		/// <returns>The indices.</returns>
		int[] GetSelected();

		/// <summary>
		/// Sets the selected item.
		/// </summary>
		/// <param name="indices">The item indices.</param>
		void SetSelected(int[] indices);
	}
}
