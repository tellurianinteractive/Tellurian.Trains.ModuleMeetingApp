# Fast Clock App 3
**Fast Clock App for model railway module meetings where you run scheduled trains.
The app works on any device - computers, tablets and phones.**

**It supports most European languages: English, Czech, Danish, Dutch, German, Finnish, French, Hungarian, Italian, 
Norwegian (nynorsk & bokm&aring;l), Polish, Slovak, and Swedish.**

## Getting the app
The cloud version app is available at https://fastclock.azurewebsites.net/.
From this link, the **Fast Clock App Client** is automatically downloaded to your device.
There is a *Demo* clock to play with, but anyone can create a named clock and become administrator of that clock.

If you cannot use Internet for some reason, it is also possible to run the **clock server app** locally. 
Read more about this option in the Wiki.

The **Fast Clock App** is well documented.
All you need to know, you will find in the [**Wiki**](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/).

Latest news are in the [Release Notes](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/blob/master/RELEASENOTES.md).

## Features
### Main Features
* Fast clock for fast time with option to start and stop from any device.
* Fast clock administration for setting game length, fast clock speed etc.
* Digital and analogue clock display.
* User interface supports **15 European languages**, ideal for module meetings with people from several countries.
* Select to show your clock with *maximized time*. This is useful when displaying the time on standalone displays.

### User features
If you user register your name or station name and optionally a user password you can do the following:
* Create new clocks.
* Stop the clock by first selecting one of the predefined *reasons*.
* Restart the clock. Note only the user that stopped the clock, or an administrator, can restart the clock again.

### Administrator features
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

### Open API Access
Develop against the [open Web API](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/API-Guidelines).
Through the API, you can get the status of the clock and start and stop the clock from any other application.
This makes integration of existing applications and devices easy. 
There is already a working integration of FREMO/RUT pulse driven analogue clocks, see below.

### Support of Analogue Clocks
The additional software [*Clock Pulse Service*](https://github.com/tellurianinteractive/ClockPulseService) 
converts the time read from the clock servers API to clock pulses that can drive an analogue clock,
for example the RUT-clocks used at FREMO meetings.
This has been verified using a Raspberry Pi and a relay board.

## Contribute!
Contribute to the development of The Module Meeting App. It is open source! 
See what [issues](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/issues) you can take on.

