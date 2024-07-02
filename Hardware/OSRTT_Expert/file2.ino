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
unsigned long samplingTime = 150000;
uint16_t adcBuff[32000];

//Button values
int buttonState = 0;
const int buttonPin = 10;

//Digital Potentiometer values (SPI)
byte address = 0x00;
int CS = 0;
SPISettings settingsA(60000000, MSBFIRST, SPI_MODE0);

//Serial connection values
int boardType = 2;
String firmware = "0.1";
int testRuns = 4;
bool vsync = true;
bool extendedGamma = true;
int fpsLimit = 49;

unsigned long loopTimer = millis();

char input[INPUT_SIZE + 1];

void Delay600ns()
{
  digitalWrite(CS, HIGH);
  for (int i = 0; i < 10; i++)
  {
    __asm__("nop");
  }
  digitalWrite(CS, LOW);
}

int getADCValue(int count)
{
  uint32_t value = 0;
  int localCounter = 0;
  while (localCounter < count)
  {
    // Pulse CS to trigger ADC conversion start
    Delay600ns();
    // SPI transaction 
    SPI.beginTransaction(settingsA);
    value += SPI.transfer16(0xFFFF);
    SPI.endTransaction();
    localCounter++;
  }
  value /= count;
  return value;
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

void runADC(int curr, int nxt, char key, String type) // Run test, press key and print results
{
  //digitalWrite(3, HIGH);
  // Set next colour
  Keyboard.print(key);

  curr_time = micros(); //need to run this in case board is left connected for long period as first run won't read any samples
  unsigned long start_time = micros();

  /////////////////////////////////////////////////////////////
  //                    Take ADC Readings                    //
  /////////////////////////////////////////////////////////////
  // Loop for samplingTime microseconds
  while (curr_time <= (start_time + (samplingTime - 1)))
  {
    
    adcBuff[sample_count] = getSingleADCValue();
    sample_count++; //Increment sample count
    curr_time = micros(); //update current time
  }
  //digitalWrite(3, LOW);

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

bool isBitSet(int var, int pos)
{
  return var & (1 << pos) ? true : false;
}

void digitalPotWrite(int value)
{
  // 5K D2, 10K D3, 24K D7, 51K D8, 100K D9, 150K D11, 390K D12, 470K D13
  if (isBitSet(value, 0)) { digitalWrite(4, HIGH); } else { digitalWrite(4, LOW); }
  if (isBitSet(value, 1)) { digitalWrite(5, HIGH); } else { digitalWrite(5, LOW); }
  if (isBitSet(value, 2)) { digitalWrite(6, HIGH); } else { digitalWrite(6, LOW); }
  if (isBitSet(value, 3)) { digitalWrite(8, HIGH); } else { digitalWrite(8, LOW); }
  if (isBitSet(value, 4)) { digitalWrite(9, HIGH); } else { digitalWrite(9, LOW); }
  if (isBitSet(value, 5)) { digitalWrite(11, HIGH); } else { digitalWrite(11, LOW); }
  if (isBitSet(value, 6)) { digitalWrite(12, HIGH); } else { digitalWrite(12, LOW); }
  if (isBitSet(value, 7)) { digitalWrite(13, HIGH); } else { digitalWrite(13, LOW); }
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
    adcBuff[sample_count] = getSingleADCValue();
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

void adctest()
{
  //digitalPotWrite(128);
  SPI.beginTransaction(settingsA);
  int samples = 10000;
  long tStart = micros();
  for (int i = 0; i < samples; i++)
  {
    Delay600ns();
    // SPI transaction 
    __asm__("nop"); // testing to see if adding no-op will make msb valid
    __asm__("nop");
    uint8_t highByte = SPI.transfer(0xFF);
    uint8_t lowByte = SPI.transfer(0xFF);
    adcBuff[i] = (highByte << 8) + lowByte;
    //adcBuff[i] = SPI.transfer16(0xFFFF);
  }
  long tEnd = micros();
  SPI.endTransaction();
  float tTaken = tEnd - tStart;
  tTaken /= samples;
  Serial.print(tTaken);
  Serial.println(" us per sample");
  long adcAvg = 0;
  int min = 65535;
  int max = 0;
  for (int i = 0; i < samples; i++)
  {
    Serial.print(adcBuff[i]);
    Serial.print(",");
    adcAvg += adcBuff[i];
    if (adcBuff[i] < min)
    {
      min = adcBuff[i];
    }
    else if (adcBuff[i] > max)
    {
      max = adcBuff[i];
    }
  }
  Serial.println();
  Serial.print("ADC Value average: ");
  Serial.print(adcAvg / samples);
  Serial.print(", Min = ");
  Serial.print(min);
  Serial.print(", Max = ");
  Serial.println(max);
}


void setup() {
  pinMode(buttonPin, INPUT_PULLUP); //Button input on pin 2
  if (!display.begin(SSD1306_SWITCHCAPVCC, SCREEN_ADDRESS)) {
    Serial.println(F("SSD1306 allocation failed"));
    for (;;);
  }
  drawSplashScreen();
  
  pinMode (4, OUTPUT); //D2, SW1
  pinMode (5, OUTPUT); //D3, SW2 
  pinMode (6, OUTPUT); //D4, SW3
  pinMode (8, OUTPUT); //D7, SW4
  pinMode (9, OUTPUT); //D9, SW5
  pinMode (11, OUTPUT); //D11, SW6
  pinMode (12, OUTPUT); //D12, SW7
  pinMode (13, OUTPUT); //D13, SW8
  
  pinMode(CS, OUTPUT); // D0, CS
  digitalWrite(CS, LOW);
  digitalPotWrite(0x00);

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
  //pinMode(13, OUTPUT); //Onboard LED
  
  SPI.begin();

  Serial.println("Begin...");
  Serial.println("EXPERT");
  
  adctest();

}
