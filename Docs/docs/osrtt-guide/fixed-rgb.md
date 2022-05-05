---
layout: default
title: Fixed RGB Tolerance
parent: OSRTT Guide
nav_oder: 8
---

## Option 4 – Fixing an RGB tolerance level with gamma correction (+/- RGB 10)

This the approach favoured by TFTCentral in their measurements. Gamma correction is used to provide a more realistic measurement of what you see visually, accounting for display gamma and avoiding the pitfalls and errors from the old “traditional response time” method.

To overcome the issues with using a % discussed in the above options 2 and 3, and having variable RGB tolerance depending on the size of the range as a result, a fixed RGB tolerance level is selected. Based on lengthily visual experimentation and testing, an RGB balance of +/- 10 was deemed appropriate to get “close enough” to the desired grey shade from a visual point of view without being overly strict or too relaxed.

The [TFTCentral article](https://tftcentral.co.uk/articles/response_time_testing) explains the approach and how they reached this method in more detail. This can be selected in OSRTT using the following option:
![RGB 10 setting in the OSRTT software](/assets/images/osrtt-guide-images/img10.png)
 
One possible concern with this approach is that for transitions that are close together (e.g. 0 – 51), there would not be much of the transition curve to measure, as you would be measuring from RGB 10 to RGB 41 in theory. This could still be considered “fair” visually since the two shades you are switching between are already very close together. Obviously if you were measuring something even closer like 0 > 20 RGB, you’d be measuring a range of 0! 

You could also instead use…

Option 5 – Fixing a tighter RGB tolerance level with gamma correction (+/- RGB 5)

Instead of using a tolerance of RGB 10, you could use a tighter RGB 5 tolerance. This captures more of the response time curve. This is the default option in the “recommended settings” although you may consider this overly strict from a visual point of view, you would have to decide whether you feel it is a better option than using RGB 10 as being “close enough” to the target grey shade.. 

If you want to use a fixed RGB 5 tolerance you can select it in the software using the following options:
![RGB 5 setting in the OSRTT software](/assets/images/osrtt-guide-images/img11.png) 

**We recommend using a fixed RGB tolerance level when measuring G2G response times, you can use either RGB 10 or RGB 5 depending on your preference. TFTCentral have settled with RGB 10 as explained in [their article here](https://tftcentral.co.uk/articles/response_time_testing). TechteamGB prefer RGB 5.**
