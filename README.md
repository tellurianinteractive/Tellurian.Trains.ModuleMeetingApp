# Module Meeting App 2
**Fast Clock App** for model railway module meetings where you run scheduled trains.
The app works on any device - computers, tablets and phones.

## Features
The app currently have these features:
* Fast clock for time simulation with option to locally start and stop simulation.
* Fast clock administration for setting game length, fast clock speed etc.
* Progressive web app support, is installable on local device.
* User interface supports english, german, dutch, polish, danish, norwegian (no and nn) and swedish.
* Configurable default settings when running locally.

## Cloud version
The clock is available as a *cloud version* that can support many simultaniuos running clocks.
Just change name of a clock, set a new password and apply the new settings. Users select clock name in the registration page.
The *Demo* clocks password cannot be protected, but password for all other clocks are only visible for users that can administrer the clock.

NOTE: The cloud version requires at least one client that requests time for one of the clocks.
If no clients requests time, after a while the server app will be recycled, which means that all clocks are deleted except for the *Demo* clock.

## User functions
If a user register their name or station name and optionally a user password (if required), they can do the follwoing:
* Select *display language* in the web browser settings.
* Stop the clock by first selecting one of the predefined *reasons*.
* Restart the clock. Note only the user that stopped the clock, or an administrator, can restart the clock again.
* Select to show your clock with *maximized time*. This is useful on standalone display.

## Many languages supported
Each client app uses your language of choice. The user interface and all preconfigured messages - stop reason, pause reason - are shown in your language.
Language is selected in your browser settings. 
Currently supported languages: English (the default), German, Dutch, Polish, Czech, Italian, Danish, Swedish, Finnish and Norwegian.

## Administrator functions
The clock can be administered from any supported device.
To be able to administrer the clock, the user must *register* the clocks password.
The administrator can change the following settings:
* Select to show fast clock or real clock.
* Start game time.
* Game duration in hours.
* Weekday for game (optional).
* Real time for pause.
* Reason for pause, i.e. lunch, dinner etc.
* Real time when game is continued.
* Select if real time clock is shown automatically during pause.
* Select to show your clock with maximized time. This is useful on standalone display.
* Only an administrator can restart game after pause.

Read how in the [Administrator Manual](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/Administrators-Manual).

## Developer opportunities
Contribute to the development of The Module Meeting App. It is open source!

Develop against the [open Web API](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/API-Guidelines).
Through the API, you can get the status of the clock and start and stop the clock from any other application.
This makes integration of existing applications and devices easy.

Also note that when run locally, the **Module Meeting App Server** supports MR-Clock polling and broadcasting protocols over TCP. 
This means that clock clients using MR-Clock protocol can connect directly to the **Module Meeting App Server** to obtaing the time.
See [Installation Manual](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/Local-Installation) how to install the app locally.

## Environment
The **Module Meeting App Server** can run on the [these operating systems](https://github.com/dotnet/core/blob/master/release-notes/5.0/5.0-supported-os.md).

The **Module Meeting App Client** can run in any modern browser that supports [Web Assembly](https://webassembly.org/). 
This includes Chrome, Firefox, Safari, and Edge on desktop computers and mobile devices. 
It also runs in Chromium on Raspberry Pi OS.

The **Module Meeting App Client** is hosted by the **Module Meeting App Server** and automatically gets downloaded to your device
when you surf to the server endpoint. Because it is a progressive app, you can choose to install it as 
an offline app with an icon on your desktop or home screen. Or just run it as a a normal *single page* web application.

## Documentation
The **Module Meeting App** is well documented.
All you need to know, you will find in the [**Wiki**](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki).