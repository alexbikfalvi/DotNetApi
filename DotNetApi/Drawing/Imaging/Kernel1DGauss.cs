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
using DotNetApi.Numeric;

namespace DotNetApi.Drawing.Imaging
{
	/// <summary>
	/// A class representing a gaussian kernel.
	/// </summary>
	public sealed class Kernel1DGauss : Kernel1D
	{
		private double[] kernel;
		private int offset;

		private const double precision = 0.99;

		/// <summary>
		/// Creates a gaussian kernel instance.
		/// </summary>
		/// <param name="size">The kernel size.</param>
		public Kernel1DGauss(int size)
			: base(size)
		{
			// Create the kernel offset;
			this.offset = this.size >> 1;
			// Compute the maximum standard deviation such that the sum of all kernel elements approximates to one.
			double sigma = this.offset / (Math.Sqrt(2) * SpecialFunctions.InvErf(Kernel1DGauss.precision));
			// Create the constants.
			double invTwoSigmaSq = -1.0 / (2 * sigma * sigma);
			double invSqrtTwoPiSigmaSq = Math.Sqrt(-invTwoSigmaSq / Math.PI);
			// Create the kernel.
			double sum = 0.0;
			this.kernel = new double[size];
			for (int i = 0, x = -this.offset; i < size; i++, x++)
			{
				this.kernel[i] = invSqrtTwoPiSigmaSq * Math.Exp(invTwoSigmaSq * (x * x));
				sum += kernel[i];
			}
			// Normalize the kernel such that the sum of all elements is one.
			for (int i = 0; i < size; i++)
			{
				this.kernel[i] /= sum;
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
