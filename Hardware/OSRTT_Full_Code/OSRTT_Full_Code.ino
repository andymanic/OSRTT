#include "Keyboard.h"
#include <SPI.h>
#define INPUT_SIZE 2

//////////////////////////
// RGB Values / Keys //
//     0      /  q   //       
//     26     /  a   //       
//     51     /  z   //       
//     77     /  w   //       
//     102    /  s   //       
//     128    /  x   //       
//     153    /  e   //       
//     179    /  d   //       
//     204    /  c   //       
//     230    /  r   //       
//     255    /  f   //       

// Framerate Cap / Keys //
//    1000FPS    /  1   //
//     360FPS    /  2   //
//     240FPS    /  3   //
//     165FPS    /  4   //
//     144FPS    /  5   //
//     120FPS    /  6   //
//     100FPS    /  7   //
//      60FPS    /  8   //
//////////////////////////

//Test values
// These are the RGB values it tests with
int RGBArr[] = {0,26,51,77,102,128,153,179,204,230,255};
// These are the keys that correspond with the above values
char Keys[] = {'q','a','z','w','s','x','e','d','c','r','f'};
// Ideally I'd pair these together in an array of key/value pairs but this works for now. 

//ADC values
long curr_time = micros();
uint32_t sample_count = 0;
int one_sample = 0;
//50ms sample time default
long samplingTime = 50000;
uint16_t adcBuff[7000];

//Button values
int buttonState = 0;
const int buttonPin = 2;

//Digital Potentiometer values (SPI)
byte address = 0x02;
int CS = 10;
SPISettings settingsA(10000000, MSBFIRST, SPI_MODE0);

//Serial connection values
bool connected = false;
String firmware = "1.0";
int testRuns = 2;
char fpsLimit = '1';

void ADC_Clocks()
{
   MCLK->APBDMASK.bit.ADC0_ = 1;
   MCLK->APBDMASK.bit.ADC1_ = 1;
   //Use GCLK1, channel it for ADC0, select DFLL(48MHz) as source and make sure no divider is selected
   GCLK->PCHCTRL[ADC0_GCLK_ID].reg = GCLK_PCHCTRL_GEN_GCLK1_Val | (1 << GCLK_PCHCTRL_CHEN_Pos); // enable gen clock 1 as source for ADC channel
   GCLK->PCHCTRL[ADC1_GCLK_ID].reg = GCLK_PCHCTRL_GEN_GCLK1_Val | (1 << GCLK_PCHCTRL_CHEN_Pos); // enable gen clock 2 as source for ADC channel
   GCLK->GENCTRL[0].reg = GCLK_GENCTRL_SRC_DFLL | GCLK_GENCTRL_GENEN | GCLK_GENCTRL_DIV(1);
   GCLK->GENCTRL[0].bit.DIVSEL = 0;
   GCLK->GENCTRL[0].bit.DIV = 0;
   GCLK->GENCTRL[1].reg = GCLK_GENCTRL_SRC_DFLL | GCLK_GENCTRL_GENEN | GCLK_GENCTRL_DIV(1);
   GCLK->GENCTRL[1].bit.DIVSEL = 0;
   GCLK->GENCTRL[1].bit.DIV = 0;
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
  ADCx->AVGCTRL.reg = ADC_AVGCTRL_SAMPLENUM_16_Val; //Sample 16 values for 16 bit output

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
  Keyboard.write('f');
  delay(200);
  int potValue = 160;
  ADC0->SWTRIG.bit.START = 1; //Start ADC 
  while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
  int value = ADC0->RESULT.reg;
  Serial.print("Value: ");
      Serial.print(value);
  while(value <= 64000 || value >= 64001)
  {
    ADC0->SWTRIG.bit.START = 1; //Start ADC 
    while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
    value = ADC0->RESULT.reg;
    if (value >= 64500)
    {
      //Set digital pot to decrease voltage
      potValue -= 1; //or the other way...
      digitalPotWrite(potValue);
      Serial.print("Value: ");
      Serial.print(value);
      Serial.print(", Pot Value: ");
      Serial.println(potValue);
    }
    else if (value <= 64000)
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
    if (potValue <= 155 || potValue == 255)
    {
      Serial.print("TEST CANCELLED - LIGHT LEVEL");
      Serial.println(value);
      Keyboard.write('q');
      return 0;
      break; //not needed?  
    }
    delay(20);
  }
  Keyboard.write('q');
  delay(200);
  return 1;
}

