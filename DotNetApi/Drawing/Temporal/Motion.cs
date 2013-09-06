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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DotNetApi.Drawing.Temporal
{
	/// <summary>
	/// An abstract class representing a motion.
	/// </summary>
	public abstract class Motion : Timer
	{
		private Stopwatch stopwatch = new Stopwatch();
		private int counter = 0;
		private int stop = -1;

		/// <summary>
		/// Creates a new motion instance.
		/// </summary>
		/// <param name="fps">The number of frames per second.</param>
		public Motion(uint fps = 25)
		{
			// Set the timer interval.
			this.Interval = (int)Math.Round(1000.0 / fps);
		}

		// Public properties.

		/// <summary>
		/// Gets the begin location.
		/// </summary>
		public Point BeginLocation { get; private set; }
		/// <summary>
		/// Gets the end location.
		/// </summary>
		public Point EndLocation { get; private set; }
		/// <summary>
		/// Gets the current location.
		/// </summary>
		public Point CurrentLocation { get; protected set; }
		/// <summary>
		/// Gets the elapsed time for the current motion.
		/// </summary>
		public TimeSpan Elapsed { get { return this.stopwatch.Elapsed; } }

		// Public methods.

		/// <summary>
		/// Starts the motion between the specified points.
		/// </summary>
		/// <param name="begin">The begin point.</param>
		/// <param name="end">The end point.</param>
		public void Start(Point begin, Point end)
		{
			// Set the locations.
			this.BeginLocation = begin;
			this.EndLocation = end;
			this.CurrentLocation = begin;
			// Reset the counters.
			this.counter = 0;
			this.stop = -1;
			// Restart the stopwatch.
			this.stopwatch.Restart();
			// Enable the timer.
			this.Enabled = true;
		}

		/// <summary>
		/// Cancels the current motion.
		/// </summary>
		public void Cancel()
		{
			// Cancels the timer.
			this.Enabled = false;
			// Reset the stopwatch.
			this.stopwatch.Reset();
			// Reset the counters.
			this.counter = 0;
			this.stop = -1;
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the timer expires.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnTick(EventArgs e)
		{
			// If the current location equals to the finish location.
			if ((this.CurrentLocation == this.EndLocation) && (this.counter > 0))
			{
				// If the previous tick was on the same position.
				if (this.stop == this.counter - 1)
				{
					// Cancel the timer.
					this.Cancel();
				}
				else
				{
					// Save the stop counter.
					this.stop = this.counter;
				}
			}
			// Increment the counter.
			this.counter++;
			// Call the base class method.
			base.OnTick(e);
		}
	}
}
