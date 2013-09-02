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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DotNetApi.Async;
using DotNetApi.Drawing;
using MapApi;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A control that displays a geographic map.
	/// </summary>
	public sealed class MapControl : ThreadSafeControl, IAnchor
	{
		private delegate void RefreshEventHandler();

		private const int mapLevels = 4;

		private const string messageNoMap = "No map";
		private const string messageRefreshing = "Refreshing map...";

		private static readonly MapRectangle mapBoundsDefault = new MapRectangle(-180, 90, 180, -90);

		// Map.

		private Map map = null;

		private List<MapRegion> regions = new List<MapRegion>();

		private MapRectangle mapBounds = MapControl.mapBoundsDefault;
		private MapSize mapSize = MapControl.mapBoundsDefault.Size;
		private MapScale mapScale = new MapScale(1.0, 1.0);

		private Bitmap bitmapMap = null;
		private Size bitmapSize;

		private MapRegion highlightRegion = null;

		// Colors.

		private Color colorMessageBorder = Color.DarkGray;
		private Color colorMessageFill = Color.LightGray;

		private Color colorMapSea = Color.SkyBlue;
		private Color colorMapRegionBorder = Color.FromArgb(255, 255, 255);
		private Color colorMapRegion = Color.Green;
		private Color colorMapRegionHighlight = Color.YellowGreen;

		private Color colorGridMajor = Color.FromArgb(128, Color.Gray);
		private Color colorGridMinor = Color.FromArgb(48, Color.Gray);

		// Drawing.

		private Mutex mutex = new Mutex();

		private Bitmap bitmapBackground = new Bitmap(20, 20);
		private TextureBrush brushBackground;

		// Annotations.

		private MapTextAnnotation annotationMessage = null;
		private MapInfoAnnotation annotationRegion = null;

		private MapAnnotation[] annotations = null;

		// Switches.

		private bool showMarkers = true;
		private bool showMajorGrid = true;
		private bool showMinorGrid = true;
		private bool showStreched = true;

		// Asynchronous.
		private AsyncTask task = new AsyncTask();

		private RefreshEventHandler delegateRefresh;

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public MapControl()
		{
			// Set the default properties.
			this.DoubleBuffered = true;

			// Create the background bitmap.
			using (Graphics graphics = Graphics.FromImage(this.bitmapBackground))
			{
				using (SolidBrush brush = new SolidBrush(Color.LightGray))
				{
					// Draw the gray checker board.
					graphics.FillRectangle(brush, 0, 0, 10, 10);
					graphics.FillRectangle(brush, 10, 10, 20, 20);
					// Draw the white checker board.
					brush.Color = Color.White;
					graphics.FillRectangle(brush, 10, 0, 20, 10);
					graphics.FillRectangle(brush, 0, 10, 10, 20);
				}
			}

			// Create the map annotations.
			this.annotationMessage = new MapTextAnnotation(MapControl.messageNoMap, this);
			this.annotationRegion = new MapInfoAnnotation(string.Empty, this);

			this.annotations = new MapAnnotation[] {
				this.annotationMessage,
				this.annotationRegion
			};

			// Create the background brush.
			this.brushBackground = new TextureBrush(this.bitmapBackground);

			// Create the refresh delegate.
			this.delegateRefresh = new RefreshEventHandler(this.Refresh);
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the current message.
		/// </summary>
		[DefaultValue(MapControl.messageNoMap)]
		public string Message
		{
			get { return this.annotationMessage.Text; }
			set { this.OnMessageChanged(value); }
		}

		/// <summary>
		/// Gets or sets whether the message is visible.
		/// </summary>
		[DefaultValue(true)]
		public bool MessageVisible
		{
			get { return this.annotationMessage.Visible; }
			set { this.OnMessageVisibleChanged(value); }
		}

		/// <summary>
		/// Gets or sets the current map.
		/// </summary>
		[DefaultValue(null)]
		public Map Map
		{
			get { return this.map; }
			set { this.OnMapChanged(value); }
		}

		/// <summary>
		/// Gets or sets the map bounds.
		/// </summary>
		public MapRectangle MapBounds
		{
			get { return this.mapBounds; }
			set { this.OnMapBoundsChanged(value); }
		}

		/// <summary>
		/// Gets the anchor bounds.
		/// </summary>
		public Rectangle AnchorBounds
		{
			get { return this.ClientRectangle; }
		}


		// Protected methods.

		/// <summary>
		/// Invalidates the client area of the current control.
		/// </summary>
		public override void Refresh()
		{
			if (this.InvokeRequired) this.Invoke(this.delegateRefresh);
			else
			{
				base.Refresh();
			}
		}

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			// Call the base class dispose method.
			base.Dispose(disposing);
			// If the object is being disposed.
			if (disposing)
			{
				// Dispose the brushes.
				this.brushBackground.Dispose();

				// Dispose the bitmaps.
				if (null != this.bitmapMap)
				{
					this.bitmapMap.Dispose();
				}
				this.bitmapBackground.Dispose();

				// Dispose the annotations.
				this.annotationMessage.Dispose();
				this.annotationRegion.Dispose();

				// Dispose the asynchronous task.
				this.task.Dispose();

				// Dispose the drawing mutex.
				this.mutex.Dispose();

				// Dispose the map regions.
				foreach (MapRegion region in this.regions)
				{
					region.Dispose();
				}
			}
		}

		/// <summary>
		/// An event handler called when the control is being repainted.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;

			// Set the graphics smoothing mode to high quality.
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

			// Try and lock the drawing mutex.
			if (this.mutex.WaitOne(0))
			{
				try
				{
					// If the current map bitmap is null.
					if (null != this.bitmapMap)
					{
						// Draw the map bitmap.
						e.Graphics.DrawImage(this.bitmapMap, new Point(0, 0));
						// Draw any highlighted region.
						if (null != this.highlightRegion)
						{
							// Create a new pen.
							using (Pen pen = new Pen(this.colorMapRegionBorder))
							{
								// Create a new brush.
								using (SolidBrush brush = new SolidBrush(this.colorMapRegionHighlight))
								{

									// Draw the highlighted region.
									this.highlightRegion.Draw(e.Graphics, brush, pen);
								}
							}
						}
					}
					else
					{
						// Draw the checkerboard background.
						e.Graphics.FillRectangle(this.brushBackground, this.ClientRectangle);
					}
				}
				finally
				{
					// Unlock the mutex.
					this.mutex.ReleaseMutex();
				}
			}
			else
			{
				// Draw the checkerboard background.
				e.Graphics.FillRectangle(this.brushBackground, this.ClientRectangle);
			}

			// Draw the map annotations.
			foreach (MapAnnotation annotation in this.annotations)
			{
				annotation.Draw(e.Graphics);
			}

			// Call the base class event handler.
			base.OnPaint(e);
		}

		/// <summary>
		/// An event handler called when the control is being resized.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnResize(EventArgs e)
		{
			// Call the base class event handler.
			base.OnResize(e);
			// Call the size changed event handler.
			this.OnMapSizeChanged();
			// Call the message bounds changed .
			this.OnAnnotationBoundsChanged();
			// Call the refresh map event handler.
			this.OnRefreshMap();
		}

		/// <summary>
		/// An event handler called when the mouse moves over the control.
		/// </summary>
		/// <param name="e">The </param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Call the base class event handler.
			base.OnMouseMove(e);
			// The current highlighted region.
			MapRegion highlightRegion = null;
			// Compute the new highlight region.
			foreach (MapRegion region in this.regions)
			{
				// If the region contains the mouse location.
				if (region.IsVisible(e.Location))
				{
					// Set the current highlighted region.
					highlightRegion = region;
					// Stop the iteration.
					break;
				}
			}
			// If the highlighted region has changed.
			if (this.highlightRegion != highlightRegion)
			{
				// If there exists a previous highlighted region.
				if (null != this.highlightRegion)
				{
					// Invalidate the bounds of that region.
					this.Invalidate(this.highlightRegion.Bounds);
				}
				// If there exists a curernt highlighted region.
				if (null != highlightRegion)
				{
					// Invalidate the bounds of that region.
					this.Invalidate(highlightRegion.Bounds);
					// Show the annotation.
					this.OnShowAnnotation(this.annotationRegion, highlightRegion.Name, highlightRegion, HorizontalAlign.Center, VerticalAlign.TopOutside);
				}
				else
				{
					// Hide the annotation.
					this.OnHideAnnotation(this.annotationRegion);
				}
				// Set the new highlighted region.
				this.highlightRegion = highlightRegion;
			}
		}

		/// <summary>
		/// An event handler called when the mouse leaves the control.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			// Call the base class methods.
			base.OnMouseLeave(e);
			// If there exists a highlighted region.
			if (null != this.highlightRegion)
			{
				// Invalidate the bounds of that region.
				this.Invalidate(this.highlightRegion.Bounds);
				// Set the highlighted region to null.
				this.highlightRegion = null;
			}
		}

		// Private methods.

		/// <summary>
		/// Sets the current map.
		/// </summary>
		/// <param name="map">The map.</param>
		private void OnMapChanged(Map map)
		{
			// If the map has not changed, do nothing.
			if (this.map == map) return;

			// Set the current map.
			this.map = map;
			
			// Clear the existing map regions.
			foreach (MapRegion region in this.regions)
			{
				region.Dispose();
			}
			this.regions.Clear();

			// If the current map is not null.
			if (null != this.map)
			{
				// Create the map shapes.
				foreach (MapShape shape in map.Shapes)
				{
					// Switch according to the shape type.
					switch (shape.Type)
					{
						case MapShapeType.Polygon:
							// Get the polygon shape.
							MapShapePolygon shapePolygon = shape as MapShapePolygon;
							// Create a map region for this shape.
							MapRegion region = new MapRegion(shapePolygon, this.mapBounds, this.mapScale);
							// Add the map region to the region items.
							this.regions.Add(region);
							break;
						default: continue;
					}
				}
			}
			// Refresh the current map.
			this.OnRefreshMap();
		}

		/// <summary>
		/// An event handler called when the map bounds have changed.
		/// </summary>
		/// <param name="mapBounds">The map bounds.</param>
		private void OnMapBoundsChanged(MapRectangle mapBounds)
		{
			// Compute the map size to use the default values in case the width and height are zero.
			this.mapSize = new MapSize(
				mapBounds.Width != 0 ? mapBounds.Width : MapControl.mapBoundsDefault.Width,
				mapBounds.Height != 0 ? mapBounds.Height : MapControl.mapBoundsDefault.Height
				);
			// Compute the map bounds.
			this.mapBounds = new MapRectangle(
				new MapPoint(mapBounds.Left, mapBounds.Top),
				this.mapSize
				);
			// Recompute the map scale and bitmap size.
			this.OnMapSizeChanged();
		}

		/// <summary>
		/// An event handler called when the map size has changed.
		/// </summary>
		private void OnMapSizeChanged()
		{
			// Compute the map scale.
			this.mapScale = new MapScale(
				this.ClientSize.Width / mapBounds.Width,
				this.ClientSize.Height / mapBounds.Height
				);
			// Compute the bitmap size.
			this.bitmapSize = this.ClientSize;
			// If the map is not streched.
			if (!this.showStreched)
			{
				// If the width scale is greater.
				if (this.mapScale.Width > this.mapScale.Height)
				{
					// Align along the width.
					this.mapScale.Height = this.mapScale.Width;
					this.bitmapSize.Height = (int)Math.Round(this.mapBounds.Height * this.mapScale.Height);
				}
				else
				{
					// Align along the height.
					this.mapScale.Width = this.mapScale.Height;
					this.bitmapSize.Width = (int)Math.Round(this.mapBounds.Width * this.mapScale.Width);
				}
			}
			// Update the map items.
			this.OnUpdateItems();
		}

		/// <summary>
		/// Shows the specified annotation.
		/// </summary>
		/// <param name="annotation">The text annotation</param>
		/// <param name="text">The message text.</param>
		/// <param name="anchor">The message anchor.</param>
		/// <param name="horizontalAlignment">The horizontal alignment.</param>
		/// <param name="verticalAlignment">The vertical alignment.</param>
		private void OnShowAnnotation(MapTextAnnotation annotation, string text, IAnchor anchor, HorizontalAlign horizontalAlignment, VerticalAlign verticalAlignment)
		{
			// Get the old message bounds.
			Rectangle oldBounds = annotation.Bounds;
			
			// Suspend the message layout.
			annotation.SuspendLayout();
			// Set the new message properties.
			annotation.Text = text;
			annotation.Anchor = anchor;
			annotation.HorizontalAlignment = horizontalAlignment;
			annotation.VerticalAlignment = verticalAlignment;
			// Result the message layout.
			annotation.ResumeLayout();
			
			// Get the new message bounds.
			Rectangle newBounds = annotation.Bounds;
			// If the message is currently visible.
			if (annotation.Visible)
			{
				// Invalidate the area corresponding to the old message bounds.
				this.Invalidate(oldBounds);
			}
			else
			{
				// Set the message visibility to true.
				annotation.Visible = true;
			}
			// Invalidated the area corresponding to the new message bounds.
			this.Invalidate(newBounds);
		}

		/// <summary>
		/// Hides the specified text annotation.
		/// </summary>
		/// <param name="annotation">The annotation.</param>
		private void OnHideAnnotation(MapTextAnnotation annotation)
		{
			// If there exists a visible message.
			if (annotation.Visible)
			{
				// Invalidate the area corresponding to the message bounds.
				this.Invalidate(annotation.Bounds);
			}
			// Set the message visibility to false.
			annotation.Visible = false;
		}

		/// <summary>
		/// Sets the specified message on the control.
		/// </summary>
		/// <param name="text">The new message text.</param>
		private void OnMessageChanged(string text)
		{
			// Get the old message bounds.
			Rectangle oldBounds = this.annotationMessage.Bounds;
			// Set the new message text.
			this.annotationMessage.Text = text;
			// Get the new message bounds.
			Rectangle newBounds = this.annotationMessage.Bounds;
			// If the bounds have changed.
			if (oldBounds != newBounds)
			{
				// Invalidate the area corresponding to the old bounds.
				this.Invalidate(oldBounds);
				// Invalidate the area corresponding to the new bounds.
				this.Invalidate(newBounds);
			}
			else
			{
				// Invalidate the area corresponding to the new bounds.
				this.Invalidate(newBounds);
			}
		}

		/// <summary>
		/// Set the message visibility.
		/// </summary>
		/// <param name="visible"><b>True</b> if the message is visible or <b>false</b> otherwise.</param>
		private void OnMessageVisibleChanged(bool visible)
		{
			// Set the new message visibility.
			this.annotationMessage.Visible = visible;
			// Invalidate the area corresponding to the message bounds.
			this.Invalidate(this.annotationMessage.Bounds);
		}

		/// <summary>
		/// An event handler called when the annotation bounds have changed.
		/// </summary>
		private void OnAnnotationBoundsChanged()
		{
			// For all map annotations.
			foreach (MapAnnotation annotation in this.annotations)
			{
				// Get the old annotation bounds.
				Rectangle oldBounds = annotation.Bounds;
				// Refresh the annotation.
				annotation.Refresh();
				// Get the new annotation bounds.
				Rectangle newBounds = annotation.Bounds;
				// If the bounds have changed.
				if (oldBounds != newBounds)
				{
					// Invalidate the area corresponding to the old bounds.
					this.Invalidate(oldBounds);
					// Invalidate the area corresponding to the new bounds.
					this.Invalidate(newBounds);
				}
			}
		}

		/// <summary>
		/// Updates all the map items to the current map bounds and scale.
		/// </summary>
		private void OnUpdateItems()
		{
			// Update all map regions.
			foreach (MapRegion region in this.regions)
			{
				region.Update(this.mapBounds, this.mapScale);
			}
		}

		/// <summary>
		/// Refreshes the current map.
		/// </summary>
		private void OnRefreshMap()
		{
			// If the map is not null.
			if (null != this.map)
			{
				// Create a new bitmap and draw the bitmap on an asynchronous task.
				this.task.ExecuteAlways((AsyncState asyncState) =>
				{
					// Lock the drawing mutex.
					this.mutex.WaitOne();
					try
					{
						// If the current bitmap is not null, dispose the current bitmap.
						if (null != this.bitmapMap)
						{
							this.bitmapMap.Dispose();
							this.bitmapMap = null;
						}
						// Create a new bitmap corresponding to the current map.
						this.bitmapMap = this.OnDrawMap(this.ClientSize, asyncState);
						// Return if the asynchronous operation has been canceled.
						if (asyncState.IsCanceled) return;
						// Hide the message.
						this.annotationMessage.Visible = false;
					}
					finally
					{
						// Unlock the mutex.
						this.mutex.ReleaseMutex();
					}
					// Refresh the control.
					this.Refresh();
				});

				// Set a new message.
				this.annotationMessage.Text = MapControl.messageRefreshing;
				this.annotationMessage.Visible = true;
			}
			else
			{
				// Set a new message.
				this.annotationMessage.Text = MapControl.messageNoMap;
				this.annotationMessage.Visible = true;
			}

			// Refresh the control.
			this.Refresh();
		}

		/// <summary>
		/// Creates a new bitmap for the current map, scaled at the specified size. The scaled version of the map fits the largest dimension between width and height.
		/// </summary>
		/// <param name="size">The bitmap minimum size.</param>
		/// <param name="asyncState">The asynchrnous state.</param>
		/// <returns>The map bitmap.</returns>
		private Bitmap OnDrawMap(Size size, AsyncState asyncState)
		{
			// Save the map object in a local variable.
			Map map = this.map;

			// Acquire an exclusive access to the map.
			lock (map)
			{
				// Create a new bitmap.
				Bitmap bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);

				// Draw the bitmap.
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					// Set the smooting mode.
					graphics.SmoothingMode = SmoothingMode.HighQuality;
					using (SolidBrush brush = new SolidBrush(this.colorMapSea))
					{
						using (Pen pen = new Pen(this.colorMapRegionBorder))
						{
							// Fill the background.
							graphics.FillRectangle(brush, 0, 0, bitmapSize.Width, bitmapSize.Height);

							// Change the brush color.
							brush.Color = this.colorMapRegion;
							// Draw the map regions.
							foreach (MapRegion region in this.regions)
							{
								// If the asynchronous operation has been canceled.
								if (asyncState.IsCanceled)
								{
									// Dispose the bitmap.
									bitmap.Dispose();
									// Return null.
									return null;
								}

								// Draw the region.
								region.Draw(graphics, brush, pen);
							}
						}
					}
				}

				// Return the bitmap.
				return bitmap;
			}
		}
	}
}
