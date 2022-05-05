---
layout: default
title: Totla System Latency
parent: OSRTT Guide
nav_oder: 11
---

## Total System Latency Testing

OSRTT also includes a handy measurement of the total system latency, in a similar way to tools like NVIDIA LDAT or NVIDIA Reflex Latency. More information about both tools is available [on TFTCentral here](https://tftcentral.co.uk/articles/nvidia_reflex).

It’s important to note that this is NOT capturing the monitor input lag on its own, which would be the lag of the display causing by the signal processing. That will be incorporated within the overall latency number somewhere, but this is basically capturing “click to photon” end to end latency. This can be influenced by many factors including graphics card, mouse, system settings, refresh rate, monitor etc. It’s a useful tool for measuring and comparing and optimising your end to end overall total system latency, but should NOT be used to represent the lag of the monitor (often referred to as “input lag”) as this cannot be isolated from within the measurement, and there are too many variables at play.

