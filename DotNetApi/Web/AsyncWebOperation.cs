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

namespace DotNetApi.Web
{
	/// <summary>
	/// A request-result pair for an asynchronous web operation.
	/// </summary>
	public struct AsyncWebOperation
	{
		private AsyncWebRequest request;
		private AsyncWebResult result;

		/// <summary>
		/// Creates a new asynchronous web operation pair.
		/// </summary>
		/// <param name="request">The asynchronous web request.</param>
		/// <param name="result">The resuest result.</param>
		public AsyncWebOperation(AsyncWebRequest request, AsyncWebResult result)
		{
			this.request = request;
			this.result = result;
		}

		// Public properties.

		/// <summary>
		/// The asynchronous web request.
		/// </summary>
		public AsyncWebRequest Request { get { return this.request; } }

		/// <summary>
		/// The request result.
		/// </summary>
		public AsyncWebResult Result { get { return this.result; } }

		// Public methods.

		/// <summary>
		/// Cancels the asynchronous web operation.
		/// </summary>
		public void Cancel()
		{
			this.request.Cancel(this.result);
		}
	}
}
