# Tellurian.Trains.ModuleMeetingApp 2.0
App for model railway module meetings. The app runs on every device that supports Web Assembly.

The app currently have these features:
* Fast clock for time simulation with option to locally start and stop simulation.
* Fast clock adinistration for setting game length, fast clock speed etc.
* Configurable default settings.
* Progressive web app support, is installable on local device.
* User interface supports english, german, dutch, polish, danish, norwegian (no and nn) and swedish.

## User functions
If a user register their name or station name, they can do the follwoing:
* Stop the clock by first selecting one of the predefined reasons.
* Start the clock. Note only the user that stopped the clock, or an administrator, can start the clock again.

## Administrator functions
The clock can be administered from any device. 
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

NOTE: the time for pause is included in the estimation of when a game ends in real time.

## Developer opportunities
Contribute to the development of The Module Meeting App. It is open source!

Develop against the [open Web API](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/blob/master/API.mdd).
Through the API, you can get the status of the clock and start and stop the clock from any other application.
This makes integration of existing applications and devices easy.

Also note that when run locally, the **Module Meeting App Server** supports MR-Clock polling and broadcasting protocols over TCP. 
This means that clock clients using MR-Clock protocol can connect directly to the **Module Meeting App Server** to obtaing the time.

## Environment
The **Module Meeting App Server** can run on the [these operating systems](https://github.com/dotnet/core/blob/master/release-notes/3.1/3.1-supported-os.md).

The **Module Meeting App Client** can run in any modern browser that supports [Web Assembly](https://webassembly.org/). 
This includes Chrome, Firefox, Safari, and Edge on both desktop computers and mobile devices. 

The **Module Meeting App Client** is hosted by the **Module Meeting App Server** and automatically gets downloaded to your device
when you surf to the server endpoint. Because it is a progressive app, you can choose to install it as 
an offline app with an icon on your desktop or home screen. Or just run it as a a normal *single page* web application.

## Installation
Currently, only source code distribution is supported. 

#### Source code installation
In order to run the clock from source code, do the following steps:

1. Download and install [NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core). 
Select the version appripriate for your operating system. 
You can run .NET on many different operating systems, not only Windows.
2. Clone or pull the [source code](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp.git) to a local folder on your computer.
3. Go to the folder **App/Server** (and <u>not</u> *Clock.Server*), open a command prompt there and execute the following command:

```
    dotnet run --urls http://0.0.0.0:5000
```
> The app will be compiled and then started. It can take a while. Ignore any warnings that show up in the command window when starting.
> 
> The TCP-support for MR-Clock may require require permissions in your firewall. If you not being prompted, 
> see the **Appsettings.json** for which port numbers that are used for polling and broadcast.

4. After the App server is started, open a web browser and surf to the endpoint above. 
The client App will be downloaded and started automatically.

> During client app loading, a special page is shown. 
> If that page don't dissappear after max 30 seconds, your brower doesn't support Web Assembly.