# Release Notes
## Updating old client
If you get the message **Invalid Client** you need to update it.
The reason is that the version that has been cached by your browser is not longer valid.
To fix it, do one of the following:
- If you run it in a browser you need to do a *hard reload*, 
often holding SHIFT down while refreshing the site.
- If you have installed a previous version on your desktop or homescreen, 
you need to uninstall it and then do a *hard reload* in your web browser to clear the apps old cached data in your browser.
- If you have added the app to the homescreen on iPad or iPhone, 
you must delete website data in order to upgrade. 
You can do this is available under *Safari's Advanced settings*.

## Keep settings over time
For integrity reasons, many browsers usually clears also its local storage when the broswer is closed.
If you want to keep your clock settings, you need to change your browsers integrity settings.

In your brower settings, create an exception for clearing data on exit for the web site *https:[]()//fastclock.azurewebsites.net*.
This will keep your clock settings including your username and selected clock name.

## Releases
### Release 3.3.0
Release date 2023-06-14
- **Slovak** translation added. This is a preliminary translation that need to be checked and improved.
- **App name changed** to *Fast Clock* with appropriate translations to all supported languages (issue #55).
- **User connecting** to clock server now logged (issue #50).
- **Bug fix** of menu icon displacement by having a shorter app name and less margins (issue #53).
- **Bug fix** of digital time truncated in small displays (issue #49).
- **Bug fix** of app failed to load in local installations (issue #54). HSTS and requirements for HTTPS can now be disabled.
- **Firewall workaround** introduced in release 3.2.10 is removed because it requires a *service worker* and HTTPS, that doesn't work locally.
- **WIKI** updated.
### Release 3.2.24
Release date 2023-06-06
- **Bug fix** of resetting not removing the *stopped reason* message.
### Release 3.2.23
Release date 2023-05-20
- **Loading progress** of application added.
### Release 3.2.22
Release date 2023-04-27
- **User name** can be set at start: *https:[]()//fastclock.azurewebsites.net/user/myusername*.
  The user name will be stored in local storage.
### Release 3.2.21
Release date 2023-04-16
- **Analouge Clock** has improved display with adapting size to various conditions. Thanks to Detlef Born for reporting this.
- **Component update** to latest versions.
### Release 3.2.10
Release date 2023-03-22
- **Firewall workaround** for cases when running the client app over a corporate network that detects false positives downloading the app.
- **Settings instructions** updated.
- **Component update** to latest Microsoft patches.
### Version 3.2.9
Release date 2023-02-17
- **French translation** completed missing parts.
- **Norwegian translation** completed missing *nynorsk* parts.
- **Added stop reasons** *Vehicle shortage* plus meals.
- **Time size** automatically adjust after message length.
- **Component update** to latest versions.
### Version 3.2.8
Release date 2022-12-01
- **App Language** can now be selected at the start page. It overrides your browser preference.
- **Bug fix** of showing *Unknown* as user name.
- **Bug fix** of issue #51 pause time counts also if pause starts after end game time.
- **Update** to .NET 7.0.
### Version 3.2.7
Release date 2022-09-26
- **Message display** improved display so messages are visible also when time is maximised.
- **Documentation** page added. This page contains links all other documentation and the WIKI.
### Version 3.2.6
Release date 2022-09-22
- **Bug fix** of users registering for clock.
- **Logging** improved, which is useful when running clock locally.
### Version 3.2.5
Release date 2022-09-20
- **Bug fix** of day changed after showing real time.
### Version 3.2.4
Release date 2022-07-18
- **Clock user list** fixed.
### Version 3.2.3
Release date 2022-07-17
- **Security update** with latest patches from Microsoft.
### Version 3.2.2
Release date 2022-03-27
- **Appsettings.json** removed obsolete settings. This apply for local installation.
- **Index.html** has relevant information at app startup, not just *Loading...*.
### Version 3.2.1
Release date 2022-03-12
- **Bug fix** of missing translations causing app to crash.
- **Clock name** can be specified in the URL. Example: *https:[]()//fastclock.azurewebsites.net/clock/myclockname*.
- **Display of messages** improved to make room for two lines.
### Version 3.2.0
Release date 2022-03-08
- **Update interval** now defaults to 2 seconds (previous 1 second), and will increase to max 2 minutes if repeaded errors occurs when requesting time.
This is to save compute and bandwidth resources. 
- **Second hand** removed from analogue clock. 
It is problematic to make it run in a predicable way when update interval is more than 1 second.
### Version 3.1.2
Release date 2022-01-31
- **Bug fix** of settings page **Save**-button, whih did'nt save under some circumstances.
- **New clock** button is now visible only if the user has given a *Username* under **Settings**.
- **Support for MR-Clock** clients is dropped. 
This support does not work in the cloud-server version anyway, and has only been used for testing purposes in local installation. 
Because the clock-app client runs on all web browsers supporting webassembly, the need for supporting MR-Clock clients is also depricated.

### Version 3.1.1
Release date 2021-11-13
- **Analogue clock** second hand now moves more smoothly. Thanks to Michael Bunka for the idea.
- **App performance** improved by Ahead-of-time compilation to web assenbly.

### Version 3.1.0
Release date 2021-11-09 - Maintenance release
- **Analogue clock** now sychronizes the second hand faster when displaying it again after inactivity.
- **Upgrade to RTM release of .NET 6**
- **Code cleanup** as a consequence of upgrading to .NET 6 RTM.
### Version 3.0.0
Release date 2021-10-26
- **Analogue clock** displaying a typical *Raiway Clock* inclucing animating ticking of seconds. Select *Analogue* in **Settings**.
- **Hungarian translation** completed.
- **French translation** added. The app now supports **13** European languages.
- **Available clocks check** in *Settings* and *Administration* pages. These now resets to the *Demo* clock if the registered clock is no longer available.
- **Vehicle breakdown** added as new *stop reason*.
- **API changes** with new way to create and update clocks settings. This only affects administrators of clocks.
- **Bug fix** of unintentionally creating clocks with random administrator password.
- **Open API documentation** corrected with additional response codes.
- **Upgrade to Microsoft .NET 6.0**.
### Version 2.6.8
Release date 2021-10-02
- **Lighter blue** colour for showing real time givs better contrast in dark theme.
- **Restart**-button is changed to **Reset**. 
- **Bug fix** of unhandled error getting language sprecific texts from unavailable server.
### Version 2.6.7
Release date 2021-09-25
- **Fix of restart button** so it appears once the clock is elapsed.
- **Italian translation** has few wordings corrected.
### Version 2.6.6
Release date 2021-06-17
- **Service release** with updated .NET components to latest security patch.
### Version 2.6.5
Release date 2021-06-08
- **Bug fixes** in clock administration page. You can now create a new clock without being administrator of the *Demo clock*.
- **Local installation** is now modified after testing to build a local clock server running .NET 5.0.
- **Bug fix** of setting clock to real-time mode.
- **Bug fix** of displaying OpenAPI documentation at */openapi*.
### Version 2.6.4
Release date 2021-03-16
- **German translation** improved. Thanks to Franz Hennies.
### Version 2.6.3
Release date 2021-02-23
- **Bug fix** of submitting invalid data in *Clock Administration* page (thanks to Peter Alsén)
### Version 2.6.2
Release date 2021-02-14
- **Redesign** of *Clock Admin* page.
- **Bug fix** of vertical scrolling.
- **Bug fix** of not able to update clock settings.
### Version 2.6.1
Release date 2021-02-12
- **User interface improvements** with better message feedback on user actions.
- **Layout fixes** of dark theme and clock time display.
- **Upgrade** to latest fixes of .NET 5.0.
### Version 2.6.0
Release date 2020-12-24
- **Open API** metadata endpoint changed from "*/swagger/v2/swagger.json*" to "*/openapi/v2/openapi.json*". This is a breaking change.
- **API error messages** improved and standardized. At any error, the API returns an error message with a detailed description of what went wrong and some help to solve it.
- **Translations** supplemented for missing items. All supported langauges are now full translated using *Google Translate* and *Apertium*.
Help improve them! See [current issues](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/issues).
### Version 2.5.7
Release date 2020-12-21
- **Added Italian** translations that may be improved.
- **Added Finnish** translations that may be improved.
- **Localization improvements** by moving texts in *Settings* page to *markdown* for easier translation. 
### Version 2.5.6
Release date 2020-12-18
- **Bug fix** of *clock name* not showning up in clock users list (issue #25).
- **Bug fix** of *Settings* button not showning upp for new user in start page (issue #26).
- **Bug fix** of Open API/Swagger endpont (issue #27)
- **Bug fix** of missing vertical scroll (issue #28).
### Version 2.5.5
Release date 2020-12-17
- **New start page** that contains a *Quick Start* guide, which is more relevant to most users.
- **Added Czech** translations, which may need to be improved and supplemented.
- **Localization improvements** by moving all language specific blocks of text content in *Index* and *About* pages
to *markdown* files, located server side and served via API. This makes it easier to translate these text blocks.
- **Easier to create new clock** by a new button on the *Clock Administration* page. 
Now you don't have to be administrator of the *Demo*-clock to create a new one any longer.
- **Settings** is a more appropriate new name for the *Register* page.
- **Tooltip** added to buttons in the *Clock Administration* page that clarifies usage.
### Version 2.5.4
Release date 2020-12-14
- **Major refactoring** and cleaning up code for preparation to new functionality in near future.
- **Improved translation** to Norweigan *nynorsk*, the other official Norwegian language besides *bokmål*.
- **Updated Wiki** where all documentation except README and RELEASENOTES is moved to 
the [Wiki](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki).
### Version 2.5.3
Release date 2020-12-10
- **Bug fix** where registered passwords where registered as new clocks!
- **Security fix in API** where getting the settings for a clock now also requires *administrator password*. This is a breaking change!
Otherwise the *administrator password* was available without having the passsword.
### Version 2.5.2
Release date 2020-12-09
- **Clock time** font changed, hopefully improve reading on longer distances.
- **German** translations improved by *Michael Bunka*. Thanks!
- **Message text** maximum length increased from 50 to 60 charatcters.
- **API clarification** regarding *password* usage.
- **Chromium on Raspberry Pi OS** is verified running the Clock App.
### Version 2.5.1
Release date 2020-11-25
- **Larger clock time** can now be achieved, suitable when displaying on a screen that has no user interaction.
- **Fixed**: Decimals in game duration.
### Version 2.5.0
Release date 2020-11-20
- **Upgraded to .NET 5.** No functional changes.
### Version 2.4.2
Release date 2020-09-15
- **Translations adjusted**
### Version 2.4.1
Release date 2020-08-22
- **Only admin can start the clock** first time and after a pause. 
This is a fix server side that does not affect the client application 2.4.0.
### Version 2.4.0
Release date 2020-08-04. This is a breaking change!
- **API change** where the *apiKey* is is no longer required because a user is now must provide a password to change a clock. 
This change requires client version 2.4.0 or later.
### Version 2.3.3
Release date: 2020-06-24 afternoon
- **Inactive clocks** are automatically removed 48 hours after last used.
- **Inactive clock users** are automatically removed after 30 minutes.
- **Username must be unique**, but same username is permitted from several clients on the same IP-address.
### Version 2.3.2
Release date: 2020-06-24 morning
- **Bug fix**: The administrator can now change clock password.
- **Bug fix**: The real time now shows correct weekday.
- **User password** can now be set by the administrator to protect anyone from stop and start the clock. 
Any user can still see any clock, so all users don't have to register. The user register either the user password or the administrator password.
### Version 2.3.1
Release date: 2020-06-23
- **Improved user interface** in start page and users page.
### Version 2.3.0
Release date: 2020-06-23
- **Reworked API** to better adhere to web api conventions. The old clients are incompatible with this 
[new API](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/API-Guidelines).
- **Extended API documentation** with all available methods and included which HTTP verb to use. 
- **Clock users** are now collected per clock instance and is available in the API.
### Version 2.2.4
Release date: 2020-06-22
- **Changed client and server compatibility** to only depend on major and minor version numbers.
- **Bug fix** of not able to update clock settings.
### Version 2.2.3
Release date: 2020-06-21
- **API-documentation added** at /swagger.
- **Input validation** in Clock Aministration page.
- **Bug fix** in Clock Polling Server.
### Version 2.2.2
Release date: 2020-06-20
- **Background colour**: Whole page now fills with correct background colour.
- **Drop down boxes** now shows with correct styles on iPhone and iPad.
- **Theme improvements** for both *light* and *dark* themes.
- **Translations** of pause reason adjusted.
### Version 2.2.1
Release date: 2020-06-19
- **Usage improvements** in registration and administration.
- **Refined theme display** and added missing translation.
### Version 2.2.0 
Release date: 2020-06-18
- **Option to select a *dark theme***. This saves battery on mobile devices. The theme takes effect after reloading the app.
- **Client version control**. Only the lastest client version will work. Otherwise you will get a prompt to update.
- **Improved and extended translations**. Some labels are changed to better reflect the measning of a field.
- **Administrator manual** english only. You find it [here](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/Administrators-Manual).
- **Separate local installation manual**. You find it [here](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp/wiki/Local-Installation).
- **Default clock changed to Demo clock**. Dont use the Demo clock for real meetings, always create a separate named clock.
- **Demo clock's password cannot be changed**. It is always *password*.
### Version 2.1.6
Release date: 2020-06-16
Improvements for the cloud version.
- **Now runs *Central European Standard Time***. This means that the real time is correct for most European countries.
- **Better information when a non-existing clock name is referred**.
- **Clocks password now only visible for the administrator!**
### Version 2.1.5
Release date: 2020-06-13
- **Support for multiple clocks in cloud version**. Any number of clocks can now run in parallel.
### Version 2.1.4
Release date: 2020-06-06
- **Extended API with time for break**
- **Added paus information under clocks *show more*** so everyone can see when a pause will occur.
### Version 2.1.3
Release date: 2020-06-05
1. **Updated README** with guides how to install and run the clock locally.
2. **Updated API documentation** 
3. **Added coffe pause** as a reason to pause the game. A nice Swedish tradition.
4. **Added Derailment** as a reason to stop the clock. Is intended for situations when it takes some time to get all wagons on track again.
5. **Limit who can restart clock**. Only administrator or the user that stopped the clock can resume game time again.
### Version 2.1.2
Release date: 2020-06-02:
1. **Minor corrections** of documentation.
### Version 2.1.1
Release date: 2020-06-02
1. **Sound files configurable**. They can now be set in *appsettings.json*. This only works when runninn locally.
1. **Added translations** to Norweigan(nynorsk), Dutch and Polish.
### Version 2.1.0
Release date: 2020-05-28
1. **Corrected side meny with**. Pull request from [Frank Geerlings](https://github.com/frankgeerlings).
2. **Corrected spelling**. Pull request from [Frank Geerlings](https://github.com/frankgeerlings).
3. **Reworked API** and added API-keys that is required to modify clock status.
### Version 2.0.2
Release date: 2020-05-26
1. **Corrections and refinements**.
### Version 2.0.0
Release date: 2020-05-24
Initial release. 
1. **Porting code from the experimental version** and upgrading to the official release of Blazor 3.2.0.
2. **Made code open source** as promised during development of the experimental version.
