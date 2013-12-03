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
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DotNetApi.IO.Csv
{
	/// <summary>
	/// Class for reading from comma-separated-value (CSV) files
	/// </summary>
	public sealed class CsvFileReader : CsvFileCommon, IDisposable
	{
		// Private members
		private StreamReader reader;
		private string currLine;
		private int currPos;
		private EmptyLineBehavior emptyLineBehavior;

		/// <summary>
		/// Initializes a new instance of the CsvFileReader class for the
		/// specified stream.
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
		public CsvFileReader(Stream stream, EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
		{
			this.reader = new StreamReader(stream);
			this.emptyLineBehavior = emptyLineBehavior;
		}

		/// <summary>
		/// Initializes a new instance of the CsvFileReader class for the
		/// specified file path.
		/// </summary>
		/// <param name="path">The name of the CSV file to read from</param>
		/// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
		public CsvFileReader(string path,
			EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
		{
			this.reader = new StreamReader(path);
			this.emptyLineBehavior = emptyLineBehavior;
		}

		/// <summary>
		/// Reads a row of columns from the current CSV file. Returns false if no
		/// more data could be read because the end of the file was reached.
		/// </summary>
		/// <param name="columns">Collection to hold the columns read</param>
		public bool ReadRow(List<string> columns)
		{
			// Verify required argument
			if (columns == null)
				throw new ArgumentNullException("columns");

		ReadNextLine:
			// Read next line from the file
			currLine = this.reader.ReadLine();
			currPos = 0;
			// Test for end of file
			if (currLine == null)
				return false;
			// Test for empty line
			if (currLine.Length == 0)
			{
				switch (emptyLineBehavior)
				{
					case EmptyLineBehavior.NoColumns:
						columns.Clear();
						return true;
					case EmptyLineBehavior.Ignore:
						goto ReadNextLine;
					case EmptyLineBehavior.EndOfFile:
						return false;
				}
			}

			// Parse line
			string column;
			int numColumns = 0;
			while (true)
			{
				// Read next column
				if (currPos < currLine.Length && currLine[currPos] == Quote)
					column = ReadQuotedColumn();
				else
					column = ReadUnquotedColumn();
				// Add column to list
				if (numColumns < columns.Count)
					columns[numColumns] = column;
				else
					columns.Add(column);
				numColumns++;
				// Break if we reached the end of the line
				if (currLine == null || currPos == currLine.Length)
					break;
				// Otherwise skip delimiter
				Debug.Assert(currLine[currPos] == Delimiter);
				currPos++;
			}
			// Remove any unused columns from collection
			if (numColumns < columns.Count)
				columns.RemoveRange(numColumns, columns.Count - numColumns);
			// Indicate success
			return true;
		}

		/// <summary>
		/// Reads a quoted column by reading from the current line until a
		/// closing quote is found or the end of the file is reached. On return,
		/// the current position points to the delimiter or the end of the last
		/// line in the file. Note: CurrLine may be set to null on return.
		/// </summary>
		private string ReadQuotedColumn()
		{
			// Skip opening quote character
			Debug.Assert(currPos < currLine.Length && currLine[currPos] == Quote);
			currPos++;

			// Parse column
			StringBuilder builder = new StringBuilder();
			while (true)
			{
				while (currPos == currLine.Length)
				{
					// End of line so attempt to read the next line
					currLine = reader.ReadLine();
					currPos = 0;
					// Done if we reached the end of the file
					if (currLine == null)
						return builder.ToString();
					// Otherwise, treat as a multi-line field
					builder.Append(Environment.NewLine);
				}

				// Test for quote character
				if (currLine[currPos] == Quote)
				{
					// If two quotes, skip first and treat second as literal
					int nextPos = (currPos + 1);
					if (nextPos < currLine.Length && currLine[nextPos] == Quote)
						currPos++;
					else
						break;  // Single quote ends quoted sequence
				}
				// Add current character to the column
				builder.Append(currLine[currPos++]);
			}

			if (currPos < currLine.Length)
			{
				// Consume closing quote
				Debug.Assert(currLine[currPos] == Quote);
				currPos++;
				// Append any additional characters appearing before next delimiter
				builder.Append(ReadUnquotedColumn());
			}
			// Return column value
			return builder.ToString();
		}

		/// <summary>
		/// Reads an unquoted column by reading from the current line until a
		/// delimiter is found or the end of the line is reached. On return, the
		/// current position points to the delimiter or the end of the current
		/// line.
		/// </summary>
		private string ReadUnquotedColumn()
		{
			int startPos = currPos;
			currPos = currLine.IndexOf(Delimiter, currPos);
			if (currPos == -1)
				currPos = currLine.Length;
			if (currPos > startPos)
				return currLine.Substring(startPos, currPos - startPos);
			return String.Empty;
		}

		/// <summary>
		/// Disposes the current object.
		/// </summary>
		public void Dispose()
		{
			// Dispose the reader.
			this.reader.Dispose();
			// Suppress the finalizer.
			GC.SuppressFinalize(this);
		}
	}
}
