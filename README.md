# Module Meeting App 2.0
App for model railway module meetings. The app runs on every device that supports Web Assembly.

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
* Stop the clock by first selecting one of the predefined reasons.
* Start the clock. Note only the user that stopped the clock, or an administrator, can start the clock again.

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
* Only an administrator can restart game after pause.

Read how in the [Administrator Manual](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/blob/master/MANUAL.md).

## Developer opportunities
Contribute to the development of The Module Meeting App. It is open source!

Develop against the [open Web API](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/blob/master/API.md).
Through the API, you can get the status of the clock and start and stop the clock from any other application.
This makes integration of existing applications and devices easy.

Also note that when run locally, the **Module Meeting App Server** supports MR-Clock polling and broadcasting protocols over TCP. 
This means that clock clients using MR-Clock protocol can connect directly to the **Module Meeting App Server** to obtaing the time.
See [Installation Manual](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/blob/master/INSTALLATION.md) how to install the app locally.

## Environment
The **Module Meeting App Server** can run on the [these operating systems](https://github.com/dotnet/core/blob/master/release-notes/3.1/3.1-supported-os.md).

The **Module Meeting App Client** can run in any modern browser that supports [Web Assembly](https://webassembly.org/). 
This includes Chrome, Firefox, Safari, and Edge on both desktop computers and mobile devices. 

The **Module Meeting App Client** is hosted by the **Module Meeting App Server** and automatically gets downloaded to your device
when you surf to the server endpoint. Because it is a progressive app, you can choose to install it as 
an offline app with an icon on your desktop or home screen. Or just run it as a a normal *single page* web application.