void runADC(int curr, int nxt, char key) // Run test, press key and print results
{
    // Set next colour
    Keyboard.print(key);  // This order may not work

    curr_time = micros(); //need to run this in case board is left connected for long period as first run won't read any samples
    long start_time = micros();
    
  
    //50ms worth of samples @100ksps= 5000 samples
    //50ms worth of samples @129ksps = 6451 samples
  
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

    ADC0->SWTRIG.bit.START = 0; //Stop ADC 
  
    /////////////////////////////////////////////////////////////
    //                       Print Readings                    //
    /////////////////////////////////////////////////////////////
    // Get how long the samples took to capture in microseconds
    int timeTaken = curr_time - start_time;
    //Print from & to RGB values, time taken, sample count and all captured results
    Serial.print("Results: ");
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

int checkUSBVoltage() // Check USB voltage is between 4.8V and 5.2V
{
  ADC1->SWTRIG.bit.START = 1; //Start ADC1 
  while(!ADC1->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
  int voltage = ADC1->RESULT.reg; 
  Serial.print("USB Voltage: ");
  Serial.println(voltage);
  
  if (voltage < 47000)
  {
    double calcVolt = (3.3 * (voltage/65536))*2;
    Serial.print("USB Voltage too low: ");
    Serial.println(calcVolt);
    ADC1->SWTRIG.bit.START = 0; //Stop ADC 
    return 0;
  }
  else if (voltage > 52250)
  {
    double calcVolt = (3.3 * (voltage/65536))*2;
    Serial.print("USB Voltage too high: ");
    Serial.println(calcVolt);
    ADC1->SWTRIG.bit.START = 0; //Stop ADC 
    return 0;
  }
  else
  {
    ADC1->SWTRIG.bit.START = 0; //Stop ADC 
    return 1;  
  }
}

void setup() {
  pinMode (CS, OUTPUT);
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
      Serial.setTimeout(100);
      digitalPotWrite(170);
      while (input[0] != 'C')
      {
          ADC0->SWTRIG.bit.START = 1; //Start ADC 
          while(!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
          int value = ADC0->RESULT.reg;
          Serial.print("Brightness:");
          Serial.println(value);

          // Check serial for cancel or new potentiometer value
          for (int i = 0; i < INPUT_SIZE + 1; i++)
          {
            input[i] = ' ';
          }
          byte sized = Serial.readBytes(input, INPUT_SIZE);
          input[sized] = 0;
          int in = input[0] - '0'; // Convert char to int  
          if (in > 1 && in <= 5)
          {
            // Increment potentiometer value by multiples of 10 up to 220
            int add = 10 * in;
            add += 170;
            digitalPotWrite(add);  
          }
          else if (in == 1)
          {
            digitalPotWrite(170);
          }
          delay(200);
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
      char runs = testRuns + '0';
      Serial.println(testRuns);
      delay(100);
      Serial.println("FW:" + firmware);
      delay(100);
      Serial.print("FPS Key:");
      Serial.println(fpsLimit);
    }
    else if (input[0] == 'M')
    {
      testRuns = input[1] - '0';
      delay(100);
      Serial.print("Runs:");
      char runs = testRuns + '0';
      Serial.println(testRuns);
    }
    else if (input[0] == 'L')
    {
      fpsLimit = input[1];
      delay(100);
      Serial.print("FPS Key:");
      Serial.println(fpsLimit);
    }
    else if (input[0] == 'T')
    {
      Serial.println("Ready to test");
      Serial.setTimeout(100);
      while (input[0] != 'C')
      {
        
        // Check if button has been pressed
        buttonState = digitalRead(buttonPin);
        if (buttonState == HIGH) //Run when button pressed
        {
          Serial.setTimeout(200);
          // Check USB voltage level
          int voltageTest = checkUSBVoltage();
          if (voltageTest == 0)
          {
            // If brightness too low or high, don't run the test
            Serial.println("TEST CANCELLED - USB VOLTAGE");
            digitalWrite(13, HIGH); 
            digitalPotWrite(0x80);
            break;
          } 
          else 
          {
            // Check monitor brightness level
            int brightnessTest = checkLightLevel();
            if (brightnessTest == 0)
            {
              // If brightness too low or high, don't run the test
              Serial.println("Cancelling test");
              digitalWrite(13, HIGH); 
              digitalPotWrite(0x80);
              break;
            }
            else
            {
              // Set FPS limit (default 1000 FPS, key '1')
              Keyboard.print(fpsLimit);
              delay(50);
              
              Serial.println("STARTING TEST"); 
              delay(50);
              for (int k = 0; k <= testRuns; k++)
              {
                digitalWrite(13, LOW);
                // If brightness fine, continue with test
                long start_time = micros();
                int delayTime = 200000;
                
                Serial.println("STARTING RUN"); 
                // Get size of array for for loop, so it's expandable for different test sizes
                int arrSize = sizeof(RGBArr) / sizeof(int); 
          
                // Loop through each colour 
                for (int i = 0; i < arrSize; i++)
                {
                  // Save current 'base' value
                  int current = RGBArr[i];
                  // Save current 'base' key
                  char currentKey = Keys[i];
                
                  if (i + 1 < arrSize) // May be redundant
                  {
                    // Loop through all other RGB values
                    for (int j = i + 1; j < arrSize; j++)
                    {
                      // Save next value
                      int next = RGBArr[j];
          
                      // Set the starting point colour
                      Keyboard.print(currentKey);
          
                      // Wait short amount of time
                      delay(200);
                    
                      // Get light output values & pass in RGB values in use
                      runADC(current, next, Keys[j]);
          
                      // Wait short amount of time after finishing capturing
                      // Swapped delay with while loop polling serial
                      start_time = micros();
                      while(curr_time <= (start_time + delayTime))
                      {
                        for (int i = 0; i < INPUT_SIZE + 1; i++)
                        {
                          input[i] = ' ';
                        }
                        byte sized = Serial.readBytes(input, INPUT_SIZE);
                        input[sized] = 0;  
                        if (input[0] == 'P') // If game not selected, pause test
                        {
                          while (input[0] != 'C' && input[0] != 'S')
                          {
                            for (int i = 0; i < INPUT_SIZE + 1; i++)
                            {
                              input[i] = ' ';
                            }
                            byte sized = Serial.readBytes(input, INPUT_SIZE);
                            input[sized] = 0; 
                          }
                        }
                        else if (input[0] == 'C')
                        {
                          break;  
                        }
                        curr_time = micros(); //update current time
                      }
            
                      // Get light output values & pass in RGB values in use
                      runADC(next, current, currentKey);
                    
                      // Wait short amount of time after finishing capturing
                      start_time = micros();
                      while(curr_time <= (start_time + delayTime))
                      {
                        for (int i = 0; i < INPUT_SIZE + 1; i++)
                        {
                          input[i] = ' ';
                        }
                        byte sized = Serial.readBytes(input, INPUT_SIZE);
                        input[sized] = 0;  
                        if (input[0] == 'P')
                        {
                          while (input[0] != 'C' && input[0] != 'S')
                          {
                            for (int i = 0; i < INPUT_SIZE + 1; i++)
                            {
                              input[i] = ' ';
                            }
                            byte sized = Serial.readBytes(input, INPUT_SIZE);
                            input[sized] = 0; 
                          }
                        }
                        else if (input[0] == 'C')
                        {
                          break;  
                        }
                        curr_time = micros(); //update current time
                      }        
                    }
                  }
                }
                Serial.println("Run Complete");                
                if (k != testRuns)
                {
                  // Swapped delay with while loop polling serial
                  start_time = micros();
                  while(curr_time <= (start_time + (3000000)))
                  {
                    for (int i = 0; i < INPUT_SIZE + 1; i++)
                    {
                      input[i] = ' ';
                    }
                    byte sized = Serial.readBytes(input, INPUT_SIZE);
                    input[sized] = 0;  
                    
                    if (input[0] == 'P')
                    {
                      while (input[0] != 'C' && input[0] != 'S')
                      {
                        for (int i = 0; i < INPUT_SIZE + 1; i++)
                        {
                          input[i] = ' ';
                        }
                        byte sized = Serial.readBytes(input, INPUT_SIZE);
                        input[sized] = 0; 
                      }
                    }
                    else if (input[0] == 'C')
                    {
                      break;  
                    }
                    curr_time = micros(); //update current time
                  }
                }
              }
              digitalPotWrite(0x80);
              delay(100);
              Serial.println("Test Complete");
            }
          }
              
        }
        else 
        {
          for (int i = 0; i < INPUT_SIZE + 1; i++)
          {
            input[i] = ' ';
          }
          byte sized = Serial.readBytes(input, INPUT_SIZE);
          input[sized] = 0;   
        }
        
      }
    }
  
  delay(100); 
}
