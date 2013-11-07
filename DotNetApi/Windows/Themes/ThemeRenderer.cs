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

namespace DotNetApi.Windows.Themes
{
	/// <summary>
	/// The themes renderer.
	/// </summary>
	public class ThemeRenderer : ToolStripProfessionalRenderer
	{
		private ThemeSettings settings;

		/// <summary>
		/// Creates a new theme renderer instance.
		/// </summary>
		/// <param name="settings">The theme color table.</param>
		public ThemeRenderer(ThemeSettings settings)
			: base(settings.ColorTable)
		{
			this.RoundedEdges = false;
			this.settings = settings;
		}

		// Public properties.

		/// <summary>
		/// Gets the theme color table.
		/// </summary>
		public new ThemeColorTable ColorTable { get { return this.settings.ColorTable; } }
		/// <summary>
		/// Gets teh theme settings.
		/// </summary>
		public ThemeSettings Settings { get { return this.settings; } }

		// Protected methods.

		/// <summary>
		/// Renders the background of a menu item.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			// If the item is disabled, do nothing.
			if (!e.Item.Enabled) return;
			// If the item does not belong to a drop down and it is pressed.
			if ((!e.Item.IsOnDropDown) && (e.Item.Pressed))
			{
				// Compute the item bounds.
				Rectangle bounds = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height);
				// Render the background using the tool strip drop down color.
				using (SolidBrush brush = new SolidBrush(this.ColorTable.ToolStripDropDownBackground))
				{
					e.Graphics.FillRectangle(brush, bounds);
				}
				// Render the border using the menu border color.
				using (Pen pen = new Pen(this.ColorTable.MenuBorder))
				{
					e.Graphics.DrawRectangle(pen, bounds);
				}
				return;
			}
			// Call the base class method.
			base.OnRenderMenuItemBackground(e);
		}

		/// <summary>
		/// Renders the background of a drop down button.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			// If the item does not belong to a drop down and it is pressed.
			if ((!e.Item.IsOnDropDown) && (e.Item.Pressed))
			{
				// Compute the item bounds.
				Rectangle bounds = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height);
				// Render the background using the tool strip drop down color.
				using (SolidBrush brush = new SolidBrush(this.ColorTable.ToolStripDropDownBackground))
				{
					e.Graphics.FillRectangle(brush, bounds);
				}
				// Render the border using the menu border color.
				using (Pen pen = new Pen(this.ColorTable.MenuBorder))
				{
					e.Graphics.DrawRectangle(pen, bounds);
				}
				return;
			}
			// Call the base class methods.
			base.OnRenderDropDownButtonBackground(e);
		}

		/// <summary>
		/// Renders the background of a split button.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			// If the item does not belong to a drop down and it is pressed.
			if ((!e.Item.IsOnDropDown) && (e.Item.Pressed))
			{
				// Compute the item bounds.
				Rectangle bounds = new Rectangle(0, 0, e.Item.Size.Width - 1, e.Item.Size.Height);
				// Render the background using the tool strip drop down color.
				using (SolidBrush brush = new SolidBrush(this.ColorTable.ToolStripDropDownBackground))
				{
					e.Graphics.FillRectangle(brush, bounds);
				}
				// Render the border using the menu border color.
				using (Pen pen = new Pen(this.ColorTable.MenuBorder))
				{
					e.Graphics.DrawRectangle(pen, bounds);
				}
				return;
			}
			// Call the base class method.
			base.OnRenderSplitButtonBackground(e);
		}
	}
}
