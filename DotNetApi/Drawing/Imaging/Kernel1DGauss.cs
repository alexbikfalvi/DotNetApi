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

namespace DotNetApi.Drawing.Imaging
{
	/// <summary>
	/// A class representing a gaussian kernel.
	/// </summary>
	public sealed class Kernel1DGauss : Kernel1D
	{
		private double[] kernel;
		private int offset;

		/// <summary>
		/// Creates a gaussian kernel instance.
		/// </summary>
		/// <param name="size">The kernel size.</param>
		/// <param name="sigma">The standard deviation of the gaussian distribution. The default value is <b>1.0</b>.</param>
		public Kernel1DGauss(int size, double sigma = 1.0)
			: base(size)
		{
			// Create the kernel offset;
			this.offset = this.size >> 1;
			// Create the constants.
			double invTwoSigmaSq = -1.0 / (2 * sigma * sigma);
			double invSqrtTwoPiSigmaSq = Math.Sqrt(-invTwoSigmaSq / Math.PI);
			// Create the kernel.
			this.kernel = new double[size];
			for (int i = 0, x = -this.offset; i < size; i++, x++)
			{
				this.kernel[i] = invSqrtTwoPiSigmaSq * Math.Exp(invTwoSigmaSq * (x * x));
			}
		}

		/// <summary>
		/// Gets the kernel value at the specified point, where (0, 0) is the the kernel center.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <returns>The kernel value.</returns>
		public override double this[int x]
		{
			get
			{
				int i = x + this.offset;
				if ((i < 0) || (i >= this.size)) return 0.0;
				return this.kernel[i];
			}
		}
	}
}
