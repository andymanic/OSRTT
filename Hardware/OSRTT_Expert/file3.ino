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
  //delay(50);
  
  unsigned long clickTime = micros();
  //Mouse.click(MOUSE_LEFT);
  //Mouse.click(MOUSE_LEFT);
  Keyboard.print('6');
  long start_time = micros();
  int timeTaken = fillADCBuffer(ArraySize - 1, 2);
  Keyboard.print('1');
  
  Serial.print("IL:");
  Serial.print(start_time - clickTime);
  Serial.print(",");
  Serial.print(timeTaken);
  Serial.print(",");
  Serial.print(ArraySize - 1);
  Serial.print(",");
  for (int i = 0; i < ArraySize - 1; i++)
  {
    Serial.print(adcBuff[i]);
    Serial.print(",");
  }
  Serial.println();
  Keyboard.print('1');
  Keyboard.print('1');
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
  
  SPI.begin();

  Serial.println("Begin...");
  Serial.println("EXPERT");

}
