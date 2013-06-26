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
using System.IO;
using System.Net;
using System.Threading;
using System.Text;

namespace DotNetApi.Web
{
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

	public delegate void AsyncWebRequestCallback(AsyncWebResult result);

	/// <summary>
	/// A class that represents an asynchronous web request.
	/// </summary>
	public class AsyncWebRequest
	{
		public static TimeSpan TIMEOUT_DEFAULT = new TimeSpan(0, 0, 10);

		/// <summary>
		/// Constructor of an asynchronous request.
		/// </summary>
		public AsyncWebRequest()
		{
			this.Timeout = AsyncWebRequest.TIMEOUT_DEFAULT;
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
		/// <param name="userState">The user state.</param>
		/// <returns>The state of the asynchronous request.</returns>
		public static AsyncWebResult Create(Uri uri, AsyncWebRequestCallback callback, object userState)
		{
			return new AsyncWebResult(uri, callback, userState);
		}

		/// <summary>
		/// Begins an asynchronous request for a web resource.
		/// </summary>
		/// <param name="uri">The URI of the web resource.</param>
		/// <param name="callback">The delegate to the callback function.</param>
		/// <param name="userState">The user state.</param>
		/// <returns>The result of the asynchronous request.</returns>
		public IAsyncResult Begin(Uri uri, AsyncWebRequestCallback callback, object userState)
		{
			return this.BeginAsyncRequest(new AsyncWebResult(uri, callback, userState));
		}

		/// <summary>
		/// Begins an asynchronous request for a web resource.
		/// </summary>
		/// <param name="asyncState">The state of the asynchronous request.</param>
		/// <returns>The result of the asynchronous request.</returns>
		public IAsyncResult Begin(AsyncWebResult asyncState)
		{
			return this.BeginAsyncRequest(asyncState);
		}

		/// <summary>
		/// Completes the asynchronous operation and returnes the received data as a string.
		/// </summary>
		/// <param name="result">The asynchronous result.</param>
		/// <returns>The asynchronous web result.</returns>
		public AsyncWebResult End(IAsyncResult result)
		{
			// Get the state of the asynchronous operation.
			AsyncWebResult asyncState = (AsyncWebResult)result;

			// If an exception was thrown during the execution of the asynchronous operation.
			if (asyncState.Exception != null)
			{
				// Throw the exception.
				throw asyncState.Exception;
			}

			// Return the web result.
			return asyncState;
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
			// Get the state of the asynchronous operation.
			AsyncWebResult asyncState = (AsyncWebResult)result;

			// If an exception was thrown during the execution of the asynchronous operation.
			if (asyncState.Exception != null)
			{
				// Throw the exception.
				throw asyncState.Exception;
			}

			Encoding encoding = Encoding.GetEncoding(asyncState.Response.CharacterSet);

			// Get the string data.
			string data = (null != asyncState.ReceiveData.Data) ? encoding.GetString(asyncState.ReceiveData.Data) : string.Empty;

			// Return the converted data.
			return func.GetResult(data);
		}

		public void Cancel(IAsyncResult result)
		{
			// Get the state of the asynchronous operation.
			AsyncWebResult asyncState = (AsyncWebResult)result.AsyncState;

			// Use the system thread pool to cancel the request on a worker thread.
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.CancelAsync), asyncState);
		}

		protected void CancelAsync(object state)
		{
			AsyncWebResult asyncState = state as AsyncWebResult;

			// Abort the web request.
			asyncState.Request.Abort();
		}

		protected IAsyncResult BeginAsyncRequest(AsyncWebResult asyncState)
		{
			// Use the system thread pool to begin the asynchronous request on a worker thread.
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.BeginWebRequest), asyncState);

			return asyncState;
		}

		protected void BeginWebRequest(object state)
		{
			AsyncWebResult asyncState = state as AsyncWebResult;

			// If there is send data.
			if (asyncState.SendData.Data != null)
			{
				// Get the request stream.
				using (Stream stream = asyncState.Request.GetRequestStream())
				{
					// Write the send data to the stream.
					stream.Write(asyncState.SendData.Data, 0, asyncState.SendData.Data.Length);
				}
			}

			// Begin the web request.
			asyncState.Request.BeginGetResponse(this.EndWebRequest, asyncState);
		}

		protected void EndWebRequest(IAsyncResult result)
		{
			// Get the state of the asynchronous operation
			AsyncWebResult asyncState = (AsyncWebResult)result.AsyncState;

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
				// Complete the asynchronous operation.
				asyncState.Complete();
			}
			catch (Exception exception)
			{
				// Set the exception.
				asyncState.Exception = exception;
				// Signal that the operation has completed.
				asyncState.Complete();
				// Call the callback function
				if (asyncState.Callback != null) asyncState.Callback(asyncState);
			}
		}

		protected IAsyncResult BeginStreamRead(AsyncWebResult asyncState)
		{
			// Begin the read operation.
			return asyncState.Stream.BeginRead(asyncState.Buffer, 0, AsyncWebResult.BUFFER_SIZE, this.EndStreamRead, asyncState);
		}

		protected void EndStreamRead(IAsyncResult result)
		{
			// Get the state of the asynchronous operation
			AsyncWebResult asyncState = (AsyncWebResult)result.AsyncState;

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
				asyncState.ReceiveData.Append(asyncState.Buffer, count);

				if (count > 0)
				{
					// If bytes were read, begin a new read request.
					this.BeginStreamRead(asyncState);
				}
				else
				{
					// Otherwise, signal that the asynchronous operation has completed.
					asyncState.Complete();
					// Call the callback function.
					if (asyncState.Callback != null) asyncState.Callback(asyncState);
				}
			}
			catch (Exception exception)
			{
				// If an exception occured, set the exception.
				asyncState.Exception = exception;
				// Complete the asynchronous operation.
				asyncState.Complete();
				// Call the callback method.
				if (asyncState.Callback != null) asyncState.Callback(asyncState);
			}
		}
	}
}
