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
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A theme tab control.
	/// </summary>
	public sealed class ThemeTabControl : TabControl
	{
		public ThemeTabControl()
		{
			this.DrawMode = TabDrawMode.OwnerDrawFixed;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			using (SolidBrush brush = new SolidBrush(Color.Yellow))
			{
				e.Graphics.FillRectangle(brush, this.Bounds);
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (SolidBrush brush = new SolidBrush(Color.Blue))
			{
				e.Graphics.FillRectangle(brush, this.Bounds);
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			//base.OnDrawItem(e);
			using (SolidBrush brush = new SolidBrush(Color.Black))
			{
				e.Graphics.FillRectangle(brush, e.Bounds);
			}
		}
	}
}
