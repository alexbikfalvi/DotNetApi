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
		}

		// Protected methods.

		/// <summary>
		/// Waits for the window handle of the current control to be created.
		/// </summary>
		protected void WaitForHandle()
		{
			// Wait for the control handle to be created.
			while (!this.IsHandleCreated)
			{
				this.eventHandleCreated.WaitOne();
			}
		}

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			// Call the base class method.
			base.Dispose(disposing);
			// Dispose the current resources.
			if (disposing)
			{
				this.eventHandleCreated.Dispose();
			}
		}

		/// <summary>
		/// An event handler called when the handle of the current control was created.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			// Call the base class method.
			base.OnHandleCreated(e);
			// Set the event.
			this.eventHandleCreated.Set();
		}
	}
}
