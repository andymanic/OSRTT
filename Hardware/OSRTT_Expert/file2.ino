#include <Keyboard.h>
#include <Mouse.h>
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
//50ms sample time default

#define ArraySize 76000
uint16_t adcBuff[ArraySize];


//Button values
int buttonState = 0;
const int buttonPin = 10;

//Digital Potentiometer values (SPI)
int CS = 1;
SPISettings settingsA(60000000, MSBFIRST, SPI_MODE0);

//Serial connection values
int boardType = 2;
String firmware = "0.1";
int testRuns = 4;
bool vsync = true;
bool extendedGamma = true;
int fpsLimit = 49;
int GlobalSampleCount = 36651;
int GlobalSampleDelay = 0;
int GlobalStartDelay = 0;

unsigned long loopTimer = millis();

char input[INPUT_SIZE + 1];

void Delay600ns()
{
  digitalWrite(CS, HIGH);
  for (int i = 0; i < 12; i++)
  {
    __asm__("nop");
  }
  digitalWrite(CS, LOW);
}

int getADCValue(int count)
{
  uint32_t value = 0;
  int localCounter = 0;
  SPI.beginTransaction(settingsA);
  while (localCounter < count)
  {
    Delay600ns();
    // SPI transaction 
    __asm__("nop");
    __asm__("nop");
    uint8_t highByte = SPI.transfer(0xFF);
    uint8_t lowByte = SPI.transfer(0xFF);
    value += (highByte << 8) + lowByte;  
    localCounter++;
  }
  SPI.endTransaction();
  value /= count;
  return value;
}
#ifdef __arm__
// should use uinstd.h to define sbrk but Due causes a conflict
extern "C" char* sbrk(int incr);
#else  // __ARM__
extern char *__brkval;
#endif  // __arm__

int freeMemory() {
  char top;
#ifdef __arm__
  return &top - reinterpret_cast<char*>(sbrk(0));
#elif defined(CORE_TEENSY) || (ARDUINO > 103 && ARDUINO != 151)
  return &top - __brkval;
#else  // __arm__
  return __brkval ? &top - __brkval : &top - __malloc_heap_start;
#endif  // __arm__
}
/// Fills ADC Buffer, either at native 2.78us per sample, or with delay
/// Triple loops needed to fill all three buffers, plus logic to handle differing sample counts
long fillADCBuffer(int sampleCount = 10000, int delayTime = 0)
{
  int count = sampleCount; 
  if (count >= ArraySize)
  {
    count = ArraySize - 1;
  }
  long tStart = 0;
  long tEnd = 0;
  SPI.beginTransaction(settingsA);
  if (delayTime != 0)
  {
    tStart = micros();
    for (int i = 0; i < count; i++)
    {
      Delay600ns();
      // SPI transaction 
      __asm__("nop");
      __asm__("nop");
      uint8_t highByte = SPI.transfer(0xFF);
      uint8_t lowByte = SPI.transfer(0xFF);
      adcBuff[i] = (highByte << 8) + lowByte;
      delayMicroseconds(delayTime);
    }
    tEnd = micros();
  }
  else
  {
    tStart = micros();
    for (int i = 0; i < count; i++)
    {
      Delay600ns();
      // SPI transaction 
      __asm__("nop");
      __asm__("nop");
      uint8_t highByte = SPI.transfer(0xFF);
      uint8_t lowByte = SPI.transfer(0xFF);
      adcBuff[i] = (highByte << 8) + lowByte;
    }
    tEnd = micros();
  }
  SPI.endTransaction();
  long tTaken = tEnd - tStart;
  return tTaken;
}

int getSingleADCValue()
{
  int val = 0;
  // Pulse CS to trigger ADC conversion start
  Delay600ns();
  // SPI transaction 
  SPI.beginTransaction(settingsA);
  val = SPI.transfer16(0xFFFF);
  SPI.endTransaction();
  return val;
}

int checkLightLevel() // Check light level & modulate potentiometer value
{
  Keyboard.write('f');
  delay(400);
  int potValue = 2;
  digitalPotWrite(potValue);
  delay(200);
  int value = getADCValue(250);
  int upperBound = 56000;
  int lowerBound = 54000;
  
  int timeoutCount = 0;
  while (value <= lowerBound || value >= upperBound)
  {
    if (timeoutCount > 500)
    {
      Serial.println("Timed out on pot val, using last value");
      break;
    }
    value = getADCValue(250);
    if (value >= upperBound)
    {
      //Set digital pot to decrease voltage
      potValue += 1; 
      if (timeoutCount == 256)
      {
        potValue += 20; // jump over overlaps in resistor array
      }
      digitalPotWrite(potValue);
      Serial.print("Value: ");
      Serial.print(value);
      Serial.print(", Pot Value: ");
      Serial.println(potValue);
    }
    else if (value <= lowerBound)
    {
      //Set digital pot to increase volt
      potValue -= 1; 
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
    timeoutCount++;
  }
  Keyboard.write('q');
  oledFourLines("POT VAL:", String(potValue), "", "");
  delay(400);
  return 1;
}

void runADC(int curr, int nxt, char key, String type, int samples = 36650, int sampleDelay = 0, int startDelay = 0) // Run test, press key and print results
{
  if (samples > ArraySize)
  {
    samples = ArraySize - 1;
  }
  // Set next colour
  Keyboard.print(key);

  if (startDelay != 0)
  {
    delay(startDelay);
  }

  int timeTaken = fillADCBuffer(samples, sampleDelay);
  //Print from & to RGB values, time taken, sample count and all captured results
  Serial.print(type);
  Serial.print(curr);
  Serial.print(",");
  Serial.print(nxt);
  Serial.print(",");
  Serial.print(timeTaken);
  Serial.print(",");
  Serial.print(samples);
  Serial.print(",");
  for (int i = 0; i < samples; i++)
  {
    Serial.print(adcBuff[i]);
    Serial.print(",");
  }
  Serial.println();


  //As using Serial slows the adc down, we take the time after Serial was used.
  curr_time = micros();
}

