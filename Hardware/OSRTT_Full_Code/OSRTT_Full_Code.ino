#include <Keyboard.h>
#include <Mouse.h>
#include <SPI.h>
#include <ArduinoUniqueID.h>
#define INPUT_SIZE 2

//Test values
// These are the RGB values it tests with
int RGBArr[] = {0,51,102,153,204,255};
// These are the keys that correspond with the above values
char Keys[] = {'1','2','3','4','5','6'};
// Ideally I'd pair these together in an array of key/value pairs but this works for now. 
// These are the RGB values it tests with
int OldRGBArr[] = {0,26,51,77,102,128,153,179,204,230,255};
// These are the keys that correspond with the above values
char OldKeys[] = {'q','a','z','w','s','x','e','d','c','r','f'};
// Ideally I'd pair these together in an array of key/value pairs but this works for now. 
int extGammaArr[] = {0,17,34,51,68,85,102,119,136,153,170,187,204,221,238,255};
char extGammaKeys[] = {'1','7','8','2','9','0','3','q','w','4','e','a','5','s','d','6',};

//ADC values
unsigned long curr_time = micros();
uint32_t sample_count = 0;
//50ms sample time default
unsigned long samplingTime = 50000;
uint16_t adcBuff[16000];

//Button values
int buttonState = 0;
const int buttonPin = 2;

//Digital Potentiometer values (SPI)
byte address = 0x02;
int CS = 10;
SPISettings settingsA(10000000, MSBFIRST, SPI_MODE0);

//Serial connection values
bool connected = false;
String firmware = "BETA 3.0";
int testRuns = 4;
bool vsync = true;
bool extendedGamma = true;
char fpsLimit = '1';
int USBV = 0;

void ADC_Clocks() // Turns out to be superfluous as Adafruit wiring.c already sets these clocks. Keeping for now to guarantee settings are set.
{
   MCLK->APBDMASK.bit.ADC0_ = 1;
   MCLK->APBDMASK.bit.ADC1_ = 1;
   //Use GCLK1, channel it for ADC0, select DFLL(48MHz) as source and make sure no divider is selected
   GCLK->PCHCTRL[ADC0_GCLK_ID].reg = GCLK_PCHCTRL_GEN_GCLK1_Val | (1 << GCLK_PCHCTRL_CHEN_Pos); // enable gen clock 1 as source for ADC channel
   GCLK->PCHCTRL[ADC1_GCLK_ID].reg = GCLK_PCHCTRL_GEN_GCLK1_Val | (1 << GCLK_PCHCTRL_CHEN_Pos); // enable gen clock 1 as source for ADC channel
}

void ADC_Init(Adc *ADCx)
{
   //////////////////////////////////////////////////////////////////////////
   //     Initialise ADC - Defaults set to 16bit results @ ~129KSPS        //
   //////////////////////////////////////////////////////////////////////////

  ADCx->INPUTCTRL.bit.DIFFMODE = 0;
  while( ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_INPUTCTRL );
  
  ADCx->INPUTCTRL.bit.MUXPOS = 0;
  while( ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_INPUTCTRL );

  //Divide 8MHz clock - default DIV4 = 2MHz ADC clock
  ADCx->CTRLA.bit.PRESCALER = ADC_CTRLA_PRESCALER_DIV4_Val;

  //Choose 16-bit resolution to oversample - use 12BIT_VAL for 1MSPS
  ADCx->CTRLB.bit.RESSEL = ADC_CTRLB_RESSEL_16BIT_Val;
  //Ensuring freerun is activated
  //Freerun means it automatically reads the next result rather than waiting for this code to tell it to check.
  ADCx->CTRLB.bit.FREERUN = 1;

  //waiting for synchronisation
  while(ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_CTRLB);

  //Sampletime set to 0
  ADCx->SAMPCTRL.reg = 0;

  //Waiting for synchronisation
  while(ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_SAMPCTRL);

  // Accumulate samples to gain higher final result precision  
  ADCx->AVGCTRL.reg = ADC_AVGCTRL_SAMPLENUM_16_Val; //Sample 16 values for 16 bit output, 8 for 15 bit, 4 for 14 bit

  //Wait for synchronisation
  while(ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_AVGCTRL)

  //Select VDDANA (3.3V chip supply voltage as reference)
  ADCx->REFCTRL.reg = ADC_REFCTRL_REFSEL_INTVCC1;

  //Enable ADC
  ADCx->SWTRIG.bit.START = 1;
  ADCx->CTRLA.bit.ENABLE = 1;

  //wait for ADC to be ready
  while(ADCx->SYNCBUSY.bit.ENABLE);
}

