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

namespace DotNetApi.IO.Csv
{
	/// <summary>
	/// Determines how empty lines are interpreted when reading CSV files.
	/// These values do not affect empty lines that occur within quoted fields
	/// or empty lines that appear at the end of the input file.
	/// </summary>
	public enum EmptyLineBehavior
	{
		/// <summary>
		/// Empty lines are interpreted as a line with zero columns.
		/// </summary>
		NoColumns,
		/// <summary>
		/// Empty lines are interpreted as a line with a single empty column.
		/// </summary>
		EmptyColumn,
		/// <summary>
		/// Empty lines are skipped over as though they did not exist.
		/// </summary>
		Ignore,
		/// <summary>
		/// An empty line is interpreted as the end of the input file.
		/// </summary>
		EndOfFile,
	}

	/// <summary>
	/// Common base class for CSV reader and writer classes.
	/// </summary>
	public abstract class CsvFileCommon
	{
		/// <summary>
		/// These are special characters in CSV files. If a column contains any
		/// of these characters, the entire column is wrapped in double quotes.
		/// </summary>
		protected static readonly char[] specialChars = new char[] { ',', '"', '\r', '\n' };

		// Indexes into SpecialChars for characters with specific meaning
		private const int delimiterIndex = 0;
		private const int quoteIndex = 1;

		/// <summary>
		/// Gets/sets the character used for column delimiters.
		/// </summary>
		public char Delimiter
		{
			get { return specialChars[delimiterIndex]; }
			set { specialChars[delimiterIndex] = value; }
		}

		/// <summary>
		/// Gets/sets the character used for column quotes.
		/// </summary>
		public char Quote
		{
			get { return specialChars[quoteIndex]; }
			set { specialChars[quoteIndex] = value; }
		}
	}
}
