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
using System.IO;
using System.Net;
using System.Threading;
using DotNetApi.Async;

namespace DotNetApi.Web
{
	/*
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
	 * */

	/// <summary>
	/// A class representing the asynchronous request state.
	/// </summary>
	public class AsyncWebResult : AsyncResult
	{
		public const int BUFFER_SIZE = 4096;
		private byte[] buffer = null;

		/// <summary>
		/// Constructs an object for an asynchronous request state.
		/// </summary>
		/// <param name="uri">The URI of the asynchronous request.</param>
		/// <param name="callback">The callback function for the asynchronous request.</param>
		/// <param name="userState">The user state.</param>
		public AsyncWebResult(Uri uri, AsyncWebRequestCallback callback, object userState = null)
			: base(userState)
		{
			this.Data = new AsyncWebBuffer();
			this.Request = (HttpWebRequest)WebRequest.Create(uri);
			this.Callback = callback;
			this.buffer = new byte[BUFFER_SIZE];
		}

		/// <summary>
		/// The data returned by the asynchrounous request.
		/// </summary>
		public AsyncWebBuffer Data { get; set; }

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
		/// The callback function for the asynchronous operation.
		/// </summary>
		public AsyncWebRequestCallback Callback { get; set; }
	}
}
