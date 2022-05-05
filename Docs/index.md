---
layout: default
title: Home
nav_order: 1
description: "Open Source Response Time Tool (OSRTT) Documentation."
permalink: /
---

# Installation
{: .fs-9 }
Ready to dive in? You can find the software built and ready to install, or the Github repo with all the open source code and designs.

{: .fs-6 .fw-300 }

[Get the Software](https://github.com/andymanic/OSRTT/releases){: .btn .btn-primary .fs-5 .mb-4 .mb-md-0 .mr-2 } [View the GitHub repo](https://github.com/andymanic/OSRTT){: .btn .fs-5 .mb-4 .mb-md-0 }

---

# Open Source Response Time Tool - OSRTT

In short, OSRTT is my attempt at making an accessible tool for reviewers and enthusiasts to test monitor response times. I should stress, **it is far from perfect.** I've done my best to make it as reliable, accurate and feature rich as possible, but it's important to remember that it's a tool for "trained professionals" and to be used with the understanding that you may need to verify the results in some cases, and that there may be bugs I'm yet to address.

## Main Features
* Response time testing
    - Default settings are set to give an easy to understand view of what the display is doing,     and to help drive the industry forward.
    - Settings are fully adjustable, differing tolerance levels, overshoot calculations and     output settings.
    - Input lag results included in response time results too!
* Input Lag Testing
    - Dedicated input lag testing mode - "Click to Photon Latency"
    - Can be adapted to in-game testing in future, as well as more data such as breakdown of USB polling delay (already included), frame processing and on-display lag.
* Raw data capture & reprocessing
    - All of the raw data from every test is saved so you can both manually verify the results in the included graph view template (or even have the program create a graph view file for you!), or re-process the raw data with different settings or with future revisions. This way if you want
    to change your published testing methodology, as long as you keep the raw data you can re-process the files and update your data without having to actually re-run the test with that display.
    - Included graph view template to easily cycle through the raw sensor data.
* Auto-updater for both the program and board firmware

## Main Goals
* Transparency - raw data saved so you can verify the results manually
* Accuracy - the raw sensor data is accurate to 18us (0.018ms) and the processing code has months worth of checks 
* Easy to use - I've tried to account for many issues and edge cases so for a typical display it **should** be plug and play. Includes USB supply stability checking, brightness calibration & 
  processing.

---

# I FOUND A BUG!! WHAT NOW?

I was expecting this... [SUBMIT A BUG REPORT ON GITHUB](https://github.com/andymanic/OSRTT/issues/new/choose)

Here's what I'll need to help:

```
    **Describe the bug**
    A clear and concise description of what the bug is.

    **To Reproduce**
    Steps to reproduce the behavior:
    1. Go to '...'
    2. Click on '....'
    3. Scroll down to '....'
    4. See error

    **Expected behavior**
    A clear and concise description of what you expected to happen.

    **Screenshots**
    If applicable, add screenshots to help explain your problem.

    **Desktop (please complete the following information):**
    - OS: [e.g. Windows 10]
    - Version [e.g. 22]

    **Additional context**
    Add any other context about the problem here.
```

Alternatively, if you have a feature request you can do that on [Github](https://github.com/andymanic/OSRTT/issues/new/choose) too! This is what I'll need to know:

```
**Is your feature request related to a problem? Please describe.**
A clear and concise description of what the problem is. Ex. I'm always frustrated when [...]

**Describe the solution you'd like**
A clear and concise description of what you want to happen.

**Describe alternatives you've considered**
A clear and concise description of any alternative solutions or features you've considered.

**Additional context**
Add any other context or screenshots about the feature request here.
```
