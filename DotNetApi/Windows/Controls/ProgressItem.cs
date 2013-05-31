﻿/* 
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
	public delegate void ProgressItemEventHandler(ProgressItem item);
	public delegate void ProgressItemInfoSetEventHandler(ProgressItem item, ProgressInfo oldProgress, ProgressInfo newProgress);
	public delegate void ProgressItemLegendSetEventHandler(ProgressItem item, ProgressInfo progress, ProgressLegend oldLegend, ProgressLegend newLegend);
	public delegate void ProgressItemLegendChangedEventHandler(ProgressItem item, ProgressInfo progress, ProgressLegend legend);

	/// <summary>
	/// An progress list box item.
	/// </summary>
	[DesignTimeVisible(false)]
	public class ProgressItem : Component
	{
		/// <summary>
		/// A collection of progress navigator item items.
		/// </summary>
		public class Collection : CollectionBase
		{
			// Public delegates.
			public delegate void ClearedEventHandler();
			public delegate void ChangedEventHandler(int index, ProgressItem item);
			public delegate void SetEventHandler(int index, ProgressItem oldItem, ProgressItem newItem);

			// Public properties.

			/// <summary>
			/// Gets or sets the item at the specified index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <returns>The item.</returns>
			public ProgressItem this[int index]
			{
				get { return this.List[index] as ProgressItem; }
				set { this.List[index] = value; }
			}

			// Public events.

			/// <summary>
			/// An event raised before the collection is cleared.
			/// </summary>
			public event ClearedEventHandler BeforeCleared;
			/// <summary>
			/// An event raised after the collection is cleared.
			/// </summary>
			public event ClearedEventHandler AfterCleared;
			/// <summary>
			/// An event raised before an item is inserted into the collection.
			/// </summary>
			public event ChangedEventHandler BeforeItemInserted;
			/// <summary>
			/// An event raised after an item is inserted into the collection.
			/// </summary>
			public event ChangedEventHandler AfterItemInserted;
			/// <summary>
			/// An event raised before an item is removed from the collection.
			/// </summary>
			public event ChangedEventHandler BeforeItemRemoved;
			/// <summary>
			/// An event raised after an item is removed from the collection.
			/// </summary>
			public event ChangedEventHandler AfterItemRemoved;
			/// <summary>
			/// An event raised before an item is set in the collection.
			/// </summary>
			public event SetEventHandler BeforeItemSet;
			/// <summary>
			/// An event raised after an item is set in the collection.
			/// </summary>
			public event SetEventHandler AfterItemSet;

			// Public methods.

			/// <summary>
			/// Adds an item to the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns>The position into which the new item was inserted,
			/// or -1 to indicate that the item was not inserted into the collection.</returns>
			public int Add(ProgressItem item)
			{
				// Add the item.
				int result = this.List.Add(item);
				// Return the result.
				return result;
			}

			/// <summary>
			/// Adds a range of items to the collection.
			/// </summary>
			/// <param name="items">The range of items.</param>
			public void AddRange(ProgressItem[] items)
			{
				// Add the items.
				foreach (ProgressItem item in items)
				{
					this.Add(item);
				}
			}

			/// <summary>
			/// Determines the index of the specific item in the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns>The index of value if found in the list; otherwise, -1.</returns>
			public int IndexOf(ProgressItem item)
			{
				return this.List.IndexOf(item);
			}

			/// <summary>
			/// Inserts the item in the collection at the specified index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="item">The item</param>
			public void Insert(int index, ProgressItem item)
			{
				// Insert the item.
				this.List.Insert(index, item);
			}

			/// <summary>
			/// Removes the item from the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			public void Remove(ProgressItem item)
			{
				// Remove the item.
				this.List.Remove(item);
			}

			/// <summary>
			/// Verifies if the specified item is found in the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns><b>True</b> if the element is found in the collection, or <b>false</b> otherwise.</returns>
			public bool Contains(ProgressItem item)
			{
				return this.List.Contains(item);
			}

			// Protected methods.

			/// <summary>
			/// Validates the specified value as an item for this collection.
			/// </summary>
			/// <param name="value">The value to validate.</param>
			protected override void OnValidate(Object value)
			{
				if (value.GetType() != typeof(ProgressItem))
					throw new ArgumentException("Value must be a progress navigator item.", "value");
			}


			/// <summary>
			/// An event handler called before clearing the item collection.
			/// </summary>
			protected override void OnClear()
			{
				// Call the base class method.
				base.OnClear();
				// Raise the event.
				if (this.BeforeCleared != null) this.BeforeCleared();
			}

			/// <summary>
			/// An event handler called after clearing the item collection.
			/// </summary>
			protected override void OnClearComplete()
			{
				// Call the base class method.
				base.OnClearComplete();
				// Raise the event.
				if (this.AfterCleared != null) this.AfterCleared();
			}

			/// <summary>
			/// An event handler called before inserting an item into the collection.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="value">The item.</param>
			protected override void OnInsert(int index, object value)
			{
				// Call the base class method.
				base.OnInsert(index, value);
				// Raise the event.
				if (this.BeforeItemInserted != null) this.BeforeItemInserted(index, value as ProgressItem);
			}

			/// <summary>
			/// An event handler called after inserting an item into the collection.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="value">The item.</param>
			protected override void OnInsertComplete(int index, object value)
			{
				// Call the base class method.
				base.OnInsertComplete(index, value);
				// Raise the event.
				if (this.AfterItemInserted != null) this.AfterItemInserted(index, value as ProgressItem);
			}

			/// <summary>
			/// An event handler called before removing an item from the collection.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="value">The item.</param>
			protected override void OnRemove(int index, object value)
			{
				// Call the base class method.
				base.OnRemove(index, value);
				// Raise the event.
				if (this.BeforeItemRemoved != null) this.BeforeItemRemoved(index, value as ProgressItem);
			}

			/// <summary>
			/// An event handler called after removing an item from the collection.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="value">The item.</param>
			protected override void OnRemoveComplete(int index, object value)
			{
				// Call the base class method.
				base.OnRemoveComplete(index, value);
				// Raise the event.
				if (this.AfterItemRemoved != null) this.AfterItemRemoved(index, value as ProgressItem);
			}

			/// <summary>
			/// An event handler called before setting the value of an item from the collection.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="oldValue">The old item.</param>
			/// <param name="newValue">The new item.</param>
			protected override void OnSet(int index, object oldValue, object newValue)
			{
				// Call the base class method.
				base.OnSet(index, oldValue, newValue);
				// Raise the event.
				if (this.BeforeItemSet != null) this.BeforeItemSet(index, oldValue as ProgressItem, newValue as ProgressItem);
			}

			/// <summary>
			/// An event handler called after setting the value of an item from the collection.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="oldValue">The old item.</param>
			/// <param name="newValue">The new item.</param>
			protected override void OnSetComplete(int index, object oldValue, object newValue)
			{
				// Call the base class method.
				base.OnSetComplete(index, oldValue, newValue);
				// Raise the event.
				if (this.AfterItemSet != null) this.AfterItemSet(index, oldValue as ProgressItem, newValue as ProgressItem);
			}
		}

		// Private variables.
	
		private string text = null;
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

		// Protected methods.

		/// <summary>
		/// An event handler called when the item text has changed.
		/// </summary>
		protected virtual void OnTextChanged()
		{
			// Raise the event.
			if (null != this.TextChanged) this.TextChanged(this);
		}

		/// <summary>
		/// An event handler called before the item progress object has changed.
		/// </summary>
		/// <param name="oldProgress">The old progress object.</param>
		/// <param name="newProgress">The new progress object.</param>
		protected virtual void OnProgressChanged(ProgressInfo oldProgress, ProgressInfo newProgress)
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
			if (null != this.ProgressSet) this.ProgressSet(this, oldProgress, newProgress);
		}

		/// <summary>
		/// An event handler called when the progress level has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		protected virtual void OnProgressLevelChanged(ProgressInfo progress)
		{
			// If the progress info is not the current progress info, do nothing.
			if (progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressLevelChanged) this.ProgressLevelChanged(this);
		}

		/// <summary>
		/// An event handler called when the progress default has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		protected virtual void OnProgressDefaultChanged(ProgressInfo progress)
		{
			// If the progress info is not the current progress info, do nothing.
			if (progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressDefaultChanged) this.ProgressDefaultChanged(this);
		}

		/// <summary>
		/// An event handler called when the progress count has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		protected virtual void OnProgressCountChanged(ProgressInfo progress)
		{
			// If the progress info is not the current progress info, do nothing.
			if (progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressCountChanged) this.ProgressCountChanged(this);
		}

		/// <summary>
		/// An event handler called when a new progress legend has been set.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		/// <param name="oldLegend">The old legend.</param>
		/// <param name="newLegend">The new legend.</param>
		protected virtual void OnProgressLegendSet(ProgressInfo progress, ProgressLegend oldLegend, ProgressLegend newLegend)
		{
			// If the progress info is not the current progress info, do nothing.
			if (progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressLegendSet) this.ProgressLegendSet(this, progress, oldLegend, newLegend);
		}

		/// <summary>
		/// An event handler called when the progress legend has changed.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		/// <param name="legend">The progress legend.</param>
		protected virtual void OnProgressLegendChanged(ProgressInfo progress, ProgressLegend legend)
		{
			// If the progress info is not the current progress info, do nothing.
			if (progress != this.progress) return;
			// Raise the event.
			if (null != this.ProgressLegendChanged) this.ProgressLegendChanged(this, progress, legend);
		}
	}
}