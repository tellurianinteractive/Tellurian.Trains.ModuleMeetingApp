# Clock API
**Version 2** valid from app version 2.3.x. This API is incompatimble with previous version. Old clients will not work.

The API is intended for supporting many clock instances running in parallel. 

Any action that modify the clocks state required an *API-key* and often also the *clocks password*. 
All calls that requires an API-key returns *Unauthorized* if no or incorrect API-key or no or incorrect *password* is provided.
The API-key is configured in the servers settings. The clock password can be set when a new clock instance is created.

The API is English-only. Clients has the responsibility to translate to other languages.

The {*clock*} placeholder in the url:s is the name of the clock instance. 
If you request a non-existing clock you get *Not Found*. 
Only an administrator can create a new clock instance.

The {*server*} placeholder in the url:s represents a name or ip-address with an optional port number,
for examle *192.168.0.182:5001* or *telluriantrainsclocksappserver.azurewebsites.net*

You can also retrieve Swagger documentation at:

    GET https://{server}/swagger

## Get avaliable clocks
    GET https://{server}/api/clocks/available
```json
[
    "Demo",
    "Custom1",
    "Custom2"
]
```
## Get time and status
    GET https://{server}/api/clocks/{clock}/time
```json
{
    "name": "Demo",
    "weekday": "Wednesday",
    "time": "06:02",
    "duration": 15,
    "isRunning": false,
    "isRealtime": false,
    "isCompleted": false,
    "message": "",
    "speed": 5.5,
    "isUnavailable": false,
    "realEndTime": "13:14",
    "fastEndTime": "21:00",
    "isPaused": false,
    "pauseTime" : "12:00",
    "pauseReason": "Dinner",
    "expectedResumeTimeAfterPause": "13:00",
    "stoppedByUser": "",
    "stoppingReason": "SelectStopReason"
}
```
- **weekday** - { None | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday }
- **duration** - is total game duration in hours. May have fraction of hours; 15.5 means 15 1/2 hour.
- **pauseReason** - { None | Breakfast | Lunch | Dinner | Meeting | Closin }
- **stopReason** - see **Stop Clock** below.
- **isUnavailable** - this is always false. Clock app should use it internally to signal that API is not available.
- **realEndTime** - this time includes time for pause if both *pauseTime* and *expectedResumeTimeAfterPause* is specified.
## Start clock
    PUT https://{server}/api/clocks/{clock}/start?apiKey={anApiKey}&user={userOrStationName}&password={clockPassword}

- **User- or station name** is required when the user that stopped the clock wants to start it again. Should be url-encoded if it contains non-ASCII characterns (like **åäø**).
- **Password** the clocks administrator password is required if an someone else than the user that stopped the clock should start the clock.

## Stop clock
    PUT https://{server}/api/clocks/{clock}/stop?apiKey={anApiKey}&user={userOrStationName}&reason={aReason}

- **User- or station name** should be url-encoded if it contains non-ASCII characterns (like **åäø**). Returns *BadRequest* if not provided.
- **Reason** should be one of the strings below. Returns *BadRequest* if other value is provided.
    - **StationControl** - problems with operating a station.
    - **PointProblem** - problems with one or several points.
    - **TrackProblem** - Problem with track , for example module joints.
    - **BoosterError** - Booster not working or shortcuts that hinders operation.
    - **LocoNetError** - Problems with LocoNet cabling and/or signals.
    - **CablingError** - Other cabling error.
    - **CentralError** - Problems with the digital central(s).
    - **Delays** - Sever delays in operation requires stopping clock to catch up.
    - **DriverShortage** - Duties become unassigned after their start time requires clock to be stopped.
    - **Derailment** - A derailment that take some time to fix.
    - **Other** - Other unspecified reason.

## Get clock users
    GET https://{server}/api/clocks/{clock}/users?apiKey={anApiKey}&password={clockPassword}
```json
[
    {
        "ipAddress": "192.168.0.182",
        "userName": "Munkeröd",
        "lastUsedTime": "2020-06-22 21:25:28"
    },
    {
        "ipAddress": "192.168.0.201",
        "userName": "Kalle",
        "lastUsedTime": "2020-06-22 21:25:26"
    }
]
```

## Get clock settings
    GET https://{server}/api/clocks/{clock}/settings
```json
{
    "name": "Demo",
    "shouldRestart": false,
    "isElapsed": false,
    "isRunning": false,
    "startWeekday": "0",
    "startTime": "06:00",
    "speed": 5.5,
    "durationHours": 15,
    "pauseTime": "",
    "pauseReason": "0",
    "expectedResumeTime": "",
    "showRealTimeWhenPaused": false,
    "overriddenElapsedTime": "",
    "message": "",
    "mode": "0",
    "password": "password"
}
```


## Update clock settings
    POST https://{server}/api/clocks/{clock}/Update?apiKey={anApiKey}&user={userOrStationName}&password={clockPassword}

Payload in post is same as in **Get clock settings**.
Sending a request with a non-existing *clock name* creates a new clock instance with that name.
