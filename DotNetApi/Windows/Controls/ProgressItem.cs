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
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// An progress list box item.
	/// </summary>
	[DesignTimeVisible(false)]
	public sealed class ProgressItem : Component
	{
		/// <summary>
		/// An internal class representing the geometric characteristics of the progress item.
		/// </summary>
		internal class Geometrics
		{
			internal Rectangle bounds = new Rectangle(0, 0, 0, 0);
			internal Rectangle itemBounds = new Rectangle(0, 0, 0, 0);
			internal Rectangle progressBorder = new Rectangle(0, 0, 0, 0);
			internal Rectangle progressBounds = new Rectangle(0, 0, 0, 0);
			internal Rectangle contentBounds = new Rectangle(0, 0, 0, 0);
			internal Rectangle textBounds = new Rectangle(0, 0, 0, 0);
			internal Rectangle legendBounds = new Rectangle(0, 0, 0, 0);
			internal Rectangle legendTextBounds = new Rectangle(0, 0, 0, 0);
			internal Rectangle legendIconBounds = new Rectangle(0, 0, 0, 0);
			internal int legendItemWidth = 0;
			internal bool showLegend = false;
			internal bool validLegend = false;
		}

		internal Geometrics geometrics = new Geometrics();

		// Private variables.
	
		private string text = null;
		private string subtext = null;
		private bool enabled = true;
		private ProgressInfo progress = null;

		/// <summary>
		/// Creates a new progress list box item.
		/// </summary>
		public ProgressItem()
		{
		}

		/// <summary>
		/// Creates a new progress list box item with the specified text and progress legend.
		/// </summary>
		/// <param name="text">The item text.</param>
		/// <param name="legend">The progress legend.</param>
		public ProgressItem(string text, ProgressLegend legend)
		{
			// Set the text.
			this.text = text;
			// Create the progress information.
			this.progress = new ProgressInfo(legend);
			// Set the progress event handlers.
			this.progress.LevelChanged += this.OnProgressLevelChanged;
			this.progress.DefaultChanged += this.OnProgressDefaultChanged;
			this.progress.CountChanged += this.OnProgressCountChanged;
			this.progress.LegendSet += this.OnProgressLegendSet;
			this.progress.LegendChanged += this.OnProgressLegendChanged;
		}

		// Public events.

		/// <summary>
		/// An event raised when the item text has changed.
		/// </summary>
		public event ProgressItemEventHandler TextChanged;
		/// <summary>
		/// An event raised when the item subtext has changed.
		/// </summary>
		public event ProgressItemEventHandler SubtextChanged;
		/// <summary>
		/// An event raised when the item enabled state has changed.
		/// </summary>
		public event ProgressItemEventHandler EnabledChanged;
		/// <summary>
		/// An event raised before a new item progress has been set.
		/// </summary>
		public event ProgressItemInfoSetEventHandler ProgressSet;
		/// <summary>
		/// An event raised when the progress level has changed.
		/// </summary>
		public event ProgressItemEventHandler ProgressLevelChanged;
		/// <summary>
		/// An event raised when the progress default has changed.
		/// </summary>
		public event ProgressItemEventHandler ProgressDefaultChanged;
		/// <summary>
		/// An event raised when the progress count has changed.
		/// </summary>
		public event ProgressItemEventHandler ProgressCountChanged;
		/// <summary>
		/// An event raised when the progress legend has been set.
		/// </summary>
		public event ProgressItemLegendSetEventHandler ProgressLegendSet;
		/// <summary>
		/// An event raised when the progress legend has changed.
		/// </summary>
		public event ProgressItemLegendChangedEventHandler ProgressLegendChanged;

		// Public properties.

		/// <summary>
		/// Gets or sets the item text.
		/// </summary>
		public string Text
		{
			get { return this.text; }
			set
			{
				// Set the text.
				this.text = value;
				// Call the event handler.
				this.OnTextChanged();
			}
		}
		/// <summary>
		/// Gets or sets the item subtext.
		/// </summary>
		public string Subtext
		{
			get { return this.subtext; }
			set
			{
				// Set the text.
				this.subtext = value;
				// Call the event handler.
				this.OnSubtextChanged();
			}
		}
		/// <summary>
		/// Gets or sets the item progress.
		/// </summary>
		public ProgressInfo Progress
		{
			get { return this.progress; }
			set
			{
				// Save the old progress.
				ProgressInfo progress = this.progress;
				// Set the progress.
				this.progress = value;
				// Call the event handler.
				this.OnProgressChanged(progress, value);
			}
		}
		/// <summary>
		/// Gets or sets whether the progress item is enabled.
		/// </summary>
		public bool Enabled
		{
			get { return this.enabled; }
			set
			{
				// Save the old enabled state.
				bool enabled = this.enabled;
				// Set the enabled state.
				this.enabled = value;
				// Call the event handler.
				this.OnEnabledChanged(enabled, value);
			}
		}
		/// <summary>
		/// Gets or sets an item tag.
		/// </summary>
		public object Tag { get; set; }

		// Private methods.

		/// <summary>
		/// An event handler called when the item text has changed.
		/// </summary>
		private void OnTextChanged()
		{
			// Raise the event.
			if (null != this.TextChanged) this.TextChanged(this, new ProgressItemEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the item subtext has changed.
		/// </summary>
		private void OnSubtextChanged()
		{
			// Raise the event.
			if (null != this.SubtextChanged) this.SubtextChanged(this, new ProgressItemEventArgs(this));
		}

		/// <summary>
		/// An event handler called before the item progress object has changed.
		/// </summary>
		/// <param name="oldProgress">The old progress object.</param>
		/// <param name="newProgress">The new progress object.</param>
		private void OnProgressChanged(ProgressInfo oldProgress, ProgressInfo newProgress)
		{
			// Remove the old progress event handlers.
			if (oldProgress != null)
			{
				oldProgress.LevelChanged -= this.OnProgressLevelChanged;
				oldProgress.DefaultChanged -= this.OnProgressDefaultChanged;
				oldProgress.CountChanged -= this.OnProgressCountChanged;
				oldProgress.LegendSet -= this.OnProgressLegendSet;
				oldProgress.LegendChanged -= this.OnProgressLegendChanged;
			}
			// Add the new progress event handlers.
			if (newProgress != null)
			{
				newProgress.LevelChanged += this.OnProgressLevelChanged;
				newProgress.DefaultChanged += this.OnProgressDefaultChanged;
				newProgress.CountChanged += this.OnProgressCountChanged;
				newProgress.LegendSet += this.OnProgressLegendSet;
				newProgress.LegendChanged += this.OnProgressLegendChanged;
			}
			// Raise the event.
			if (null != this.ProgressSet) this.ProgressSet(this, new ProgressItemInfoSetEventArgs(this, oldProgress, newProgress));
		}

		/// <summary>
		/// An event handler called when the progress level has changed.
		/// </summary>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnProgressLevelChanged(object sender, ProgressInfoEventArgs e)
		{
			// If the progress info is not the current progress info, do nothing.
			if (e.Progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressLevelChanged) this.ProgressLevelChanged(this, new ProgressItemEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the progress default has changed.
		/// </summary>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnProgressDefaultChanged(object sender, ProgressInfoEventArgs e)
		{
			// If the progress info is not the current progress info, do nothing.
			if (e.Progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressDefaultChanged) this.ProgressDefaultChanged(this, new ProgressItemEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the progress count has changed.
		/// </summary>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnProgressCountChanged(object sender, ProgressInfoEventArgs e)
		{
			// If the progress info is not the current progress info, do nothing.
			if (e.Progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressCountChanged) this.ProgressCountChanged(this, new ProgressItemEventArgs(this));
		}

		/// <summary>
		/// An event handler called when a new progress legend has been set.
		/// </summary>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnProgressLegendSet(object sender, ProgressLegendSetEventArgs e)
		{
			// If the progress info is not the current progress info, do nothing.
			if (e.Progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressLegendSet) this.ProgressLegendSet(this, new ProgressItemLegendSetEventArgs(this, progress, e.OldLegend, e.NewLegend));
		}

		/// <summary>
		/// An event handler called when the progress legend has changed.
		/// </summary>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnProgressLegendChanged(object sender, ProgressLegendChangedEventArgs e)
		{
			// If the progress info is not the current progress info, do nothing.
			if (e.Progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressLegendChanged) this.ProgressLegendChanged(this, new ProgressItemLegendChangedEventArgs(this, e.Progress, e.Legend));
		}

		/// <summary>
		/// An event handler called when the item enabled state has changed.
		/// </summary>
		/// <param name="oldEnabled">The old enabled state.</param>
		/// <param name="newEnabled">The new enabled state.</param>
		private void OnEnabledChanged(bool oldEnabled, bool newEnabled)
		{
			// If the enabled state has not changed, do nothing.
			if (oldEnabled == newEnabled) return;
			// Raise the event.
			if (null != this.EnabledChanged) this.EnabledChanged(this, new ProgressItemEventArgs(this));
		}
	}
}
