/* 
 * Copyright (C) 2012-2013 Alex Bikfalvi
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
using DotNetApi.Windows.Controls;

namespace YtAnalytics.Controls
{
	public delegate void ShowNotificationEventHandler(Image image, string title, string text, bool progress, int duration, NotificationTaskEventHandler task, object[] parameters);
	public delegate void HideNotificationEventHandler(NotificationTaskEventHandler task, object[] parameters);

	/// <summary>
	/// A controls that displays a notification box.
	/// </summary>
	public class NotificationControl : ThreadSafeControl
	{
		private NotificationBox notification = new NotificationBox();

		private ShowNotificationEventHandler delegateShowMessage;
		private HideNotificationEventHandler delegateHideMessage;

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public NotificationControl()
		{
			// Add the message control.
			this.Controls.Add(this.notification);
			// Create the delegates.
			this.delegateShowMessage = new ShowNotificationEventHandler(this.ShowMessage);
			this.delegateHideMessage = new HideNotificationEventHandler(this.HideMessage);
		}

		// Protected methods.

		/// <summary>
		/// Shows an alerting message on top of the control.
		/// </summary>
		/// <param name="image">The message icon.</param>
		/// <param name="title">The message title.</param>
		/// <param name="text">The message text.</param>
		/// <param name="progress">The visibility of the progress bar.</param>
		/// <param name="duration">The duration of the message in milliseconds. If negative, the message will be displayed indefinitely.</param>
		/// <param name="task">A task to execute on the UI thread before the message is shown.</param>
		/// <param name="parameters">The task parameters.</param>
		protected void ShowMessage(
			Image image,
			string title,
			string text,
			bool progress = true,
			int duration = -1,
			NotificationTaskEventHandler task = null,
			object[] parameters = null)
		{
			// Invoke the function on the UI thread.
			if (this.InvokeRequired)
				this.Invoke(this.delegateShowMessage, new object[] { image, title, text, progress, duration, task, parameters });
			else
			{
				// Set the message on top of all other controls.
				this.Controls.SetChildIndex(this.notification, 0);
				// Show the message.
				this.notification.Show(image, title, text, progress, duration, task, parameters);
			}
		}

		/// <summary>
		/// Hides the alerting message.
		/// </summary>
		/// <param name="task">A task to execute on the UI thread after the message is hidden.</param>
		/// <param name="parameters">The task parameters.</param>
		protected void HideMessage(NotificationTaskEventHandler task = null, object[] parameters = null)
		{
			// Invoke the function on the UI thread.
			if (this.InvokeRequired)
				this.Invoke(this.delegateHideMessage, new object[] { task, parameters });
			else
			{
				// Hide the message.
				this.notification.Hide();
				// Execute the task on the UI thread.
				if (task != null) task(parameters);
			}
		}

		/// <summary>
		/// An event handler called when the control is being resized.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnResize(EventArgs e)
		{
			// If the notification box is visible.
			if (this.notification.Visible)
			{
				// Reposition the notification box.
				this.notification.Reposition();
			}
			// Call the base class method.
			base.OnResize(e);
		}
	}
}
