using System;
using System.Drawing;

namespace Wisej.Web.Ext.ChatControl
{
	/// <summary>
	/// A <see cref="Message"/> with a deferred result.
	/// </summary>
	public class LazyMessage : Message
	{
		/// <summary>
		/// Creates a new instance of <see cref="LazyMessage"/> with the given user.
		/// </summary>
		/// <param name="user">The user.</param>
		public LazyMessage(User user = null) : this(user, null)
		{
		}

		/// <summary>
		/// Creates a new instance of <see cref="LazyMessage"/> with the given user and content type.
		/// </summary>
		/// <param name="user">The message's user.</param>
		/// <param name="contentType">The content type of the message.</param>
		public LazyMessage(User user, string contentType) : base("", contentType, user)
		{
			MessageControlAssigned += LazyMessage_MessageControlAssigned;
		}

		private void LazyMessage_MessageControlAssigned(object sender, EventArgs e)
		{
			this.Control.MinimumSize = new Size(60, 16);
			this.Control.BackgroundImageLayout = ImageLayout.Zoom;
			this.Control.BackgroundImageSource = "resource.wx/Wisej.Web.Ext.ChatControl/Images/loading.svg";
		}

		/// <summary>
		/// Sets the content of the message.
		/// </summary>
		/// <param name="content">The message content.</param>
		public void SetResult(string content)
		{
			this.Content = content;
			this.Control.Text = content;
			this.Control.BackgroundImageSource = "";
			this.Control.MinimumSize = new Size(0, 0);
		}
	}
}
