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
using DotNetApi.Windows.Controls;
using DotNetApi.Windows.Themes;

namespace DotNetApi.Windows.Forms
{
	/// <summary>
	/// A class representing a notification form.
	/// </summary>
	public class NotificationForm : ThreadSafeForm
	{
		private string title = null;
		private Image image = null;
		private readonly ProgressBar progressBar = new ProgressBar();
		private readonly Button button = new Button();
		private readonly Timer timer = new Timer();

		private const int titleHeight = 35;
		private const int defaultWidth = 450;
		private const int defaultHeight = 150;

		private Rectangle borderControl = new Rectangle();
		private Rectangle borderTitle = new Rectangle();
		private Rectangle borderTitleText = new Rectangle();
		private Rectangle borderContent = new Rectangle();
		private Rectangle borderImage = new Rectangle();
		private Rectangle borderText = new Rectangle();

		private readonly ThemeColorTable colorTable;

		private NotificationTaskAction task = null;
		private object[] parameters = null;

		/// <summary>
		/// Creates a new notification form instance.
		/// </summary>
		public NotificationForm()
		{
			// Suspend the layout.
			this.SuspendLayout();

			// Set the theme color table.
			this.colorTable = ToolStripManager.Renderer is ThemeRenderer ? (ToolStripManager.Renderer as ThemeRenderer).ColorTable : ThemeColorTable.DefaultColorTable;

			// Default properties.
			this.FormBorderStyle = FormBorderStyle.None;
			this.Width = NotificationForm.defaultWidth;
			this.Height = NotificationForm.defaultHeight;
			this.Padding = new Padding(16, 8, 16, 16);
			this.DoubleBuffered = true;
			this.BackColor = this.colorTable.NotificationBoxBackground;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ShowInTaskbar = false;
			this.ShowIcon = false;

			// Default components.
			this.button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.button.Size = new Size(75, 23);
			this.button.Location = new Point(this.Right - this.Padding.Right - this.button.Width, this.Bottom - this.Padding.Bottom - this.button.Height + 3);
			this.button.Visible = false;
			this.button.Text = "&Cancel";

			this.progressBar.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
			this.progressBar.Size = new Size(this.Width - this.Padding.Left - this.Padding.Right - this.button.Width - 10, 17);
			this.progressBar.Location = new Point(this.Padding.Left, this.Bottom - this.Padding.Bottom - this.progressBar.Height);
			this.progressBar.MarqueeAnimationSpeed = 10;
			this.progressBar.Style = ProgressBarStyle.Marquee;
			this.progressBar.Visible = false;

			// Set the timer event handler.
			this.timer.Tick += this.OnTick;

			// Add the controls.
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.button);

			// Apply the window font.
			Window.SetFont(this);

			// Resume the layout.
			this.ResumeLayout();
		}

		// Protected properties.

		/// <summary>
		/// Gets or sets the window title.
		/// </summary>
		protected string Title
		{
			get { return this.title; }
			set { this.OnTitleSet(value); }
		}
		/// <summary>
		/// Gets or sets the window text.
		/// </summary>
		protected new string Text
		{
			get { return this.Text; }
			set { this.OnTextSet(value); }
		}
		/// <summary>
		/// Gets or sets whether the button is enabled for a progress control.
		/// </summary>
		protected bool ButtonEnabled
		{
			get { return this.button.Enabled; }
			set { this.button.Enabled = value; }
		}
		/// <summary>
		/// Gets or sets whether the progress bar is visible.
		/// </summary>
		protected bool ProgressVisible
		{
			get { return this.progressBar.Visible; }
			set { this.progressBar.Visible = value; }
		}

		// Public methods.

		/// <summary>
		/// Shows the notification form. The method is thread-safe.
		/// </summary>
		/// <param name="owner">The owner control.</param>
		/// <param name="image">The image.</param>
		/// <param name="title">The title.</param>
		/// <param name="text">The text.</param>
		/// <param name="progress">The visibility of the progress bar.</param>
		/// <param name="duration">The duration of the message in milliseconds. If negative, the message will be displayed indefinitely.</param>
		/// <param name="task">A task to execute on the UI thread before the message is shown.</param>
		/// <param name="parameters">The task parameters.</param>
		/// <returns>The dialog result.</returns>
		public DialogResult ShowDialog(
			IWin32Window owner,
			Image image,
			string title,
			string text,
			bool progress,
			int duration = -1,
			NotificationTaskAction task = null,
			object[] parameters = null)
		{
			// Set the message box parameters.
			this.image = image;
			this.title = title;
			this.Text = text;
			this.progressBar.Visible = progress;
			this.button.Visible = progress;
			this.button.Enabled = true;
			this.task = task;
			this.parameters = parameters;

