#include <Keyboard.h>
#include <Mouse.h>
#include <SPI.h>
#include <ArduinoUniqueID.h>
#define INPUT_SIZE 8

//Test values
// These are the RGB values it tests with
int RGBArr[] = {0, 51, 102, 153, 204, 255};
// These are the keys that correspond with the above values
char Keys[] = {'q', 'z', 's', 'e', 'c', 'f'};
// Ideally I'd pair these together in an array of key/value pairs but this works for now.
// These are the RGB values it tests with
int OldRGBArr[] = {0, 26, 51, 77, 102, 128, 153, 179, 204, 230, 255};
// These are the keys that correspond with the above values
char OldKeys[] = {'q', 'a', 'z', 'w', 's', 'x', 'e', 'd', 'c', 'r', 'f'};
// Ideally I'd pair these together in an array of key/value pairs but this works for now.
int extGammaArr[] = {0, 17, 34, 51, 68, 85, 102, 119, 136, 153, 170, 187, 204, 221, 238, 255};
char extGammaKeys[] = {'q', 'y', 'h', 'z', 'n', 'u', 's', 'j', 'm', 'e', 'i', 'k', 'c', 'o', 'l', 'f',};

//ADC values
unsigned long curr_time = micros();
uint32_t sample_count = 0;
//50ms sample time default
unsigned long samplingTime = 50000;
uint16_t adcBuff[32000];

//Button values
int buttonState = 0;
const int buttonPin = 10;

//Digital Potentiometer values (SPI)
byte address = 0x00;
int CS = 2;
SPISettings settingsA(1000000, MSBFIRST, SPI_MODE0);

//Serial connection values
int boardType = 1;
String firmware = "1.8";
int testRuns = 4;
bool vsync = true;
bool extendedGamma = true;
int fpsLimit = 49;
bool highspeed = false;

unsigned long loopTimer = millis();

char input[INPUT_SIZE + 1];

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
  while ( ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_INPUTCTRL );

  ADCx->INPUTCTRL.bit.MUXPOS = 0;
  while ( ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_INPUTCTRL );

  //Divide 8MHz clock - default DIV4 = 2MHz ADC clock
  ADCx->CTRLA.bit.PRESCALER = ADC_CTRLA_PRESCALER_DIV4_Val;

  //Choose 16-bit resolution to oversample - use 12BIT_VAL for 1MSPS
  ADCx->CTRLB.bit.RESSEL = ADC_CTRLB_RESSEL_16BIT_Val;
  //Ensuring freerun is activated
  //Freerun means it automatically reads the next result rather than waiting for this code to tell it to check.
  ADCx->CTRLB.bit.FREERUN = 1;

  //waiting for synchronisation
  while (ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_CTRLB);

  //Sampletime set to 0
  ADCx->SAMPCTRL.reg = 0;

  //Waiting for synchronisation
  while (ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_SAMPCTRL);

  // Accumulate samples to gain higher final result precision
  ADCx->AVGCTRL.reg = ADC_AVGCTRL_SAMPLENUM_16_Val; //Sample 16 values for 16 bit output, 8 for 15 bit, 4 for 14 bit

  //Wait for synchronisation
  while (ADCx->SYNCBUSY.reg & ADC_SYNCBUSY_AVGCTRL)

    //Select VDDANA (3.3V chip supply voltage as reference)
    ADCx->REFCTRL.reg = ADC_REFCTRL_REFSEL_INTVCC1;

  //Enable ADC
  ADCx->SWTRIG.bit.START = 1;
  ADCx->CTRLA.bit.ENABLE = 1;

  //wait for ADC to be ready
  while (ADCx->SYNCBUSY.bit.ENABLE);
}

void ADCHighSpeedMode(int highSpeedNumber)
{
  Serial.println(highSpeedNumber);
  if (highSpeedNumber == 1)
  {
    // Swap to just 4 sample average for 250KSPS instead of 62,500
    ADC0->AVGCTRL.reg = ADC_AVGCTRL_SAMPLENUM_4_Val;
    Serial.println("HighSpeed:on");
    highspeed = true;
  }
  else
  {
    ADC0->AVGCTRL.reg = ADC_AVGCTRL_SAMPLENUM_16_Val;
    Serial.println("HighSpeed:off");
    highspeed = false;
  }
  //Wait for synchronisation
  while (ADC0->SYNCBUSY.reg & ADC_SYNCBUSY_AVGCTRL);
}

