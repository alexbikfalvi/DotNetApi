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
using System.Drawing;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	public delegate void NotificationTaskEventHandler();

	/// <summary>
	/// A control that displays a message box overlayed on a given control.
	/// </summary>
	public class NotificationBox : ThreadSafeControl
	{
		private string title = null;
		private Image image = null;
		private ProgressBar progressBar = new ProgressBar();
		private Timer timer = new Timer();
		private int titleHeight = 35;
		private int defaultWidth = 400;
		private int defaultHeight = 130;

		private Rectangle borderControl = new Rectangle();
		private Rectangle borderTitle = new Rectangle();
		private Rectangle borderTitleText = new Rectangle();
		private Rectangle borderContent = new Rectangle();
		private Rectangle borderImage = new Rectangle();
		private Rectangle borderText = new Rectangle();

		private Color colorBorder = ProfessionalColors.MenuItemBorder;
		private Color colorTitleBackground = Color.FromArgb(239, 239, 242);
		private Color colorTitleForeground = SystemColors.GrayText;

		private NotificationTaskEventHandler task = null;

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public NotificationBox()
		{
			// Suspend the layout.
			this.SuspendLayout();

			// Default properties.
			this.Width = this.defaultWidth;
			this.Height = this.defaultHeight;
			this.Margin = new Padding(10);
			this.Padding = new Padding(16, 8, 16, 16);
			this.Visible = false;
			this.DoubleBuffered = true;
			this.BackColor = Color.FromArgb(246, 246, 246);

			// Default components.
			this.progressBar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
			this.progressBar.Size = new Size(this.Width - this.Padding.Left - this.Padding.Right, 17);
			this.progressBar.Location = new Point(this.Padding.Left, this.Bottom - this.Padding.Bottom - this.progressBar.Height);
			this.progressBar.MarqueeAnimationSpeed = 10;
			this.progressBar.Style = ProgressBarStyle.Marquee;

			// Set the timer event handler.
			this.timer.Tick += this.OnTick;

			// Add the controls.
			this.Controls.Add(this.progressBar);

			// Resume the layout.
			this.ResumeLayout();
		}

		/// <summary>
		/// Shows the message control. The method is thread-safe.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="title">The title.</param>
		/// <param name="text">The text.</param>
		/// <param name="progress">The visibility of the progress bar.</param>
		/// <param name="duration">The duration of the message in milliseconds. If negative, the message will be displayed indefinitely.</param>
		/// <param name="task">A task to execute on the UI thread before the message is shown.</param>
		public void Show(Image image, string title, string text, bool progress, int duration = -1, NotificationTaskEventHandler task = null)
		{
			// Reposition the control.
			this.Reposition();

			// Set the message box parameters.
			this.image = image;
			this.title = title;
			this.Text = text;
			this.progressBar.Visible = progress;
			this.task = task;

			// If the duration is positive
			if (duration > 0)
			{
				// Set the timer interval.
				this.timer.Interval = duration;
				// Enable the timer.
				this.timer.Enabled = true;
			}
			// Show the control.
			this.Show();
			// Refresh the control.
			this.OnRefresh();
		}

		/// <summary>
		/// Repositions the progress box in the middle of the parent control.
		/// </summary>
		public void Reposition()
		{
			// If the parent control is not null, reposition the control to the middle of the parent.
			if (this.Parent != null)
			{
				// If the parent width is smaller than the default control width, resize the control.
				this.Width = (this.Parent.ClientRectangle.Width < this.defaultWidth - this.Margin.Left - this.Margin.Right) ?
					this.Parent.ClientRectangle.Width - this.Margin.Left - this.Margin.Right : this.defaultWidth;
				// Compute the position of the control.
				this.Left = (this.Parent.Width - this.Width) / 2;
				this.Top = (this.Parent.Height - this.Height) / 2;
			}
		}

		// Protected methods.

		/// <summary>
		/// A method called when the control is being disposed.
		/// </summary>
		/// <param name="disposing">Indicates the disposal of managed resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Dispose the timer.
				this.timer.Dispose();
			}
			// Call the base class dispose method.
			base.Dispose(disposing);
		}

		/// <summary>
		/// An event handler called when painting the control.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Paint the notification box.
			using (Pen pen = new Pen(this.colorBorder))
			{
				e.Graphics.DrawRectangle(pen, this.borderControl);
			}
			// Paint the background.
			using (SolidBrush brush = new SolidBrush(this.colorTitleBackground))
			{
				e.Graphics.FillRectangle(brush, this.borderTitle);
				brush.Color = this.BackColor;
				e.Graphics.FillRectangle(brush, this.borderContent);
			}
			// Draw the title.
			TextRenderer.DrawText(
				e.Graphics,
				this.title,
				this.Font,
				this.borderTitleText,
				this.colorTitleForeground,
				TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
			
			// Draw the image.
			if (this.image != null)
			{
				e.Graphics.DrawImage(this.image, this.borderImage);
			}
			
			// Draw the text.
			TextRenderer.DrawText(
				e.Graphics,
				this.Text,
				this.Font,
				this.borderText,
				this.ForeColor,
				TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);
			
			// Call the base class method.
			base.OnPaint(e);
		}

		/// <summary>
		/// An event handler called when the 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			// Call the base class method.
			base.OnResize(e);
			// Call the refresh event handler.
			this.OnRefresh();
		}

		/// <summary>
		/// An event handler called when the text has changed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			// Call the base class method.
			base.OnTextChanged(e);
			// Refresh the control.
			this.Refresh();
		}

		// Private methods.

		/// <summary>
		/// An event handler called when the timer expires.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnTick(object sender, EventArgs e)
		{
			// Disable the timer.
			this.timer.Enabled = false;
			// Hide the control.
			this.Hide();
			// Execute the task on the UI thread.
			if (task != null) task();
		}

		/// <summary>
		/// An event handler called when recomputing the control borders and refreshing the controls.
		/// </summary>
		private void OnRefresh()
		{
			// Control border.
			this.borderControl.X = this.ClientRectangle.X;
			this.borderControl.Y = this.ClientRectangle.Y;
			this.borderControl.Width = this.ClientRectangle.Width - 1;
			this.borderControl.Height = this.ClientRectangle.Height - 1;

			// Title border.
			this.borderTitle.X = this.borderControl.X + 1;
			this.borderTitle.Y = this.borderControl.Y + 1;
			this.borderTitle.Width = this.borderControl.Width - 1;
			this.borderTitle.Height = this.titleHeight;

			// Title text border.
			this.borderTitleText.X = this.borderTitle.X + 8;
			this.borderTitleText.Y = this.borderTitle.Y;
			this.borderTitleText.Width = this.borderTitle.Width - 16;
			this.borderTitleText.Height = this.titleHeight;

			// The content border.
			this.borderContent.X = this.borderTitle.X;
			this.borderContent.Y = this.borderTitle.Bottom + 1;
			this.borderContent.Width = this.borderTitle.Width;
			this.borderContent.Height = this.borderControl.Height - this.borderTitle.Height - 2;

			// The image border.
			this.borderImage.X = this.borderControl.X + this.Padding.Left;
			this.borderImage.Y = this.borderTitle.Bottom + this.Padding.Top;
			this.borderImage.Width = image != null ? image.Width : 0;
			this.borderImage.Height = image != null ? image.Height : 0;

			// The text border.
			this.borderText.X = this.borderImage.Right + this.Padding.Left;
			this.borderText.Y = this.borderImage.Y;
			this.borderText.Width = this.borderControl.Right - this.borderText.X - this.Padding.Right;
			this.borderText.Height = this.progressBar.Top - this.borderText.Y - this.Padding.Bottom;

			// Repaint the control.
			this.Refresh();
		}
	}
}
