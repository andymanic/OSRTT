---
title: 'What Is OSRTT?'

layout: null
---

### Open Source Response Time Tool - OSRTT

In short, OSRTT is my attempt at making an accessible tool for reviewers and enthusiasts to test monitor response times. I should stress, **it is far from perfect.** I've done my best to make it as reliable, accurate and feature rich as possible, but it's important to remember that it's a tool for "trained professionals" and to be used with the understanding that you may need to verify the results in some cases, and that there may be bugs I'm yet to address.

### Main Features
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

### Main Goals
* Transparency - raw data saved so you can verify the results manually
* Accuracy - the raw sensor data is accurate to 18us (0.018ms) and the processing code has months worth of checks 
* Easy to use - I've tried to account for many issues and edge cases so for a typical display it **should** be plug and play. Includes USB supply stability checking, brightness calibration & 
  processing.
