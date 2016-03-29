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
//   IntelliLan AuthWay NFC
//
// Contributors:
//   Adam Milton-Barker  - Initial Contribution
//
// *****************************************************************************

#if 0
#include <SPI.h>
#include <PN532_SPI.h>
#include <PN532.h>
#include <NfcAdapter.h>
PN532_SPI pn532spi(SPI, 10);
NfcAdapter nfc = NfcAdapter(pn532spi);
#else
#include <Wire.h>
#include <PN532_I2C.h>
#include <PN532.h>
#include <NfcAdapter.h>  
PN532_I2C pn532_i2c(Wire);
NfcAdapter nfc = NfcAdapter(pn532_i2c);
#endif

int LED = 13;

void setup(void) {
    pinMode(LED, OUTPUT);
    Serial.begin(9600);
    nfc.begin();   
    Serial.println("READY");
}

void waitForStart()
{
   
  Serial.println("DEVICE READY");
  while(true){
    String returned = Serial.readString();
    if(returned == "S"){
          Serial.println("AuthWay NFC Online!");
          delay(1000);
          break;
    } 
  }
  
}

void waitForScan()
{  
  Serial.println("Please scan your chip");
  while(true){
    if (nfc.tagPresent())
    {
        NfcTag tag = nfc.read();
        Serial.print("FOUND");
        Serial.println(tag.getUidString());
        break;
    }
  }
  
}

void waitForResponse()
{
  while(true){
      String returned = Serial.readString();
      if(returned == "K"){
        digitalWrite(LED, HIGH); 
        break;
      } 
  }
  
}

void loop(void) {
  waitForStart();
  waitForScan();
  waitForResponse();
}
