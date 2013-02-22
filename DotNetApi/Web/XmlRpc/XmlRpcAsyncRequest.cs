/* 
 * Copyright (C) 2013 Alex Bikfalvi
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
using DotNetApi.Web;

namespace DotNetApi.Web.XmlRpc
{
	public class XmlRpcAsyncRequest : AsyncRequest
	{
		/// <summary>
		/// Conversion class for an asynchronous operation returning a string.
		/// </summary>
		public class XmlRpcResponseRequestFunction : IAsyncFunction<XmlRpcResponse>
		{
			/// <summary>
			/// Returns a string for the received asynchronous data.
			/// </summary>
			/// <param name="data">The data string.</param>
			/// <returns>The XML RPC response.</returns>
			public XmlRpcResponse GetResult(string data)
			{
				return XmlRpcResponse.Create(data);
			}
		}

		private XmlRpcResponseRequestFunction funcConvert = new XmlRpcResponseRequestFunction();

		/// <summary>
		/// Begins an asynchronous XML RPC request.
		/// </summary>
		/// <param name="uri">The request URI.</param>
		/// <param name="method">The method name.</param>
		/// <param name="parameters">The method parameters.</param>
		/// <param name="callback">The callback delegate.</param>
		/// <param name="state">The user state.</param>
		/// <returns>The asynchronous result.</returns>
		public IAsyncResult Begin(
			Uri uri,
			string method,
			object[] parameters,
			AsyncRequestCallback callback,
			object state = null)
		{
			// Create the asynchronous state.
			AsyncRequestState asyncState = AsyncRequest.Create(uri, callback);

			// Create the request XML.
			byte[] request = XmlRpcRequest.Create(method, parameters);

			asyncState.Request.Method = "POST";
			asyncState.Request.ContentType = "text/xml";
			asyncState.Request.ContentLength = request.Length;
			asyncState.Request.GetRequestStream().Write(request, 0, request.Length);

			// Set the request user state.
			asyncState.State = state;

			// Begin the request.
			return this.Begin(asyncState);
		}

		/// <summary>
		/// Ends the asynchronus request.
		/// </summary>
		/// <param name="result">The asynchronous result.</param>
		/// <param name="state">The request state.</param>
		/// <returns>The request response.</returns>
		public XmlRpcResponse End(IAsyncResult result, out object state)
		{
			// Get the asynchronous result.
			AsyncRequestResult asyncResult = (AsyncRequestResult)result;

			// Get the asynchronous state.
			AsyncRequestState asyncState = (AsyncRequestState)asyncResult.AsyncState;

			// Set the user state
			state = asyncState.State;

			// Determine the encoding of the received response
			return this.End<XmlRpcResponse>(result, this.funcConvert);
		}
	}
}
