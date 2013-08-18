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
	/// <summary>
	/// A class representing a progress box.
	/// </summary>
	public sealed class ProgressBox : ThreadSafeControl
	{
		private ProgressInfo progress = null;

		private int progressHeight = 12;
		private int spacing = 4;
		private Size legendSize = new Size(12, 12);
		private Color colorProgressBorder = ProfessionalColors.MenuBorder;
		private Color colorProgressDefault = SystemColors.ControlLightLight;
		
		private Size textSize;

		private Rectangle controlBounds;
		private Rectangle controlBorder;
		private Rectangle progressBounds;
		private Rectangle progressBorder;
		private Rectangle contentBounds;
		private Rectangle textBounds;

		/// <summary>
		/// Creates a new progress box control instance.
		/// </summary>
		public ProgressBox()
		{
			this.Size = new Size(200, 25);
			this.Padding = new Padding(4);

			// Update the geometry of the control.
			this.OnUpdateGeometrics();
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the progress information.
		/// </summary>
		public ProgressInfo Progress
		{
			get { return this.progress; }
			set
			{
				// Save the old value.
				ProgressInfo progress = this.progress;
				// Set the new value.
				this.progress = value;
				// Call the progress set event handler.
				this.OnProgressSet(progress, value);
			}
		}
		/// <summary>
		/// Gets or sets the progress bar height.
		/// </summary>
		public int ProgressHeight
		{
			get { return this.progressHeight; }
			set
			{
				// Save the old value.
				int progressHeight = this.progressHeight;
				// Set the new value.
				this.progressHeight = value;
				// Call the event handler.
				this.OnProgressHeightSet(progressHeight, value);
			}
		}
		/// <summary>
		/// Gets or sets the progress bar border color.
		/// </summary>
		public Color ColorProgressBorder
		{
			get { return this.colorProgressBorder; }
			set { this.colorProgressBorder = value; }
		}
		/// <summary>
		/// Gets or sets the default progress color.
		/// </summary>
		public Color ColorProgressDefault
		{
			get { return this.colorProgressDefault; }
			set { this.colorProgressDefault = value; }
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the control is being painted.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Call the base class method.
			base.OnPaint(e);

			// Create the drawing brush.
			using (SolidBrush brush = new SolidBrush(Color.Black))
			{
				// Create the drawing pen.
				using (Pen pen = new Pen(this.colorProgressBorder))
				{
					// If the progress is not null.
					if (null != this.progress)
					{
						// Draw the progress bar border.
						e.Graphics.DrawRectangle(pen, this.progressBorder);
					}
				}
			}
		}

		/// <summary>
		/// An event handler called when the control is being resized.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnResize(EventArgs e)
		{
			// Update the progress.
			this.OnUpdateProgress();
			// Update the legend.
			this.OnUpdateLegend();
			// Update the geometry of the control.
			this.OnUpdateGeometrics();
			// Call the base class method.
			base.OnResize(e);
		}

		/// <summary>
		/// An event handler called when the control padding has changed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaddingChanged(EventArgs e)
		{
			// Update the progress.
			this.OnUpdateProgress();
			// Update the legend.
			this.OnUpdateLegend();
			// Update the geometry of the control.
			this.OnUpdateGeometrics();
			// Call the base class method.
			base.OnPaddingChanged(e);
		}

		/// <summary>
		/// An event handler called when the text has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			// Update the text width.
			this.textSize = TextRenderer.MeasureText(this.Text, this.Font);
			// Update the geometry of the control.
			this.OnUpdateGeometrics();
			// Call the base class method.
			base.OnTextChanged(e);
		}

		// Private methods.

		/// <summary>
		/// An event handler called when a new progress info has been set.
		/// </summary>
		/// <param name="oldProgress">The old progress info.</param>
		/// <param name="newProgress">The new progress info.</param>
		private void OnProgressSet(ProgressInfo oldProgress, ProgressInfo newProgress)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Remove the event handler for the old progress info.
			if (null != oldProgress)
			{
				oldProgress.CountChanged -= this.OnProgressCountChanged;
				oldProgress.DefaultChanged -= this.OnProgressDefaultChanged;
				oldProgress.LevelChanged -= this.OnProgressLevelChanged;
				oldProgress.LegendSet -= OnProgressLegendSet;
				oldProgress.LegendChanged -= OnProgressLegendChanged;
			}
			// Add the event handler for the new progress info.
			if (null != newProgress)
			{
				newProgress.CountChanged += this.OnProgressCountChanged;
				newProgress.DefaultChanged += this.OnProgressDefaultChanged;
				newProgress.LevelChanged += this.OnProgressLevelChanged;
				newProgress.LegendSet += this.OnProgressLegendSet;
				newProgress.LegendChanged += this.OnProgressLegendChanged;
			}
			// Update the progress.
			this.OnUpdateProgress();
			// Update the legend.
			this.OnUpdateLegend();
			// Update the geometric measurements.
			this.OnUpdateGeometrics();
			// Refresh the control.
			this.Invalidate(this.controlBorder);
		}

		/// <summary>
		/// An event handler called when the progress count has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		private void OnProgressCountChanged(ProgressInfo progress)
		{
			// If the event is not for the current progress info, do nothing.
			if (progress != this.progress) return;
		}

		/// <summary>
		/// An event handler called when the progress default has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		private void OnProgressDefaultChanged(ProgressInfo progress)
		{
			// If the event is not for the current progress info, do nothing.
			if (progress != this.progress) return;
		}

		/// <summary>
		/// An event handler called when the progress level has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		private void OnProgressLevelChanged(ProgressInfo progress)
		{
			// If the event is not for the current progress info, do nothing.
			if (progress != this.progress) return;
		}

		/// <summary>
		/// An event handler called when the progress legend is being set.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		/// <param name="oldLegend">The old legend.</param>
		/// <param name="newLegend">The new legend.</param>
		private void OnProgressLegendSet(ProgressInfo progress, ProgressLegend oldLegend, ProgressLegend newLegend)
		{
			// If the old legend is the same as the new legend, do nothing.
			if (oldLegend == newLegend) return;
		}

		/// <summary>
		/// An event handler called when the progress legend has changed.
		/// </summary>
		/// <param name="progress">The old legend.</param>
		/// <param name="legend">The new legend.</param>
		private void OnProgressLegendChanged(ProgressInfo progress, ProgressLegend legend)
		{
		}

		/// <summary>
		/// An event handler called when a new progress bar height has been set.
		/// </summary>
		/// <param name="oldHeight">The old height.</param>
		/// <param name="newHeight">The new height.</param>
		private void OnProgressHeightSet(int oldHeight, int newHeight)
		{
		}


		/// <summary>
		/// An event handler called when the progress information is being updated.
		/// </summary>
		private void OnUpdateProgress()
		{
		}

		/// <summary>
		/// An event handler called when the progress legend is being updated.
		/// </summary>
		private void OnUpdateLegend()
		{
		}

		/// <summary>
		/// A method called to update the geometric characteristics of the progress control.
		/// </summary>
		private void OnUpdateGeometrics()
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			
			// If the control bounds have changed.
			if (this.controlBounds != this.ClientRectangle)
			{
				// Compute the control bounds.
				this.controlBounds = this.ClientRectangle;
				// Compute the control border.
				this.controlBorder = new Rectangle(
					this.controlBounds.X + this.Padding.Left,
					this.controlBounds.Y + this.Padding.Top,
					this.controlBounds.Width - this.Padding.Top - this.Padding.Bottom,
					this.controlBounds.Height - this.Padding.Left - this.Padding.Right);
				// Compute the progress border.
				this.progressBorder = new Rectangle(
					this.controlBorder.X,
					this.controlBorder.Bottom - this.progressHeight,
					this.controlBorder.Width,
					this.progressHeight);
				// Compute the progress bounds.
				this.progressBounds = new Rectangle(
					this.progressBorder.X + 1,
					this.progressBorder.Y + 1,
					this.progressBorder.Width - 1,
					this.progressBorder.Height - 1);
				// Compute the content bounds.
				this.contentBounds = new Rectangle(
					this.controlBorder.X,
					this.controlBorder.Y,
					this.controlBorder.Width,
					this.progressBorder.Top - this.controlBorder.Y);
			}

			// Compute the text bounds.
			this.textBounds = new Rectangle(
				this.contentBounds.X,
				this.contentBounds.Y,
				this.textSize.Width,
				this.controlBounds.Height);
		}

		/// <summary>
		/// A method called to update the geometrics of the progress legend.
		/// </summary>
		private void OnUpdateLegendGeometrics()
		{
		}
	}
}
