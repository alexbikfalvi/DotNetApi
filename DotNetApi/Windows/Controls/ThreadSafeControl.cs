﻿/* 
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
		private readonly ManualResetEvent eventHandleCreated = new ManualResetEvent(false);
		private readonly Action<Action> action;

		private bool lastFocusEvent = false;

		/// <summary>
		/// Creates a new thread-safe control instance.
		/// </summary>
		public ThreadSafeControl()
		{
			this.action = new Action<Action>(this.Invoke);
		}

		// Public methods.

		/// <summary>
		/// Executes the specified action, on the thread that owns the control's underlying window handle, with the specified list of arguments.
		/// </summary>
		/// <param name="action">The action.</param>
		public void Invoke(Action action)
		{
			// If the method is called on a different thread.
			if (this.InvokeRequired)
			{
				// Invoke the action delegate.
				this.Invoke(this.action, new object[] { action });
			}
			else if(!this.IsDisposed)
			{
				// Call the action.
				action();
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
				// If the control is disposed, return false.
				if (this.IsDisposed) return false;
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

		/// <summary>
		/// An event handler called when a child control has been added.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			// Call the base class method.
			base.OnControlAdded(e);
			// Subscribe to the child control events.
			this.OnSubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when a child control has been removed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			// Call the base classs method.
			base.OnControlRemoved(e);
			// Unsubscribe from the child control events.
			this.OnUnsubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when the control got focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			// Call the base class method.
			base.OnGotFocus(e);
			// Call the any got focus handler.
			this.OnCheckGotFocus(e);
		}

		/// <summary>
		/// An event handler called when the control lost focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			// Call the base class method.
			base.OnLostFocus(e);
			// Call the any lost focus handler.
			this.OnCheckLostFocus(e);
		}

		/// <summary>
		/// An event handler called when the current control or any child control got focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected virtual void OnAnyGotFocus(EventArgs e)
		{

		}

		/// <summary>
		/// An event handler called when the current control or any child control lost focus.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnAnyLostFocus(EventArgs e)
		{

		}

		// Private methods.

		/// <summary>
		/// Subscribes to the events of a child control.
		/// </summary>
		/// <param name="control">The child control.</param>
		private void OnSubscribeEvents(Control control)
		{
			// Add child control event handlers.
			control.GotFocus += this.OnControlGotFocus;
			control.LostFocus += this.OnControlLostFocus;
			control.ControlAdded += this.OnControlControlAdded;
			control.ControlRemoved += this.OnControlControlRemoved;

			// Subscribe to the events of the child controls.
			foreach (Control child in control.Controls)
			{
				this.OnSubscribeEvents(child);
			}
		}

		/// <summary>
		/// Unsubscribes from the events of a child control.
		/// </summary>
		/// <param name="control">The child control.</param>
		private void OnUnsubscribeEvents(Control control)
		{
			// Remove child control event handlers.
			control.GotFocus -= this.OnControlGotFocus;
			control.LostFocus -= this.OnControlLostFocus;
			control.ControlAdded -= this.OnControlControlAdded;
			control.ControlRemoved -= this.OnControlControlRemoved;

			// Unsubscribe from the events of the child controls.
			foreach (Control child in control.Controls)
			{
				this.OnUnsubscribeEvents(child);
			}
		}

		/// <summary>
		/// An event handler called when a child control got focus.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlGotFocus(object sender, EventArgs e)
		{
			this.OnCheckGotFocus(e);
		}

		/// <summary>
		/// An event handler called when a child control lost focus.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlLostFocus(object sender, EventArgs e)
		{
			this.OnCheckLostFocus(e);
		}

		/// <summary>
		/// An event handler called when a child control added a control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlControlAdded(object sender, ControlEventArgs e)
		{
			// Subscribe to the child control events.
			this.OnSubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when a child control removed a control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlControlRemoved(object sender, ControlEventArgs e)
		{
			// Unsubscribe from the child control events.
			this.OnUnsubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when any control got focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		private void OnCheckGotFocus(EventArgs e)
		{
			if (!this.lastFocusEvent)
			{
				// Set the flag to true.
				this.lastFocusEvent = true;
				// Call the event handler.
				this.OnAnyGotFocus(e);
			}
		}

		/// <summary>
		/// An event handler called when any control lost focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		private void OnCheckLostFocus(EventArgs e)
		{
			if (!this.ContainsFocus)
			{
				// Set the flag to false.
				this.lastFocusEvent = false;
				// Call the event handler.
				this.OnAnyLostFocus(e);
			}
		}
	}
}
