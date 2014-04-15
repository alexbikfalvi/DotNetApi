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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DotNetApi.Windows.Themes;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A theme tab control.
	/// </summary>
	[ToolboxBitmap(typeof(TabControl))]
	public sealed class ThemeTabControl : TabControl
	{
		/// <summary>
		/// A structure representing a window rectangle
		/// </summary>
		public struct WinRectangle
		{
			public int Left, Top, Right, Bottom;
		}

		private const int wmTcmAdjustRect = 0x1328;

		private readonly ThemeSettings themeSettings;

		private readonly SolidBrush brush = new SolidBrush(Color.White);
		private readonly Pen pen = new Pen(Color.White);

		private readonly Action<PaintEventArgs>[] actionsPaint;
		private readonly Action[] actionsInvalidateTabs;
		private static readonly Padding[] adjustSizePadding = {
																  new Padding(-2, -2, 2, 2),
																  new Padding(-1, -1, 1, 2)
															  };

		private bool hasFocus = false;

		/// <summary>
		/// Creates a new theme tab control instance.
		/// </summary>
		public ThemeTabControl()
		{
			// Set the control style.
			base.SetStyle(
				ControlStyles.UserPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.ResizeRedraw |
				ControlStyles.AllPaintingInWmPaint, true);
			
			// Set the padding.
			base.Padding = new Point(0, 0);

			// Set the paint actions.
			this.actionsPaint = new Action<PaintEventArgs>[] {
				this.OnPaintTop,
				this.OnPaintBottom
			};
			// Set the invalidate tabs actions.
			this.actionsInvalidateTabs = new Action[] {
				this.OnInvalidateTabsTop,
				this.OnInvalidateTabsBottom
			};

			// Set the theme settings.
			this.themeSettings = ToolStripManager.Renderer is ThemeRenderer ? (ToolStripManager.Renderer as ThemeRenderer).Settings : ThemeSettings.Default;
		}

		#region Public properties.

		/// <summary>
		/// Gets whether the control has focus.
		/// </summary>
		[Browsable(false)]
		public bool HasFocus
		{
			get { return this.hasFocus; }
		}
		/// <summary>
		/// Gets or sets the alignment of the tabs.
		/// </summary>
		[Browsable(false)]
		[DisplayName("Alignment")]
		[Description("Determines whether the tabs appear on the top or bottom side of the control. For any other alignment value, the default is top.")]
		public new TabAlignment Alignment
		{
			get { return base.Alignment; }
			set
			{
				switch (value)
				{
					case TabAlignment.Top:
					case TabAlignment.Bottom:
						base.Alignment = value;
						break;
					default:
						base.Alignment = TabAlignment.Top;
						break;
				}
			}
		}
		/// <summary>
		/// Gets whether the tab control is multilined.
		/// </summary>
		[Browsable(true)]
		[DisplayName("Multiline")]
		[Description("Indicates if more than one row of tabs is allowed. It is always false.")]
		public new bool Multiline
		{
			get { return base.Multiline; }
		}

		#endregion

		#region Protected methods.

		/// <summary>
		/// A method called when the object is being disposed.
		/// </summary>
		/// <param name="disposing"><b>True</b> if the managed resources are being disposed <b>false</b> otherwise.</param>
		protected override void Dispose(bool disposing)
		{
			// Dispose the brush.
			this.brush.Dispose();
			// Dispose the pen.
			this.pen.Dispose();
			// Call the base class method.
			base.Dispose(disposing);
		}

		/// <summary>
		/// An event handler called when a child control has been added.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			// Call the base class method.
			base.OnControlAdded(e);
			// Subscribe to the child control events.
			this.OnSubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when a child control has been removed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			// Call the base classs method.
			base.OnControlRemoved(e);
			// Unsubscribe from the child control events.
			this.OnUnsubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when the control got focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			// Call the base class method.
			base.OnGotFocus(e);
			// Call the any got focus handler.
			this.OnControlGotFocusChild(this, e);
		}

		/// <summary>
		/// An event handler called when the control lost focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			// Call the base class method.
			base.OnLostFocus(e);
			// Call the any lost focus handler.
			this.OnControlLostFocusChild(this, e);
		}

		/// <summary>
		/// An event handler called when painting the tab control background.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Set the brush color.
			this.brush.Color = this.themeSettings.ColorTable.TabControlBackgroundColor;
			// Paint the background.
			e.Graphics.FillRectangle(this.brush, this.ClientRectangle);
		}

		/// <summary>
		/// A method called when painting the control.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Set the units to pixels.
			e.Graphics.PageUnit = GraphicsUnit.Pixel;
			// Call the paint action for the current alignment.
			this.actionsPaint[(int)this.Alignment](e);
		}

		/// <summary>
		/// Overrides the window procedure method.
		/// </summary>
		/// <param name="m">The window message.</param>
		protected override void WndProc(ref Message m)
		{
			// If the message receive adjusts the tab control rectangle.
			if (m.Msg == ThemeTabControl.wmTcmAdjustRect)
			{
				// Compute the rectangle padding.
				Padding padding = ThemeTabControl.adjustSizePadding[(int)this.Alignment];
				// Update the bounds.
				WinRectangle bounds = (WinRectangle)m.GetLParam(typeof(WinRectangle));
				bounds.Left += padding.Left;
				bounds.Right += padding.Right;
				bounds.Top += padding.Top;
				bounds.Bottom += padding.Bottom;
				Marshal.StructureToPtr(bounds, m.LParam, true);
			}
			// Call the base class method.
			base.WndProc(ref m);
		}

		#endregion

		#region Private methods.

		/// <summary>
		/// Subscribes to the events of a child control.
		/// </summary>
		/// <param name="control">The child control.</param>
		private void OnSubscribeEvents(Control control)
		{
			// Add child control event handlers.
			control.GotFocus += this.OnControlGotFocusChild;
			control.LostFocus += this.OnControlLostFocusChild;
			control.ControlAdded += this.OnControlControlAdded;
			control.ControlRemoved += this.OnControlControlRemoved;

			// Subscribe to the events of the child controls.
			foreach (Control child in control.Controls)
			{
				this.OnSubscribeEvents(child);
			}
		}

		/// <summary>
		/// Unsubscribes from the events of a child control.
		/// </summary>
		/// <param name="control">The child control.</param>
		private void OnUnsubscribeEvents(Control control)
		{
			// Remove child control event handlers.
			control.GotFocus -= this.OnControlGotFocusChild;
			control.LostFocus -= this.OnControlLostFocusChild;
			control.ControlAdded -= this.OnControlControlAdded;
			control.ControlRemoved -= this.OnControlControlRemoved;

			// Unsubscribe from the events of the child controls.
			foreach (Control child in control.Controls)
			{
				this.OnUnsubscribeEvents(child);
			}
		}

		/// <summary>
		/// An event handler called when a child control got focus.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlGotFocusChild(object sender, EventArgs e)
		{
			if (!this.hasFocus)
			{
				// Set the flag to true.
				this.hasFocus = true;
				// Call the got focus handler.
				this.OnAnyGotFocus(e);
			}
		}

		/// <summary>
		/// An event handler called when a child control lost focus.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlLostFocusChild(object sender, EventArgs e)
		{
			if (!this.ContainsFocus)
			{
				// Set the flag to false.
				this.hasFocus = false;
				// Call the lost focus handler.
				this.OnAnyLostFocus(e);
			}
		}

		/// <summary>
		/// An event handler called when the control got focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		private void OnAnyGotFocus(EventArgs e)
		{
			// Invalidate the tabs.
			this.actionsInvalidateTabs[(int)this.Alignment]();
		}

		/// <summary>
		/// An event handler called when the control lost focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		private void OnAnyLostFocus(EventArgs e)
		{
			// Invalidate the tabs.
			this.actionsInvalidateTabs[(int)this.Alignment]();
		}

		/// <summary>
		/// An event handler called when a child control added a control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlControlAdded(object sender, ControlEventArgs e)
		{
			// Subscribe to the child control events.
			this.OnSubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when a child control removed a control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlControlRemoved(object sender, ControlEventArgs e)
		{
			// Unsubscribe from the child control events.
			this.OnUnsubscribeEvents(e.Control);
		}

		/// <summary>
		/// A method called when painting the top alignment.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		private void OnPaintTop(PaintEventArgs e)
		{
			// Paint the tabs.
			for (int index = 0; index < this.TabCount; index++)
			{
				this.OnPaintTabTop(e, index);
			}
			// Set the tab line color.
			this.pen.Color = this.ContainsFocus ?
				this.themeSettings.ColorTable.TabControlSelectedFocusedTopTabColor :
				this.themeSettings.ColorTable.TabControlSelectedUnfocusedTopTabColor;
			this.pen.Width = 2.0f;
			// Draw the tab line.
			e.Graphics.DrawLine(pen,
				this.ClientRectangle.Left + 2,
				this.ClientRectangle.Top + this.ItemSize.Height + 1,
				this.ClientRectangle.Right - 2,
				this.ClientRectangle.Top + this.ItemSize.Height + 1);
		}

		/// <summary>
		/// A method called when painting the bottom alignment.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		private void OnPaintBottom(PaintEventArgs e)
		{
			// Paint the tabs.
			for (int index = 0; index < this.TabCount; index++)
			{
				this.OnPaintTabBottom(e, index);
			}
			
			// Get the bounds of the selected tab.
			Rectangle bounds = this.GetTabRect(this.SelectedIndex);
			
			// Set the tab line color.
			this.pen.Color = this.themeSettings.ColorTable.ToolSplitContainerBorder;
			this.pen.Width = 1.0f;
			// Draw the tab line.
			e.Graphics.DrawLines(pen, new Point[] {
				new Point(bounds.Right, this.ClientRectangle.Bottom - this.ItemSize.Height - 2),
				new Point(this.ClientRectangle.Right - 3, this.ClientRectangle.Bottom - this.ItemSize.Height - 2),
				new Point(this.ClientRectangle.Right - 3, this.ClientRectangle.Top + 2),
				new Point(this.ClientRectangle.Left + 2, this.ClientRectangle.Top + 2),
				new Point(this.ClientRectangle.Left + 2, this.ClientRectangle.Bottom - this.ItemSize.Height - 2),
				new Point(bounds.Left, this.ClientRectangle.Bottom - this.ItemSize.Height - 2)
			});
		}

		/// <summary>
		/// A method called when painting a top tab.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		/// <param name="index">The tab index.</param>
		private void OnPaintTabTop(PaintEventArgs e, int index)
		{
			// Get the bounds for the tab at the specified index.
			Rectangle bounds = this.GetTabRect(index);
			
			// Set the color.
			this.brush.Color = (index == this.SelectedIndex) ? this.ContainsFocus ?
				this.themeSettings.ColorTable.TabControlSelectedFocusedTopTabColor :
				this.themeSettings.ColorTable.TabControlSelectedUnfocusedTopTabColor :
				this.themeSettings.ColorTable.TabControlUnselectedTopTabColor;
			// Paint the tab.
			e.Graphics.FillRectangle(this.brush, bounds);
			// Draw the text.
			TextRenderer.DrawText(
				e.Graphics,
				this.TabPages[index].Text,
				this.Font,
				new Rectangle(bounds.Left + 2, bounds.Top, bounds.Width - 2, bounds.Height - 1),
				(index == this.SelectedIndex) ? this.ContainsFocus ?
				this.themeSettings.ColorTable.TabControlSelectedFocusedTopTextColor :
				this.themeSettings.ColorTable.TabControlSelectedUnfocusedTopTextColor :
				this.themeSettings.ColorTable.TabControlUnselectedTopTextColor,
				TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.VerticalCenter
				);
		}

		/// <summary>
		/// A method called when painting a bottom tab.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		/// <param name="index">The tab index.</param>
		private void OnPaintTabBottom(PaintEventArgs e, int index)
		{
			// Get the bounds for the tab at the specified index.
			Rectangle bounds = this.GetTabRect(index);

			// Set the tab color.
			this.brush.Color = (index == this.SelectedIndex) ?
				this.themeSettings.ColorTable.TabControlSelectedTabColor :
				this.themeSettings.ColorTable.TabControlUnselectedTabColor;
			// Paint the tab.
			e.Graphics.FillRectangle(this.brush, bounds);
			
			// If this is the selected item.
			if (index == this.SelectedIndex)
			{
				// Draw the tab border.
				this.pen.Color = this.themeSettings.ColorTable.ToolSplitContainerBorder;
				this.pen.Width = 1.0f;
				// Paint the border.
				e.Graphics.DrawLines(this.pen,
					new Point[] {
					new Point(bounds.Left, bounds.Top),
					new Point(bounds.Left, bounds.Bottom - 1),
					new Point(bounds.Right - 1, bounds.Bottom - 1),
					new Point(bounds.Right - 1, bounds.Top)
				});
			}

			// Draw the text.
			TextRenderer.DrawText(
				e.Graphics,
				this.TabPages[index].Text,
				this.Font,
				new Rectangle(bounds.Left + 2, bounds.Top, bounds.Width - 2, bounds.Height),
				(index == this.SelectedIndex) ?
				this.themeSettings.ColorTable.TabControlSelectedTextColor :
				this.themeSettings.ColorTable.TabControlUnselectedTextColor,
				TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
				);
		}

		/// <summary>
		/// Invalidates the top tabs.
		/// </summary>
		private void OnInvalidateTabsTop()
		{
			this.Invalidate(new Rectangle(
				this.ClientRectangle.Left + 2,
				this.ClientRectangle.Top + 2,
				this.ClientRectangle.Width - 4,
				this.ItemSize.Height
				));
		}

		/// <summary>
		/// Invalidates the bottom tabs.
		/// </summary>
		private void OnInvalidateTabsBottom()
		{
			this.Invalidate(new Rectangle(
				this.ClientRectangle.Left + 2,
				this.ClientRectangle.Bottom - this.ItemSize.Height - 2,
				this.ClientRectangle.Width - 4,
				this.ItemSize.Height
				));
		}

		#endregion
	}
}
