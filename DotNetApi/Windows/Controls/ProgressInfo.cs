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
using System.ComponentModel;

namespace DotNetApi.Windows.Controls
{
	public delegate void ProgressEventHandler(ProgressInfo progress);
	public delegate void ProgressLegendChangedEventHandler(ProgressInfo progress, ProgressLegend legend);
	public delegate void ProgressLegendSetEventHandler(ProgressInfo progress, ProgressLegend oldLegend, ProgressLegend newLegend); 

	/// <summary>
	/// A class representing multi-level progress information.
	/// </summary>
	public class ProgressInfo : Component
	{
		private ProgressLegend legend = null;
		private int count = 0;
		private int[] progress = new int[0];
		private int defaultProgress = 0;

		/// <summary>
		/// Creates a new progress info component instance.
		/// </summary>
		public ProgressInfo()
		{
		}

		/// <summary>
		/// Creates a new progress info with the specified legend.
		/// </summary>
		/// <param name="legend">The progress legend.</param>
		public ProgressInfo(ProgressLegend legend)
		{
			// Set the legend.
			this.legend = legend;
			// Set the legend event handlers.
			this.legend.ItemChanged += this.OnLegendItemChanged;
			this.legend.ItemsChanged += this.OnLegendItemsChanged;
			// Initialize the progress.
			this.OnInitialize();
		}

		// Public events.

		/// <summary>
		/// An event raised when the progress level has changed.
		/// </summary>
		public event ProgressEventHandler LevelChanged;
		/// <summary>
		/// An event raised when the progress default value has changed.
		/// </summary>
		public event ProgressEventHandler DefaultChanged;
		/// <summary>
		/// An event raised when the progress count has changed.
		/// </summary>
		public event ProgressEventHandler CountChanged;
		/// <summary>
		/// An event raised when the progress legend has changed.
		/// </summary>
		public event ProgressLegendChangedEventHandler LegendChanged;
		/// <summary>
		/// An event raised when a new legend has been set.
		/// </summary>
		public event ProgressLegendSetEventHandler LegendSet;

		// Public properties.

		/// <summary>
		/// Returns the progress for the legend item at the specific index.
		/// </summary>
		/// <param name="index">The legend index.</param>
		/// <returns>The progress.</returns>
		public int this[int index]
		{
			get { return this.progress[index]; }
		}
		/// <summary>
		/// Gets or sets the progress legend.
		/// </summary>
		public ProgressLegend Legend
		{
			get { return this.legend; }
			set
			{
				// Save the old legend.
				ProgressLegend legend = this.legend;
				// Set the legend.
				this.legend = value;
				// Initialize the progress values.
				this.OnInitialize();
				// Call the legend changed event handler.
				this.OnLegendSet(legend, value);
			}
		}
		/// <summary>
		/// Gets or sets the default progress value;
		/// </summary>
		public int Default
		{
			get { return this.defaultProgress; }
			set
			{
				// Set the default progress.
				this.defaultProgress = value;
				// Initialize the progress values.
				this.OnInitialize();
				// Call the default changed event handler.
				this.OnDefaultChanged();
			}
		}
		/// <summary>
		/// Gets or sets the default count value.
		/// </summary>
		public int Count
		{
			get { return this.count; }
			set
			{
				// Set the count value.
				this.count = value;
				// Initialize the progress values.
				this.OnInitialize();
				// Call the count changes event handler.
				this.OnCountChanged();
			}
		}

		// Public methods.

		/// <summary>
		/// Increments the progress at the specified legend index, decrementing from the default progress. If the default
		/// legend index is already at zero, the method does nothing.
		/// </summary>
		/// <param name="to">The legend index to increment.</param>
		public void Change(int to)
		{
			// If the default progress is already at zero, do nothing.
			if (0 == this.progress[this.defaultProgress]) return;
			// If the from and to are equal, do nothing.
			if (this.defaultProgress == to) return;
			// Else, decrement one progress, increment the other.
			this.progress[this.defaultProgress]--;
			this.progress[to]++;
			// Call the level changed event handler.
			this.OnLevelChanged();
		}

