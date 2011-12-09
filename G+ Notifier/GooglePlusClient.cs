using System;
using AwesomiumSharp;

namespace DanTup.GPlusNotifier
{
	class GooglePlusClient : IDisposable
	{
		WebView webView;

		public event JSCallback UpdateNotificationCount;

		public GooglePlusClient()
		{
			webView = WebCore.CreateWebView(128, 128);

			// Load a page that contains the notification box, but isn't as heavy as the Plus site.
			webView.CreateObject("GNotifier");
			webView.SetObjectCallback("GNotifier", "updateCount", OnUpdateNotificationCount);
			webView.DomReady += new EventHandler(WebView_DomReady);
			webView.LoadCompleted += new EventHandler(WebView_LoadCompleted);
			webView.JSConsoleMessageAdded += new JSConsoleMessageAddedEventHandler(WebView_JSConsoleMessageAdded);
			WebCore.Update();

			/**
			 * We use a fake URL because:
			 * A) We want the page to load quickly
			 * B) We are only loading this URL to spoof cross-domain security restrictions
			 * 
			 * We will be making AJAX calls from within the 404 page served by Google
			 * to make it seem as if the API calls are coming from an actual GPlus page.
			 * Hacky? Yes. Does it work? Definitely. :-)
			 **/
			webView.LoadURL("http://plus.google.com/foobar/fakeurl");
		}

		public bool IsLoggedIn()
		{
			string googleCookies = WebCore.GetCookies("http://www.google.com", false);
			return googleCookies.Contains("HSID=");
		}

		public void ForceCheck()
		{
			// We set window.count to an arbitrary value to invalidate it and force an update
			webView.ExecuteJavascript("window.count = -5");

			// Force an update by calling 'tick()' immediately
			BindNotificationScripts();
		}

		void WebView_JSConsoleMessageAdded(object sender, JSConsoleMessageEventArgs e)
		{
			Console.WriteLine(e.Message + ", " + e.Source);
		}

		// Attempt to bind the notification function once the DOM has finished initializing
		void WebView_DomReady(object sender, EventArgs e)
		{
			BindNotificationScripts();
		}

		// For extra measure, we'll attempt to bind the function at the end of each page load
		void WebView_LoadCompleted(object sender, EventArgs e)
		{
			BindNotificationScripts();
		}

		public void BindNotificationScripts()
		{
			// We make AJAX requests directly to Google Plus' internal API from an
			// empty page served on the http://plus.google.com domain.
			// We poll the API every 30 seconds for the notification count.
			// If we receive a bad status code, we assume we are not logged in.
			if (!webView.IsDisposed)
				webView.ExecuteJavascript(@"
					if(typeof window.xhr == 'undefined') { 
						window.notify = function(x) { 
							if(window.count != x) { window.count = x; GNotifier.updateCount(x); } 
						}; 
						window.xhr = new XMLHttpRequest();
						window.tick = function() {
							xhr.open('get','https://plus.google.com/u/0/_/n/guc'); 
							xhr.onreadystatechange = function() { 
								console.log('readystate: ' + xhr.readyState);
								if (xhr.readyState == 4) { 
									console.log('status: ' + xhr.status);
									if(xhr.status != 200) { 
										notify(-1); 
									} 
									else { 
										console.log('response: ' + xhr.responseText);
										var result = JSON.parse(xhr.responseText.substr(4)); 
										console.log('result type: ' + (typeof result));
										if(typeof result != 'object') 
											notify(-2); 
										else 
											notify(result[0][1]); 
									}
								} 
							}; 
							xhr.send(null);
						};
						window.setInterval('tick()', 30000);
					}; 
					tick();"); // We fire off one tick immediately
		}

		private void OnUpdateNotificationCount(object sender, JSCallbackEventArgs e)
		{
			if (UpdateNotificationCount != null)
				UpdateNotificationCount(sender, e);
		}

		#region IDisposable

		private bool disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					// Dispose managed
				}

				// Dispose Unmanaged

				((IDisposable)webView).Dispose();
			}
		}

		#endregion
	}
}
