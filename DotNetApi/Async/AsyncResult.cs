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
	/// A class representing the state for a database asynchronous operation.
	/// </summary>
	public class AsyncResult : IAsyncResult
	{
		private object state;
		private AutoResetEvent wait = new AutoResetEvent(false);
		private bool completedSynchronously = false;
		private bool completed = false;
		private Exception exception = null;

		/// <summary>
		/// Creates a new instance of the asynchronous state.
		/// </summary>
		/// <param name="state">The user state.</param>
		public AsyncResult(object state)
		{
			// Save the user state.
			this.state = state;
		}

		// Public properties.

		/// <summary>
		/// Returns the asynchronous user state.
		/// </summary>
		public object AsyncState { get { return this.state; } }

		/// <summary>
		/// Returns the wait handle.
		/// </summary>
		public WaitHandle AsyncWaitHandle { get { return this.wait; } }

		/// <summary>
		/// Indicates whether the operation completed synchronously.
		/// </summary>
		public bool CompletedSynchronously
		{
			get { return this.completedSynchronously; }
			set { this.completedSynchronously = value; }
		}
		/// <summary>
		/// Gets or sets the asynchronous state exception.
		/// </summary>
		public Exception Exception
		{
			get { return this.exception; }
			set { this.exception = value; }
		}
		/// <summary>
		/// Indicates whether the operation completed.
		/// </summary>
		public bool IsCompleted { get { return this.completed; } }

		// Public methods.

		/// <summary>
		/// Completes the asynchronous operation by setting the completed property and the wait handle
		/// to <b>true</b>.
		/// </summary>
		public void Complete()
		{
			this.completed = true;
			this.wait.Set();
		}
	}
}
