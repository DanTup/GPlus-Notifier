# G+ Notifier

G+ Notifier ([gplusnotifier.com](http://gplusnotifier.com)) is a Windows utility that sits in the notification area and
alerts you to notifications in the Google+ social network.

## Download & Installation

Download the latest zip file from the downloads page and extract it to a folder (eg. "My Documents\G+ Notifier\").
Run the file DanTup.GPlusNotifier.exe. If you would like G+ Notifier to run at startup, you must (for now) manually
create a shortcut to it in your Startup folder.

Alternatively, you can download the source code and build it yourself.

## TODO

Currently the app only shows the number of unread messages in the notification area. Double-clicking the icon will
launch Google+ using your default browser (it does this via Process.Start). I wanted to get something useful out there
as quickly as possible, but the plan is to continue developing. Please send me feature requests and bug reports and I'll
do what I can as quickly as I can :-)

* Fix issues with some keys/modifiers in the login form/browser
* Preview of messages without launching Google+ (as per screenshot at http://gplusnotifier.com/Images/Preview.png)
 * Inline replies (like the Google-bar, either via API or rendering that iframe)
* Auto-updating (prompt user first - don't like the idea of stealth updates from unknown devs)
* Change to official Google+ API (when available)
* Translate into other languages
* Installer (Clickonce?)
* Settings
 * Set Polling frequency (API)
 * Selection of browser to use when launching Google+ (inc. Chrome application mode)
 * Add to Startup automatically

## How It Works

As there is currently no API for Google+, this application uses the [Awesomium Web-Browser
Framework](http://awesomium.com/). You will be prompted to login to Google+ at startup. The browser window will be
hidden but kept alive. G+ Notifier reads the number of notifications from the box in the corner of the page.

 ## Security

This project is open source so that you may check the source and build it yourself. The application does not attempt to
capture your details or post anything to your account without your knowledge. Building from source is the safest way,
however I will include binary copies for convenience. All binary copies will be built from the source here and I will
thoroughly check any code contributions before allowing them into the repository.