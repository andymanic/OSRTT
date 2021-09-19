# OSRTT
Open Source Response Time Tool - LCD response time tool, includes hardware, firmware and software!

# Project Aim
The goal of this project is to offer an open source, cost effective solution to testing LCD monitor's input lag. The hardware as described in the circuit diagrams is required, as is a copy of the Windows Forms app. The hardware isn't too difficult to build if you'd like to do it yourself, and the software is fully open source so you are welcome to make alterations to suit your testing needs.

The C# based program handles all interaction with the hardware, primarily over Serial (over USB), both connecting to the board, updating the board's firmware (using the [Arduino CLI](https://github.com/arduino/arduino-cli), and recording results. It's also capable of setting how many repeats of the test it will do before averaging them out (max supported 10 repeats). 

My aim was to keep everything as transparent as possible, so when a test is begun it will create a numbered folder within the /Results folder using the monitor, refresh rate and connection you are using. It will save the raw results, as well as an interpolated gamma table and processed results to individual files for each run, then will output a "FINAL-DATA-OSRTT.csv" file that averages all the processed data. If you are unsure of a particular result, you are able to check each processed file to find any outliers. You can also analyse both individual files and whole folders of raw results by pressing the "Analyse Results" button in the top menu bar.

# Usage
- Assuming you have the hardware built, connect it via USB then run the launcher.
- Once the launcher has connected to the device, you can launch the test program. 
- Once the test program is up, press the button on the device. Use numbers 1 - 7 to limit the frame rate: 1 = 1000, 2 = 360, 3 = 240, 4 = 165, 5 = 144, 6 = 120, 7 = 60
- Assuming the monitor's brightness is in the correct range, the device will run through the test sequence. By default it will run 3 times, although you can set that in the launcher. 
- Once the test is complete, check the "Results" folder, the highest numbered folder with your current monitor, refresh rate and connection is the most recent result. "FINAL-DATA-OSRTT.csv" includes a list of the starting and ending RGB values, response time and overshoot percentage (gamma corrected)

# Current hardware limitations
- ~160 nits of brightness is the recommended target using the included 3D printed casing. Different Z heights for the sensor to the display lead to differing maximum supported brightness levels and vary the noise level
- Currently the device records 50ms of samples, with between 2 and 10ms of that being prior to the transition starting. This is due to the int array needing to be allocated it's size at compile time rather than dynamically. Changing the array size and updating the board's firmware will solve this, the maximum size for the array I believe is 48000 (current 7000). Only change this if you are expecting transitions to take more than 35ms.
- The current design doesn't effectively filter all monitor backlight strobing frequencies. This doesn't affect the final results too heavily but is worth noting here.
- 10 repeats of the test are the maximum the current version of the firmware - this isn't a fixed limit and could be changed in future

# Current software limitations
- The program has a hand written algorithm for finding the start and end points of the transition. It bases this on 250 points at the start of the result and 400 points at the end. If the transition starts within that first 250 points, or ends during the final 400, the results may be slightly off. 
- I'm not a professional developer, so I can guarantee this program doesn't conform to best practices. Don't worry, I'm not running the processing on the UI thread! I've tested everything before publication but if you do find any errors either in use or in the code please do let me know in an Issue or Pull Request

# Current human limitations
Please be patient when it comes to solving issues or implementing new feature requests. I'm only human, a bearly functional one at that. I struggle with mental and physical limitations and disabilities, and run both [TechteamGB](https://youtube.com/techteamgb) and [At The Wheel](https://youtube.com/c/atthewheel), and run the short linking platform [Locally Links](https://locallylinks.com), so please excuse any delays. 

# Licensing 
I've gone with the GNU GPL V3 License for the whole project, both hardware and software. That means you are free to use anything here as you like, on the condition that anything you make with this is also open source. If you'd like to use anything here in a closed-source project, please send me [an email](mailto:inbox@techteamgb.com). 
