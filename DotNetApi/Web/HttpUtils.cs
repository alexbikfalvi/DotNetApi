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
using System.Collections;
using System.Collections.Specialized;
using System.Net;

namespace DotNetApi.Web
{
	/// <summary>
	/// A class representing a set of utilities for HTTP.
	/// </summary>
	public class HttpUtils
	{
		private static OrderedDictionary requestHeaderNames = new OrderedDictionary
		{
			{ HttpRequestHeader.Accept, "Accept" },
			{ HttpRequestHeader.AcceptCharset, "Accept-Charset" },
			{ HttpRequestHeader.AcceptEncoding, "Accept-Encoding" },
			{ HttpRequestHeader.AcceptLanguage, "Accept-Language" },
			{ HttpRequestHeader.Allow, "Allow" },
			{ HttpRequestHeader.Authorization, "Authorization" },
			{ HttpRequestHeader.CacheControl, "Cache-Control" },
			{ HttpRequestHeader.Connection, "Connection" },
			{ HttpRequestHeader.ContentEncoding, "Content-Encoding" },
			{ HttpRequestHeader.ContentLanguage, "Content-Language" },
			{ HttpRequestHeader.ContentLength, "Content-Length" },
			{ HttpRequestHeader.ContentLocation, "Content-Location" },
			{ HttpRequestHeader.ContentMd5, "Content-MD5" },
			{ HttpRequestHeader.ContentRange, "Content-Range" },
			{ HttpRequestHeader.ContentType, "Content-Type" },
			{ HttpRequestHeader.Cookie, "Cookie" },
			{ HttpRequestHeader.Date, "Date" },
			{ HttpRequestHeader.Expect, "Expect" },
			{ HttpRequestHeader.Expires, "Expires" },
			{ HttpRequestHeader.From, "From" },
			{ HttpRequestHeader.Host, "Host" },
			{ HttpRequestHeader.IfMatch, "If-Match" },
			{ HttpRequestHeader.IfModifiedSince, "If-Modified-Since" },
			{ HttpRequestHeader.IfNoneMatch, "If-None-Match" },
			{ HttpRequestHeader.IfRange, "If-Range" },
			{ HttpRequestHeader.IfUnmodifiedSince, "If-Unmodified-Since" },
			{ HttpRequestHeader.KeepAlive, "Keep-Alive" },
			{ HttpRequestHeader.LastModified, "Last-Modified" },
			{ HttpRequestHeader.MaxForwards, "Max-Forwards" },
			{ HttpRequestHeader.Pragma, "Pragma" },
			{ HttpRequestHeader.ProxyAuthorization, "Proxy-Authorization" },
			{ HttpRequestHeader.Range, "Range" },
			{ HttpRequestHeader.Referer, "Referer" },
			{ HttpRequestHeader.Te, "TE" },
			{ HttpRequestHeader.Trailer, "Trailer" },
			{ HttpRequestHeader.TransferEncoding, "Transfer-Encoding" },
			{ HttpRequestHeader.Translate, "Translate" },
			{ HttpRequestHeader.Upgrade, "Upgrade" },
			{ HttpRequestHeader.UserAgent, "User-Agent" },
			{ HttpRequestHeader.Via, "Via" },
			{ HttpRequestHeader.Warning, "Warning" }
		};

		// Public properties.

		/// <summary>
		/// Returns the collection of HTTP request headers.
		/// </summary>
		public static ICollection RequestHeaders
		{
			get { return HttpUtils.requestHeaderNames; }
		}

		// Public methods.

		/// <summary>
		/// Returns the name of the specified request header.
		/// </summary>
		/// <param name="header">The HTTP request header.</param>
		/// <returns>The header name.</returns>
		public static string GetRequestHeaderName(HttpRequestHeader header)
		{
			return HttpUtils.requestHeaderNames[header] as string;
		}
	}
}
