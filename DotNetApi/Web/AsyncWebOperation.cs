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
		private IAsyncResult result;

		/// <summary>
		/// Creates a new asynchronous web operation pair.
		/// </summary>
		/// <param name="request">The asynchronous web request. It cannot be null.</param>
		/// <param name="result">The resuest result. It cannot be null.</param>
		public AsyncWebOperation(AsyncWebRequest request, IAsyncResult result)
		{
			// Validate the arguments.
			if (null == request) throw new ArgumentNullException("request");
			if (null == result) throw new ArgumentNullException("result");

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
		public IAsyncResult Result { get { return this.result; } }

		// Public methods.

		/// <summary>
		/// Cancels the asynchronous web operation.
		/// </summary>
		public void Cancel()
		{
			this.request.Cancel(this.result);
		}

		/// <summary>
		/// Compares two web operation objects for equality.
		/// </summary>
		/// <param name="left">The left object.</param>
		/// <param name="right">The right object.</param>
		/// <returns><b>True</b> if the two web operations objects are equal, otherwise, false.</returns>
		public static bool operator ==(AsyncWebOperation left, AsyncWebOperation right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two web operation objects for inequality.
		/// </summary>
		/// <param name="left">The left object.</param>
		/// <param name="right">The right object.</param>
		/// <returns><b>True</b> if the two web operations objects are different, otherwise, false.</returns>
		public static bool operator !=(AsyncWebOperation left, AsyncWebOperation right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Compares two web operations objects.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns><b>True</b> if the two web operations objects are equal, otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (null == obj) return false;
			if (!(obj is AsyncWebOperation)) return false;
			AsyncWebOperation operation = (AsyncWebOperation)obj;
			return object.ReferenceEquals(this.request, operation.request) && object.ReferenceEquals(this.result, operation.result);
		}

		/// <summary>
		/// Gets the hash code for the web operation object.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			return this.request.GetHashCode() ^ this.result.GetHashCode();
		}
	}
}
