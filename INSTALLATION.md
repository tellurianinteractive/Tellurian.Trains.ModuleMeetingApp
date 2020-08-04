# Clock Server App Manual

## Local installation
The app can also be run locally. 
The advantage of running locally is that sound and that TCP-support for MR-Clock can be activated.
Currently, only source code distribution is supported. 

### Source code installation
In order to run the clock from source code, do the following steps:

1. Download and install [NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core). 
Select the version appropriate for your operating system. 
You can run .NET on many different operating systems, not only Windows.
2. Clone or pull the [source code](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp.git) to a local folder on your computer.
3. Go to the folder **App/Server** (and <u>not</u> *Clock.Server*), open a command prompt there and execute the following command:

```
    dotnet run --urls http://0.0.0.0:5000
```
> The app will be compiled and then started. It can take a while. Ignore any warnings that show up in the command window when starting.
> 
> External access to the app over HTTP and TCP-support for MR-Clock may require require permissions in your firewall. 
> If you not being prompted, 
> see the **Appsettings.json** for which port numbers that are used for polling and broadcast.

4. After the App server is started, open a web browser and surf to http://localhost:5000. 
The client App will be downloaded and started automatically.
5. Other users surf to your computers IP-address and same port number, for example to http://192.168.0.182:5000.


> During client app loading, a special page is shown. 
> If that page don't dissappear after max 30 seconds, your brower doesn't support Web Assembly.

### Binary installation

.NET Core can run om many platforms with or without having .NET installed on beforhand. 
This creates too many different deployment options to produce and deploy.
Instead, you can create binary distributions of choice.

First, do the **source code installation** described above.
Below are some samples how to build a release as a selfcontained execulable, that runs without having .NET installed on beforehand.

> Build for Apple OSX
```
    dotnet publish -c Release -r osx-x64 -o C:\Deploy\osx -p:PublishSingleFile=true -p:PublishTrimmed=true 
```

> Build for Windows 10
```
    dotnet publish -c Release  -r win-x64 -o C:\Deploy\windows -p:PublishSingleFile=true -p:PublishTrimmed=true
```

In order to run the commands above, you need to have the .NET Core 3.1 SDK installed on your build machine. 
The builded app folder can just be copied to any other machine and run there without having .NET installed.
For more information, see https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish.

## Settings
The default settings for the clock server is *appsettings.json* located in the **App/Server**-folder.
Change the appropriate settings as needed, for example *StartTime*, *Duration* and *Speed*.
You can also enable sound and TCP *Polling* and/or *Mulicast* support for *MR-Clock*.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ClockServerOptions": {
    "Name": "Demo",
    "Password": "password",
    "StartTime": "06:00",
    "Duration": "15:00",
    "Speed": 5.5,
    "TimeZoneId": "Central Europe Standard Time",
    "Sounds": {
      "PlayAnnouncements": false,
      "StartSoundFilePath": "Sounds\\Ringtone.wav",
      "StopSoundFilePath": "Sounds\\Ringtone.wav"
    },
    "Polling": {
      "IsEnabled": false,
      "PortNumber": 2500
    },
    "Multicast": {
      "IsEnabled": false,
      "IPAddress": "239.50.50.20",
      "PortNumber": 2000,
      "LocalPortNumber": 0,
      "IntervalSeconds": 2
    }
  }
}

```
