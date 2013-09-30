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
using System.Globalization;
using DotNetApi.Web;

namespace DotNetApi.Web.XmlRpc
{
	public class XmlRpcAsyncRequest : AsyncWebRequest
	{
		/// <summary>
		/// Conversion class for an asynchronous operation returning a string.
		/// </summary>
		public class XmlRpcResponseRequestFunction : IAsyncFunction<XmlRpcResponse>
		{
			private IFormatProvider format;

			public XmlRpcResponseRequestFunction(IFormatProvider format)
			{
				this.format = format;
			}

			/// <summary>
			/// Returns a string for the received asynchronous data.
			/// </summary>
			/// <param name="data">The data string.</param>
			/// <param name="format">The format.</param>
			/// <returns>The XML RPC response.</returns>
			public XmlRpcResponse GetResult(string data)
			{
				return XmlRpcResponse.Create(data, this.format);
			}
		}

		private readonly XmlRpcResponseRequestFunction funcConvert;
		private readonly CultureInfo culture;

		/// <summary>
		/// Creates a new request instance.
		/// </summary>
		/// <param name="culture">The culture used for the current request.</param>
		public XmlRpcAsyncRequest(CultureInfo culture)
		{
			this.funcConvert = new XmlRpcResponseRequestFunction(culture);
			this.culture = culture;
		}

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
			AsyncWebRequestCallback callback,
			object state = null)
		{
			// Create the asynchronous state.
			AsyncWebResult asyncState = AsyncWebRequest.Create(uri, callback, state);

			// Create the request XML.
			byte[] request = XmlRpcRequest.Create(method, parameters);

			asyncState.Request.Method = "POST";
			asyncState.Request.ContentType = "text/xml";
			asyncState.Request.ContentLength = request.Length;
			asyncState.Request.Headers["Accept-Language"] = this.culture.Name;
			asyncState.Request.GetRequestStream().Write(request, 0, request.Length);

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
			// Get the asynchronous state.
			AsyncWebResult asyncState = result as AsyncWebResult;

			// Set the user state
			state = asyncState.AsyncState;

			// Determine the encoding of the received response
			return this.End<XmlRpcResponse>(result, this.funcConvert);
		}
	}
}
