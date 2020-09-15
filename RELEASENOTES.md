# Release Notes
> NOTE: Clients 2.3.x will not work correctly due to API change.
> Older clients 2.2.x and older will hang **Please wait...**. 
> You must do a hard reload or eventually clear the apps data in your browser.
## Version 2.4.2
Release date 2020-09-15
- **Translations adjusted**
## Version 2.4.1
Release date 2020-08-22
- **Only admin can start the clock** first time and after a pause. 
This is a fix server side that does not affect the client application 2.4.0.
## Version 2.4.0
Release date 2020-08-04. This is a breaking change!
- **API change** where the *apiKey* is is no longer required because a user is now must provide a password to change a clock. 
This change requires client version 2.4.0 or later.
## Version 2.3.3
Release date: 2020-06-24 afternoon
- **Inactive clocks** are automatically removed 48 hours after last used.
- **Inactive clock users** are automatically removed after 30 minutes.
- **Username must be unique**, but same username is permitted from several clients on the same IP-address.
## Version 2.3.2
Release date: 2020-06-24 morning
- **Bug fix**: The administrator can now change clock password.
- **Bug fix**: The real time now shows correct weekday.
- **User password** can now be set by the administrator to protect anyone from stop and start the clock. 
Any user can still see any clock, so all users don't have to register. The user register either the user password or the administrator password.
## Version 2.3.1
Release date: 2020-06-23
- **Improved user interface** in start page and users page.
## Version 2.3.0
Release date: 2020-06-23
- **Reworked API** to better adhere to web api conventions. The old clients are incompatible with this [new API](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/blob/master/API.md).
- **Extended API documentation** with all available methods and included which HTTP verb to use. 
- **Clock users** are now collected per clock instance and is available in the API.
## Version 2.2.4
Release date: 2020-06-22
- **Changed client and server compatibility** to only depend on major and minor version numbers.
- **Bug fix** of not able to update clock settings.
## Version 2.2.3
Release date: 2020-06-21
- **API-documentation added** at /swagger.
- **Input validation** in Clock Aministration page.
- **Bug fix** in Clock Polling Server.
## Version 2.2.2
Release date: 2020-06-20
- **Background colour**: Whole page now fills with correct background colour.
- **Drop down boxes** now shows with correct styles on iPhone and iPad.
- **Theme improvements** for both *light* and *dark* themes.
- **Translations** of pause reason adjusted.
## Version 2.2.1
Release date: 2020-06-19
- **Usage improvements** in registration and administration.
- **Refined theme display** and added missing translation.
## Version 2.2.0 
Release date: 2020-06-18
- **Option to select a *dark theme***. This saves battery on mobile devices. The theme takes effect after reloading the app.
- **Client version control**. Only the lastest client version will work. Otherwise you will get a prompt to update.
- **Improved and extended translations**. Some labels are changed to better reflect the measning of a field.
- **Administrator manual** english only. You find it [here](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/blob/master/MANUAL.md).
- **Separate local installation manual**. You find it [here](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/blob/master/INSTALLATION.md).
- **Default clock changed to Demo clock**. Dont use the Demo clock for real meetings, always create a separate named clock.
- **Demo clock's password cannot be changed**. It is always *password*.
## Version 2.1.6
Release date: 2020-06-16
Improvements for the cloud version.
- **Now runs *Central European Standard Time***. This means that the real time is correct for most European countries.
- **Better information when a non-existing clock name is referred**.
- **Clocks password now only visible for the administrator!**
## Version 2.1.5
Release date: 2020-06-13
- **Support for multiple clocks in cloud version**. Any number of clocks can now run in parallel.
## Version 2.1.4
Release date: 2020-06-06
- **Extended API with time for break**
- **Added paus information under clocks *show more*** so everyone can see when a pause will occur.
## Version 2.1.3
Release date: 2020-06-05
1. **Updated README** with guides how to install and run the clock locally.
2. **Updated API documentation** 
3. **Added coffe pause** as a reason to pause the game. A nice Swedish tradition.
4. **Added Derailment** as a reason to stop the clock. Is intended for situations when it takes some time to get all wagons on track again.
5. **Limit who can restart clock**. Only administrator or the user that stopped the clock can resume game time again.
## Version 2.1.2
Release date: 2020-06-02:
1. **Minor corrections** of documentation.
## Version 2.1.1
Release date: 2020-06-02
1. **Sound files configurable**. They can now be set in *appsettings.json*. This only works when runninn locally.
1. **Added translations** to Norweigan(nynorsk), Dutch and Polish.
## Version 2.1.0
Release date: 2020-05-28
1. **Corrected side meny with**. Pull request from [Frank Geerlings](https://github.com/frankgeerlings).
2. **Corrected spelling**. Pull request from [Frank Geerlings](https://github.com/frankgeerlings).
3. **Reworked API** and added API-keys that is required to modify clock status.
## Version 2.0.2
Release date: 2020-05-26
1. **Corrections and refinements**.
## Version 2.0.0
Release date: 2020-05-24
Initial release. 
1. **Porting code from the experimental version** and upgrading to the official release of Blazor 3.2.0.
2. **Made code open source** as promised during development of the experimental version.
