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
using System.Threading;

namespace DotNetApi.Async
{
	/// <summary>
	/// A class representing the state of an asynchronous operation.
	/// </summary>
	public class AsyncState : IDisposable
	{
		private readonly object state;

		private bool canceled = false;
		private WaitCallback cancelCallback = null;
		private object cancelState = null;

		private ManualResetEvent wait = new ManualResetEvent(false);

		/// <summary>
		/// Creates a new asynchronous state.
		/// </summary>
		/// <param name="state">The user state.</param>
		public AsyncState(object state)
		{
			this.state = state;
		}

		// Public properties.

		/// <summary>
		/// Gets whether the asynchronous operation should be canceled.
		/// </summary>
		public bool IsCanceled { get { return this.canceled; } }

		/// <summary>
		/// Gets the wait handle for the current asynchronous operation.
		/// </summary>
		public WaitHandle Handle { get { return this.wait; } }

		// Public methods.

		/// <summary>
		/// Disposes the current object.
		/// </summary>
		public void Dispose()
		{
			// Call the dispose event handler.
			this.Dispose(true);
			// Suppress the finalizer.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Signals the asynchronous operation to cancel its operation
		/// </summary>
		/// <param name="callback">The method to call after the operation has been canceled.</param>
		/// <param name="state">The cancel callback state.</param>
		public void Cancel(WaitCallback callback, object state = null)
		{
			// Set the canceled callback and the canceled state.
			this.cancelCallback = callback;
			this.cancelState = state;
			// Set the canceled flag to true.
			this.canceled = true;
		}

		// Internal methods.

		/// <summary>
		/// Signals the completion of the asynchronous operation.
		/// </summary>
		internal void Complete()
		{
			this.wait.Set();
		}

		/// <summary>
		/// Executes the cancel callbeck method.
		/// </summary>
		internal void CancelCallback()
		{
			if (null != this.cancelCallback)
			{
				this.cancelCallback(this.cancelState);
			}
		}

		// Protected methods.

		/// <summary>
		/// Disposes the current object.
		/// </summary>
		/// <param name="disposing">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			// Dispose the current objects.
			if (disposing)
			{
				this.wait.Dispose();
			}
		}
	}
}
