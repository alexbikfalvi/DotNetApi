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
using System.Xml.Linq;

namespace DotNetApi.Web.XmlRpc
{
	/// <summary>
	/// A class for an XML-RPC method call.
	/// </summary>
	[Serializable]
	public sealed class XmlRpcMethodCall
	{
		private string methodName;
		private XmlRpcParameters parameters;

		/// <summary>
		/// Creates a new XML RPC method call instance.
		/// </summary>
		/// <param name="methodName">The method name.</param>
		/// <param name="parameters">The method parameters.</param>
		public XmlRpcMethodCall(string methodName, object[] parameters = null)
		{
			this.methodName = methodName;
			this.parameters = new XmlRpcParameters(parameters);
		}


	}
}
