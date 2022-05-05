---
layout: default
title: OSRTT Guide
nav_order: 4
has_children: true
permalink: docs/osrtt-guide
---

# OSRTT Guide – Understanding the Many Options, Deciding on Recommended Settings, and Ensuring Consistency Across Reviewers
### Issue 1.1, last update 4 May 2022
{: .no_toc }

## Introduction
The OSRTT provides a wide range of options and features designed to test all elements of a display’s performance for pixel transitions and response times. There are many ways to analyze the data, and many possible ways you may wish to present the data to your readers and viewers depending on what you are trying to capture and cover. To ensure there is consistency across the industry as much as possible, and clear guidance for an average consumer trying to make sense of the data, it’s important that we try to retain some level of uniformity if we can in the way this data is presented. 
This guide is to explain what the different options and measurements methods can do, how they might be interpreted and how ideally they should be presented to readers/viewers to achieve that consistent approach. It covers the “g2g” response time measurements as well as “overshoot” calculations.
The guide has been produced in conjunction with several established reviewers who already carry out this kind of testing, including **TFTCentral and Hardware Unboxed**. The aim is for it to represent an “industry standard” for this kind of display measurement if possible.

---

## TL;DR: Summary of Recommendations
-	Make sure you have consistency in your PC and display setup for this testing wherever possible (explained below)
-	When presenting G2G measurements please try and use the suggested names in this guide – please do not bundle everything under the name “response time” without explanation of what you are presenting as there are many different approaches possible
-	Please state which method you are using if possible including any tolerance levels etc as it helps ensure results can be compared between reviewers
-	The OSRTT ‘recommended settings’ option can be left selected if you wish although it is probably useful to understand what it is capturing and what will be output in the results as explained below. This mode uses:
    -	Gamma corrected response times
    -	The “complete response time” (this is always captured in the data, no matter what options you select)
    -	“Initial response time” (the bit before any overshoot – explained later) with a fixed RGB 5 offset tolerance level
    -	“Visual response time” (including any overshoot in the figure – explained later) with a fixed RGB 5 offset tolerance level
    -	Gamma corrected overshoot in RGB values
-	If you instead want to instead use other settings away from the ‘recommended settings’ mode you can select “advanced settings” in the menu, but please take note of the guide below when selecting options, and when presenting data to your audience.

### Guide written by Simon from [TFT Central](https://tftcentral.co.uk) and notes from Andrew - [TechteamGB](https://youtube.com/techteamgb)

{: .fs-6 .fw-300 }
