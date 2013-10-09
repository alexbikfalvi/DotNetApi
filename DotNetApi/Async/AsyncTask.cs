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
using DotNetApi;

namespace DotNetApi.Async
{
	/// <summary>
	/// An enumeration representing the state of the asynchronous task.
	/// </summary>
	public enum AsyncTaskState
	{
		Ready = 0,
		Running = 1,
		Canceling = 2
	}

	/// <summary>
	/// A delegate for asynchronous tasks.
	/// </summary>
	/// <param name="asyncState">The asynchronous state.</param>
	public delegate void AsyncTaskCallback(AsyncState asyncState);

	/// <summary>
	/// A class representing a single asynchronous task.
	/// </summary>
	public class AsyncTask
	{
		private AsyncTaskState taskState = AsyncTaskState.Ready;
		private AsyncState asyncState = null;

		private readonly object syncState = new object();
		private readonly object syncTask = new object();

		/// <summary>
		/// Creates a new asynchronous task instance.
		/// </summary>
		public AsyncTask()
		{
		}

		// Public methods.

		/// <summary>
		/// Executes the specified task if the asynchronous task is in a ready state. Otherwise, it throws an exception.
		/// </summary>
		/// <param name="method">The method to execute.</param>
		/// <param name="userState">The user state.</param>
		public void ExecuteReady(AsyncTaskCallback method, object userState = null)
		{
			// Validate the arguments.
			if (null == method) throw new ArgumentNullException("method");

			// Lock the state.
			lock (this.syncState)
			{
				// If the task state is not ready, throw an exception.
				if (this.taskState != AsyncTaskState.Ready) throw new InvalidOperationException("Cannot execute the method because the asynchronous task is not in the ready state.");
				// Execute the method on the thread pool.
				this.Execute(method, userState);
			}
		}

		/// <summary>
		/// Executes the specified task always. If there exists a current task, the task is canceled and the new task waits for its completion.
		/// </summary>
		/// <param name="method">The method to execute.</param>
		/// <param name="userState">The user state.</param>
		public void ExecuteAlways(AsyncTaskCallback method, object userState = null)
		{
			// Validate the arguments.
			if (null == method) throw new ArgumentNullException("method");

			// Lock the state.
			lock (this.syncState)
			{
				// If the task state is not ready.
				if (this.taskState != AsyncTaskState.Ready)
				{
					// Cancel the current operation.
					this.asyncState.Cancel((object state) =>
						{
							this.Execute(method, userState);
						});
				}
				else
				{
					// Execute the method.
					this.Execute(method, userState);
				}
			}
		}
		
		// Private methods.

		/// <summary>
		/// Creates an asynchronous state, and executes the method on the thread pool.
		/// </summary>
		/// <param name="method">The method to execute.</param>
		/// <param name="userState">The user state.</param>
		private void Execute(AsyncTaskCallback method, object userState)
		{
			// Create the asynchronous state for the current method.
			AsyncState asyncState = new AsyncState(userState);
			// Execute the method on the thread pool.
			ThreadPool.QueueUserWorkItem((object state) =>
			{
				// Lock the task.
				lock (this.syncTask)
				{
					// Lock the state.
					lock (this.syncState)
					{
						// Change the task state.
						this.taskState = AsyncTaskState.Running;
						// Set the current asychronous state.
						this.asyncState = asyncState;
					}
					// Execute the task method.
					method(asyncState);
					// If the method has been canceled.
					if (asyncState.IsCanceled)
					{
						// Execute the cancel callback method.
						asyncState.CancelCallback();
					}
					// Lock the state.
					lock (this.syncState)
					{
						// Change the task state.
						this.taskState = AsyncTaskState.Ready;
						// Set the current asychronous state to null.
						this.asyncState = null;
						// Signal the completion of the asynchronous operation.
						asyncState.Complete();
					}
					// Dispose of the asynchronous state.
					asyncState.Dispose();
				}
			});
		}
	}
}
