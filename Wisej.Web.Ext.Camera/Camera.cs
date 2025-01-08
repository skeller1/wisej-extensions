﻿///////////////////////////////////////////////////////////////////////////////
//
// (C) 2020 ICE TEA GROUP LLC - ALL RIGHTS RESERVED
//
// 
//
// ALL INFORMATION CONTAINED HEREIN IS, AND REMAINS
// THE PROPERTY OF ICE TEA GROUP LLC AND ITS SUPPLIERS, IF ANY.
// THE INTELLECTUAL PROPERTY AND TECHNICAL CONCEPTS CONTAINED
// HEREIN ARE PROPRIETARY TO ICE TEA GROUP LLC AND ITS SUPPLIERS
// AND MAY BE COVERED BY U.S. AND FOREIGN PATENTS, PATENT IN PROCESS, AND
// ARE PROTECTED BY TRADE SECRET OR COPYRIGHT LAW.
//
// DISSEMINATION OF THIS INFORMATION OR REPRODUCTION OF THIS MATERIAL
// IS STRICTLY FORBIDDEN UNLESS PRIOR WRITTEN PERMISSION IS OBTAINED
// FROM ICE TEA GROUP LLC.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Wisej.Base;
using Wisej.Core;
using Wisej.Design;

namespace Wisej.Web.Ext.Camera
{
	/// <summary>
	/// The Camera component makes it possible to take pictures with the device's camera and upload them to the server.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(Camera))]
	[Description("The Camera component makes it possible to take pictures with the device's camera and upload them to the server.")]
	[ApiCategory("Camera")]
	public partial class Camera : Control, IWisejHandler
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Wisej.Ext.Camera" /> class.
		/// </summary>
		public Camera()
		{
		}

		#endregion

		#region Events

		/// <summary>
		/// Fired when the current recording is available for download.
		/// </summary>
		public event UploadedEventHandler Uploaded
		{
			add { base.AddHandler(nameof(Uploaded), value); }
			remove { base.RemoveHandler(nameof(Uploaded), value); }
		}

		/// <summary>
		/// Fired while the <see cref="Camera" /> control receives the recording stream being uploaded.
		/// </summary>
		/// <remarks>
		/// This event fires only if there is an handler attached to it. A simple overload of the On[Event] method in 
		/// a derived class will not be invoked unless there is at least one handler attached to the event.
		/// </remarks>
		public event UploadProgressEventHandler Progress
		{
			add { base.AddHandler(nameof(Progress), value); }
			remove { base.RemoveHandler(nameof(Progress), value); }
		}

		/// <summary>
		/// Fired when an error occurs in the camera setup or usage.
		/// </summary>
		public event CameraErrorHandler Error
		{
			add { base.AddHandler(nameof(Error), value); }
			remove { base.RemoveHandler(nameof(Error), value); }
		}

		/// <summary>
		/// Fires the <see cref="Error"/> event.
		/// </summary>
		/// <param name="e">A <see cref="CameraErrorEventArgs" /> that contains the event data. </param>
		protected virtual void OnError(CameraErrorEventArgs e)
		{
			((CameraErrorHandler)base.Events[nameof(Error)])?.Invoke(this, e);
		}

		/// <summary>
		/// Fires the <see cref="Uploaded" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:Wisej.Web.UploadedEventArgs" /> that contains the event data.</param>
		protected virtual void OnUploaded(UploadedEventArgs e)
		{
			((UploadedEventHandler)base.Events[nameof(Uploaded)])?.Invoke(this, e);
		}

