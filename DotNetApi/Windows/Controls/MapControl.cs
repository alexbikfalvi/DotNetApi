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
using DotNetApi.Drawing.Temporal;
using DotNetApi.Drawing.Transforms;
using MapApi;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A control that displays a geographic map.
	/// </summary>
	public sealed class MapControl : ThreadSafeControl, IAnchor, ITranslation
	{
		private delegate void RefreshEventHandler();
		private delegate void MessageEventHandler(string text);

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
		private MapPoint mapLocation = new MapPoint(-180, 90);

		private Bitmap bitmapMap = null;
		private Point bitmapLocation;
		private Size bitmapSize;

		//private MapSize majorGridSize;
		//private Size minorGridSize;

		private MapRegion highlightRegion = null;
		private MapMarker highlightMarker = null;
		private MapMarker emphasizedMarker = null;

		// Colors.

		private Color colorMessageBorder = Color.DarkGray;
		private Color colorMessageFill = Color.LightGray;

		private Color colorBackground = Color.SkyBlue;
		
		private Color colorRegionNormalBorder = Color.White;
		private Color colorRegionNormalBackground = Color.Green;
		private Color colorRegionHighlightBorder = Color.White;
		private Color colorRegionHighlightBackground = Color.YellowGreen;

		private Color colorMarkerNormalBorder = Color.DarkOrange;
		private Color colorMarkerNormalBackground = Color.Yellow;
		private Color colorMarkerEmphasisBorder = Color.DarkRed;
		private Color colorMarkerEmphasisBackground = Color.Red;
		private Color colorMarkerHighlightBorder = Color.DarkViolet;
		private Color colorMarkerHighlightBackground = Color.Violet;

		private Color colorGridMajor = Color.FromArgb(128, Color.Gray);
		private Color colorGridMinor = Color.FromArgb(48, Color.Gray);

		// Drawing.

		private Mutex mutex = new Mutex();

		private Bitmap bitmapBackground = new Bitmap(20, 20);
		private TextureBrush brushBackground;

		private Shadow shadow = new Shadow(Color.Black, 0, 20);

		// Interaction.

		private MotionSpring scrollSpring = new MotionSpring();
		private TransformAsymptotic scrollTransform = new TransformAsymptotic(100, 100);

		private bool mouseGripFlag = false;
		private Point mouseGripLocation;

		// Annotations.

		private MapTextAnnotation annotationMessage = null;
		private MapInfoAnnotation annotationInfo = null;

		private MapAnnotation[] annotations;

		// Markers.

		private MapMarkerCollection markers = new MapMarkerCollection();
		private bool markerAutoDispose = true;

		// Switches.

		private bool showStreched = true;
		private bool showBorders = true;
		private bool showMarkers = true;
		private bool showMajorGrid = true;
		private bool showMinorGrid = true;

		// Asynchronous.
		private AsyncTask task = new AsyncTask();

		private RefreshEventHandler delegateRefresh;
		private MessageEventHandler delegateMessage;

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
			this.annotationMessage = new MapTextAnnotation(MapControl.messageNoMap, this, null);
			this.annotationInfo = new MapInfoAnnotation(null, this, this);

			this.annotations = new MapAnnotation[] {
				this.annotationMessage,
				this.annotationInfo
			};

			// Create the background brush.
			this.brushBackground = new TextureBrush(this.bitmapBackground);

			// Create the delegates.
			this.delegateRefresh = new RefreshEventHandler(this.Refresh);
			this.delegateMessage = new MessageEventHandler(this.OnMessageChanged);

			// Create the spring motion event handler.
			this.scrollSpring.Tick += this.OnSpringTick;

			// Create the marker collection event handlers.
			this.markers.BeforeCleared += this.OnBeforeMarkersCleared;
			this.markers.AfterItemInserted += this.OnAfterMarkerInserted;
			this.markers.AfterItemRemoved += this.OnAfterMarkerRemoved;
			this.markers.AfterItemSet += this.OnAfterMarkerSet;
		}

		// Public events.

		public event EventHandler MarkerClick;
		public event EventHandler MarkerDoubleClick;

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
		/// Gets or sets the show borders flag.
		/// </summary>
		[DefaultValue(true)]
		public bool ShowBorders
		{
			get { return this.showBorders; }
			set { this.OnShowBordersChanged(value); }
		}

		/// <summary>
		/// Gets or sets the show markers flag.
		/// </summary>
		[DefaultValue(true)]
		public bool ShowMarkers
		{
			get { return showMarkers; }
			set { this.OnShowMarkersChanged(value); }
		}

		/// <summary>
		/// Gets the anchor bounds.
		/// </summary>
		public Rectangle AnchorBounds
		{
			get { return this.ClientRectangle; }
		}

		/// <summary>
		/// Gets the translation delta.
		/// </summary>
		public Point TranslationDelta
		{
			get { return this.bitmapLocation; }
		}

		/// <summary>
		/// Gets the map markers.
		/// </summary>
		public MapMarkerCollection Markers
		{
			get { return this.markers; }
		}

		/// <summary>
		/// Gets or sets whether the markers are automatically disposed when removed from the map.
		/// </summary>
		[DefaultValue(true)]
		public bool MarkersAutoDispose
		{
			get { return this.markerAutoDispose; }
			set { this.markerAutoDispose = value; }
		}

		[Browsable(false)]
		public MapMarker HighlightedMarker
		{
			get { return this.highlightMarker; }
		}

		// Public methods.

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
		/// Loads the map with the specified name from the current resource file.
		/// </summary>
		/// <param name="data">The map name.</param>
		public void LoadMap(string name)
		{
			// Display a loading message.
			this.OnMessageChanged("Loading...");
			// Load the map data on the thread pool.
			ThreadPool.QueueUserWorkItem((object state) =>
				{
					try
					{
						// Get the map.
						this.Map = Maps.Get(name);
					}
					catch (Exception)
					{
						// Display a message.
						this.OnMessageChanged("Loading map failed.");
					}
				});
		}

		// Protected methods.

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
				foreach (MapAnnotation annotation in this.annotations)
				{
					annotation.Dispose();
				}

				// Dispose the shadow.
				this.shadow.Dispose();

				// Dispose the motion spring.
				this.scrollSpring.Dispose();

				// Dispose the asynchronous task.
				this.task.Dispose();

				// Dispose the drawing mutex.
				this.mutex.Dispose();

				// Dispose the map regions.
				foreach (MapRegion region in this.regions)
				{
					region.Dispose();
				}

				// Dispose tha map markers.
				if (this.markerAutoDispose)
				{
					foreach (MapMarker marker in this.markers)
					{
						marker.Dispose();
					}
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
					// Draw the checkerboard background.
					e.Graphics.FillRectangle(this.brushBackground, this.ClientRectangle);
					// If the current map bitmap is null.
					if (null != this.bitmapMap)
					{
						// Translate the graphics for the map location.
						e.Graphics.TranslateTransform(this.bitmapLocation.X, this.bitmapLocation.Y);

						// Create a new pen.
						using (Pen pen = new Pen(this.colorRegionHighlightBorder))
						{
							// Create a new brush.
							using (SolidBrush brush = new SolidBrush(this.colorRegionHighlightBackground))
							{
								// Draw the map shadow.
								e.Graphics.DrawShadow(this.shadow, new Rectangle(new Point(0, 0), this.bitmapSize));
								// Draw the map bitmap.
								e.Graphics.DrawImage(this.bitmapMap, new Point(0, 0));
								// Draw any highlighted region.
								if (null != this.highlightRegion)
								{
									// Draw the highlighted region.
									this.highlightRegion.Draw(e.Graphics, brush, pen);
								}

								// Draw the major grid.

								// If the show markers flag is set.
								if (this.showMarkers)
								{
									// Change the pen and brush colors.
									pen.Color = this.colorMarkerNormalBorder;
									brush.Color = this.colorMarkerNormalBackground;

									// Draw the normal markers.
									foreach (MapMarker marker in this.markers)
									{
										if (!marker.Emphasized)
										{
											marker.Draw(e.Graphics, brush, pen);
										}
									}

									// Change the pen and brush colors.
									pen.Color = this.colorMarkerEmphasisBorder;
									brush.Color = this.colorMarkerEmphasisBackground;
									
									// Draw the emphasized markers.
									foreach (MapMarker marker in this.markers)
									{
										if (marker.Emphasized)
										{
											marker.Draw(e.Graphics, brush, pen);
										}
									}

									// Draw the highlighed marker.
									if ((null != this.highlightMarker) && (this.emphasizedMarker != this.highlightMarker))
									{
										pen.Color = this.colorMarkerHighlightBorder;
										brush.Color = this.colorMarkerHighlightBackground;

										this.highlightMarker.Draw(e.Graphics, brush, pen);
									}
								}								

								// Draw the minor horizontal grid.
								//for (double x = 0; )
								//{
								//	e.Graphics.DrawLine(pen, this.Convert(new MapPoint(x, this.mapBounds.Top)), this.Convert(new MapPoint(x, this.mapBounds.Bottom)));
								//}
							}
						}

						// Translate the graphics for the map location.
						e.Graphics.TranslateTransform(-this.bitmapLocation.X, -this.bitmapLocation.Y);
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

			// The current highlighted marker.
			MapMarker highlightMarker = null;
			// The current highlighted region.
			MapRegion highlightRegion = null;

			// If the mouse grip is set.
			if (this.mouseGripFlag)
			{
				// Compute the bitmap location based on the difference between the current mouse location and the grip location.
				Point bitmapLocation = this.mouseGripLocation.Add(this.scrollTransform.GetRelative(e.Location));
				// If the location is different from the current location.
				if (bitmapLocation != this.bitmapLocation)
				{
					// Invalidate the map rectangle for the old location.
					this.Invalidate(this.shadow.GetShadowRectangle(new Rectangle(this.bitmapLocation, this.bitmapSize)));
					// Set the bitmap location at the new location.
					this.bitmapLocation = bitmapLocation;
					// Invalidate the map rectangle for the new location.
					this.Invalidate(this.shadow.GetShadowRectangle(new Rectangle(this.bitmapLocation, this.bitmapSize)));
					// Refresh the text annotation.
					this.OnRefreshAnnotation(this.annotationInfo);
				}
			}

			// Compute the mouse location.
			Point location = e.Location.Subtract(this.bitmapLocation);
			// Compute the new highlight marker.
			foreach (MapMarker marker in this.markers)
			{
				// If the marker contains the mouse location.
				if (marker.IsVisible(location))
				{
					// Set the current highlighted marker.
					highlightMarker = marker;
					// Stop the iteration.
					break;
				}
			}
			// Compute the new highlight region.
			foreach (MapRegion region in this.regions)
			{
				// If the region contains the mouse location.
				if (region.IsVisible(location))
				{
					// Set the current highlighted region.
					highlightRegion = region;
					// Stop the iteration.
					break;
				}
			}

			// If the highlighted marker has changed.
			if (this.highlightMarker != highlightMarker)
			{
				// If there exists a previous highlighted marker.
				if (null != this.highlightMarker)
				{
					// Invalidate the bounds of that marker.
					this.Invalidate(this.highlightMarker.Bounds.Add(this.bitmapLocation));
				}
				// If there exists a current highlighted marker.
				if ((null != highlightMarker) && this.showMarkers)
				{
					// Invalidate the bounds of that marker.
					this.Invalidate(highlightMarker.Bounds.Add(this.bitmapLocation));
					// Show the info annotation.
					this.OnShowAnnotation(this.annotationInfo, highlightMarker.Name, highlightMarker);
				}
				else
				{
					// If there is an emphasized marker.
					if ((null != this.emphasizedMarker) && this.showMarkers)
					{
						// Show the info annotation.
						this.OnShowAnnotation(this.annotationInfo, this.emphasizedMarker.Name, this.emphasizedMarker);
					}
					// Else, if there is a highlighted region.
					else if (null != highlightRegion)
					{
						// Show the info annotation.
						this.OnShowAnnotation(this.annotationInfo, highlightRegion.Name, highlightRegion);
					}
					// Else.
					else
					{
						// Hide the info annotation.
						this.OnHideAnnotation(this.annotationInfo);
					}
				}
				// Set the new highlighted marker.
				this.highlightMarker = highlightMarker;
			}

			// If the highlighted region has changed.
			if (this.highlightRegion != highlightRegion)
			{
				// If there exists a previous highlighted region.
				if (null != this.highlightRegion)
				{
					// Invalidate the bounds of that region.
					this.Invalidate(this.highlightRegion.Bounds.Add(this.bitmapLocation));
				}
				// If there exists a current highlighted region.
				if (null != highlightRegion)
				{
					// Invalidate the bounds of that region.
					this.Invalidate(highlightRegion.Bounds.Add(this.bitmapLocation));
					// If there is no highlighted marker and no emphasized marker or if the markers are hidden.
					if (((null == this.highlightMarker) && (null == this.emphasizedMarker)) || !this.showMarkers)
					{
						// Show the info annotation.
						this.OnShowAnnotation(this.annotationInfo, highlightRegion.Name, highlightRegion);
					}
				}
				else
				{
					// If there is no highlighted marker and no emphasized marker.
					if (((null == this.highlightMarker) && (null == this.emphasizedMarker)) || !this.showMarkers)
					{
						// Hide the info annotation.
						this.OnHideAnnotation(this.annotationInfo);
					}
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
			// If there exists a highlighted marker.
			if (null != this.highlightMarker)
			{
				// Invalidate the bounds of that region.
				this.Invalidate(this.highlightMarker.Bounds.Add(this.bitmapLocation));
				// Set the highlighted marker to null.
				this.highlightMarker = null;
			}
			// If there exists a highlighted region.
			if (null != this.highlightRegion)
			{
				// Invalidate the bounds of that region.
				this.Invalidate(this.highlightRegion.Bounds.Add(this.bitmapLocation));
				// Set the highlighted region to null.
				this.highlightRegion = null;
			}
			// If there exists an emphasized marker.
			if ((null != this.emphasizedMarker) && this.showMarkers)
			{
				// Show the info annotation.
				this.OnShowAnnotation(this.annotationInfo, this.emphasizedMarker.Name, this.emphasizedMarker);
			}
			else
			{
				// Hide the info annotation.
				this.OnHideAnnotation(this.annotationInfo);
			}
		}

		/// <summary>
		/// An event handler called when the mouse button is pressed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Call the base class method.
			base.OnMouseDown(e);
			// Cancel the spring motion.
			this.scrollSpring.Cancel();

			// If there is no highlighted marker.
			if (null == this.highlightMarker)
			{
				// Set the mouse cursor to grip.
				this.Cursor = Cursors.HandGrab;
				// Set the mouse grip variables.
				this.mouseGripFlag = true;
				this.mouseGripLocation = this.bitmapLocation;
				this.scrollTransform.Anchor = e.Location;
				// Call the mouse move event handler.
				this.OnMouseMove(e);
			}
		}

		/// <summary>
		/// An event handler called when the mouse button is released.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Call the base class method.
			base.OnMouseUp(e);

			// If the grip mouse flag was set.
			if (this.mouseGripFlag)
			{
				// Set the mouse cursor to default.
				this.Cursor = System.Windows.Forms.Cursors.Default;
				// Compute the map rectangle.
				Rectangle mapRectangle = new Rectangle(this.bitmapLocation, this.bitmapSize);
				// If the map rectangle does not fill the client rectangle.
				if ((mapRectangle.Left > this.ClientRectangle.Left) && (mapRectangle.Top > this.ClientRectangle.Top))
				{
					// Align the top-left corner.
					this.scrollSpring.Start(this.bitmapLocation, this.ClientRectangle.Location);
				}
				else if ((mapRectangle.Right < this.ClientRectangle.Right) && (mapRectangle.Top > this.ClientRectangle.Top))
				{
					// Align the top-right corner.
					this.scrollSpring.Start(this.bitmapLocation, this.bitmapLocation.Add(this.ClientRectangle.Right - mapRectangle.Right, this.ClientRectangle.Top - mapRectangle.Top));
				}
				else if ((mapRectangle.Left > this.ClientRectangle.Left) && (mapRectangle.Bottom < this.ClientRectangle.Bottom))
				{
					// Align the bottom-left corner.
					this.scrollSpring.Start(this.bitmapLocation, this.bitmapLocation.Add(this.ClientRectangle.Left - mapRectangle.Left, this.ClientRectangle.Bottom - mapRectangle.Bottom));
				}
				else if ((mapRectangle.Right < this.ClientRectangle.Right) && (mapRectangle.Bottom < this.ClientRectangle.Bottom))
				{
					// Align the bottom-right corner.
					this.scrollSpring.Start(this.bitmapLocation, this.bitmapLocation.Add(this.ClientRectangle.Right - mapRectangle.Right, this.ClientRectangle.Bottom - mapRectangle.Bottom));
				}
				else if (mapRectangle.Left > this.ClientRectangle.Left)
				{
					// Align the left edge.
					this.scrollSpring.Start(this.bitmapLocation, this.bitmapLocation.Add(this.ClientRectangle.Left - mapRectangle.Left, 0));
				}
				else if (mapRectangle.Top > this.ClientRectangle.Top)
				{
					// Align the top edge.
					this.scrollSpring.Start(this.bitmapLocation, this.bitmapLocation.Add(0, this.ClientRectangle.Top - mapRectangle.Top));
				}
				else if (mapRectangle.Right < this.ClientRectangle.Right)
				{
					// Align the right edge.
					this.scrollSpring.Start(this.bitmapLocation, this.bitmapLocation.Add(this.ClientRectangle.Right - mapRectangle.Right, 0));
				}
				else if (mapRectangle.Bottom < this.ClientRectangle.Bottom)
				{
					// Align the bottom edge.
					this.scrollSpring.Start(this.bitmapLocation, this.bitmapLocation.Add(0, this.ClientRectangle.Bottom - mapRectangle.Bottom));
				}
				// Set the grip mouse flag to false.
				this.mouseGripFlag = false;
				// Call the mouse move event handler.
				this.OnMouseMove(e);
			}
		}

		/// <summary>
		/// An event handler called when the mouse is clicked.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			// If there exists a highlighted marker.
			if (null != this.highlightMarker)
			{
				// Raise the event.
				if (null != this.MarkerClick) this.MarkerClick(this, e);
			}
			// Call the base class event handler.
			base.OnMouseClick(e);
		}

		/// <summary>
		/// An event handler called when the mouse is double-clicked.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			// If there exists a highlighted marker.
			if (null != this.highlightMarker)
			{
				// Raise the event.
				if (null != this.MarkerDoubleClick) this.MarkerDoubleClick(this, e);
			}
			// Call the base class event handler.
			base.OnMouseDoubleClick(e);
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

			// Lock the map mutex.
			this.mutex.WaitOne();

			try
			{
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
			}
			finally
			{
				// Unlock the mutex.
				this.mutex.ReleaseMutex();
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
			// Refresh the current map.
			this.OnRefreshMap();
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
			// Update the bitmap location.
			this.bitmapLocation = this.Convert(this.mapBounds.Location);
			// Compute the grid size.
			//this.majorGridSize = new MapSize(30, 30);
			//this.minorGridSize = new Size(3, 3);
			// Update the map items.
			this.OnUpdateItems();
		}

		/// <summary>
		/// Shows the specified annotation.
		/// </summary>
		/// <param name="annotation">The text annotation</param>
		/// <param name="text">The message text.</param>
		/// <param name="anchor">The message anchor.</param>
		private void OnShowAnnotation(MapTextAnnotation annotation, string text, IAnchor anchor)
		{
			// Get the old message bounds.
			Rectangle oldBounds = annotation.Bounds;
			
			// Suspend the message layout.
			annotation.SuspendLayout();
			// Set the new message properties.
			annotation.Text = text;
			annotation.Anchor = anchor;
			// Result the message layout.
			annotation.ResumeLayout();
			
			// Get the new message bounds.
			Rectangle newBounds = annotation.Bounds;
			// If the message is currently visible.
			if (annotation.Visible)
			{
				// Invalidate the area corresponding to the old annotation bounds.
				this.Invalidate(oldBounds);
			}
			else
			{
				// Set the annotation visibility to true.
				annotation.Visible = true;
			}
			// Invalidated the area corresponding to the new annotation bounds.
			this.Invalidate(newBounds);
		}

		/// <summary>
		/// Hides the specified text annotation.
		/// </summary>
		/// <param name="annotation">The annotation.</param>
		private void OnHideAnnotation(MapTextAnnotation annotation)
		{
			// If the annotation is visible.
			if (annotation.Visible)
			{
				// Invalidate the area corresponding to the annotation bounds.
				this.Invalidate(annotation.Bounds);
			}
			// Set the annotation visibility to false.
			annotation.Visible = false;
		}

		/// <summary>
		/// Refreshes the specified text annotation.
		/// </summary>
		/// <param name="annotation">The annotation.</param>
		private void OnRefreshAnnotation(MapTextAnnotation annotation)
		{
			// If the annotation is visible.
			if (annotation.Visible)
			{
				// Invalidate the area corresponding to the old annotation bounds.
				this.Invalidate(annotation.Bounds);
				// Refresh the annotation.
				annotation.Refresh();
				// Invalidate the area corresponding to the new annotation bounds.
				this.Invalidate(annotation.Bounds);
			}
		}

		/// <summary>
		/// Sets the specified message on the control.
		/// </summary>
		/// <param name="text">The new message text.</param>
		private void OnMessageChanged(string text)
		{
			// Execute the method on the UI thread.
			if (this.InvokeRequired)
			{
				this.Invoke(this.delegateMessage, new object[] { text });
				return;
			}
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
		/// An event handler called when the show borders flag has changed.
		/// </summary>
		/// <param name="showBorders">The value of the show borders flag.</param>
		private void OnShowBordersChanged(bool showBorders)
		{
			// Set the flag value.
			this.showBorders = showBorders;
			// Refresh the map.
			this.OnRefreshMap();
		}

		/// <summary>
		/// An event handler called when the show markers flag has changed.
		/// </summary>
		/// <param name="showMarkers">The value of the show markers flag.</param>
		private void OnShowMarkersChanged(bool showMarkers)
		{
			// Set the flag value.
			this.showMarkers = showMarkers;
			// If the show markers flag is set.
			if (this.showMarkers)
			{
				// If there exists a highlighted marker.
				if (null != this.highlightMarker)
				{
					// Show the info annotation.
					this.OnShowAnnotation(this.annotationInfo, this.highlightMarker.Name, this.highlightMarker);
				}
				// If there exists an emphasized marker.
				else if (null != this.emphasizedMarker)
				{
					// Show the info annotation.
					this.OnShowAnnotation(this.annotationInfo, this.emphasizedMarker.Name, this.emphasizedMarker);
				}
			}
			else
			{
				// If there exists a highlighted region.
				if (null != this.highlightRegion)
				{
					// Show the info annotation.
					this.OnShowAnnotation(this.annotationInfo, this.highlightRegion.Name, this.highlightRegion);
				}
				else
				{
					// Hide the info annotation.
					this.OnHideAnnotation(this.annotationInfo);
				}
			}
			// Invalidate the area corresponding to all markers.
			foreach (MapMarker marker in this.markers)
			{
				// Refresh the marker.
				this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
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
			// Update all map markers.
			foreach (MapMarker marker in this.markers)
			{
				marker.Update(this.mapBounds, this.mapScale);
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
						this.bitmapMap = this.OnDrawMap(asyncState);
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
		/// <param name="asyncState">The asynchrnous state.</param>
		/// <returns>The map bitmap.</returns>
		private Bitmap OnDrawMap(AsyncState asyncState)
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
					// Set the smooting mode to default.
					graphics.SmoothingMode = SmoothingMode.Default;
					using (SolidBrush brush = new SolidBrush(this.colorBackground))
					{
						using (Pen pen = new Pen(this.colorRegionNormalBorder))
						{
							// Fill the background.
							graphics.FillRectangle(brush, 0, 0, bitmapSize.Width, bitmapSize.Height);

							// Set the smooting mode to high quality.
							graphics.SmoothingMode = SmoothingMode.HighQuality;
							// Change the brush color.
							brush.Color = this.colorRegionNormalBackground;
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
								region.Draw(graphics, brush, this.showBorders ? pen : null);
							}
						}
					}
				}

				// Return the bitmap.
				return bitmap;
			}
		}

		/// <summary>
		/// An event handler called when the spring timer expires.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnSpringTick(object sender, EventArgs e)
		{
			// If the spring location is different from the current location.
			if (this.scrollSpring.CurrentLocation != this.bitmapLocation)
			{
				// Invalidate the map rectangle for the old location.
				this.Invalidate(this.shadow.GetShadowRectangle(new Rectangle(this.bitmapLocation, this.bitmapSize)));
				// Set the bitmap location at the new location.
				this.bitmapLocation = this.scrollSpring.CurrentLocation;
				// Invalidate the map rectangle for the new location.
				this.Invalidate(this.shadow.GetShadowRectangle(new Rectangle(this.bitmapLocation, this.bitmapSize)));
				// Refresh the text annotation.
				this.OnRefreshAnnotation(this.annotationInfo);
			}
		}

		/// <summary>
		/// An event handler called before the markers are cleared.
		/// </summary>
		private void OnBeforeMarkersCleared()
		{
			// Remove the event handlers all markers in the markers collection.
			foreach (MapMarker marker in this.markers)
			{
				this.OnRemoveMarkerEventHandlers(marker);
				// Dispose the marker.
				if (this.markerAutoDispose)
				{
					marker.Dispose();
				}
			}
		}

		/// <summary>
		/// An event handler called after a marker is inserted.
		/// </summary>
		/// <param name="index">The marker index in the collection.</param>
		/// <param name="marker">The marker.</param>
		private void OnAfterMarkerInserted(int index, MapMarker marker)
		{
			// If the marker is null, do nothing.
			if (null == marker) return;
			// Add the marker event handlers.
			this.OnAddMarkerEventHandlers(marker);
			// Refresh the marker.
			this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
		}

		/// <summary>
		/// An event handler called after a marker is removed.
		/// </summary>
		/// <param name="index">The marker index in the collection.</param>
		/// <param name="marker">The marker.</param>
		private void OnAfterMarkerRemoved(int index, MapMarker marker)
		{
			// If the marker is null, do nothing.
			if (null == marker) return;
			// Remove the marker event handlers.
			this.OnRemoveMarkerEventHandlers(marker);
			// Refresh the marker.
			this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
			// Dispose the marker.
			if (this.markerAutoDispose)
			{
				marker.Dispose();
			}
		}

		/// <summary>
		/// An event handler called when a marker is set.
		/// </summary>
		/// <param name="index">The marker index in the collection.</param>
		/// <param name="oldMarker">The old marker.</param>
		/// <param name="newMarker">The new marker.</param>
		private void OnAfterMarkerSet(int index, MapMarker oldMarker, MapMarker newMarker)
		{
			// If the old marker is different from the new marker.
			if (oldMarker != newMarker)
			{
				// If the old marker is not null.
				if (null != oldMarker)
				{
					// Remove the old marker event handlers.
					this.OnRemoveMarkerEventHandlers(oldMarker);
					// Refresh the old marker.
					this.Invalidate(oldMarker.Bounds.Add(this.bitmapLocation));
					// Dispose the marker.
					if (this.markerAutoDispose)
					{
						oldMarker.Dispose();
					}
				}
				// If the new marker is not null.
				if (null != newMarker)
				{
					// Add the new marker event handlers.
					this.OnAddMarkerEventHandlers(newMarker);
					// Refresh the new marker.
					this.Invalidate(newMarker.Bounds.Add(this.bitmapLocation));
				}
			}
		}

		/// <summary>
		/// Adds the event handlers for the specified marker.
		/// </summary>
		/// <param name="marker">The marker.</param>
		private void OnAddMarkerEventHandlers(MapMarker marker)
		{
			marker.ColorChanged += this.OnMarkerColorChanged;
			marker.EmphasisChanged += this.OnMarkerEmphasisChanged;
			marker.LocationChanged += this.OnMarkerLocationChanged;
			marker.SizeChanged += this.OnMarkerSizeChanged;
		}

		/// <summary>
		/// Removes the event handlers for the specified marker.
		/// </summary>
		/// <param name="marker">The marker.</param>
		private void OnRemoveMarkerEventHandlers(MapMarker marker)
		{
			marker.ColorChanged -= this.OnMarkerColorChanged;
			marker.EmphasisChanged -= this.OnMarkerEmphasisChanged;
			marker.LocationChanged -= this.OnMarkerLocationChanged;
			marker.SizeChanged -= this.OnMarkerSizeChanged;
		}

		/// <summary>
		/// An event handler called when a marker color has changed.
		/// </summary>
		/// <param name="marker">The marker.</param>
		private void OnMarkerColorChanged(MapMarker marker)
		{
			// Refresh the marker.
			this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
		}

		/// <summary>
		/// An event handler called when a marker emphasis has changed.
		/// </summary>
		/// <param name="marker">The marker.</param>
		/// <param name="oldEmphasis">The old emphasis state.</param>
		/// <param name="newEmphasis">The new emphasis state.</param>
		private void OnMarkerEmphasisChanged(MapMarker marker, bool oldEmphasis, bool newEmphasis)
		{
			// If the marker is emphasized.
			if (newEmphasis)
			{
				// If the markers are shown.
				if (this.showMarkers)
				{
					// Show the info annotation.
					this.OnShowAnnotation(this.annotationInfo, marker.Name, marker);
				}
				// Set the emphasized marker.
				this.emphasizedMarker = marker;
			}
			else
			{
				// If there is a highlighted marker.
				if ((null != this.highlightMarker) && this.showMarkers)
				{
					// Show the info annotation.
					this.OnShowAnnotation(this.annotationInfo, this.highlightMarker.Name, this.highlightMarker);
				}
				// If there is a highlighted region.
				else if (null != this.highlightRegion)
				{
					// Show the info annotation.
					this.OnShowAnnotation(this.annotationInfo, this.highlightRegion.Name, this.highlightRegion);
				}
				else
				{
					// Hide the info annotation.
					this.OnHideAnnotation(this.annotationInfo);
				}
				// Set the emphasized marker.
				this.emphasizedMarker = null;
			}
			// Refresh the marker.
			this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
		}

		/// <summary>
		/// An event handler called when a marker location has changed.
		/// </summary>
		/// <param name="marker">The marker.</param>
		/// <param name="oldLocation">The old location.</param>
		/// <param name="newLocation">The new location.</param>
		private void OnMarkerLocationChanged(MapMarker marker, MapPoint oldLocation, MapPoint newLocation)
		{
			// Refresh the marker.
			this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
			// Update the marker.
			marker.Update(this.mapBounds, this.mapScale);
			// Refresh the marker.
			this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
		}

		/// <summary>
		/// An event handler called when a marker size has changed.
		/// </summary>
		/// <param name="marker">The marker.</param>
		/// <param name="oldSize">The old size.</param>
		/// <param name="newSize">The new size.</param>
		private void OnMarkerSizeChanged(MapMarker marker, Size oldSize, Size newSize)
		{
			// Refresh the marker.
			this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
			// Update the marker.
			marker.Update(this.mapBounds, this.mapScale);
			// Refresh the marker.
			this.Invalidate(marker.Bounds.Add(this.bitmapLocation));
		}

		/// <summary>
		/// Converts the specified map point to the screen coordinates, depending on the current map location and scale.
		/// </summary>
		/// <param name="point">The map point.</param>
		/// <returns>The screen coordinates point.</returns>
		private Point Convert(MapPoint point)
		{
			return new Point(
				(int)Math.Round((point.X - this.mapLocation.X) * this.mapScale.Width),
				(int)Math.Round((this.mapLocation.Y - point.Y) * this.mapScale.Height)
				);
		}
	}
}