int checkLightLevel() // Check light level & modulate potentiometer value 
{
  //Keyboard.write('f');
  //delay(400);
  int potValue = 160;
  digitalPotWrite(potValue);
  delay(200);
  ADC0->SWTRIG.bit.START = 1; //Start ADC 
  while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
  int value = ADC0->RESULT.reg;
  while(value <= 63000 || value >= 64000)
  {
    ADC0->SWTRIG.bit.START = 1; //Start ADC 
    while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
    value = ADC0->RESULT.reg;
    if (value >= 64000)
    {
      //Set digital pot to decrease voltage
      potValue -= 1; //or the other way...
      digitalPotWrite(potValue);
      Serial.print("Value: ");
      Serial.print(value);
      Serial.print(", Pot Value: ");
      Serial.println(potValue);
    }
    else if (value <= 63000)
    {
      //Set digital pot to increase volt
      potValue += 1; //or the other way...
      digitalPotWrite(potValue);
      Serial.print("Value: ");
      Serial.print(value);
      Serial.print(", Pot Value: ");
      Serial.println(potValue);
    }
    else
    {
      break;  
    }
    if (potValue <= 159 || potValue == 255)
    {
      Serial.print("TEST CANCELLED - LIGHT LEVEL:");
      Serial.println(value);
      Keyboard.write('q');
      return 0;
      break; //not needed?  
    }
    delay(20);
  }
  Keyboard.write('q');
  delay(400);
  return 1;
}

void runADC(int curr, int nxt, char key, String type) // Run test, press key and print results
{
    // Set next colour
    //Keyboard.print(key);

    curr_time = micros(); //need to run this in case board is left connected for long period as first run won't read any samples
    unsigned long start_time = micros();
    
    //50ms worth of samples @100ksps= 5000 samples
    //50ms worth of samples @129ksps = 6451 samples
    digitalWrite(3, HIGH);
    /////////////////////////////////////////////////////////////
    //                    Take ADC Readings                    //
    /////////////////////////////////////////////////////////////
    // Loop for samplingTime microseconds
    while(curr_time <= (start_time + (samplingTime - 1)))
    {
      ADC0->SWTRIG.bit.START = 1; //Start ADC 
      while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
      adcBuff[sample_count] = ADC0->RESULT.reg; //save new ADC value to buffer @ sample_count position  
      sample_count++; //Increment sample count
      curr_time = micros(); //update current time
    }
    digitalWrite(3,LOW);
    ADC0->SWTRIG.bit.START = 0; //Stop ADC 
  
    /////////////////////////////////////////////////////////////
    //                       Print Readings                    //
    /////////////////////////////////////////////////////////////
    // Get how long the samples took to capture in microseconds
    int timeTaken = curr_time - start_time;
    //Print from & to RGB values, time taken, sample count and all captured results
    Serial.print(type);
    Serial.print(curr);
    Serial.print(",");
    Serial.print(nxt);
    Serial.print(",");
    Serial.print(timeTaken);
    Serial.print(",");
    Serial.print(sample_count);
    Serial.print(",");
    for (int i = 0; i < sample_count; i++)
    {
        Serial.print(adcBuff[i]);
        Serial.print(",");
    }
    Serial.println();
    
    sample_count = 0; //reset sample count
    
    //As using Serial slows the adc down, we take the time after Serial was used.
    curr_time = micros();
}

void digitalPotWrite(int value)
{
  SPI.beginTransaction(settingsA);
  digitalWrite(CS,LOW);
  SPI.transfer(address);
  SPI.transfer(value);
  digitalWrite(CS,HIGH);
  SPI.endTransaction();  
}

