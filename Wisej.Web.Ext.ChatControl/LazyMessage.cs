using System;
using System.Drawing;

namespace Wisej.Web.Ext.ChatControl
{
	public class LazyMessage : Message
	{
		/// <summary>
		/// Creates a new instance of <see cref="LazyMessage"/> with the given user.
		/// </summary>
		/// <param name="user">The user.</param>
		public LazyMessage(User user = null) : base("", null, user) 
		{
			MessageControlAssigned += LazyMessage_MessageControlAssigned;
		}

		private void LazyMessage_MessageControlAssigned(object sender, EventArgs e)
		{
			this.Control.MinimumSize = new Size(60, 16);
			this.Control.BackgroundImageLayout = ImageLayout.Zoom;
			this.Control.BackgroundImageSource = "resource.wx/Wisej.Web.Ext.ChatControl/Images/loading.gif";
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
			this.Control.MinimumSize = new System.Drawing.Size(0, 0);
		}
	}
}
