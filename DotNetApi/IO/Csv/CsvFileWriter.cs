/*
 * This source code and all associated files and resources are copyrighted by
 * the author(s). This source code and all associated files and resources may
 * be used as long as they are used according to the terms and conditions set
 * forth in The Code Project Open License (CPOL).
 *
 * Copyright (c) 2012 Jonathan Wood
 * http://www.blackbeltcoder.com
 */


using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetApi.IO.Csv
{
	/// <summary>
	/// Class for writing to comma-separated-value (CSV) files.
	/// </summary>
	public sealed class CsvFileWriter : CsvFileCommon, IDisposable
	{
		// Private members
		private StreamWriter writer;
		private string oneQuote = null;
		private string twoQuotes = null;
		private string quotedFormat = null;

		/// <summary>
		/// Initializes a new instance of the CsvFileWriter class for the
		/// specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		public CsvFileWriter(Stream stream)
		{
			this.writer = new StreamWriter(stream);
		}

		/// <summary>
		/// Initializes a new instance of the CsvFileWriter class for the
		/// specified file path.
		/// </summary>
		/// <param name="path">The name of the CSV file to write to</param>
		public CsvFileWriter(string path)
		{
			this.writer = new StreamWriter(path);
		}

		/// <summary>
		/// Writes a row of columns to the current CSV file.
		/// </summary>
		/// <param name="columns">The list of columns to write</param>
		public void WriteRow(List<string> columns)
		{
			// Verify required argument
			if (columns == null)
				throw new ArgumentNullException("columns");

			// Ensure we're using current quote character
			if (oneQuote == null || oneQuote[0] != Quote)
			{
				oneQuote = String.Format("{0}", Quote);
				twoQuotes = String.Format("{0}{0}", Quote);
				quotedFormat = String.Format("{0}{{0}}{0}", Quote);
			}

			// Write each column
			for (int i = 0; i < columns.Count; i++)
			{
				// Add delimiter if this isn't the first column
				if (i > 0)
					writer.Write(Delimiter);
				// Write this column
				if (columns[i].IndexOfAny(specialChars) == -1)
					writer.Write(columns[i]);
				else
					writer.Write(quotedFormat, columns[i].Replace(oneQuote, twoQuotes));
			}
			this.writer.WriteLine();
		}

		/// <summary>
		/// Disposes the current object.
		/// </summary>
		public void Dispose()
		{
			// Dispose the writer.
			this.writer.Dispose();
			// Suppress the finalizer.
			GC.SuppressFinalize(this);
		}
	}
}
