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
using DotNetApi.Drawing;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a map annotation.
	/// </summary>
	public abstract class MapAnnotation : Component
	{
		private bool visible = true;
		private bool layoutSuspeded = false;

		private IAnchor anchor;
		private ITranslation translation;
		private Rectangle rectangleBounds;

		private HorizontalAlign horizontalAlignment = HorizontalAlign.Center;
		private VerticalAlign verticalAlignment = VerticalAlign.Center;

		/// <summary>
		/// Creates a new map annotation instance.
		/// </summary>
		/// <param name="anchor">The annotation anchor.</param>
		/// <param name="translation">The annotation translation.</param>
		public MapAnnotation(IAnchor anchor, ITranslation translation)
		{
			this.anchor = anchor;
			this.translation = translation;
		}

		/// <summary>
		/// Gets or sets whether the annotation is visible.
		/// </summary>
		public bool Visible
		{
			get { return this.visible; }
			set { this.visible = value; }
		}
		/// <summary>
		/// Gets or sets the anchor of the map message.
		/// </summary>
		public IAnchor Anchor
		{
			get { return this.anchor; }
			set { this.OnAnchorChanged(value); }
		}
		/// <summary>
		/// Gets or sets the annotation translation.
		/// </summary>
		public ITranslation Translation
		{
			get { return this.translation; }
			set { this.OnTranslationChanged(value); }
		}
		/// <summary>
		/// Gets or sets the message horizontal alignment.
		/// </summary>
		public HorizontalAlign HorizontalAlignment
		{
			get { return this.horizontalAlignment; }
			set { this.OnHorizontalAlignmentChanged(value); }
		}
		/// <summary>
		/// Gets or sets the message vertical alignment.
		/// </summary>
		public VerticalAlign VerticalAlignment
		{
			get { return this.verticalAlignment; }
			set { this.OnVerticalAlignmentChanged(value); }
		}
		/// <summary>
		/// Gets the bounds of the map annotation.
		/// </summary>
		public Rectangle Bounds
		{
			get { return this.rectangleBounds; }
			set { this.rectangleBounds = value; }
		}

		// Protected properties.

		/// <summary>
		/// Gets whether the annotation layout is suspended.
		/// </summary>
		public bool LayoutSuspeded { get { return this.layoutSuspeded; } }

		// Public methods.

		/// <summary>
		/// Refreshes the map annotation.
		/// </summary>
		public void Refresh()
		{
			// Call the event handler.
			this.OnRefresh();
		}

		/// <summary>
		/// Suspends the layout computations for the map message.
		/// </summary>
		public void SuspendLayout()
		{
			// Set the suspended layout to true.
			this.layoutSuspeded = true;
		}

		/// <summary>
		/// Resumes the layout computations for the map message.
		/// </summary>
		public void ResumeLayout()
		{
			// Set the suspended layout to false.
			this.layoutSuspeded = false;
			// Refresh the message.
			this.Refresh();
		}

		/// <summary>
		/// Draws the current message on the specified graphics object.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		public void Draw(Graphics graphics)
		{
			this.OnDraw(graphics);
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when drawing the annotation on the specified graphics object.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		protected virtual void OnDraw(Graphics graphics)
		{
		}

		/// <summary>
		/// An event handler called when refreshing the map annotation.
		/// </summary>
		protected virtual void OnRefresh()
		{
		}

		/// <summary>
		/// An event handler called when computing the bounds of the map annotation.
		/// </summary>
		protected virtual void OnMeasureBounds()
		{
		}

		/// <summary>
		/// Changes the anchor of the map annotation.
		/// </summary>
		/// <param name="anchor">The new anchor.</param>
		protected virtual void OnAnchorChanged(IAnchor anchor)
		{
			// Check the anchor is not null.
			if (null == anchor) throw new ArgumentNullException("anchor");
			// Set the anchor.
			this.anchor = anchor;
			// Update the bounds measurements.
			this.OnMeasureBounds();
		}

		/// <summary>
		/// Changesthe translation of the map annotation.
		/// </summary>
		/// <param name="translation">The new translation.</param>
		protected virtual void OnTranslationChanged(ITranslation translation)
		{
			// Check the anchor is not null.
			if (null == translation) throw new ArgumentNullException("translation");
			// Set the translation.
			this.translation = translation;
			// Update the bounds measurements.
			this.OnMeasureBounds();
		}

		/// <summary>
		/// Changes the horizontal alignment.
		/// </summary>
		/// <param name="horizontalAlignment">The horizontal alignment.</param>
		protected virtual void OnHorizontalAlignmentChanged(HorizontalAlign horizontalAlignment)
		{
			// Set the horizontal alignment.
			this.horizontalAlignment = horizontalAlignment;
			// Update the bounds measurements.
			this.OnMeasureBounds();
		}

		/// <summary>
		/// Changes the vertical alignment.
		/// </summary>
		/// <param name="verticalAlignment">The vertical alignment.</param>
		protected virtual void OnVerticalAlignmentChanged(VerticalAlign verticalAlignment)
		{
			// Set the vertical alignment.
			this.verticalAlignment = verticalAlignment;
			// Update the bounds measurements.
			this.OnMeasureBounds();
		}
	}
}
