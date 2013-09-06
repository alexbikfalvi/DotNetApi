using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A delegate representing a progress legend set event handler.
	/// </summary>
	/// <param name="sender">The sender object.</param>
	/// <param name="e">The event arguments.</param>
	public delegate void ProgressLegendSetEventHandler(object sender, ProgressLegendSetEventArgs e);

	/// <summary>
	/// A class representing a progress legend changed event arguments.
	/// </summary>
	public class ProgressLegendSetEventArgs : ProgressInfoEventArgs
	{
		/// <summary>
		/// Creates a new event arguments instance.
		/// </summary>
		/// <param name="progress">The progress info.</param>
		/// <param name="oldLegend">The old progress legend.</param>
		/// <param name="newLegend">The new progress legend.</param>
		public ProgressLegendSetEventArgs(ProgressInfo progress, ProgressLegend oldLegend, ProgressLegend newLegend)
			: base(progress)
		{
			this.OldLegend = oldLegend;
			this.NewLegend = newLegend;
		}

		// Public properties.

		/// <summary>
		/// Gets the old legend.
		/// </summary>
		public ProgressLegend OldLegend { get; private set; }
		/// <summary>
		/// Gets the new legend.
		/// </summary>
		public ProgressLegend NewLegend { get; private set; }
	}
}
