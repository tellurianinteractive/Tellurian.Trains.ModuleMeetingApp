## Frequently Asked Questions

**Can anyone stop and start the clock?**

Its up to the *clock administrator* to decide. 

There are two password for a clock: 
the *administrator password* that only the *clock administrators* should know, and
the *user password* that none, some or all users may know.

If the *clock administrator* sets the *user password*,
only users that the *clock administrator* tells that password
may register it in order to stop and start the clock. 

Without a *user password*, anyone can stop and start the clock.

Regardless of if the *user password* is set or not, 
only the *clock administrator* can initially start the clock.
Also, only the *clock administrator* can restart the clock after a *pause*.

**What if we want several clocks at the same time?**

Any person can create a new clock with a unique name,
and become *administrator* of that clock.
Each such clock runs completley independent from each other.
It is recommended to always create a new clock at any meeting,
and not use the *Demo* clock.

User can se available clocks and select which clock to display in the app.
This is made on the *Registration* page in the app.
Once saved, the app remembers the selected clock next time
the user starts the app in the same browser.

**My clock has disappeard!**

The *cloud version* runs in a free hosting site and becomes
inactive after some time.
When someone starts the app after this happens,
all clocks but the *Demo* clock is gone.
Clocks also disappears after an update of the *cloud version*.
As long as any *client* is active, the *cloud version*
will also be active.

**We want to use other clocks!**

First, reconsider the need for other clocks. This clock:
- runs on any device with a modern web browser.
- has a number of features that are not supported on other clocks.
- has support for the most common FREMO languages.

If you still want to use other clocks there are a number of options:
- Integrate the other clock by getting time/status from the open API. 
For example, integrations has already been made with JMRI.
- If you have *MR Clock* clients, run the *Clock Server App* locally. 
It has built in support for *MR Clock* clients.

Integrating *analouge clocks* is possibe. 
These clocks ticks by sending electrical pulses for each minute.
When resetting time when starting a new game, *fast pulsing*
the clock to the start time must be considered.

**It's not working!**

The **Clock App** has been througly tested and used at meetings without
any problems.

If you have trouble getting it to work, it is probably caused by some
mistake you made. Please, consult the documentation again.

An *real* error is often indicated in bottom of the web browser window.
The *error* is often logged in the *console* window of the web browsers
develompent tools.

If you find that the **Clock App** misbehaves, you are welcome open an issue at
[the Clock App GitHub repository](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp).

