---
layout: default
title: Notes, warnings and comments
parent: OSRTT Guide
nav_order: 3
---

# Notes, warnings and comments
{: .no_toc }

1.	Displays that still feature PWM for backlight dimming (thankfully very rare nowadays) may create issues with the measurements since the brightness will fluctuate and this may mess up the calculations. It should be possible to still calculate response times but you may need to more closely validate or calculate the results using the ‘Graph View Template’ sheet. The software uses a moving average filter to reduce noise before processing the data. It’s adaptive based on how noisy the data is, although in some very niche cases it may fail to process, hence the need to check and validate your results.

2.	On some displays where the transitions reach towards black, and the contrast ratio of the display is low, you may find the results hard to capture. You may need to exclude these from your results table if the figures look odd. You can also check the data in the ‘Graph View Template’ sheet if you want

3.	Why not capture more transitions in the table? – after lots of experimentation and discussion we felt that more data was not necessarily better here. It over-complicated things for readers, added unnecessary time to testing, and created issues with measurements when transitions were very close together. We felt that the provided 30 transitions was the most appropriate sample set to use

4.	Please remember these measurements and numbers can never fully replace the need for subjective assessment in games, movies and other fast motion content. 

