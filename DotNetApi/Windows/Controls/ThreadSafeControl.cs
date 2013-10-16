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
using System.Runtime;
using System.Windows.Forms;
using System.Threading;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A base class for thread-safe controls.
	/// </summary>
	public class ThreadSafeControl : UserControl
	{
		private ManualResetEvent eventHandleCreated = new ManualResetEvent(false);
		private bool disposing = false;
		private object sync = new object();

		/// <summary>
		/// Creates a new thread-safe control instance.
		/// </summary>
		public ThreadSafeControl()
		{
		}

		// Public methods.

		/// <summary>
		/// Executes the specified delegate on the thread that owns the control's underlying window handle.
		/// </summary>
		/// <param name="method">A delegate that contains a method to be called in the control's thread context.</param>
		/// <returns>The return value from the delegate being invoked, or null if the delegate has no return value.</returns>
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		public new object Invoke(Delegate method)
		{
			lock (this.sync)
			{
				return this.disposing ? null : base.Invoke(method);
			}
		}

		/// <summary>
		/// Executes the specified delegate, on the thread that owns the control's underlying window handle, with the specified list of arguments.
		/// </summary>
		/// <param name="method">A delegate to a method that takes parameters of the same number and type that are contained in the args parameter.</param>
		/// <param name="args">An array of objects to pass as arguments to the specified method. This parameter can be null if the method takes no arguments.</param>
		/// <returns>An System.Object that contains the return value from the delegate being invoked, or null if the delegate has no return value.</returns>
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		public new object Invoke(Delegate method, params object[] args)
		{
			lock (this.sync)
			{
				return this.disposing ? null : base.Invoke(method, args);
			}
		}

		// Protected methods.

		/// <summary>
		/// Waits for the window handle of the current control to be created.
		/// </summary>
		/// <returns><b>True</b> if the wait was successful, <b>false</b> if the method exited because the control was disposed.</returns>
		protected bool WaitForHandle()
		{
			// Wait for the control handle to be created.
			while (!this.IsHandleCreated)
			{
				// If the control is disposing, return false.
				if (this.disposing) return false;
				// Wait for the control handle to be created.
				this.eventHandleCreated.WaitOne();
			}
			return true;
		}

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			lock (this.sync)
			{
				// Set the disposing to true.
				this.disposing = true;
			}
			// Dispose the current resources.
			if (disposing)
			{
				this.eventHandleCreated.Close();
			}
			// Call the base class method.
			base.Dispose(disposing);
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