			// If the duration is positive.
			if (duration > 0)
			{
				// Set the timer interval.
				this.timer.Interval = duration;
				// Enable the timer.
				this.timer.Enabled = true;
			}
			else
			{
				// Execute the task.
				if (this.task != null) this.task(this.parameters);
			}

			// Call the form opened
			this.OnFormOpened();

			// Show the control.
			return this.ShowDialog(owner);
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Dispose the timer.
				this.timer.Dispose();
				// Dispose the progerss bar.
				this.progressBar.Dispose();
				// Dispose the button.
				this.button.Dispose();
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
			using (Pen pen = new Pen(this.colorTable.NotificationBoxBorder))
			{
				e.Graphics.DrawRectangle(pen, this.borderControl);
			}
			// Paint the background.
			using (SolidBrush brush = new SolidBrush(this.colorTable.NotificationBoxTitle))
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
				this.colorTable.NotificationBoxTitleText,
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
			// Measure the new size of the controls.
			this.OnMeasureControls(this.progressBar.Visible);
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

		/// <summary>
		/// A methods called when the form has been opened.
		/// </summary>
		protected virtual void OnFormOpened()
		{

		}

		// Private methods.

		/// <summary>
		/// Shows the notification form.
		/// </summary>
		private new void Show()
		{
			base.Show();
		}

		/// <summary>
		/// Shows the notification form with the specified owner.
		/// </summary>
		/// <param name="owner">The owner window.</param>
		private new void Show(IWin32Window owner)
		{
			base.Show(owner);
		}

		/// <summary>
		/// Shows the notification dialog.
		/// </summary>
		/// <returns>The dialog result.</returns>
		private new DialogResult ShowDialog()
		{
			return base.ShowDialog();
		}

		/// <summary>
		/// Shows the notification dialog with the specified owner.
		/// </summary>
		/// <param name="owner">The owner window.</param>
		/// <returns>The dialog result.</returns>
		private new DialogResult ShowDialog(IWin32Window owner)
		{
			return base.ShowDialog(owner);
		}

		/// <summary>
		/// An event handler called when the timer expires.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnTick(object sender, EventArgs e)
		{
			// Disable the timer.
			this.timer.Enabled = false;
			// Close the dialog.
			this.Close();
			// Execute the task on the UI thread.
			if (this.task != null) this.task(this.parameters);
		}

		/// <summary>
		/// An event handler called when recomputing the control borders and refreshing the controls.
		/// </summary>
		private void OnRefresh()
		{
			// Repaint the control.
			this.Refresh();
		}

		/// <summary>
		/// Measures the size of the controls.
		/// </summary>
		/// <param name="progress">Indicates if the progress bar is showed.</param>
		private void OnMeasureControls(bool progress)
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
			this.borderTitle.Height = NotificationForm.titleHeight;

			// Title text border.
			this.borderTitleText.X = this.borderTitle.X + 8;
			this.borderTitleText.Y = this.borderTitle.Y;
			this.borderTitleText.Width = this.borderTitle.Width - 16;
			this.borderTitleText.Height = NotificationForm.titleHeight;

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
			this.borderText.Height = progress ? this.progressBar.Top - this.borderText.Y - this.Padding.Bottom :
				this.ClientRectangle.Bottom - this.borderText.Y - this.Padding.Bottom;
		}

		/// <summary>
		/// Sets the current title.
		/// </summary>
		/// <param name="title">The title.</param>
		private void OnTitleSet(string title)
		{
			// Set the title.
			this.title = title;
			// Refresh the title region.
			this.Invalidate(this.borderTitleText);
		}

		/// <summary>
		/// Sets the current text.
		/// </summary>
		/// <param name="text">The text.</param>
		private void OnTextSet(string text)
		{
			// Set the text.
			this.Text = text;
			// Refresh the text region.
			this.Invalidate(this.borderText);
		}
	}
}
