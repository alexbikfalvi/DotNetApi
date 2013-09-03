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
using System.Drawing;

namespace DotNetApi.Drawing.Temporal
{
	/// <summary>
	/// A class representing a spring motion.
	/// </summary>
	public sealed class MotionSpring : Motion
	{
		private double dampingRatio;
		private double angularFrequency;

		private double r;
		private double r1;
		private double r2;

		private double coeff1;
		private double coeff2;

		/// <summary>
		/// Creates a new spring motion instance.
		/// </summary>
		/// <param name="dampingRatio">The spring damping ratio.</param>
		/// <param name="angularFrequency">The spring angular frequency.</param>
		public MotionSpring(double dampingRatio = 0.5, double angularFrequency = 2.0 * Math.PI)
		{
			// Validate the arguments.
			if (dampingRatio <= 0.0) throw new ArgumentException("The parameter must be positive.", "dampingRatio");
			if (angularFrequency <= 0.0) throw new ArgumentException("The parameter must be positive.", "angularFrequency");
			// Save the parameters.
			this.dampingRatio = dampingRatio;
			this.angularFrequency = angularFrequency;

			// Precompute coefficients according to the motion equations.
			if (this.dampingRatio > 1.0)
			{
				// Over-damped system.
				this.r1 = this.angularFrequency * (Math.Sqrt(this.dampingRatio * this.dampingRatio - 1.0) - this.dampingRatio);
				this.r2 = this.angularFrequency * (-Math.Sqrt(this.dampingRatio * this.dampingRatio - 1.0) - this.dampingRatio);
				this.coeff1 = -this.r2 / (this.r1 - this.r2);
				this.coeff2 = this.r1 / (this.r1 - this.r2);
			}
			else if (this.dampingRatio == 1.0)
			{
				// Critically damped system.
				this.r = -this.angularFrequency * this.dampingRatio;
				this.coeff1 = 1.0;
				this.coeff2 = -this.r;
			}
			else
			{
				// Under-damped system.
				this.r1 = -this.angularFrequency * this.dampingRatio;
				this.r2 = this.angularFrequency * Math.Sqrt(1.0 - this.dampingRatio * this.dampingRatio);
				this.coeff1 = 1.0;
				this.coeff2 = this.angularFrequency * this.dampingRatio / this.r2;
			}
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the timer expires.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnTick(EventArgs e)
		{
			// Get the current elapsed time.
			double time = this.Elapsed.TotalSeconds;
			
			// Compute the new location using the motion equations.
			if (this.dampingRatio > 1.0)
			{
				// Over-damped system.
				double delta = this.coeff1 * Math.Exp(this.r1 * time) + this.coeff2 * Math.Exp(this.r2 * time);
				int x = (int)Math.Round(this.EndLocation.X + (this.BeginLocation.X - this.EndLocation.X) * delta);
				int y = (int)Math.Round(this.EndLocation.Y + (this.BeginLocation.Y - this.EndLocation.Y) * delta);
				this.CurrentLocation = new Point(x, y);
			}
			else if (this.dampingRatio == 1.0)
			{
				// Critically damped system.
				double delta = (this.coeff1 + this.coeff2 * time) * Math.Exp(this.r * time);
				int x = (int)Math.Round(this.EndLocation.X + (this.BeginLocation.X - this.EndLocation.X) * delta);
				int y = (int)Math.Round(this.EndLocation.Y + (this.BeginLocation.Y - this.EndLocation.Y) * delta);
				this.CurrentLocation = new Point(x, y);
			}
			else
			{
				// Under-damped system.
				double delta = this.coeff1 * Math.Exp(this.r1 * time) * Math.Cos(this.r2 * time) + this.coeff2 * Math.Exp(this.r1 * time) * Math.Sin(this.r2 * time);
				int x = (int)Math.Round(this.EndLocation.X + (this.BeginLocation.X - this.EndLocation.X) * delta);
				int y = (int)Math.Round(this.EndLocation.Y + (this.BeginLocation.Y - this.EndLocation.Y) * delta);
				this.CurrentLocation = new Point(x, y);
			}

			// Call the base class method.
			base.OnTick(e);
		}
	}
}
