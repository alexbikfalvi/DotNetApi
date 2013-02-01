/* 
 * Copyright (C) 2012 Alex Bikfalvi
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YtApi
{
	/// <summary>
	/// A class representing the asynchronous request result.
	/// </summary>
	public class AsyncRequestResult : IAsyncResult
	{
		/// <summary>
		/// Indicates whether the asynchronous operation has completed.
		/// </summary>
		public bool IsCompleted { get; set; }

		/// <summary>
		/// The state of the asynchronous operation.
		/// </summary>
		public object AsyncState { get; set; }

		/// <summary>
		/// Indicates whether the operation completed synchronously.
		/// </summary>
		public bool CompletedSynchronously { get; set; }

		/// <summary>
		/// The wait handle of the asynchrounous operation.
		/// </summary>
		public WaitHandle AsyncWaitHandle { get; set; }
	}

	/// <summary>
	/// An interface for conversion of the asynchronous operation data to a custom type.
	/// </summary>
	/// <typeparam name="T">The custom type used for conversion.</typeparam>
	public interface IAsyncFunction<T>
	{
		/// <summary>
		/// Processes the string data from the asynchronous operation, and returns a custom type result.
		/// </summary>
		/// <returns>The custom type result.</returns>
		T GetResult(string data);
	}

	public delegate void AsyncRequestCallback(AsyncRequestResult result);

	/// <summary>
	/// A class representing the asynchronous request state.
	/// </summary>
	public class AsyncRequestState
	{
		public const int BUFFER_SIZE = 4096;

		private byte[] buffer = null;
		private AutoResetEvent waitHandle = new AutoResetEvent(false);

		/// <summary>
		/// Constructs an object for an asynchronous request state.
		/// </summary>
		/// <param name="uri">The URI of the asynchronous request.</param>
		/// <param name="callback">The callback function for the asynchronous request.</param>
		public AsyncRequestState(Uri uri, AsyncRequestCallback callback)
		{
			this.Data = new AsyncBuffer();
			this.Request = (HttpWebRequest)WebRequest.Create(uri);
			this.Callback = callback;
			this.buffer = new byte[BUFFER_SIZE];
		}

		/// <summary>
		/// The data returned by the asynchrounous request.
		/// </summary>
		public AsyncBuffer Data { get; set; }

		/// <summary>
		/// The request object corresponding to the asynchrounous operation.
		/// </summary>
		public HttpWebRequest Request { get; set; }

		/// <summary>
		/// The response object corresponding to the asynchronous request.
		/// </summary>
		public HttpWebResponse Response { get; set; }

		/// <summary>
		/// The stream object corresponding to the asynchronous request.
		/// </summary>
		public Stream Stream { get; set; }

		/// <summary>
		/// The buffer used to store the data returned by the asynchronous request.
		/// </summary>
		public byte[] Buffer { get { return this.buffer; } }

		/// <summary>
		/// The exception thrown during the asynchronous operation.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// The result of the asynchrnous operation.
		/// </summary>
		public AsyncRequestResult Result { get; set; }

		/// <summary>
		/// The callback function for the asynchronous operation.
		/// </summary>
		public AsyncRequestCallback Callback { get; set; }

		/// <summary>
		/// The user state for the asynchronous request.
		/// </summary>
		public object State { get; set; }

		/// <summary>
		/// The wait handle of the asynchronous operation.
		/// </summary>
		public AutoResetEvent WaitHandle { get { return this.waitHandle; } }
	}

	public class AsyncBuffer
	{
		/// <summary>
		/// Appends byte data to the current buffer.
		/// </summary>
		/// <param name="data">A byte array containing the data to append.</param>
		/// <param name="count">The number of bytes to append.</param>
		public void Append(byte[] data, int count)
		{
			// Get the size of the old buffer.
			int sizeOld = (null != this.Bytes) ? this.Bytes.Length : 0;
			// Compute the size of the new buffer.
			int sizeNew = sizeOld + count;
			// Allocate a new buffer.
			byte[] buffer = new byte[sizeNew];
			// Copy the old buffer into the new buffer.
			if (null != this.Bytes)
			{
				Buffer.BlockCopy(this.Bytes, 0, buffer, 0, sizeOld);
			}
			// Append the new data into the new buffer.
			Buffer.BlockCopy(data, 0, buffer, sizeOld, count);
			// Assign the new buffer.
			this.Bytes = buffer;
		}

		public byte[] Bytes { get; set; }
	};

	public class AsyncRequest
	{
		public static TimeSpan TIMEOUT_DEFAULT = new TimeSpan(0, 0, 10);

		/// <summary>
		/// Constructor of an asynchronous request.
		/// </summary>
		public AsyncRequest()
		{
			this.Timeout = AsyncRequest.TIMEOUT_DEFAULT;
		}

		/// <summary>
		/// The timeout for the asynchrnous operation, when executed synchronously.
		/// </summary>
		public TimeSpan Timeout { get; set; }

		/// <summary>
		/// Creates the state of an asynchronous request for a web resource.
		/// </summary>
		/// <param name="uri">The URI of the web resource.</param>
		/// <param name="callback">The delegate to the callback function.</param>
		/// <returns>The state of the asynchronous request.</returns>
		public static AsyncRequestState Create(Uri uri, AsyncRequestCallback callback)
		{
			return new AsyncRequestState(uri, callback);
		}

		/// <summary>
		/// Begins an asynchronous request for a web resource.
		/// </summary>
		/// <param name="uri">The URI of the web resource.</param>
		/// <param name="callback">The delegate to the callback function.</param>
		/// <returns>The result of the asynchronous request.</returns>
		public IAsyncResult Begin(Uri uri, AsyncRequestCallback callback)
		{
			return this.BeginAsyncRequest(new AsyncRequestState(uri, callback));
		}

		/// <summary>
		/// Begins an asynchronous request for a web resource.
		/// </summary>
		/// <param name="asyncState">The state of the asynchronous request.</param>
		/// <returns>The result of the asynchronous request.</returns>
		protected IAsyncResult Begin(AsyncRequestState asyncState)
		{
			return this.BeginAsyncRequest(asyncState);
		}

		/// <summary>
		/// Completes the asynchronous operation and returns the received data.
		/// </summary>
		/// <typeparam name="T">The type of the returned data.</typeparam>
		/// <param name="result">The asynchronous result.</param>
		/// <param name="func">An instance used to convert the received data to the desired format.</param>
		/// <returns>The data received during the asynchronous operation.</returns>
		protected T End<T>(IAsyncResult result, IAsyncFunction<T> func)
		{
			// Get the result of the asynchronous operation
			AsyncRequestResult asyncResult = (AsyncRequestResult)result;

			// Get the state of the asynchronous operation.
			AsyncRequestState asyncState = (AsyncRequestState)result.AsyncState;

			// If an exception was thrown during the execution of the asynchronous operation.
			if (asyncState.Exception != null)
			{
				// Throw the exception.
				throw asyncState.Exception;
			}

			Encoding encoding = Encoding.GetEncoding(asyncState.Response.CharacterSet);

			// Return the converted data.
			return func.GetResult((null != asyncState.Data.Bytes) ? encoding.GetString(asyncState.Data.Bytes) : string.Empty);
		}

		public void Cancel(IAsyncResult result)
		{
			// Get the state of the asynchronous operation.
			AsyncRequestState asyncState = (AsyncRequestState)result.AsyncState;

			// Use the system thread pool to cancel the request on a worker thread.
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.CancelAsync), asyncState);
		}

		protected void CancelAsync(object state)
		{
			AsyncRequestState asyncState = state as AsyncRequestState;

			// Abort the web request.
			asyncState.Request.Abort();
		}

		protected IAsyncResult BeginAsyncRequest(AsyncRequestState asyncState)
		{
			// Create the result of the asynchronous operation
			asyncState.Result = new AsyncRequestResult();
			asyncState.Result.AsyncState = asyncState;
			asyncState.Result.AsyncWaitHandle = asyncState.WaitHandle;
			asyncState.Result.CompletedSynchronously = false;
			asyncState.Result.IsCompleted = false;

			// Use the system thread pool to begin the asynchronous request on a worker thread.
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.BeginWebRequest), asyncState);

			return asyncState.Result;
		}

		protected void BeginWebRequest(object state)
		{
			AsyncRequestState asyncState = state as AsyncRequestState;

			// Begin the web request.
			asyncState.Request.BeginGetResponse(this.EndWebRequest, asyncState);
		}

		protected void EndWebRequest(IAsyncResult result)
		{
			// Get the state of the asynchronous operation
			AsyncRequestState asyncState = (AsyncRequestState)result.AsyncState;

			try
			{
				// If the asynchrounous operation is not completed
				if (!result.IsCompleted)
				{
					// Wait on the operation handle until the timeout expires
					if (!result.AsyncWaitHandle.WaitOne(this.Timeout))
					{
						// If no signal was received in the timeout period, throw a timeout exception.
						throw new TimeoutException(String.Format("The web request did not complete within the timeout {0}.", this.Timeout));
					}
				}

				// Complete the web request and get the result.
				asyncState.Response = (HttpWebResponse)asyncState.Request.EndGetResponse(result);
				// Get the stream corresponding to the web response.
				asyncState.Stream = asyncState.Response.GetResponseStream();
				// Begin reading for the returned data.
				this.BeginStreamRead(asyncState);
			}
			catch (Exception exception)
			{
				// Set the exception.
				asyncState.Exception = exception;
				// Signal that the operation has completed.
				asyncState.Result.IsCompleted = true;
				if (asyncState.Callback != null)
				{
					asyncState.Callback(asyncState.Result);
				}
			}
			finally
			{
				// Signal the wait handle.
				asyncState.WaitHandle.Set();
			}
		}

		protected IAsyncResult BeginStreamRead(AsyncRequestState asyncState)
		{
			// Begin the read operation.
			return asyncState.Stream.BeginRead(asyncState.Buffer, 0, AsyncRequestState.BUFFER_SIZE, this.EndStreamRead, asyncState);
		}

		protected void EndStreamRead(IAsyncResult result)
		{
			// Get the state of the asynchronous operation
			AsyncRequestState asyncState = (AsyncRequestState)result.AsyncState;

			try
			{
				// If the asynchrounous operation is not completed
				if (!result.IsCompleted)
				{
					// Wait on the operation handle until the timeout expires
					if (!result.AsyncWaitHandle.WaitOne(this.Timeout))
					{
						// If no signal was received in the timeout period, throw a timeout exception.
						throw new TimeoutException(String.Format("The read request did not complete within the timeout {0}.", this.Timeout));
					}
				}

				// Complete the asynchronous request and get the bytes read.
				int count = asyncState.Stream.EndRead(result);

				// Append the bytes read to the string buffer.
				asyncState.Data.Append(asyncState.Buffer, count);
				
				if (count > 0)
				{
					// If bytes were read, begin a new read request.
					this.BeginStreamRead(asyncState);
				}
				else
				{
					// Otherwise, signal that the asynchronous operation has completed.
					asyncState.Result.IsCompleted = true;
					if (asyncState.Callback != null)
					{
						asyncState.Callback(asyncState.Result);
					}
				}
			}
			catch (Exception exception)
			{
				// If an exception occured, set the exception and complete the asynchronous operation
				asyncState.Exception = exception;
				asyncState.Result.IsCompleted = true;
				if (asyncState.Callback != null)
				{
					try { asyncState.Callback(asyncState.Result); } catch(Exception) { }
				}
			}
		}
	}
}
