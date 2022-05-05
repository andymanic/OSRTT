---
layout: default
title: Overshoot
parent: OSRTT Guide
nav_oder: 9
---

## Overshoot Calculations

Above covers the pixel transition times and “G2G” figures. The other aspect that can be measured by OSRTT is “overshoot”. On a response time measurement graph the overshoot would be represented by the peak where the shade reaches beyond the desired grey shade, before it then drops back down to where it was supposed to be. This can also appear on the downward “fall time” as well at the end of the curve which would be “undershoot”.
 
There are a few options included within the software for this:
![Overshoot graph shown from an oscilloscope](/assets/images/osrtt-guide-images/img12.png) 

1.	**Gamma corrected (RGB values) overshoot – RECOMMENDED** - this will measure how many RGB values the pixel transition overshoots the desired shade. This is the recommended approach. Select RGB values (Gamma Corrected) > Raw RGB values in the settings as shown below.

2.	**RGB value Percentage** – while not within the official “traditional response time” method defined in the industry 20+ years ago, the long standing approach has been to use a % to define how much the shade overshoots its target. This has always been based on light level readings, without gamma correction. Hopefully you can see why switching to RGB values and gamma correction is useful for these measurements.

If you really wanted you could do this as a % overshoot in RGB values as well, although it is not recommended. This can be selected as well within the software for completeness, but again has issues with variation. For instance if you measure 0 > 50 RGB and the shade overshoots by a fairly modest and hard to distinguish 10 RGB values, that would be a 20% overshoot and would be considered “high”. That difference of only 10 RGB values is not very visible in practice. If the transition being measured was 0 > 200 RGB instead and it overshoots by the same 10 RGB values, that would now only be 5% overshoot, even though it’s the exact same RGB difference. This is why the % method is flawed and creates issues with overshoot presentation.

![RGB 5 setting in the OSRTT software](/assets/images/osrtt-guide-images/img13.png) 

The light level % can also be selected within the overshoot section if you really want to:
![RGB 5 setting in the OSRTT software](/assets/images/osrtt-guide-images/img14.png) 
 
If you are experimenting with using % for overshoot there are two options available. This wil calculate the % depending on whether 1) you’re only considering how far beyond the end voltage value it goes. i.e only the target final voltage is considered along with the overshoot voltage, or 2) You’re considering the overshoot as a portion of the overall curve (end-start) setting. i.e. the range of the transition is considered in the calculation. Again these methods are not recommended for accurate overshoot calculations. 

**The recommendation for a fairer reflection of overshoot is using the gamma corrected “RGB value overshoot” option.**