int checkUSBVoltage(int l) // Check USB voltage is between 4.8V and 5.2V
{
  int counter = 0;
  while (counter < l)
  {
    ADC1->SWTRIG.bit.START = 1; //Start ADC1 
    while(!ADC1->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
    adcBuff[counter] = ADC1->RESULT.reg;
    counter++; 
  }
  Serial.print("USB V:");
  for (int i = 0; i < counter; i++)
      {
        Serial.print(adcBuff[i]);
        Serial.print(",");
      }
  Serial.println();
  Serial.println();
  ADC1->SWTRIG.bit.START = 0; //Stop ADC 
  return 1;
}

void runGammaTest()
{
    Serial.println("G Test Starting");
    if (extendedGamma)
    {
        int arrSize = sizeof(extGammaArr) / sizeof(int);
        for (int i = 0; i < arrSize; i++)
        {
          Keyboard.print(extGammaKeys[i]);
          delay(200);
          runADC(extGammaArr[i], extGammaArr[i], extGammaKeys[i], "Gamma: ");
          delay(200);
        }
    }
    else
    {
        int arrSize = sizeof(RGBArr) / sizeof(int);
        for (int i = 0; i < arrSize; i++)
        {
          Keyboard.print(Keys[i]);
          delay(200);
          runADC(RGBArr[i], RGBArr[i], Keys[i], "Gamma: ");
          delay(200);
        }
    }
    Serial.println("G Test Complete");
}

void runInputLagTest(int timeBetween)
{
  int sampleTime = 200000;
  if (timeBetween == 100)
  {
    sampleTime = 100000;
  }
  curr_time = micros();
  unsigned long clickTime = micros();
  Mouse.click(MOUSE_LEFT);
  unsigned long start_time = micros();  
  while(curr_time <= (start_time + (sampleTime - 1)))
  {
    ADC0->SWTRIG.bit.START = 1; 
    while(!ADC0->INTFLAG.bit.RESRDY); 
    adcBuff[sample_count] = ADC0->RESULT.reg;  
    sample_count++; 
    curr_time = micros(); 
  }
  ADC0->SWTRIG.bit.START = 0; 
  int timeTaken = curr_time - start_time;
  Serial.print("IL:");
  Serial.print(start_time - clickTime);
  Serial.print(",");
  Serial.print(timeTaken);
  Serial.print(",");
  Serial.print(sample_count);
  Serial.print(",");
  for (int i = 0; i < sample_count; i++)
  {
    Serial.print(adcBuff[i]);
    Serial.print(",");
  }
  Serial.println();
    
  sample_count = 0; //reset sample count
    
  curr_time = micros();
  Mouse.click(MOUSE_RIGHT);
}

void checkLatency() {
  delay(100);
  Keyboard.print('Q');
  delay(100);
  runADC(1000,1000,'F',"TL:");
  char input[INPUT_SIZE + 1];
  unsigned long startTime = micros();
  while (curr_time < (startTime + 3000))
  {
    curr_time = micros();
    for (int i = 0; i < INPUT_SIZE + 1; i++)
    {
      input[i] = ' ';
    }
    byte sized = Serial.readBytes(input, INPUT_SIZE);
    input[sized] = 0;  
    if (input[0] == 'X')
    {
      break;
    }
    else if (input[0] == 'S')
    {
      int t = input[1] - '0';
      t++;
      samplingTime = 50000 * t;
      break;
    }  
  }
}

void setup() {
  pinMode (CS, OUTPUT);
  pinMode (3, OUTPUT);
  SPI.begin();
  
  digitalPotWrite(0x80);
  ADC_Clocks();
  ADC_Init(ADC0); //Initialise ADC0
  ADC_Init(ADC1); //Initialise ADC1
  Serial.begin(115200); //Open Serial connection at 115200 baud
  while(!Serial); //Wait for Serial to be connected
  Keyboard.begin(); //Open keyboard connection over USB
  pinMode(buttonPin, INPUT_PULLUP); //Button input on pin 2
  pinMode(13, OUTPUT); //Onboard LED
  Serial.println("Begin...");
}

void loop() {
    Serial.setTimeout(1000);
    char input[INPUT_SIZE + 1];
    for (int i = 0; i < INPUT_SIZE + 1; i++)
    {
      input[i] = ' ';
    }
    byte size = Serial.readBytes(input, INPUT_SIZE);
    input[size] = 0; 
    if (input[0] == 'A')
    {
      connected = true;
      int arrSize = sizeof(RGBArr) / sizeof(int);
      Serial.print("RGB Array : ");
      for(int i = 0; i < arrSize; i++)
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
      int potVal = 165 + mod;
      digitalPotWrite(potVal);
      Serial.println("BRIGHTNESS CHECK");
      delay(500);
      int sample_count = 0;
      while(sample_count < 1000)
      {
        ADC0->SWTRIG.bit.START = 1; //Start ADC 
        while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
        adcBuff[sample_count] = ADC0->RESULT.reg; //save new ADC value to buffer @ sample_count position  
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
          for (int i = 0; i < INPUT_SIZE + 1; i++)
          {
            input[i] = ' ';
          }
          byte sized = Serial.readBytes(input, INPUT_SIZE);
          input[sized] = 0;
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
            int add = 2 * in;
            potVal = 165 + add;
            digitalPotWrite(potVal);  
          }
          else if (in == 0)
          {
            potVal = 165;
            digitalPotWrite(potVal);
          }
          int counter = 0;
          long value = 0;
          while (counter < 10)
          {
          ADC0->SWTRIG.bit.START = 1; //Start ADC 
          while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
          value += ADC0->RESULT.reg;
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
      Serial.println("FW:" + firmware);
      delay(100);
      Serial.print("FPS Key:");
      Serial.println(fpsLimit);
      delay(100);
      int voltageTest = checkUSBVoltage(2000);
      int arrSize = sizeof(RGBArr) / sizeof(int);
      Serial.print("RGB Array:");
      for(int i = 0; i < arrSize; i++)
      {
        Serial.print(RGBArr[i]);
        Serial.print(",");
      }
      
      Serial.println();
      UniqueIDdump(Serial);
      Serial.println("Handshake");
    }
    else if (input[0] == 'U')
    {
      checkUSBVoltage(10000);
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
      fpsLimit = input[1];
      delay(100);
      Serial.print("FPS Key:");
      Serial.println(fpsLimit);
    }
    else if (input[0] == 'G')
    {
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
      if (length == 0)
      {
        samplingTime = 50000;
      }
      else if (length == 1)
      {
        samplingTime = 100000;
      }
      else if (length == 2)
      {
        samplingTime = 150000;
      }
      else if (length == 3)
      {
        samplingTime = 200000;
      }
      else if (length == 4)
      {
        samplingTime = 250000;
      }
      Serial.print("Sampling Time:");
      Serial.println(samplingTime);
    }
    else if (input[0] == 'T')
    {
      Serial.println("Ready to test");
      Serial.setTimeout(200);
      while (input[0] != 'X')
      {
        // Check if button has been pressed
        buttonState = digitalRead(buttonPin);
        if (buttonState == HIGH) //Run when button pressed
        {
          Serial.setTimeout(500);
          
            
              //checkLatency();
              delay(100);
              Serial.println("Test Started");
              // Set FPS limit (default 1000 FPS, key '1')
              //delay(50);
              //runGammaTest();
              //delay(100);
              while (input[0] != 'X')
              {
                for (int i = 0; i < INPUT_SIZE + 1; i++)
                {
                  input[i] = ' ';
                }
                byte sized = Serial.readBytes(input, INPUT_SIZE);
                input[sized] = 0;  
                if (input[0] == 'X')
                {
                  break;
                }
                else if (input[0] == 'S')
                {
                  int t = input[1] - '0';
                  t++;
                  samplingTime = 50000 * t;
                }
                else if (input[0] == 'G')
                {
                  int rgb = 0;
                  if (input[0] <= 57)
                  {
                    rgb = input[0] - '0'; // Convert char to int  
                  }
                  else
                  {
                    rgb = input[0] - 55;
                  }
                  runADC(rgb, rgb, ' ', "Gamma: ");
                }
                else if (input[0] == 'L')
                {
                  runADC(1000,1000,' ',"TL:");
                }
                else if (input[0] == 'R')
                {
                  int brightnessTest = checkLightLevel();
                  if (brightnessTest == 0)
                  {
                    // If brightness too low or high, don't run the test
                    Serial.println("Cancelling test");
                    digitalWrite(13, HIGH); 
                    digitalPotWrite(0x80);
                    break;
                  }
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
                  int arrSize = sizeof(RGBArr) / sizeof(int); 
                  if (currentIndex >= 0 && currentIndex < arrSize)
                  {
                    int current = RGBArr[currentIndex];
                    int next = RGBArr[nextIndex];
                    //Keyboard.print(Keys[currentIndex]);
                    //delay(300);
                    runADC(current, next, Keys[nextIndex], "Results: ");
                    delay(50);
                    Serial.println("NEXT");
                  }
                }
                delay(50);  
              }
            
            digitalPotWrite(0x80);
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
              for (int i = 0; i < INPUT_SIZE + 1; i++)
              {
                input[i] = ' ';
              }
              byte sized = Serial.readBytes(input, INPUT_SIZE);
              input[sized] = 0; 
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
      Serial.println("IL Clicks");
      while (input[0] != 'X')
      {
        for (int i = 0; i < INPUT_SIZE + 1; i++)
        {
          input[i] = ' ';
        }
        byte sized = Serial.readBytes(input, INPUT_SIZE);
        input[sized] = 0; 
        if (input[0] != ' ')
        {
          int firstDigit = input[0] - '0';
          int secondDigit = input[1] - '0';
          if (firstDigit == 0)
          {
            clicks = secondDigit;
          }
          else
          {
            clicks = ((firstDigit * 10) + secondDigit);
          }
          break;
        }
      }
      if (input[0] != 'X')  { 
        Serial.println("IL Time");
        while (input[0] != 'X')
        {
          for (int i = 0; i < INPUT_SIZE + 1; i++)
          {
            input[i] = ' ';
          }
          byte sized = Serial.readBytes(input, INPUT_SIZE);
          input[sized] = 0; 
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
        digitalPotWrite(170);
        while (input[0] != 'X')
        {
          buttonState = digitalRead(buttonPin);
          if (buttonState == HIGH) //Run when button pressed
          {
            Keyboard.print('Q');
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
            input[0] = 'X';
            break;
          }
          for (int i = 0; i < INPUT_SIZE + 1; i++)
          {
            input[i] = ' ';
          }
          byte sized = Serial.readBytes(input, INPUT_SIZE);
          input[sized] = 0; 
          delay(10);
        }
        digitalPotWrite(0x80);
      }
    }
    else if (input[0] == 'O')
    {
      // Brightness Calibration screen
      Serial.setTimeout(200);
      int mod = input[1] - '0';
      int potVal = 165 + mod;
      digitalPotWrite(potVal);
      Serial.println("LIVE VIEW");
      delay(200);      
      while (input[0] != 'X')
      {
        for (int i = 0; i < INPUT_SIZE + 1; i++)
        {
          input[i] = ' ';
        }
        byte sized = Serial.readBytes(input, INPUT_SIZE);
        input[sized] = 0;
        if (input[0] == 'P')
        {
          
          long startTime = micros();
          long currentTime = micros();
          long times[16000]; 
          int count = 0;
          while (currentTime < (startTime + 3000000))
          {
              ADC0->SWTRIG.bit.START = 1; //Start ADC 
              while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
              currentTime = micros();
              times[count] = currentTime - startTime;
              adcBuff[count] = ADC0->RESULT.reg;
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
          int add = 3 * in;
          potVal = 165 + add;
          digitalPotWrite(potVal); 
          Serial.print("pot val:");
          Serial.println(potVal); 
        }
        else if (in == 0)
        {
          potVal = 165;
          digitalPotWrite(potVal);
          Serial.print("pot val:");
          Serial.println(potVal);
        }  
      }
    }
  delay(100); 
}
