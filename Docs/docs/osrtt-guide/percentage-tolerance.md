---
layout: default
title: Percentage Tolerance
parent: OSRTT Guide
nav_oder: 7
---

## Option 2 – Using a tight tolerance level to capture nearly all the initial response time curve (3 – 97%) but excluding overshoot

At the time of writing in Mar 2022 this is the method currently used by Hardware Unboxed and discussed in [their video here](https://www.youtube.com/watch?v=-Zmxl-Btpgk&feature=youtu.be). The desire is to capture as much of the response curve as possible, but eliminating some of the problems discussed above with getting very close to the desired shade, removing the issue with the slow tapering off, and also ignoring the overshoot impact in the response time figure itself. Overshoot is captured as a separate table of data, it is not just ignored.
 
![3-97% measurement on an oscilloscope](/assets/images/osrtt-guide-images/img6.png)

This can be selected within OSRTT using the following option:
![3% of RGB values setting in the OSRTT software](/assets/images/osrtt-guide-images/img7.png)
 
This avoids some of the issues in presenting the “complete response time” but also has some limitations of its own:

1.	The % will result in the actual RGB tolerance being variable each time. For instance when measuring from 0 > 100 RGB as a transition, this would mean you’re really measuring between RGB 3 and RGB 97 values. But if you measure from 0 > 200 RGB you are measuring between RGB 6 and RGB 194 because the total range is wider. So on the one hand, it is being more strict and only allowing an RGB tolerance of 3, and in the other it’s measuring a slightly more relaxed tolerance of 6. 

This doesn’t make a huge difference to the resulting measurements as really it’s trying to capture nearly the whole “total transition time” anyway, but it does cause some variance.

2.	Like with the “total transition time” approach, it doesn’t necessarily consider the visual difference as well and whether a shade gets “close enough” sooner than the strict tolerance levels used, especially when measuring transitions close together. It may also in some cases capture portions of the slow tapering off in the grey shade, where it may be visually indistinguishable in practice. 

**We do not recommend using a % tolerance level for the above reasons**