		/// <summary>
		/// Fires the <see cref="Progress" /> event.
		/// </summary>
		/// <remarks>
		/// This event fires only if there is an handler attached to it. A simple overload of the On[Event] method in 
		/// a derived class will not be invoked unless there is at least one handler attached to the event.
		/// </remarks>
		/// <param name="e">A <see cref="T:Wisej.Web.UploadProgressEventArgs" /> that contains the event data. </param>
		protected virtual void OnProgress(UploadProgressEventArgs e)
		{
			((UploadProgressEventHandler)base.Events[nameof(Progress)])?.Invoke(this, e);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Returns or sets the filter (<see href="https://developer.mozilla.org/en-US/docs/Web/CSS/filter"/>)
		/// to apply to the video.
		/// </summary>
		[DefaultValue(null)]
		public string VideoFilter
		{
			get { return this._videoFilter; }
			set
			{
				value = value == "" ? null : value;

				if (this._videoFilter != value)
				{
					this._videoFilter = value;
					Update();
				}
			}
		}
		private string _videoFilter = null;

		/// <summary>
		/// Specifies the object-fit to apply to the video.
		/// </summary>
		/// <remarks>
		/// See <see href="https://www.w3schools.com/css/css3_object-fit.asp"/>.
		/// </remarks>
		[DesignerActionList]
		[DefaultValue(true)]
		public ObjectFit ObjectFit
		{
			get
			{
				return this._objectfit;
			}
			set
			{
				if (this._objectfit != value)
				{
					this._objectfit = value;
					Update();
				}
			}
		}
		private ObjectFit _objectfit = ObjectFit.Contain;

		/// <summary>
		/// Specifies whether audio should be recorded.
		/// </summary>
		[DesignerActionList]
		[DefaultValue(false)]
		public bool Audio
		{
			get
			{
				return this._audio;
			}
			set
			{
				if (this._audio != value)
				{
					this._audio = value;
					Update();
				}
			}
		}
		private bool _audio = false;

		/// <summary>
		/// Specifies whether video should be recorded.
		/// </summary>
		[DesignerActionList]
		[DefaultValue(true)]
		public bool Video
		{
			get
			{
				return this._video;
			}
			set
			{
				if (this._video != value)
				{
					this._video = value;
					Update();
				}
			}
		}
		private bool _video = true;

        /// <summary>
        /// Specifies whether the media should be mirrored.
        /// </summary>
        [DesignerActionList]
        [DefaultValue(false)]
        public bool Mirror
        {
            get
            {
                return this._mirror;
            }
            set
            {
                if (this._mirror != value)
                {
                    this._mirror = value;
                    Update();
                }
            }
        }
        private bool _mirror = false;

		/// <summary>
		/// Specifies the video device to use.
		/// </summary>
		/// <remarks>
		/// If not specified or invalid it will use the first device returned by the browser.
		/// </remarks>
		[DefaultValue("")]
		public string DeviceName
		{
			get
			{
				return this._deviceName ?? String.Empty;
			}
			set
			{
				if (this._deviceName != value)
				{
					this._deviceName = value;
					Update();
				}
			}
		}
		private string _deviceName = null;

		/// <summary>
		/// Specifies whether the video is front-facing (mobile-only).
		/// </summary>
		[DesignerActionList]
		[DefaultValue(true)]
		public VideoFacingMode FacingMode
		{
			get
			{
				return this._facingMode;
			}
			set
			{
				if (this._facingMode != value)
				{
					this._facingMode = value;
					Update();
				}
			}
		}
		private VideoFacingMode _facingMode = VideoFacingMode.User;

		
		/// <summary>
		/// Video capture resolution - Width
		/// </summary>
		[DesignerActionList]
		[DefaultValue(true)]
		public int WidthCapture
		{
			get
			{
				return this._widthCapture;
			}
			set
			{
				if (this._widthCapture != value)
				{
					this._widthCapture = value;
					Update();
				}
			}
		}
		private int _widthCapture = 640;

		/// <summary>
		/// Video capture resolution - Height
		/// </summary>
		[DesignerActionList]
		[DefaultValue(true)]
		public int HeightCapture
		{
			get
			{
				return this._heightCapture;
			}
			set
			{
				if (this._heightCapture != value)
				{
					this._heightCapture = value;
					Update();
				}
			}
		}
		private int _heightCapture = 480;

		
			/// <summary>
			/// Indicates the border style for the control.
			/// </summary>
			/// <returns>One of the <see cref="T:Wisej.Web.BorderStyle" /> values. The default is BorderStyle.None.</returns>
			[DefaultValue(BorderStyle.Solid)]
		[SRCategory("CatAppearance")]
		[SRDescription("Indicates the border style for the control.")]
		public virtual BorderStyle BorderStyle
		{
			get
			{
				return this._borderStyle;
			}
			set
			{
				if (this._borderStyle != value)
				{
					this._borderStyle = value;

					Refresh();

					OnStyleChanged(EventArgs.Empty);
				}
			}
		}
		private BorderStyle _borderStyle = BorderStyle.Solid;

		/// <summary>
		/// Gets or sets the zoom value for the camera.
		/// </summary>
		/// <remarks>
		/// Not all cameras support zooming.
		/// </remarks>
		[DefaultValue(1)]
		public int Zoom
		{
			get
			{
				return this._zoom;
			}
			set
			{
				if (this._zoom != value)
				{
					this._zoom = value;

					Update();
				}
			}
		}
		private int _zoom = 1;

		#endregion

		#region Methods

		/// <summary>
		/// Returns the current image from the camera.
		/// </summary>
		/// <param name="callback">Callback method to receive the <see cref="Image"/> or null.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback"/> is null.</exception>
		public void GetImage(Action<Image> callback)
		{
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));

