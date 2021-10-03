/*<img src="icon_small.png" width="200">*/

# OSRTT
Open Source Response Time Tool - LCD response time tool, includes hardware, firmware and software!

# Project Aim
The goal of this project is to offer an open source, cost effective solution to testing LCD monitor's input lag. The hardware as described in the circuit diagrams is required, as is a copy of the Windows Forms app. The hardware isn't too difficult to build if you'd like to do it yourself, and the software is fully open source so you are welcome to make alterations to suit your testing needs.

The C# based program handles all interaction with the hardware, primarily over Serial (over USB), both connecting to the board, updating the board's firmware (using the [Arduino CLI](https://github.com/arduino/arduino-cli), and recording results. It's also capable of setting how many repeats of the test it will do before averaging them out (max supported 10 repeats). 

My aim was to keep everything as transparent as possible, so when a test is begun it will create a numbered folder within the /Results folder using the monitor, refresh rate and connection you are using. It will save the raw results, as well as an interpolated gamma table and processed results to individual files for each run, then will output a "FINAL-DATA-OSRTT.csv" file that averages all the processed data. If you are unsure of a particular result, you are able to check each processed file to find any outliers. You can also analyse both individual files and whole folders of raw results by pressing the "Analyse Results" button in the top menu bar.

# Installation
- Download the latest zip file from the [releases](https://github.com/andymanic/OSRTT/releases) page. 
- Extract the OSRTT folder anywhere you like, personally I keep it on the desktop.
- Run the "OSRTT Launcher.exe" program. On first run it will ask to install additional files, openning a CMD window to run arduino CLI commands.

# Usage
- Assuming you have the hardware built, connect it via USB then run the launcher. **CONNECT IT DIRECTLY TO YOUR SYSTEM, OR AT LEAST A POWERED HUB**
- On the first launch on a new device, you will be prompted to allow installation of additional files. This is installing the Arduino & Adafruit SAMD cores and Adafruit board library in %APP_DATA%\Arduino15. It may take a minute or two, but is necessary to connect to the board.
- Once the launcher has connected to the device, you can set how many times the test should run. Default is 3.
- You can then launch the test program with the large button on the bottom. 
- Once the test program is up, press the button on the device. Use numbers 1 - 8 to limit the frame rate: 1 = 1000, 2 = 360, 3 = 240, 4 = 165, 5 = 144, 6 = 120, 7 = 100, 8 = 60
- Assuming the monitor's brightness is in the correct range, the device will run through the test sequence. By default it will run 3 times, although you can set that in the launcher. 
- Once the test is complete, check the "Results" folder, the highest numbered folder with your current monitor, refresh rate and connection is the most recent result. "FINAL-DATA-OSRTT.csv" includes a list of the starting and ending RGB values, response time and overshoot percentage (gamma corrected)
- If you'd like to manually analyse an individual file, or import an existing folder, click the "Analyse Results" button in the menu bar at the top, then use either button to select a file or folder to analyse.

# The Files
The desktop program saves 3 files per test run, plus a final averaged file at the end. So, by default the folder with the monitor's name, refresh rate and connection type will contain 3 "RAW" files, 3 "PROCESSED" files, and 3 "GAMMA" files.
- RAW: The raw files are the full sample list from each run. The file doesn't have column headers, but is arranged as: Initial RGB value; Final RGB value; Time taken to collect samples; Number of samples; Data - where data is ~6420 16 bit integers the correspond to ADC (analog-to-digital-converter) readings from the light sensor on the bottom of the device. 
- PROCESSED: The processed files are a column-headed table of the starting and ending RGB values, the response time in milliseconds and a gamma-corrected percentage of over/undershoot. Gamma corrected meaning instead of giving a percentage of how much extra light output there was, instead referencing that light level against the full gamma table and using the RGB value that light reading corresponds to as our comparison to what the final RGB value should be.
- GAMMA: Gamma files are an interpolated list of every RGB value from 0 to 255, calculated from the 11 'base' measurements the device takes during the test. These 'in-between' points currently aren't following a perfect trend line, instead are flat point-to-point additions so accuracy may be 1 or 2 RGB values off.

# Building from source
If you'd prefer to compile it yourself, you'll need to download the latest [Arduino CLI](https://github.com/arduino/arduino-cli), and place it in a folder called "arduinoCLI" in the working directory of the launcher. 

# Current hardware limitations
- ~160 nits of brightness is the recommended target using the included 3D printed casing. Different Z heights for the sensor to the display lead to differing maximum supported brightness levels and vary the noise level
- Currently the device records 50ms of samples, with between 2 and 10ms of that being prior to the transition starting. This is due to the int array needing to be allocated it's size at compile time rather than dynamically. Changing the array size and updating the board's firmware will solve this, the maximum size for the array I believe is 48000 (current 7000). Only change this if you are expecting transitions to take more than 35ms.
- The current design doesn't effectively filter all monitor backlight strobing frequencies. This doesn't affect the final results too heavily but is worth noting here.
- 10 repeats of the test are the maximum the current version of the firmware - this isn't a fixed limit and could be changed in future

# Current software limitations
- The program has a hand written algorithm for finding the start and end points of the transition. It bases this on 250 points at the start of the result and 400 points at the end. If the transition starts within that first 250 points, or ends during the final 400, the results may be slightly off. 
- The hardware has no way to cancel the test mid-run except for disconnecting the USB cable - I have some ideas that may help with that but not yet implemented.
- The hardware has no way of knowing if the test program (UE4 game) is the selected window. Again I've got ideas but not added any yet.
- The test pattern is currently fixed. The processing software in theory is written such that you can run as many or as few RGB values as you like, with the key difficulty being RGB values that are too close to eachother. RGB 0 to RGB 26 is already so close that the program has had a hard time picking a start and end point effectively. Anything smaller than this I would expect to break the program - although shorter sequences should work just fine. You'll need to modify the OSRTT_FULL_CODE.ino file, and if you want to use different RGB values rather than remove some, you'll also need to modify the Unreal Engine project too.
- The serial connection isn't perfect - I'm sure this could be made better so if you've done reliable data transfer over serial with C# please do have a look! My biggest issue is with the amount of data, it's likely upwards of 50KB per line so the in buffer size is now 64KB just in case.
- The hardware is well suited to doing input lag testing, but as of yet that's something I've not explored. I do aim to do that fairly soon, possibly with a "switch mode" option which will load a different .ino file rather than extending the response time file. Not sure yet.
- I'm not a professional developer, so I can guarantee this program doesn't conform to best practices. Don't worry, I'm not running the processing on the UI thread! I've tested everything before publication but if you do find any errors either in use or in the code please do let me know in an Issue or Pull Request

# Current human limitations
Please be patient when it comes to solving issues or implementing new feature requests. I'm only human, a bearly functional one at that. I struggle with mental and physical limitations and disabilities, and run both [TechteamGB](https://youtube.com/techteamgb) and [At The Wheel](https://youtube.com/c/atthewheel), and run the short linking platform [Locally Links](https://locallylinks.com), so please excuse any delays. 

# Support the project
If you would like to support this project, there are a number of both direct and indirect ways to help. As stated above, the main goal of this project is to create an accessible solution for testing monitor response times. I want as many reviewers and enthusiasts to have the ability to test this with a degree of accuracy that is currently painstaking to reach. 
- Non-monetary support: please do reach out to your favourite reviewers who don't currently test monitor response times and (politely!) ask them to check out the project. 
- Buy one prebuilt by me: I very much appreciate many won't have the equipment, skills or time to build these units themselves, which is why I'm happy to purchase the parts in bulk, solder everything up, flash the bootloader and firmware, and 3D print the case. I'm still finalising that process, so haven't got a final price as of yet but I'm expecting it to be around $100/£75. Ping me [an email](mailto:inbox@techteamgb.com) if you want one!
- Buy me a ~~Beer~~ - ~~Coffee~~ - Cookie. I like cookies. [Paypal](https://paypal.me/techteamgb) | [Patreon](https://patreon.com/techteamgb)

# Warranties & Guarantees
I should make it clear that this software, circuit diagrams, STL and Gerber files, and any hardware bundles I produce, are provided AS-IS. I can't guarantee I've accounted for every possibility or edge case, or even that what I've written will be infinitely reliable. I have it output every step of processing, so if you want to verify any result you can do so manually. 

# Licensing 
I've gone with the GNU GPL V3 License for the whole project, both hardware and software. That means you are free to use anything here as you like, on the condition that anything you make with this is also open source. If you'd like to use anything here in a closed-source project or if you'd like to manufacture the hardware elements commercially, please send me [an email](mailto:inbox@techteamgb.com). 
