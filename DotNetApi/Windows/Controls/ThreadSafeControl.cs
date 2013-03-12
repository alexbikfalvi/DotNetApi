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
using System.Windows.Forms;
using System.Threading;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A base class for thread-safe controls.
	/// </summary>
	public class ThreadSafeControl : UserControl
	{
		private AutoResetEvent eventHandleCreated = new AutoResetEvent(false);

		/// <summary>
		/// Creates a new thread-safe control instance.
		/// </summary>
		public ThreadSafeControl()
		{
			this.HandleCreated += this.OnHandleCreated;
		}

		/// <summary>
		/// Waits for the window handle of the current control to be created.
		/// </summary>
		protected void WaitForHandle()
		{
			do
			{
				this.eventHandleCreated.WaitOne();
			}
			while (!this.IsHandleCreated);
		}

		/// <summary>
		/// An event handler called when the handle of the current control was created.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnHandleCreated(object sender, EventArgs e)
		{
			this.eventHandleCreated.Set();
		}
	}
}
