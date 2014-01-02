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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DotNetApi.Globalization
{
	/// <summary>
	/// A class representing a locale writer.
	/// </summary>
	public class LocaleWriter : IDisposable
	{
		private readonly TextWriter writer = null;
		private readonly Stream stream = null;

		/// <summary>
		/// Creates a locale writer attached to the specified writer.
		/// </summary>
		/// <param name="writer">The text writer.</param>
		public LocaleWriter(TextWriter writer)
		{
			// Validate the argument.
			if (null == writer) throw new ArgumentNullException("writer");

			// Set the writer.
			this.writer = writer;
		}

		/// <summary>
		/// Creates a locale writer attected to the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public LocaleWriter(Stream stream)
		{
			// Validate the argument.
			if (null == stream) throw new ArgumentNullException("stream");

			// Set the stream.
			this.stream = stream;
		}

		// Public methods.

		/// <summary>
		/// Disposes the current object.
		/// </summary>
		public void Dispose()
		{
			// Call the event handler.
			this.Dispose(true);
			// Supress the finalizer.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Writes the specified locale collection.
		/// </summary>
		/// <param name="locales">The locale collection.</param>
		public void WriteLocaleCollection(LocaleCollection locales)
		{
			// Create the XML document.
			XDocument document = new XDocument(
				new XElement("Locales",
					from locale in locales select new XElement("Locale",
						new XElement("Culture",
							new XAttribute("Language", locale.Culture.Language),
							locale.Culture.Script != null ? new XAttribute("Script", locale.Culture.Script) : null,
							locale.Culture.Territory != null ? new XAttribute("Territory", locale.Culture.Territory) : null),
						new XElement("Languages",
							from language in locale.Languages select new XElement("Language",
								new XAttribute("Type", language.Type),
								language.Name)),
						new XElement("Scripts",
							from script in locale.Scripts select new XElement("Script",
								new XAttribute("Type", script.Type),
								script.Name)),
						new XElement("Territories",
							from territory in locale.Territories select new XElement("Territory",
								new XAttribute("Type", territory.Type),
								territory.Name)))));

			// Write the document.
			if (null != this.writer)
				document.Save(writer);
			else
				document.Save(stream);
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposing">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
