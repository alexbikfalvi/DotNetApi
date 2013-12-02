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
using System.Drawing;

namespace DotNetApi.Windows.Themes.Code
{
	/// <summary>
	/// The base class for code colors.
	/// </summary>
	public class CodeColorTable
	{
		public virtual Color CommentForeground { get { return Color.Green; } }
		public virtual Color CommentBackground { get { return Color.White; } }
		public virtual Color StringForeground { get { return Color.FromArgb(163, 21, 21); } }
		public virtual Color StringBackground { get { return Color.White; } }
		public virtual Color KeywordForeground { get { return Color.Blue; } }
		public virtual Color KeywordBackground { get { return Color.White; } }
		public virtual Color NumberForeground { get { return Color.Black; } }
		public virtual Color NumberBackground { get { return Color.White; } }
		public virtual Color OperatorForeground { get { return Color.Black; } }
		public virtual Color OperatorBackground { get { return Color.White; } }
		public virtual Color TextForeground { get { return Color.Black; } }
		public virtual Color TextBackground { get { return Color.White; } }
		public virtual Color UserTypeForeground { get { return Color.FromArgb(43, 145, 175); } }
		public virtual Color UserTypeBackground { get { return Color.White; } }
	}
}
