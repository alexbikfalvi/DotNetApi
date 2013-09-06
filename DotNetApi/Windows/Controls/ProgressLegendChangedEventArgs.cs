using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A delegate representing a progress legend changed event handler.
	/// </summary>
	/// <param name="sender">The sender object.</param>
	/// <param name="e">The event arguments.</param>
	public delegate void ProgressLegendChangedEventHandler(object sender, ProgressLegendChangedEventArgs e);

	/// <summary>
	/// A class representing a progress legend changed event arguments.
	/// </summary>
	public class ProgressLegendChangedEventArgs : ProgressInfoEventArgs
	{
		/// <summary>
		/// Creates a new event arguments instance.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		/// <param name="legend">The progress legend.</param>
		public ProgressLegendChangedEventArgs(ProgressInfo progress, ProgressLegend legend)
			: base(progress)
		{
			this.Legend = legend;
		}

		// Public properties.

		/// <summary>
		/// Gets the legend.
		/// </summary>
		public ProgressLegend Legend { get; private set; }
	}
}
