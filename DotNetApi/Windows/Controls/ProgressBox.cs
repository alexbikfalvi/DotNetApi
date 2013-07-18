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
	public class ProgressBox : ThreadSafeControl
	{
		private ProgressInfo progress = null;

		private int progressHeight = 12;
		private int spacing = 4;
		private Size legendSize = new Size(12, 12);
		private string text = null;

		private Rectangle rectangleBounds;
		private Rectangle rectangleBorder;
		private Rectangle rectangleProgress;

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

		// Protected methods.

		/// <summary>
		/// An event handler called when a new progress info has been set.
		/// </summary>
		/// <param name="oldProgress">The old progress info.</param>
		/// <param name="newProgress">The new progress info.</param>
		protected virtual void OnProgressSet(ProgressInfo oldProgress, ProgressInfo newProgress)
		{
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
		}

		/// <summary>
		/// An event handler called when the progress count has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		protected virtual void OnProgressCountChanged(ProgressInfo progress)
		{
			// If the event is not for the current progress info, do nothing.
			if (progress != this.progress) return;
		}

		/// <summary>
		/// An event handler called when the progress default has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		protected virtual void OnProgressDefaultChanged(ProgressInfo progress)
		{
			// If the event is not for the current progress info, do nothing.
			if (progress != this.progress) return;
		}

		/// <summary>
		/// An event handler called when the progress level has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		protected virtual void OnProgressLevelChanged(ProgressInfo progress)
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
		protected virtual void OnProgressLegendSet(ProgressInfo progress, ProgressLegend oldLegend, ProgressLegend newLegend)
		{
		}

		/// <summary>
		/// An event handler called when the progress legend has changed.
		/// </summary>
		/// <param name="progress">The old legend.</param>
		/// <param name="legend">The new legend.</param>
		protected virtual void OnProgressLegendChanged(ProgressInfo progress, ProgressLegend legend)
		{
		}

		/// <summary>
		/// An event handler called when a new progress bar height has been set.
		/// </summary>
		/// <param name="oldHeight">The old height.</param>
		/// <param name="newHeight">The new height.</param>
		protected virtual void OnProgressHeightSet(int oldHeight, int newHeight)
		{
		}

		/// <summary>
		/// An event handler called when the control is being painted.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Call the base class method.
			base.OnPaint(e);

			using (SolidBrush brush = new SolidBrush(Color.Black))
			{
				e.Graphics.FillRectangle(brush, this.rectangleBounds);
			}
		}

		/// <summary>
		/// An event handler called when the control is being resized.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnResize(EventArgs e)
		{
			// Call the base class method.
			base.OnResize(e);
			// Update the geometry of the control.
			this.OnUpdateGeometrics();
		}

		/// <summary>
		/// An event handler called when the control padding has changed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaddingChanged(EventArgs e)
		{
			// Call the base class method.
			base.OnPaddingChanged(e);
			// Update the geometry of the control.
			this.OnUpdateGeometrics();
		}

		/// <summary>
		/// An event handler called when the text has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			// Call the base class method.
			base.OnTextChanged(e);
		}


		// Private methods.

		/// <summary>
		/// A method called to update the geometric characteristics of the progress control.
		/// </summary>
		private void OnUpdateGeometrics()
		{
			// Update the geometry of the control.
			this.rectangleBounds = this.ClientRectangle;
			this.rectangleBorder = new Rectangle(
				this.rectangleBounds.X + this.Padding.Left,
				this.rectangleBounds.Y + this.Padding.Top,
				this.rectangleBounds.Width - this.Padding.Top - this.Padding.Bottom,
				this.rectangleBounds.Height - this.Padding.Left - this.Padding.Right);
		}
	}
}
