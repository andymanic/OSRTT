void loop() {
  //Delay600ns();
  Serial.setTimeout(100);
  getSerialChars();
  if (millis() == (loopTimer + 180000))
  {
    clearDisplayBuffer();
    loopTimer = millis();
  }
  buttonState = digitalRead(buttonPin);
  if (buttonState == HIGH)
  {
    // oledFourLines("CONNECTED", "TO", "DESKTOP", "APP");
    //digitalPotWrite(208);
    fillADCBuffer(10000, 100);
    for (int i = 0; i < 10000; i++)
    {
      Serial.print(adcBuff[i]);
      Serial.print(",");
    }
    Serial.println();
    
  }
  if (input[0] == 'A')
  {
    int arrSize = sizeof(RGBArr) / sizeof(int);
    Serial.print("RGB Array : ");
    for (int i = 0; i < arrSize; i++)
    {
      Serial.print(RGBArr[i]);
      Serial.print(",");
    }
    Serial.println();
  }
  else if (input[0] == 'B')
  {
    // Brightness Calibration screen
    Serial.setTimeout(200);
    int mod = input[1] - '0';
    int potVal = 1 + mod;
    digitalPotWrite(potVal);
    Serial.println("BRIGHTNESS CHECK");
    delay(500);
    int sample_count = 0;
    while (sample_count < 1000)
    {
      adcBuff[sample_count] = getSingleADCValue(); //save new ADC value to buffer @ sample_count position
      sample_count++; //Increment sample count
    }
    Serial.print("Stability:");
    for (int i = 0; i < sample_count; i++)
    {
      Serial.print(adcBuff[i]);
      Serial.print(",");
    }
    Serial.println();
    sample_count = 0;

    while (input[0] != 'X')
    {
      // Check serial for cancel or new potentiometer value
      getSerialChars();
      int in = 0;
      if (input[0] <= 57)
      {
        in = input[0] - '0'; // Convert char to int
      }
      else
      {
        in = input[0] - 55;
      }

      if (in >= 1 && in <= 15)
      {
        // Increment potentiometer value by multiples of 10 up to 220
        int add = 15 * in;
        potVal = 1 + add;
        digitalPotWrite(potVal);
      }
      else if (in == 0)
      {
        potVal = 1;
        digitalPotWrite(potVal);
      }
      int counter = 0;
      long value = 0;
      while (counter < 250)
      {
        value += getSingleADCValue();
        counter++;
      }
      value /= counter;
      Serial.print("Brightness:");
      Serial.print(value);
      Serial.print(":");
      Serial.println(potVal);
      delay(300);
    }
  }
  else if (input[0] == 'F')
  {
    Serial.println("FW:" + firmware);
  }
  else if (input[0] == 'I')
  {
    testRuns = input[1] - '0';
    delay(100);
    Serial.print("Runs:");
    Serial.println(testRuns);
    delay(100);
    Serial.print("BoardType:");
    Serial.println(boardType);
    delay(50);
    Serial.print("BoardType:");
    Serial.println(boardType);
    delay(100);
    Serial.println("FW:" + firmware);
    delay(100);
    Serial.print("FPS Key:");
    Serial.println(fpsLimit);
    delay(100);
    int arrSize = sizeof(RGBArr) / sizeof(int);
    Serial.print("RGB Array:");
    for (int i = 0; i < arrSize; i++)
    {
      Serial.print(RGBArr[i]);
      Serial.print(",");
    }

    Serial.println();
    UniqueIDdump(Serial);
    Serial.println("Handshake");
  }
  else if (input[0] == 'J')
  {
    Serial.println("Starting Pro Potentiometer Check");
    int count = 0;
    while (count < 256)
    {
      digitalPotWrite(count);
      Serial.println(count);
      //Serial.print(",");
      delay(100);
      adcBuff[count] = getSingleADCValue();
      //Serial.println(value);
      //delay(2000);
      count++;
    }
    Serial.print("PROADC:");
    for (int i = 0; i < 256; i++)
    {
      Serial.print(i);
      Serial.print(":");
      Serial.print(adcBuff[i]);
      Serial.print(",");
    }
    Serial.println();
    //checkLightLevel();
  }
  else if (input[0] == 'K')
  {
    int count = 1;
    Serial.println(count);
    while (count < 256)
    {
      buttonState = digitalRead(buttonPin);
      if (buttonState == HIGH) //Run when button pressed
      {
        digitalPotWrite(count);
        digitalPotWrite(count);
        //Serial.print(count);
        //Serial.print(",");
        delay(200);
        adcBuff[count] = getSingleADCValue();
        //Serial.println(value);
        //delay(2000);
        count*=2;
        Serial.println(count);
      }
      delay(50);
    }
    Serial.print("PROADC:");
    for (int i = 0; i < 256; i++)
    {
      Serial.print(i);
      Serial.print(":");
      Serial.print(adcBuff[i]);
      Serial.print(",");
    }
    Serial.println();
    //checkLightLevel();
  }
  else if (input[0] == 'M')
  {
    testRuns = input[1] - '0';
    delay(100);
    Serial.print("Runs:");
    Serial.println(testRuns);
  }
  else if (input[0] == 'L')
  {
    int hundreds = convertHexToDec(input[1]) * 100;
    int tens = convertHexToDec(input[2]) * 10;
    int ones = convertHexToDec(input[3]);
    fpsLimit = hundreds + tens + ones;
    delay(100);
    Serial.print("FPS Key:");
    Serial.println(fpsLimit);
  }
  else if (input[0] == 'G')
  {
    delay(2000);
    int brightnessTest = checkLightLevel();
    runGammaTest();
  }
  else if (input[0] == 'V')
  {
    int vState = input[1] - '0';
    if (vState == 0)
    {
      vsync = false;
    }
    else if (vState == 1)
    {
      vsync = true;
    }
    Serial.print("VSync:");
    Serial.println(vsync);
  }
  else if (input[0] == 'Q')
  {
    int extGammaState = input[1] - '0';
    if (extGammaState == 0)
    {
      extendedGamma = false;
    }
    else if (extGammaState == 1)
    {
      extendedGamma = true;
    }
    Serial.print("Extended Gamma:");
    Serial.println(extendedGamma);
  }
  else if (input[0] == 'N')
  {
    int length = input[1] - '0';
    if (length == 0) { GlobalSampleCount = 18326; } // 50ms
    else if (length == 1) { GlobalSampleCount = 36651; } // 100ms
    else if (length == 2) { GlobalSampleCount = 54976; } // 150ms
    else if (length == 3) { GlobalSampleCount = 73302; } // 200ms
    else if (length == 4) { // 250ms
      GlobalSampleCount = 18326; 
      GlobalSampleDelay = 1;
    } 
    else if (length == 5) { // 350ms
      GlobalSampleCount = 73877; 
      GlobalSampleDelay = 2;
    } 
    else if (length == 6) { // 500ms
      GlobalSampleCount = 64615; 
      GlobalSampleDelay = 5;
    } 
    else if (length == 7) { // 1s
      GlobalSampleCount = 72790; 
      GlobalSampleDelay = 11;
    } 
    // Serial.print("Sampling Time:");
    // Serial.println(samplingTime);
  }
  else if (input[0] == 'T')
  {
    Serial.println("Ready to test");
    oledFourLines("PRESS", "BUTTON", "TO START", "THE TEST");
    Serial.setTimeout(200);
    while (input[0] != 'X')
    {
      // Check if button has been pressed
      buttonState = digitalRead(buttonPin);
      if (buttonState == HIGH) //Run when button pressed
      {
        Serial.setTimeout(500);
        Keyboard.print((char)fpsLimit);
        Keyboard.print((char)fpsLimit);
        Keyboard.print('f');
        delay(100);
        oledFourLines("CHECKING", "FOR", "STROBING", "");
        int sample_count = 0;
        while (sample_count < 1000)
        {
          adcBuff[sample_count] = getSingleADCValue(); //save new ADC value to buffer @ sample_count position
          sample_count++; //Increment sample count
        }
        int minVal = 65520;
        int maxVal = 0;
        for (int i = 200; i < sample_count; i++)
        {
          if (adcBuff[i] < minVal)
          {
            minVal = adcBuff[i];
          }
          else if (adcBuff[i] > maxVal)
          {
            maxVal = adcBuff[i];
          }
        }
        oledFourLines("Strobing", "value:", F(maxVal - minVal), "");
        if ((maxVal - minVal) > 1000)
        {
          oledFourLines("BACKLIGHT", "STROBING,", "CONTINUE?", "PRESS BTN");
          bool btn = digitalRead(buttonPin);
          while (input[0] != 'X' && !btn)
          {
            getSerialChars();
            btn = digitalRead(buttonPin);
          }
          if (input[0] == 'X')
          {
            break;
          }
        }
        sample_count = 0;

        // Check monitor brightness level
        oledFourLines("CHECKING", "LIGHT", "LEVEL", "");
        int brightnessTest = checkLightLevel();
        if (brightnessTest == 0)
        {
          // If brightness too low or high, don't run the test
          Serial.println("Cancelling test");
          //digitalWrite(13, HIGH);
          digitalPotWrite(0x00);
          oledFourLines("FAILED TO", "CALIBRATE", "LIGHT", "LEVEL");
          break;
        }
        else
        {
          oledFourLines("CHECKING", "SYSTEM", "LATENCY", "");
          checkLatency();
          delay(100);
          Serial.println("Test Started");
          // Set FPS limit (default 1000 FPS, key '1')
          delay(50);
          oledFourLines("RUNNING", "GAMMA", "TEST", "");
          runGammaTest();
          delay(100);
          int runCount = 0;
          while (input[0] != 'X')
          {
            //oledFourLines("RUNNING", "FULL", "TEST", "");
            getSerialChars();
            if (input[0] == 'X')
            {
              break;
            }
            else if (input[0] == 'S')
            {
              int t = input[1] - '0';
              t++;
              
            }
            else
            {
              int currentIndex = 0;
              int nextIndex = 0;
              if (input[0] <= 57)
              {
                currentIndex = input[0] - '0'; // Convert char to int
              }
              else
              {
                currentIndex = input[0] - 55;
              }
              if (input[1] <= 57)
              {
                nextIndex = input[1] - '0'; // Convert char to int
              }
              else
              {
                nextIndex = input[1] - 55;
              }
              if (currentIndex == 0 && nextIndex == 1)
              {
                runCount++;
              }
              int arrSize = sizeof(RGBArr) / sizeof(int);
              if (currentIndex >= 0 && currentIndex < arrSize)
              {
                int current = RGBArr[currentIndex];
                int next = RGBArr[nextIndex];
                oledTestRunning(current, next, runCount);
                Keyboard.print(Keys[currentIndex]);
                delay(300);
                runADC(current, next, Keys[nextIndex], "Results: ", GlobalSampleCount, GlobalSampleDelay, GlobalStartDelay);
                delay(50);
                Serial.println("NEXT");
              }
            }
            delay(50);
          }
        }
        oledFourLines("TEST", "COMPLETE", "CHECK", "DESKTOP");
        digitalPotWrite(0x01);
        //}
      }
      else
      {
        for (int i = 0; i < INPUT_SIZE + 1; i++)
        {
          input[i] = ' ';
        }
        byte sized = Serial.readBytes(input, INPUT_SIZE);
        input[sized] = 0;
        if (input[0] == 'P')
        {
          while (input[0] != 'X' && input[0] != 'S')
          {
            getSerialChars();
            curr_time = micros(); //update current time
          }
        }
        else if (input[0] == 'X')
        {
          break;
        }
      }
    }
  }
  else if (input[0] == 'P')
  {
    // Input lag testing
    int clicks = 1;
    int timeBetween = 300;
    int totalTime = 100;
    delay(100);
    oledFourLines("SETTING","CLICK","COUNT","");
    Serial.println("IL Clicks");
    while (input[0] != 'X')
    {
      getSerialChars();
      if (input[0] != ' ')
      {
        int firstDigit = input[0] - '0';
        int secondDigit = input[1] - '0';
        if (firstDigit == 0)
        {
          clicks = (secondDigit * 10);
        }
        else
        {
          clicks = ((firstDigit * 100) + (secondDigit*10));
        }
        break;
      }
    }
    Serial.print("C: ");
    Serial.println(clicks);
    if (input[0] != 'X')  {
      oledFourLines("SETTING","TIME","BETWEEN","");
      Serial.println("IL Time");
      while (input[0] != 'X')
      {
        getSerialChars();
        if (input[0] != ' ')
        {
          int firstDigit = input[0] - '0';
          int secondDigit = input[1] - '0';
          if (firstDigit == 0)
          {
            timeBetween = secondDigit * 100;
          }
          else
          {
            firstDigit *= 1000;
            secondDigit *= 100;
            timeBetween = firstDigit + secondDigit;
          }
          if (timeBetween == 100)
          {
            totalTime = 100;
            timeBetween = 0;
          }
          else if (timeBetween == 200)
          {
            totalTime = 200;
            timeBetween = 0;
          }
          else
          {
            totalTime = timeBetween;
            timeBetween = timeBetween - 200;
          }
          Serial.print("Time between saved: ");
          Serial.println(timeBetween);
          break;
        }
      }
    }
    if (input[0] != 'X')  {
      Serial.setTimeout(200);
      digitalPotWrite(1);
      while (input[0] != 'X')
      {
        oledFourLines("PRESS","BUTTON","TO","START");
        buttonState = digitalRead(buttonPin);
        if (buttonState == HIGH) //Run when button pressed
        {
          oledFourLines("RUNNING","INPUT","LATENCY","TEST");
          Keyboard.print('1');
          delay(100);
          int sw = micros();
          for (int k = 0; k < clicks; k++)
          {
            runInputLagTest(totalTime);
            int sw2 = micros();
            if (sw2 < (sw + timeBetween))
            {
              delay((sw + timeBetween) - sw2);
            }
          }
          Serial.println("IL Finished");
          Keyboard.press(KEY_ESC);
            Keyboard.releaseAll();
          oledFourLines("LATENCY","TEST","FINISHED","");
          input[0] = 'X';
          break;
        }
        getSerialChars();
        delay(10);
      }
    }
  }
  else if (input[0] == 'O')
  {
    // Brightness Calibration screen
    Serial.setTimeout(200);
    int mod = input[1] - '0';
    int potVal = 1 + mod;
    digitalPotWrite(potVal);
    Serial.println("LIVE VIEW");
    delay(200);
    while (input[0] != 'X')
    {
      getSerialChars();
      if (input[0] == 'P')
      {

        long startTime = micros();
        long currentTime = micros();
        uint16_t times[16000];
        int count = 0;
        times[count] = currentTime - startTime;
        adcBuff[count] = getSingleADCValue();
        delayMicroseconds(250);
        count++;
        while (currentTime < (startTime + 3000000))
        {
          currentTime = micros();
          times[count] = currentTime - times[count - 1];
          adcBuff[count] = getSingleADCValue();
          delayMicroseconds(250);
          count++;
        }
        Serial.print("LiveData:");
        for (int i = 0; i < count; i++)
        {
          Serial.print(times[i]);
          Serial.print(":");
          Serial.print(adcBuff[i]);
          Serial.print(",");
        }
        Serial.println();
        Serial.println("End");
      }
      int in = 0;
      if (input[0] <= 57)
      {
        in = input[0] - '0'; // Convert char to int
      }
      else
      {
        in = input[0] - 55;
      }

      if (in >= 1 && in <= 15)
      {
        // Increment potentiometer value by multiples of 10 up to 220
        int add = 15 * in;
        potVal = 1 + add;
        digitalPotWrite(potVal);
        Serial.print("pot val:");
        Serial.println(potVal);
      }
      else if (in == 0)
      {
        potVal = 1;
        digitalPotWrite(potVal);
        Serial.print("pot val:");
        Serial.println(potVal);
      }
    }
  }
  else if (input[0] == 'W')
  {
    // long s = micros();
    // int counter = 10000;
    // for(int i = 0; i < counter; i++)
    // {
    //   adcBuff[i] = getSingleADCValue();
    // }
    // long e = micros();
    // Serial.print("Sample time: ");
    // float t = (e - s) / counter;
    // Serial.println(t);
    while (input[0] != 'X')
    {
      getSerialChars();
      if (digitalRead(buttonPin))
      {
        Serial.println("EXSTART");
        for (int p = 0; p < 256; p++)
        {
          digitalPotWrite(p);
          delay(10);
          long t = fillADCBuffer();
          Serial.print("EXSWEEP:");
          Serial.print(p);
          Serial.print(",");
          Serial.print(p);
          Serial.print(",");
          Serial.print(t);
          Serial.print(",10000,");
          for (int l = 0; l < 10000; l++)
          {
            Serial.print(adcBuff[l]);
            Serial.print(",");
          }
          Serial.println();
        }
        Serial.println("EXFIN");
      }
    }
  }
  else if (input[0] == 'Y')
  {
    int counter = 0;
    digitalPotWrite(0);
    while (input[0] != 'X')
    {
      if (digitalRead(buttonPin))
      {
        counter *= 2;
        if (counter == 0)
        {
          counter++;
        } else if (counter > 128)
        {
          counter = 0;
        }
        digitalPotWrite(counter);
        Serial.print("Current value:");
        Serial.println(counter);
        delay(300);
      }
      delay(10);
    }
  }
  else if (input[0] == 'Z')
  {
    int potVal = 1;
    uint16_t result = 0;
    Serial.println("PotVal,Result");
    for (int i = 0; i < 255; i++)
    {
      int counter = 0;
      digitalPotWrite(i);
      Serial.print(i);
      Serial.print(",");
      while (counter < 100)
      {
        result = getSingleADCValue(); //save new ADC value to buffer @ sample_count position
        counter++;
      }
      Serial.println(result);
      oledFourLines("POT VAL:", String(potVal), "VAL:", String(result));
    }
  }
}
