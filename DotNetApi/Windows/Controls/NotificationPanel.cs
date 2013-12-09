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
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a notification panel control.
	/// </summary>
	public class NotificationPanel : ThreadSafeControl
	{
		private readonly ProgressBar progressBar;
		private readonly Label labelProgress;
		private readonly PictureBox pictureProgress;

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public NotificationPanel()
		{
			this.progressBar = new ProgressBar();
			this.labelProgress = new Label();
			this.pictureProgress = new PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureProgress)).BeginInit();
			this.SuspendLayout();
			// progressBar
			this.progressBar.Location = new Point(62, 40);
			this.progressBar.Size = new Size(730, 16);
			this.progressBar.TabIndex = 1;
			this.progressBar.Visible = false;
			// labelProgress
			this.labelProgress.Location = new Point(62, 8);
			this.labelProgress.Size = new Size(730, 29);
			this.labelProgress.TabIndex = 0;
			this.labelProgress.TextAlign = ContentAlignment.MiddleLeft;
			// pictureProgress
			this.pictureProgress.Location = new Point(8, 8);
			this.pictureProgress.Size = new Size(48, 48);
			this.pictureProgress.TabIndex = 3;
			this.pictureProgress.TabStop = false;
			// Control.
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.labelProgress);
			this.Controls.Add(this.pictureProgress);
			this.MaximumSize = new Size(0, 64);
			this.MinimumSize = new Size(0, 64);
			this.Size = new Size(800, 64);
			((System.ComponentModel.ISupportInitialize)(this.pictureProgress)).EndInit();
			this.ResumeLayout(false);
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the notification image.
		/// </summary>
		[Browsable(true), DisplayName("Text"), Description("The notification image."), Category("Appearance")]
		public Image Image
		{
			get { return this.pictureProgress.Image; }
			set { this.pictureProgress.Image = value; }
		}
		/// <summary>
		/// Gets or sets the notification message.
		/// </summary>
		[Browsable(true), DisplayName("Message"), Description("The notification text."), Category("Appearance")]
		public string Message
		{
			get { return this.labelProgress.Text; }
			set { this.labelProgress.Text = value; }
		}

		// Public methods.

		/// <summary>
		/// Displays the specified progress information.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="text">The text</param>
		/// <param name="progress">If <b>true</b>, displays a marquee progress bar.</param>
		public void Show(Image image, string text, bool progress = true)
		{
			// Show the progress on the UI thread.
			this.Invoke(() =>
			{
				this.pictureProgress.Image = image;
				this.labelProgress.Text = text;

				this.progressBar.MarqueeAnimationSpeed = 10;
				this.progressBar.Style = ProgressBarStyle.Marquee;
				this.progressBar.Visible = progress;
			});
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			// Dispose the child controls.
			this.pictureProgress.Dispose();
			this.labelProgress.Dispose();
			this.progressBar.Dispose();
			// Call the base class method.
			base.Dispose(disposing);
		}

		/// <summary>
		/// An event handler called when the size of the control has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			// Update the control sizes.
			this.labelProgress.Size = new Size(this.Size.Width - 70, this.Size.Height - 35);
			this.progressBar.Location = new Point(62, this.Size.Height - 24);
			this.progressBar.Size = new Size(this.Size.Width - 70, 16);
			// Call the base class method.
			base.OnSizeChanged(e);
		}
	}
}
