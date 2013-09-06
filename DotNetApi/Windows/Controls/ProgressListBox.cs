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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// An progress list box control.
	/// </summary>
	public sealed class ProgressListBox : ListBox
	{
		private ComponentCollection<ProgressItem> items = new ComponentCollection<ProgressItem>();

		private int itemHeight = 48;
		private int progressHeight = 12;
		private int spacing = 4;
		private Size legendSize = new Size(12, 12);
		private string textNotAvailable = "Not available";
		private string textDisabled = "Not selected";
		private Color colorProgressBorder = ProfessionalColors.MenuBorder;
		private Color colorProgressDefault = SystemColors.ControlLightLight;
		private Padding itemPadding = new Padding(4);
		private Font fontText;

		// List items measurements.

		private int itemMaximumWidth = 0;
		private int itemMaximumVariableWidth = 0;
		private int itemMaximumFixedWidth = 0;
		private int itemMaximumTextWidth = 0;
		private int itemProgressMaximumCountDigits = 0;
		private int itemProgressMaximumCountWidth = 0;
		private int itemProgressLegendMaximumCount = 0;
		private int itemProgressLegendMaximumTextWidth = 0;
		private int itemProgressLegendMaximumItemWidth = 0;
		private int itemProgressLegendMaximumWidth = 0;

		// List events.

		private bool suspendProgressEvents = false;
		private bool eventProgressLevelChanged = false;
		private bool eventProgressDefaultChanged = false;
		private bool eventProgressCountChanged = false;

		/// <summary>
		/// Creates a new progress list box instance.
		/// </summary>
		public ProgressListBox()
		{
			// Set control properties.
			this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.DoubleBuffered = true;
			this.ItemHeight = this.itemHeight;
			this.IntegralHeight = false;

			// Set the control variables.
			this.fontText = new Font(this.Font, FontStyle.Bold);

			// Set collection event handlers.
			this.items.BeforeCleared += this.OnBeforeItemsCleared;
			this.items.AfterItemInserted += this.OnAfterItemInserted;
			this.items.AfterItemRemoved += this.OnAfterItemRemoved;
			this.items.AfterItemSet += this.OnAfterItemSet;
		}

		// Public properties.

		/// <summary>
		/// Gets the collection of list box items.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public new ComponentCollection<ProgressItem> Items { get { return this.items; } }

		// Public methods.

		/// <summary>
		/// Suspends all progress events.
		/// </summary>
		public void SuspendProgressEvents()
		{
			// Set the suspend progress event to true.
			this.suspendProgressEvents = true;
		}

		/// <summary>
		/// Resumes all progress events.
		/// </summary>
		public void ResumeProgressEvents()
		{
			// Set the suspend progress event to false.
			this.suspendProgressEvents = false;
			// Raise the pending progress events.
			if (this.eventProgressLevelChanged || this.eventProgressDefaultChanged || this.eventProgressCountChanged)
			{
				// If the progress count has changed.
				if (this.eventProgressCountChanged)
				{
					// Update the items progress.
					this.OnUpdateItemsProgress();
					// Update the item measurements.
					this.OnUpdateGeometrics();
					// Set the pending event to false.
					this.eventProgressCountChanged = false;
				}
				// Refresh the list box.
				this.Refresh();
				// Set the pending events to false.
				this.eventProgressLevelChanged = false;
				this.eventProgressDefaultChanged = false;
			}
		}
		
		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			// Check whether this method must be called on a different thread.
			if (this.InvokeRequired) System.Diagnostics.Debugger.Break();
			// Dispose the control variables.
			if (disposing)
			{
				this.fontText.Dispose();
			}
			// Call the base class method.
			base.Dispose(disposing);
		}

		/// <summary>
		/// Draws the image list box item.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			// Call the base class method.
			base.OnDrawItem(e);

			// If the item index is outside the items range, do nothing.
			if ((e.Index < 0) || (e.Index >= this.Items.Count)) return;

			// Get the image list box item.
			ProgressItem item = this.Items[e.Index] as ProgressItem;

			// Update the progress item geometry.
			this.OnUpdateItemGeometrics(item, e.Bounds);

			Rectangle legendTextBounds = item.geometrics.legendTextBounds;
			Rectangle legendIconBounds = item.geometrics.legendIconBounds;

			// Create the brush.
			using (SolidBrush brush = new SolidBrush(e.BackColor))
			{
				// Create the pen.
				using (Pen pen = new Pen(this.colorProgressBorder))
				{
					// Fill the background rectangle.
					e.Graphics.FillRectangle(brush, e.Bounds);

					// If the item is enabled.
					if (item.Enabled)
					{
						// If the item progress is not null.
						if (null != item.Progress)
						{
							// Draw the progress rectangle.
							e.Graphics.DrawRectangle(pen, item.geometrics.progressBorder);

							// If the item legend is not null.
							if (null != item.Progress.Legend)
							{
								// Get the progress.
								ProgressInfo progress = item.Progress;
								// Get the legend.
								ProgressLegend legend = progress.Legend;

								// Draw the progress.

								// If the progress count is greater than zero.
								if (progress.Count > 0)
								{
									// If the legend is displayed.
									if (item.geometrics.showLegend)
									{
										// Draw the legend items, starting from the last.
										legendIconBounds.X = item.geometrics.legendBounds.Right;
										for (int index = legend.Items.Count - 1; index >= 0; index--)
										{
											// Set the legend icon bounds.
											legendIconBounds.X = legendIconBounds.X - item.geometrics.legendItemWidth;
											// Set the legend text bounds.
											legendTextBounds.X = legendIconBounds.Right + this.spacing;
											// Draw the legend icon.
											brush.Color = legend.Items[index].Color;
											e.Graphics.FillRectangle(brush, legendIconBounds);
											e.Graphics.DrawRectangle(pen, legendIconBounds);
											// Draw the legend text.
											TextRenderer.DrawText(
												e.Graphics,
												string.Format("{0} ({1})", legend.Items[index].Text, item.Progress[index]),
												this.Font,
												legendTextBounds,
												e.ForeColor,
												TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.Top);
										}
									}

									// Create the progress item bounds.
									Rectangle progressItemBounds = new Rectangle(
										item.geometrics.progressBounds.X,
										item.geometrics.progressBounds.Y,
										0,
										item.geometrics.progressBounds.Height);
									// For each progress item, except the last one.
									for (int index = 0; index < legend.Items.Count - 1; index++)
									{
										// Compute the progress bounds width.
										progressItemBounds.Width = progress[index] * item.geometrics.progressBounds.Width / progress.Count;
										// If the rectangle width is greater than zero.
										if (progressItemBounds.Width > 0)
										{
											// Change the brush color.
											brush.Color = legend.Items[index].Color;
											// Draw the progress item rectangle.
											e.Graphics.FillRectangle(brush, progressItemBounds);
											// Update the rectangle left position.
											progressItemBounds.X = progressItemBounds.Right;
										}
									}
									// Draw the last item.
									progressItemBounds.Width = item.geometrics.progressBounds.Right - progressItemBounds.X;
									// If the rectangle width is greater than zero.
									if (progressItemBounds.Width > 0)
									{
										// Change the brush color.
										brush.Color = legend.Items[legend.Items.Count - 1].Color;
										// Draw the progress item rectangle.
										e.Graphics.FillRectangle(brush, progressItemBounds);
										// Update the rectangle left position.
										progressItemBounds.X = progressItemBounds.Right;
									}
								}
								else
								{
									// Draw the not available text.
									TextRenderer.DrawText(
										e.Graphics,
										this.textNotAvailable,
										this.Font,
										item.geometrics.legendBounds,
										e.ForeColor,
										TextFormatFlags.EndEllipsis | TextFormatFlags.Right | TextFormatFlags.Top);
								}
							}
						}
					}
					else
					{
						// Draw the disabled text.
						TextRenderer.DrawText(
							e.Graphics,
							this.textDisabled,
							this.Font,
							item.geometrics.legendBounds,
							SystemColors.GrayText,
							TextFormatFlags.EndEllipsis | TextFormatFlags.Right | TextFormatFlags.Top);
					}
				}
			}

			// Get the item text.
			string text = item.Text != null ? item.Text : string.Empty;

			// Draw the text
			TextRenderer.DrawText(
				e.Graphics,
				text,
				this.fontText,
				item.geometrics.itemBounds,
				item.Enabled ? e.ForeColor : SystemColors.GrayText,
				TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.Top);
		}

		/// <summary>
		/// An event handler called when the control font has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnFontChanged(EventArgs e)
		{
			// Call the base class method.
			base.OnFontChanged(e);
			// Dispose the current font.
			if (this.fontText != null) this.fontText.Dispose();
			// Create a new font.
			this.fontText = new Font(this.Font, FontStyle.Bold);
			// Update the item text.
			this.OnUpdateItemsText();
			// Update the item progress.
			this.OnUpdateItemsProgress();
			// Update the item legend.
			this.OnUpdateItemsLegend();
			// Update the item measurements.
			this.OnUpdateGeometrics();
			// Refresh the list box.
			this.Refresh();
		}

		/// <summary>
		/// An event handler called when the control is being resized.
		/// </summary>
		/// <param name="e">The event argumeents.</param>
		protected override void OnResize(EventArgs e)
		{
			// Call the base class method.
			base.OnResize(e);
			// Workaround for .NET bug which duplicates the number of list box items when refreshing during a resize.
			//ProgressListBox.SendMessage(this.Handle, 0x0184 /*LB_RESETCONTENT*/, 0, 0);
			// Refresh the list box.
			this.Refresh();
		}

		// Private methods.

		/// <summary>
		/// An event handler called before the item collection has been cleared.
		/// </summary>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnBeforeItemsCleared(object sender, EventArgs e)
		{
			// Remove the event handlers for all items in the collection.
			foreach (ProgressItem item in this.items)
			{
				this.OnItemRemoved(item);
			}
			// Clear the base class items.
			base.Items.Clear();
		}

		/// <summary>
		/// An event handler called after an item has been inserted into the collection.
		/// </summary>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterItemInserted(object sender, ComponentCollection<ProgressItem>.ItemChangedEventArgs e)
		{
			// Add the event handlers to the new item.
			this.OnItemAdded(e.Item);
			// Insert the item into the base class items.
			base.Items.Insert(e.Index, e.Item);
		}

		/// <summary>
		/// An event handler called after an item has been removed from the collection. 
		/// </summary>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterItemRemoved(object sender, ComponentCollection<ProgressItem>.ItemChangedEventArgs e)
		{
			// Remove the event handlers from the old item.
			this.OnItemRemoved(e.Item);
			// Remove the item from the base class items.
			base.Items.RemoveAt(e.Index);
		}

		/// <summary>
		/// An event handler called after an item has been set in the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="object">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterItemSet(object sender, ComponentCollection<ProgressItem>.ItemSetEventArgs e)
		{
			// Add the event handlers to the new item.
			this.OnItemAdded(e.NewItem);
			// Remove the event handlers from the old item.
			this.OnItemRemoved(e.OldItem);
			// Set the item into the base class items.
			base.Items[e.Index] = e.NewItem;
		}

		/// <summary>
		/// An event handler called when an item has been added to the progress list box.
		/// </summary>
		/// <param name="item">The item.</param>
		private void OnItemAdded(ProgressItem item)
		{
			// If the item is null, do nothing.
			if (null == item) return;
			// Add the item event handlers.
			item.TextChanged += this.OnItemTextChanged;
			item.EnabledChanged += this.OnItemEnabledChanged;
			item.ProgressSet += this.OnItemProgressSet;
			item.ProgressLevelChanged += this.OnItemProgressLevelChanged;
			item.ProgressDefaultChanged += this.OnItemProgressDefaultChanged;
			item.ProgressCountChanged += this.OnItemProgressCountChanged;
			item.ProgressLegendSet += this.OnItemProgressLegendSet;
			item.ProgressLegendChanged += this.OnItemProgressLegendChanged;
			// Update the item text.
			this.OnUpdateItemText(item);
			// Update the items progress.
			this.OnUpdateItemsProgress();
			// Update the items legend.
			this.OnUpdateItemsLegend();
			// Update the item measurements.
			this.OnUpdateGeometrics();
			// Refresh the list box.
			this.Refresh();
		}

		/// <summary>
		/// An event handler called when an item has been removed from the progress list box.
		/// </summary>
		/// <param name="item">The item.</param>
		private void OnItemRemoved(ProgressItem item)
		{
			// If the item is null, do nothing.
			if (null == item) return;
			// Remove the item event handlers.
			item.TextChanged -= this.OnItemTextChanged;
			item.EnabledChanged -= this.OnItemEnabledChanged;
			item.ProgressSet -= this.OnItemProgressSet;
			item.ProgressLevelChanged -= this.OnItemProgressLevelChanged;
			item.ProgressDefaultChanged -= this.OnItemProgressDefaultChanged;
			item.ProgressCountChanged -= this.OnItemProgressCountChanged;
			item.ProgressLegendSet -= this.OnItemProgressLegendSet;
			item.ProgressLegendChanged -= this.OnItemProgressLegendChanged;
			// Update the items text.
			this.OnUpdateItemsText();
			// Update the items progress.
			this.OnUpdateItemsProgress();
			// Update the items legend.
			this.OnUpdateItemsLegend();
			// Update the item measurements.
			this.OnUpdateGeometrics();
			// Refresh the list box.
			this.Refresh();
		}

		/// <summary>
		/// An event handler called when the item text has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemTextChanged(object sender, ProgressItemEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Update the item text.
			this.OnUpdateItemText(e.Item);
			// Update the item measurements.
			this.OnUpdateGeometrics();
			// Update the geometrics of the specified item.
			this.OnUpdateItemGeometrics(e.Item, this.GetItemRectangle(this.Items.IndexOf(e.Item)));
			// Refresh the list box item.
			this.Invalidate(e.Item.geometrics.textBounds);
		}

		/// <summary>
		/// An event handler called when the item enabled state has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemEnabledChanged(object sender, ProgressItemEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Refresh the list box item.
			this.Invalidate(this.GetItemRectangle(this.Items.IndexOf(e.Item)));
		}

		/// <summary>
		/// An event handler called when the item progress has been set.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemProgressSet(object sender, ProgressItemInfoSetEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Update the item progress.
			this.OnUpdateItemProgress(e.Item);
			// Update the item legend.
			this.OnUpdateItemLegend(e.Item);
			// Update the item measurements.
			this.OnUpdateGeometrics();
			// Refresh the list box item.
			this.Invalidate(this.GetItemRectangle(this.Items.IndexOf(e.Item)));
		}

		/// <summary>
		/// An event handler called when the item progress level has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemProgressLevelChanged(object sender, ProgressItemEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// If the progress events are suspended.
			if (this.suspendProgressEvents)
			{
				// Set the pending progress level changed event to true.
				this.eventProgressLevelChanged = true;
			}
			else
			{
				// Update the geometrics of the specified item.
				this.OnUpdateItemGeometrics(e.Item, this.GetItemRectangle(this.Items.IndexOf(e.Item)));
				// Refresh the list box item.
				this.Invalidate(e.Item.geometrics.legendBounds);
				this.Invalidate(e.Item.geometrics.progressBounds);
			}
		}

		/// <summary>
		/// An event handler called when the item progress default has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemProgressDefaultChanged(object sender, ProgressItemEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// If the progress events are suspended.
			if (this.suspendProgressEvents)
			{
				// Set the pending progress default changed event to true.
				this.eventProgressDefaultChanged = true;
			}
			else
			{
				// Refresh the list box item.
				this.Invalidate(this.GetItemRectangle(this.Items.IndexOf(e.Item)));
			}
		}

		/// <summary>
		/// An event handler called when the item progress count has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemProgressCountChanged(object sender, ProgressItemEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// If the progress events are suspended.
			if (this.suspendProgressEvents)
			{
				// Set the pending progress count changed event to true.
				this.eventProgressCountChanged = true;
			}
			else
			{
				// Update the item progress.
				this.OnUpdateItemProgress(e.Item);
				// Update the item measurements.
				this.OnUpdateGeometrics();
				// Refresh the list box item.
				this.Invalidate(this.GetItemRectangle(this.Items.IndexOf(e.Item)));
			}
		}

		/// <summary>
		/// An event handler called when the item progress legend has been set.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemProgressLegendSet(object sender, ProgressItemLegendSetEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Update the item legend.
			this.OnUpdateItemLegend(e.Item);
			// Update the item measurements.
			this.OnUpdateGeometrics();
			// Refresh the list box item.
			this.Invalidate(this.GetItemRectangle(this.Items.IndexOf(e.Item)));
		}

		/// <summary>
		/// An event handler called when the item progress legend has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemProgressLegendChanged(object sender, ProgressItemLegendChangedEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Update the item legend.
			this.OnUpdateItemLegend(e.Item);
			// Update the item measurements.
			this.OnUpdateGeometrics();
			// Refresh the list box item.
			this.Invalidate(this.GetItemRectangle(this.Items.IndexOf(e.Item)));
		}

		/// <summary>
		/// An event handler called to update the maximum text width for the specified item.
		/// </summary>
		/// <param name="item">The progress item</param>
		private void OnUpdateItemText(ProgressItem item)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Recompute the text maximum width.
			int width = TextRenderer.MeasureText(item.Text, this.fontText).Width;
			// If the text width is greater than the previous maximum.
			if (width > this.itemMaximumTextWidth)
			{
				// Set the maximum text width.
				this.itemMaximumTextWidth = width;
			}
			else if (width < this.itemMaximumTextWidth)
			{
				// Else, update the text width for all items.
				this.OnUpdateItemsText();
			}
		}

		/// <summary>
		/// An event handler called to update the maximum text width for all items.
		/// </summary>
		private void OnUpdateItemsText()
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Set the maximum width to zero.
			this.itemMaximumTextWidth = 0;
			// Update the text width for all items.
			foreach (ProgressItem item in this.items)
			{
				// Get the item text width.
				int textWidth = TextRenderer.MeasureText(item.Text, this.fontText).Width;
				// Update the maximum width.
				if (this.itemMaximumTextWidth < textWidth)
				{
					this.itemMaximumTextWidth = textWidth;
				}
			}
		}

		/// <summary>
		/// An event handler called to update the maximum progress count for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		private void OnUpdateItemProgress(ProgressItem item)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Compute the count digits for this item.
			int digits = item.Progress != null ? (int)Math.Ceiling(Math.Log10(item.Progress.Count)) : 0;
			// If the number of digits is greater than the current maximum.
			if (digits > this.itemProgressMaximumCountDigits)
			{
				// Set the maximum number of digits.
				this.itemProgressMaximumCountDigits = digits;
			}
			else if (digits < this.itemProgressMaximumCountDigits)
			{
				// Else, update the count digits for all items.
				this.OnUpdateItemsProgress();
			}
		}

		/// <summary>
		/// An event handler called to update the maximum progress count for all items.
		/// </summary>
		private void OnUpdateItemsProgress()
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Set the maximum count digits to zero.
			this.itemProgressMaximumCountDigits = 0;
			// Update the maximum count digits for all items.
			foreach (ProgressItem item in this.items)
			{
				// Set the item legend as invalid.
				item.geometrics.validLegend = false;
				// Compute the item number of digits.
				int digits = item.Progress != null ? (int)Math.Ceiling(Math.Log10(item.Progress.Count)) : 0;
				// Update the maximum number of digits.
				if (digits > this.itemProgressMaximumCountDigits)
				{
					this.itemProgressMaximumCountDigits = digits;
				}
			}
		}

		/// <summary>
		/// An event handler called to update the maximum legend entries for the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		private void OnUpdateItemLegend(ProgressItem item)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Set the item legend as invalid.
			item.geometrics.validLegend = false;
			// Set the maximum number of legend items to zero.
			int count = 0;
			// Set the maximum text width to zero.
			int width = 0;

			// If the item has a progress info.
			if (null != item.Progress)
			{
				// If the item has a legend.
				if (null != item.Progress.Legend)
				{
					// Get the legend.
					ProgressLegend legend = item.Progress.Legend;

					// Set the legent items count.
					count = legend.Items.Count; 
					// Update the legend maximum items width.
					foreach (ProgressLegendItem legendItem in legend.Items)
					{
						// Compute the legend item width.
						int itemWidth = TextRenderer.MeasureText(legendItem.Text, this.Font).Width;
						// Get the maximum width.
						if (width < itemWidth)
						{
							width = itemWidth;
						}
					}
				}
			}

			// If the legend item count is greater, update the legend item count.
			if (count > this.itemProgressLegendMaximumCount)
			{
				this.itemProgressLegendMaximumCount = count;
			}

			// If the legend item width is greater, update the legend item width.
			if (width > this.itemProgressLegendMaximumTextWidth)
			{
				this.itemProgressLegendMaximumTextWidth = width;
			}

			// If either the legend item count or the legend item width is smaller, update all the legend for all items.
			if ((count < this.itemProgressLegendMaximumCount) || (width < this.itemProgressLegendMaximumTextWidth))
			{
				this.OnUpdateItemsLegend();
			}
		}

		/// <summary>
		/// An event handler called to update the maximum legend entries for all items.
		/// </summary>
		private void OnUpdateItemsLegend()
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Set the maximum legend item count to zero.
			this.itemProgressLegendMaximumCount = 0;
			// Set the maximum legend item width to zero.
			this.itemProgressLegendMaximumTextWidth = 0;
			
			// Update the legend for all items.
			foreach (ProgressItem item in this.items)
			{
				// Set the item legend as invalid.
				item.geometrics.validLegend = false;
				// If the item has a progress info.
				if (null != item.Progress)
				{
					// If the progress info has a legend.
					if (null != item.Progress.Legend)
					{
						// Get the legend.
						ProgressLegend legend = item.Progress.Legend;
						// Set the maximum legend items count.
						if (this.itemProgressLegendMaximumCount < legend.Items.Count)
						{
							this.itemProgressLegendMaximumCount = legend.Items.Count;
						}
						// For all the legend items.
						foreach (ProgressLegendItem legendItem in legend.Items)
						{
							// Compute the item width.
							int width = TextRenderer.MeasureText(legendItem.Text, this.Font).Width;
							// Set the maximum legend items width.
							if (this.itemProgressLegendMaximumTextWidth < width)
							{
								this.itemProgressLegendMaximumTextWidth = width;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// An event handler called when updating the item measurements.
		/// </summary>
		private void OnUpdateGeometrics()
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Compute the progress string for the progress count digits.
			StringBuilder stringBuilder = new StringBuilder(this.itemProgressMaximumCountDigits + 3);
			stringBuilder.Append(" (").Append('0', this.itemProgressMaximumCountDigits).Append(')');
			// Compute the text width for the progress count digits.
			this.itemProgressMaximumCountWidth = TextRenderer.MeasureText(stringBuilder.ToString(), this.Font).Width;
			// Compute the legend item maximum width (legend size + spacing + text width + count width + spacing).
			this.itemProgressLegendMaximumItemWidth = 
				this.legendSize.Width + 
				this.spacing +
				this.spacing +
				this.itemProgressLegendMaximumTextWidth +
				this.itemProgressMaximumCountWidth;
			// Compute the legend maximum width (legend item width x legend count).
			this.itemProgressLegendMaximumWidth = this.itemProgressLegendMaximumItemWidth * this.itemProgressLegendMaximumCount;
			// Compute the item maximum width (text + spacing + legend width).
			this.itemMaximumWidth = this.itemMaximumTextWidth + this.spacing + this.itemProgressLegendMaximumWidth;
			// Compute the item maximum variable width (text + legend text width x legend count).
			this.itemMaximumVariableWidth = (this.itemProgressLegendMaximumTextWidth + this.itemProgressMaximumCountWidth) * this.itemProgressLegendMaximumCount;
			// Compute the item maximum fixed width.
			this.itemMaximumFixedWidth = this.itemMaximumWidth - this.itemMaximumVariableWidth;
		}

		/// <summary>
		/// Updates the geometrics characteristics of the specified progress item.
		/// </summary>
		/// <param name="item">The progress item.</param>
		/// <param name="bounds">The item bounds.</param>
		private void OnUpdateItemGeometrics(ProgressItem item, Rectangle bounds)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;

			// Only updated the geometrics if the bounds are different.
			if (bounds != item.geometrics.bounds)
			{
				// Save the bounds.
				item.geometrics.bounds = bounds;

				// Compute the item bounds.
				item.geometrics.itemBounds = new Rectangle(
					item.geometrics.bounds.X + this.itemPadding.Left,
					item.geometrics.bounds.Y + this.itemPadding.Top,
					item.geometrics.bounds.Width - this.itemPadding.Left - this.itemPadding.Right - 1,
					item.geometrics.bounds.Height - this.itemPadding.Top - this.itemPadding.Bottom - 1);
				// Compute the progress border.
				item.geometrics.progressBorder = new Rectangle(
					item.geometrics.itemBounds.X,
					item.geometrics.itemBounds.Bottom - this.progressHeight,
					item.geometrics.itemBounds.Width,
					this.progressHeight);
				// Compute the progress bounds.
				item.geometrics.progressBounds = new Rectangle(
					item.geometrics.progressBorder.X + 1,
					item.geometrics.progressBorder.Y + 1,
					item.geometrics.progressBorder.Width - 1,
					item.geometrics.progressBorder.Height - 1);
				// Compute the content bounds.
				item.geometrics.contentBounds = new Rectangle(
					item.geometrics.itemBounds.X,
					item.geometrics.itemBounds.Y,
					item.geometrics.itemBounds.Width,
					item.geometrics.progressBorder.Top - item.geometrics.itemBounds.Y);
			}

			// Compute the text bounds.
			item.geometrics.textBounds = new Rectangle(
				item.geometrics.contentBounds.X,
				item.geometrics.contentBounds.Y,
				this.itemMaximumTextWidth,
				item.geometrics.contentBounds.Height);

			// If the legend is invalid, update the legend geometric characteristics.
			if (!item.geometrics.validLegend) this.OnUpdateItemLegendGeometrics(item);
		}

		/// <summary>
		/// Updates the legend geometrics characteristics for the specified progress item.
		/// </summary>
		/// <param name="item">The progress item.</param>
		private void OnUpdateItemLegendGeometrics(ProgressItem item)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;
			// Compute whether th item displays the legend.
			item.geometrics.showLegend = this.itemMaximumFixedWidth < item.geometrics.itemBounds.Width;
			// Compute the text width scale.
			double widthScale = item.geometrics.showLegend ?
				(this.itemMaximumWidth > item.geometrics.itemBounds.Width) ? (item.geometrics.itemBounds.Width - this.itemMaximumFixedWidth) / ((double)this.itemMaximumVariableWidth) : 1.0 :
				(this.itemMaximumTextWidth > item.geometrics.itemBounds.Width) ? widthScale = item.geometrics.itemBounds.Width / ((double)this.itemMaximumTextWidth) : 1.0;

			// The legend bounds.
			item.geometrics.legendBounds = new Rectangle(
				item.geometrics.textBounds.Right,
				item.geometrics.contentBounds.Y,
				item.geometrics.contentBounds.Width - item.geometrics.textBounds.Width,
				item.geometrics.contentBounds.Height);
			// The legend text bounds.
			item.geometrics.legendTextBounds = new Rectangle(
				0,
				item.geometrics.legendBounds.Y,
				(int)((this.itemProgressLegendMaximumTextWidth + this.itemProgressMaximumCountWidth) * widthScale),
				item.geometrics.legendBounds.Height);
			// The legend icon bounds.
			item.geometrics.legendIconBounds = new Rectangle(new Point(0, item.geometrics.legendBounds.Y), this.legendSize);
			// The legend item width.
			item.geometrics.legendItemWidth = item.geometrics.legendIconBounds.Width + this.spacing + item.geometrics.legendTextBounds.Width + this.spacing;

			// Set the legend as valid.
			item.geometrics.validLegend = true;
		}
	}
}
