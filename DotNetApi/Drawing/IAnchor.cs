using System;
using System.Drawing;

namespace DotNetApi.Drawing
{
	/// <summary>
	/// An interface for all objects that have anchor bounds.
	/// </summary>
	public interface IAnchor
	{
		/// <summary>
		/// Gets the anchor bounds.
		/// </summary>
		Rectangle AnchorBounds { get; }
	}
}
