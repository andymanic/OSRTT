---
layout: default
title: Visual Response Rating
parent: OSRTT Guide
nav_oder: 11
---

## Visual Response Rating

**Please note this is a work in progress and subject to change and improvements**

Especially when presenting just the “perceived response time” (which incorporates the overshoot as well as explained above), you lose a potentially important piece of information which is how quickly the display can move away from the initial colour - regardless of any overshoot. This is important as while the total transition time is incredibly key, from a visual perspective, you are likely to have a better viewing experience on a panel that is fast to reach it’s target colour, even if it misses it slightly, than a display that takes the same complete transition time but is just much slower to get there.

The “Visual Response Rating” has also been added as a potentially useful additional metric. It is a finite score rather than a direct measurement. The calculation is pretty simple, it’s: “100 – (Initial Response Time + Perceived Response Time)”. Since both metrics are using the same tolerance level, if a display doesn’t overshoot both times will be identical. This essentially rewards displays that are fast with a small amount of overshoot over displays that aren’t as fast even if they don’t overshoot at all – while still overall preferring ultra-fast, accurate monitors. 

Some examples may help explain – these are theoretical rather than direct measurements:

|Initial Response Time | Perceived Response Time | Visual Response Rating |
|----------------------|-------------------------|------------------------|
|2.8ms	|2.8ms|	94.4|
|2.8ms	|5.6ms|	91.6|
|5.6ms	|5.6ms|	88.8|
|3.8ms	|9.1ms|	87.1|
|10.8ms	|10.8ms|	78.4|

