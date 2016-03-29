// *****************************************************************************
// Copyright (c) 2016 Adam Milton-Barker - TechBubble Technologies and other Contributors.
//
// The MIT License (MIT)
//
// Copyright (c) 2016 AdamMiltonBarker
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// Module:
//   IntelliLan AuthWay FP
//
// Contributors:
//   Adam Milton-Barker  - Initial Contribution
//
// *****************************************************************************

#include <Adafruit_Fingerprint.h>
#include <SoftwareSerial.h>

int getFingerprintIDez();

SoftwareSerial mySerial(2, 3);

#include <Wire.h>

int gLED = 13;
bool loopit;

Adafruit_Fingerprint finger = Adafruit_Fingerprint(&mySerial);

void setup()  
{
  Serial.begin(9600);
  finger.begin(57600);
  delay(3000);
  pinMode(gLED, OUTPUT); 
  digitalWrite(gLED, LOW);  
}

void waitForStart()
{
  while(true){
    String returned = Serial.readString();
    if(returned == "S"){
        digitalWrite(gLED, LOW);
        Serial.println("AuthWay FP Online!");
        delay(3000);
        break;
    } 
  }
  
}

void waitForScan()
{  
  Serial.println("Please scan your finger");
  while(true){
    int result = getFingerprintIDez();
    if(result == 1){
      delay(1000);
      break;
    } 
  }
  
}

void waitForResponse()
{
  delay(1000);
  while(true){
    String returned = Serial.readString();
    if(returned == "K"){
        digitalWrite(gLED, HIGH); 
        break;
    } else if(returned == "F"){
        digitalWrite(gLED, LOW);
        break;
    } 
  }
  
}

void loop()                    
{
  waitForStart();
  waitForScan();
  waitForResponse();
}

int getFingerprintIDez() {
  uint8_t p = finger.getImage();
  if (p != FINGERPRINT_OK){  
    return -1; 
  }

  p = finger.image2Tz();
  if (p != FINGERPRINT_OK){  
    return -1; 
  }

  p = finger.fingerFastSearch();
  if (p != FINGERPRINT_OK){ 
    return -1; 
  }
  
  Serial.print("FOUND");
  Serial.println(1);
  //return finger.fingerID; 
  return 1; 
}
void softReset(){
  asm volatile ("  jmp 0");
}
