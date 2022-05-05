---
layout: default
title: Getting Started
nav_order: 3
---

# Getting Started
{: .no_toc }


Assuming you have an OSRTT unit to hand, here's what you'll need to do to get up and running.
{: .fs-6 .fw-300 }


## Install The Software

Head to [the releases page](https://github.com/andymanic/OSRTT/releases/latest) and download the "OSRTT Launcher.exe" file. When you run it Windows will likely say this is an untrusted file, you'll need to click the "more info" link/button, then "Run Anyway". I'll fix that as soon as I work out how... 

Anyway, run the installation wizard as usual. It will run the program after you finish (by default anyway). 

You'll be greeted with a message box asking if you want to let the program continue with further setup. You'll need to select yes to make the tool work. 

This installs required Arduino and Adafruit libraries and the required drivers too. You'll need to click "Install" on both driver pop-ups. 

## Settings

There are a whole bunch of settings everywhere - I'm working on consolidating these into a single easy-access place, but for the time being...

### Main Window - Response Time Test
- Framerate limit: This setting limits the in-game framerate during the test. This is most useful for testing Variable Refresh Rate (VRR), as you can set the FPS limit to below the monitor's refresh rate, forcing it to refresh slower than normal. Set this to equal or above the quoted refresh rate for normal testing.
- Number of test runs: This controls how many times you would like the test to repeat before averaging the data. The more runs, the more accurate the data is likely to be. Current maximum is 10.
- Capture time: Adjust how long per transition the device should capture light samples. Slower monitors, or while testing with VSync ON, should use a longer time (i.e. 150ms+).
- Vsync State: Toggle VSync on or off. On means more stable results, less prone to capture errors. Off means true input latency results,  but capture errors are more likely. 

### Main Window - Total System Latency Test
- Time between clicks: How much time should be left between mouse clicks during the test. 0.5s is default.
- Number of clicks: How many clicks should the test fire before finishing. Default is 20.

### Test Settings
This is all the settings described in the methodologies section - default is "Recommended".