int getADCValue(int count)
{
  uint32_t value = 0;
  int localCounter = 0;
  while (localCounter < count)
  {
    ADC0->SWTRIG.bit.START = 1; //Start ADC
    while (!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
    value += ADC0->RESULT.reg;
    localCounter++;
  }
  value /= count;
  return value;
}

int checkLightLevel() // Check light level & modulate potentiometer value
{
  Keyboard.write('f');
  delay(400);
  int potValue = 2;
  digitalPotWrite(potValue);
  delay(200);
  int value = getADCValue(250);
  Serial.println("got a value");
  int upperBound = 56000;
  int lowerBound = 54000;
  if (highspeed)
  {
    upperBound = 15000;
    lowerBound = 14000;
  }
  while (value <= lowerBound || value >= upperBound)
  {
    value = getADCValue(250);
    if (value >= upperBound)
    {
      //Set digital pot to decrease voltage
      potValue += 1; //or the other way...
      digitalPotWrite(potValue);
      Serial.print("Value: ");
      Serial.print(value);
      Serial.print(", Pot Value: ");
      Serial.println(potValue);
    }
    else if (value <= lowerBound)
    {
      //Set digital pot to increase volt
      potValue -= 1; //or the other way...
      if (potValue <= 32 && potValue >= 24)
      {
        potValue = 23;
      }
      else if (potValue <= 128 && potValue >= 83)
      {
        potValue = 81;
      }
      else if (potValue <= 160 && potValue >= 153)
      {
        potValue = 152;
      }
      else if (potValue <= 224 && potValue >= 217)
      {
        potValue = 216;
      }
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
    if (potValue <= 1 || potValue == 255)
    {
      Serial.print("TEST CANCELLED - LIGHT LEVEL:");
      Serial.println(value);
      Keyboard.write('q');
      oledFourLines("UNABLE","TO SET","LIGHT","LEVEL");
      return 0;
      break; //not needed?
    }
    delay(20);
  }
  Keyboard.write('q');
  oledFourLines("POT VAL:", String(potValue), "", "");
  delay(400);
  return 1;
}

void runADC(int curr, int nxt, char key, String type) // Run test, press key and print results
{
  digitalWrite(3, HIGH);
  // Set next colour
  Keyboard.print(key);

  curr_time = micros(); //need to run this in case board is left connected for long period as first run won't read any samples
  unsigned long start_time = micros();

  //50ms worth of samples @100ksps= 5000 samples
  //50ms worth of samples @129ksps = 6451 samples

  /////////////////////////////////////////////////////////////
  //                    Take ADC Readings                    //
  /////////////////////////////////////////////////////////////
  // Loop for samplingTime microseconds
  while (curr_time <= (start_time + (samplingTime - 1)))
  {
    ADC0->SWTRIG.bit.START = 1; //Start ADC
    while (!ADC0->INTFLAG.bit.RESRDY); //wait for ADC to have a new value
    adcBuff[sample_count] = ADC0->RESULT.reg; //save new ADC value to buffer @ sample_count position
    sample_count++; //Increment sample count
    curr_time = micros(); //update current time
  }
  digitalWrite(3, LOW);
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
  digitalWrite(2, LOW);
  //delayMicroseconds(500);
  delay(1);
  SPI.transfer(value);
  //SPI.transfer(0);
  delay(1);
  //delayMicroseconds(500);
  digitalWrite(2, HIGH);
  SPI.endTransaction();
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
  //Keyboard.print('1');
  //Keyboard.print('Q');
  delay(50);
  int sampleTime = 200000;
  if (timeBetween == 100)
  {
    sampleTime = 100000;
  }
  curr_time = micros();
  unsigned long clickTime = micros();
  //Mouse.click(MOUSE_LEFT);
  //Mouse.click(MOUSE_LEFT);
  Keyboard.print('6');
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
  Keyboard.print('1');
  
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
  Keyboard.print('1');
  Keyboard.print('1');
  sample_count = 0; //reset sample count
    
  curr_time = micros();
}

void checkLatency() {
  delay(100);
  Keyboard.print('Q');
  delay(100);
  runADC(1000, 1000, 'F', "TL:");
  unsigned long startTime = micros();
  while (curr_time < (startTime + 3000))
  {
    curr_time = micros();
    getSerialChars();
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

int convertHexToDec(char c) {
  if (c <= 57) {
    return c - '0';  // Convert char to int
  } else {
    return c - 55;
  }
}

void getSerialChars() {
  for (int i = 0; i < INPUT_SIZE + 1; i++) {
    input[i] = ' ';
  }
  byte size = Serial.readBytes(input, INPUT_SIZE);
  input[size] = 0;
}

void setup() {
  pinMode (CS, OUTPUT);
  digitalWrite(CS, HIGH);
  SPI.begin();
  pinMode(buttonPin, INPUT_PULLUP); //Button input on pin 2
  if (!display.begin(SSD1306_SWITCHCAPVCC, SCREEN_ADDRESS)) {
    Serial.println(F("SSD1306 allocation failed"));
    for (;;);
  }
  drawSplashScreen();

  digitalPotWrite(0x00);
  ADC_Clocks();
  ADC_Init(ADC0); //Initialise ADC0
  ADC_Init(ADC1); //Initialise ADC1
  Serial.begin(115200); //Open Serial connection at 115200 baud
  long timer = millis();
  while (!Serial)
  {
    if (millis() > (timer + 180000))
    {
      clearDisplayBuffer();
    }
    buttonState = digitalRead(buttonPin);
    if (buttonState == HIGH)
    {
      oledAwaitingSerial("CONNECTION");
    }
  }
  Keyboard.begin(); //Open keyboard connection over USB
  pinMode(buttonPin, INPUT_PULLUP); //Button input on pin 2
  pinMode(13, OUTPUT); //Onboard LED
  Serial.println("Begin...");
}
