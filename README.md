# OSRTT
Open Source Response Time Tool - LCD response time tool, includes hardware, firmware and software!

<a href="https://andymanic.github.io/OSRTTDocs/" target="_blank">![View the docs](view-the-docs.png)</a>

# Project Aim
The goal of this project is to offer an open source, cost effective solution to testing LCD monitor's input lag. The hardware as described in the circuit diagrams is required, as is a copy of the Windows Forms app. The hardware isn't too difficult to build if you'd like to do it yourself, and the software is fully open source so you are welcome to make alterations to suit your testing needs.

The C# based program handles all interaction with the hardware, primarily over Serial (over USB), both connecting to the board, updating the board's firmware (using the [Arduino CLI](https://github.com/arduino/arduino-cli), and recording results. It's also capable of setting how many repeats of the test it will do before averaging them out (max supported 10 repeats). 

My aim was to keep everything as transparent as possible, so when a test is begun it will create a numbered folder within the /Results folder using the monitor, refresh rate and connection you are using. It will save the raw results and processed results to individual files for each run, then will output a "FINAL-DATA-OSRTT.csv" file that averages all the processed data. You can now use the Results View form to both view the data as heatmaps, and the raw data in graph form.

# Installation
- Download the latest zip file from the [releases](https://github.com/andymanic/OSRTT/releases) page. 
- Extract the OSRTT folder anywhere you like, personally I keep it on the desktop.
- Run the "OSRTT Launcher.exe" program. On first run it will ask to install additional files, openning a CMD window to run arduino CLI commands.

# Usage
- Assuming you have the hardware built, connect it via USB then run the launcher. **CONNECT IT DIRECTLY TO YOUR SYSTEM, OR AT LEAST A POWERED HUB**
- On the first launch on a new device, you will be prompted to allow installation of additional files. This is installing the Arduino & Adafruit SAMD cores and Adafruit board library in %APP_DATA%\Arduino15. It may take a minute or two, but is necessary to connect to the board.
- Once the launcher has connected to the device, you can set how many times the test should run, what framerate limit to use, if VSync should be enabled and for how long the device should capture each transition.
- You can then launch the test program with the large button at the top of the section.
- Once the test program is up, press the button on the device. Use numbers 1 - 8 to limit the frame rate: 1 = 1000, 2 = 360, 3 = 240, 4 = 165, 5 = 144, 6 = 120, 7 = 100, 8 = 60
- Assuming the monitor's brightness is in the correct range, the device will run through the test sequence. By default it will run 3 times, although you can set that in the launcher. 


# Building from source
If you'd prefer to compile it yourself, you'll need to download the latest [Arduino CLI](https://github.com/arduino/arduino-cli), and place it in a folder called "arduinoCLI" in the working directory of the launcher. 

# Current hardware limitations
- ~160 nits of brightness is the recommended target using the included 3D printed casing. Different Z heights for the sensor to the display lead to differing maximum supported brightness levels and vary the noise level
- The current design doesn't effectively filter all monitor backlight strobing frequencies. This doesn't affect the final results too heavily but is worth noting here.
- 10 repeats of the test are the maximum the current version of the firmware - this isn't a fixed limit and could be changed in future

# Current software limitations
- The processing algorithm doesn't currently measure cumulative deviation. Luckily I don't think it'd be all that hard to add, and since all the raw data gets saved already you can always re-import any existing folders to re-process that data with any changes to the alogrithm!
- The serial connection isn't perfect - I'm sure this could be made better so if you've done reliable data transfer over serial with C# please do have a look! My biggest issue is with the amount of data, it's likely upwards of 50KB per line so the in buffer size is now 64KB just in case.

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