		/// <summary>
		/// Changes the progress between the specified legend indices. If the legend index to decrement is already zero,
		/// the method does nothing.
		/// </summary>
		/// <param name="from">The legend index to decrement.</param>
		/// <param name="to">The legend index to increment.</param>
		public void Change(int from, int to)
		{
			// If the progress to decrement is already at zero, do nothing.
			if (0 == this.progress[from]) return;
			// If the from and to are equal, do nothing.
			if (from == to) return;
			// Else, decrement one progress, increment the other.
			this.progress[from]--;
			this.progress[to]++;
			// Call the level changed event handler.
			this.OnLevelChanged();
		}

		// Protected methods.
		protected virtual void OnLevelChanged()
		{
			// Raise the event.
			if (null != this.LevelChanged) this.LevelChanged(this);
		}

		/// <summary>
		/// An event handler called when the progress default has changed.
		/// </summary>
		protected virtual void OnDefaultChanged()
		{
			// Raise the event.
			if (null != this.DefaultChanged) this.DefaultChanged(this);
		}

		/// <summary>
		/// An event handler called when the count value has changed.
		/// </summary>
		protected virtual void OnCountChanged()
		{
			// Raise the event.
			if (null != this.CountChanged) this.CountChanged(this);
		}

		/// <summary>
		/// An event handler called when a new progress legend has been set.
		/// </summary>
		/// <param name="oldLegend">The old legend.</param>
		/// <param name="newLegend">The new legend.</param>
		protected virtual void OnLegendSet(ProgressLegend oldLegend, ProgressLegend newLegend)
		{
			// Remove the old legend event handlers.
			if (oldLegend != null)
			{
				oldLegend.ItemsChanged -= this.OnLegendItemsChanged;
				oldLegend.ItemChanged -= this.OnLegendItemChanged;
			}
			// Add the new legend event handlers.
			if (newLegend != null)
			{
				newLegend.ItemsChanged += this.OnLegendItemsChanged;
				newLegend.ItemChanged += this.OnLegendItemChanged;
			}

			// Raise the event.
			if (null != this.LegendSet) this.LegendSet(this, oldLegend, newLegend);
		}

		/// <summary>
		/// An event handler called when the legend items have changed.
		/// </summary>
		/// <param name="legend">The legend.</param>
		protected virtual void OnLegendItemsChanged(ProgressLegend legend)
		{
			// If the legend is not the current legend, do nothing.
			if (legend != this.legend) return;
			// Initialize the progress values.
			this.OnInitialize();
			// Raise the legend changed event.
			if (null != this.LegendChanged) this.LegendChanged(this, legend);
		}

		/// <summary>
		/// An event handler called whn a leged item has changed.
		/// </summary>
		/// <param name="legend">The progress legend.</param>
		/// <param name="item">The legend item.</param>
		protected virtual void OnLegendItemChanged(ProgressLegend legend, ProgressLegendItem item)
		{
			// If the legend is not the current legend, do nothing.
			if (legend != this.legend) return;
			// Raise the legend changed event.
			if (null != this.LegendChanged) this.LegendChanged(this, legend);
		}

		// Private methods.

		/// <summary>
		/// An event handler called when the progress information is initialized.
		/// </summary>
		private void OnInitialize()
		{
			// If the legend is null, do nothing.
			if (null == this.legend) return;

			// Resize the progress array to match the number of progress items.
			Array.Resize<int>(ref this.progress, this.legend.Items.Count);
			// If the default progress is greater than the item count, reset the default progress.
			if (this.defaultProgress >= this.legend.Items.Count) this.defaultProgress = 0;
			// Intialize the progress with the default progress value.
			for (int index = 0; index < this.legend.Items.Count; index++)
			{
				this.progress[index] = index == this.defaultProgress ? this.count : 0;
			}
		}
	}
}