			Call("getImage", (base64) =>
			{

				callback(ImageFromBase64(base64));

			}, null);
		}

		/// <summary>
		/// Returns the current image from the camera asynchronously.
		/// </summary>
		/// <returns>An awaitable <see cref="Task"/>.</returns>
		public Task<Image> GetImageAsync()
		{
			var tcs = new TaskCompletionSource<Image>();
			GetImage((image) =>
			{
				tcs.SetResult(image);
			});
			return tcs.Task;
		}

		/// <summary>
		/// Starts recording.
		/// </summary>
		/// <param name="format">The video encoding mime type format, <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types"/>.</param>
		/// <param name="bitsPerSecond">Audio and video bits per second. <see href="https://developer.mozilla.org/en-US/docs/Web/API/MediaRecorder/MediaRecorder"/>.</param>
		/// <param name="updateInterval">Update interval in seconds. The default is zero causing the video to be uploaded on <see cref="StopRecording"/>.</param>
		/// <remarks>You must call <see cref="StopRecording"/>to end recording.</remarks>
		public void StartRecording(string format=null, int bitsPerSecond = 2500000, int updateInterval = 0)
		{
			Call("startRecording", format, bitsPerSecond, updateInterval);
		}

		/// <summary>
		/// Stops recording and uploads the recorded stream to the <see cref="Uploaded"/> event.
		/// </summary>
		public void StopRecording()
		{
			Call("stopRecording");
		}

		/// <summary>
		/// Returns the names of the available video devices.
		/// </summary>
		/// <param name="callback">Callback method to receive the device names.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback"/> is null.</exception>
		public void GetDevices(Action<string[]> callback)
		{
			GetDevices(false, callback);
		}

		/// <summary>
		/// Returns the names of the available video devices.
		/// </summary>
		/// <param name="refresh">Refreshes the list of devices from the browser.</param>
		/// <param name="callback">Callback method to receive the device names.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback"/> is null.</exception>
		public void GetDevices(bool refresh, Action<string[]> callback)
		{
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));

			Call("getDevices", (list) =>
			{

				callback(list);

			}, new object[] { refresh });
		}

		/// <summary>
		/// Returns the names of the available video devices.
		/// </summary>
		/// <param name="refresh">Refreshes the list of devices from the browser. Default is false.</param>
		/// <returns>An awaitable <see cref="Task"/>.</returns>
		public Task<string[]> GetDevicesAsync(bool refresh = false)
		{
			var tcs = new TaskCompletionSource<string[]>();
			GetDevices(refresh, (list) =>
			{

				tcs.SetResult(list);

			});
			return tcs.Task;
		}

		/// <summary>
		/// Returns the Image encoded in a base64 string.
		/// </summary>
		/// <param name="base64">The base64 string representation of the image from the client.</param>
		/// <returns>An <see cref="Image"/> created from the <paramref name="base64"/> string.</returns>
		private static Image ImageFromBase64(string base64)
		{
			// data:image/gif;base64,R0lGODlhCQAJAIABAAAAAAAAACH5BAEAAAEALAAAAAAJAAkAAAILjI+py+0NojxyhgIAOw==
			try
			{
				if (String.IsNullOrEmpty(base64))
					return null;

				int pos = base64.IndexOf("base64,");
				if (pos < 0)
					return null;

				base64 = base64.Substring(pos + 7);
				byte[] buffer = Convert.FromBase64String(base64);
				MemoryStream stream = new MemoryStream(buffer);
				return new Bitmap(stream);
			}
			catch { }

			return null;
		}

		#endregion

		#region Wisej Implementation

		// Handles progress events from the client.
		private void ProcessProgressWebEvent(WisejEventArgs e)
		{
			var data = e.Parameters.Data;
			OnProgress(new UploadProgressEventArgs(data.loaded ?? 0, data.total ?? 0));
		}

		/// <summary>
		/// Processes the event from the client.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected override void OnWebEvent(WisejEventArgs e)
		{
			switch (e.Type)
			{
				case "error":
					OnError(new CameraErrorEventArgs(e.Parameters.Message));
					break;

				case "progress":
					ProcessProgressWebEvent(e);
					break;

				default:
					base.OnWebEvent(e);
					break;
			}
		}

		/// <summary>
		/// Renders the client component.
		/// </summary>
		/// <param name="config">Dynamic configuration object.</param>
		protected override void OnWebRender(dynamic config)
		{
			base.OnWebRender((object)config);

			config.className = "wisej.web.ext.Camera";

			config.zoom = this.Zoom;
			config.mirror = this.Mirror;
			config.objectFit = this.ObjectFit;
			config.borderStyle = this.BorderStyle;
			config.videoFilter = this.VideoFilter;
			config.submitURL = this.GetPostbackURL();

			// apply video constraints.
			if (this.Video)
			{
				config.constraints = new
				{
					video = new
					{
						width = this._widthCapture,
						height = this._heightCapture,
						facingMode = this._facingMode,
						deviceName = this._deviceName
					},
					audio = this.Audio
				};
			}
			else
			{
				config.constraints = new
				{
					video = false,
					audio = this.Audio
				};
			}

			config.wiredEvents.Add("error(Message)");

			if (base.Events[nameof(Progress)] != null)
				config.wiredEvents.Add("progress(Data)");
		}

		#endregion

		#region IWisejHandler

		/// <summary>
		/// Compress the output.
		/// </summary>
		bool IWisejHandler.Compress { get { return false; } }

		/// <summary>
		/// Process the HTTP request.
		/// </summary>
		/// <param name="context">The current <see cref="T:System.Web.HttpContext"/>.</param>
		void IWisejHandler.ProcessRequest(HttpContext context)
		{
			OnUploaded(new UploadedEventArgs(context.Request.Files));
		}

		#endregion
	}
}
