---
layout: default
title: Gamma Corrected Percentage
parent: OSRTT Guide
nav_oder: 7
---

## Option 3 – Using the original tolerance levels but now with gamma correction (10 - 90%)


There is of course the option to stick with the original 10 – 90% tolerance levels that have been used in the market for 20+ years. It is worth including the gamma correction to more accurately capture the response times relative to what you see in RGB values. This removes some of the errors in sticking only to the voltage reading on the graphs, and allows you to account for display gamma. 
 
![10-90% measurement on an oscilloscope](/assets/images/osrtt-guide-images/img8.png)

This can be selected within OSRTT using the following option:
![10% of RGB values setting in the OSRTT software](/assets/images/osrtt-guide-images/img9.png)
 
The limitations with this approach are:

1.	1)	Like with the 3 – 97% tolerance levels discussed above, using any % will result in the actual RGB tolerance being variable each time. In this instance when measuring from 0 > 100 RGB, this would mean you’re really measuring between RGB 10 and RGB 90. But if you measure from 0 > 200 RGB you are measuring between RGB 20 and RGB 180 because the total range is wider. So on the one hand, it is being more strict and only allowing an RGB tolerance of 10, and in the other it’s measuring a more relaxed tolerance of 20.

Visually you are not being consistent in the tolerance allowed to get “close enough” to the end shade. This is far more variable when using 10/90% tolerance levels. If you are going to the lengths of correcting for gamma in the response times in order to provide a better indication of visual experience, this variation may not be ideal.

**We do not recommend using a % tolerance level for the above reasons**

