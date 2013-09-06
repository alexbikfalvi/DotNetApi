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

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A delegate representing a progress info event handler.
	/// </summary>
	/// <param name="sender">The sender object.</param>
	/// <param name="e">The event arguments.</param>
	public delegate void ProgressInfoEventHandler(object sender, ProgressInfoEventArgs e);

	/// <summary>
	/// A class representing a progress info event arguments.
	/// </summary>
	public class ProgressInfoEventArgs : EventArgs
	{
		/// <summary>
		/// Creates a new event arguments instance.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		public ProgressInfoEventArgs(ProgressInfo progress)
		{
			this.Progress = progress;
		}

		// Public properties.

		/// <summary>
		/// Gets the progress info.
		/// </summary>
		public ProgressInfo Progress { get; private set; }
	}
}
