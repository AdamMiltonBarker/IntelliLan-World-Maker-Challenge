/*
  HomeGuard 1000 Azure HTTP sketch
  Part of competition entry for World's Largest Arduino Maker Challenge
  Created 13 July 2010 by dlf (Metodo2 srl)

  This sketch is to be used with the TechBubble Technologies HomeGuard1000 

 */


#include <SPI.h>
#include <WiFi101.h>
#include <ArduinoJson.h>

int tempPin = 0;
float tempMax = 23.00;

int ProximityPin = 1;
float ProximityMax = 50.00;

int doorSensePin = 4;    

int ledPin = 2;    

char ssid[] = "YOURSSID"; 
char pass[] = "YOURPASS";  

char serverHost[] = "YOURIOTHUB";   
String basePostURL = "/devices/YOURDEVICEID/messages"; 
String apiVersion = "2016-02-03";
String postURI = "/events?api-version="+apiVersion;
char authSAS[] = "YOURSHAREDACCESSSIGNATURE";       

char buffer[64];
boolean posting = false;

int status = WL_IDLE_STATUS;
WiFiServer server(80);

void initiateWifi(){
  
  if (WiFi.status() == WL_NO_SHIELD) {
    
    Serial.println("WiFi shield not present");
    while (true);
    
  }

  while (status != WL_CONNECTED) {
    
    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    status = WiFi.begin(ssid, pass);
    delay(10000);
    
  }
  
  Serial.println("Successfully connected to WiFi");

  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);
  long rssi = WiFi.RSSI();
  Serial.print("Signal strength (RSSI): ");
  Serial.print(rssi);
  Serial.println(" dBm");
  
}

void apiRequest(String method, String base, String URI, String contentType, String contentBody){
  posting = true;
  WiFiSSLClient client;
  Serial.println("\nStarting connection to server...");
  if (client.connect(serverHost, 443)) {
    
    Serial.println("Connected to server!");
    client.print(method); 
    client.print(base);
    client.print(URI); 
    client.println(" HTTP/1.1"); 
    client.print("Host: "); 
    client.println(serverHost);  
    client.print("Authorization: ");
    client.println(authSAS);
    client.println("Connection: close");
 
    if(method == "POST "){
      
        Serial.println("POSTING");
        client.print("Content-Type: ");
        client.println(contentType);
        client.print("Content-Length: ");
        client.println(contentBody.length());
        client.println();
        client.println(contentBody);
        delay(100);
        
    }else{
      
        client.println();
        
    }
    
    client.stop();
    
  } else {
    
    Serial.println("Connection to server failed!");
    
  }
  posting = false; 
}

String checkSensors(){
  
  Serial.println("");
  Serial.println("Checking Sensors");
  Serial.println("");
  
  float ProximityReading = calculateProximity();
  float temperatureReading = calculateTemperature();
  String ProximityStatus = "";
  String temperatureStatus = "";
  String doorStatus = "";
  
  if(checkProximity(ProximityReading)){
    Serial.println("WARNING");
    ProximityStatus = "WARNING";
  } else {
    Serial.println("OK");
    ProximityStatus = "OK";
  }
  
  if(checkTemperature(temperatureReading)){
    Serial.println("WARNING");
    temperatureStatus = "WARNING";
  } else {
    Serial.println("OK");
    temperatureStatus = "OK";
  }
  
  if(checkDoorSense()){
    Serial.println("OPEN");
    doorStatus = "OPEN";
  } else {
    Serial.println("CLOSED");
    doorStatus = "CLOSED";
  }
  
  return "{\"Response\": \"OK\",\"ResponseData\": { \"DoorSense\": {\"Status\": \"" + doorStatus + "\"},\"Proximity\": {\"Status\": \"" + ProximityStatus + "\",\"Value\": \"" + String(ProximityReading, 2) + "\",\"Max\": \"" + String(ProximityMax, 2) + "\"},\"Temperature\": {\"Status\": \"" + temperatureStatus + "\",\"Value\": \"" + String(temperatureReading, 2) + "\",\"Max\": \"" + String(tempMax, 2) + "\"}}}";
 
}

void postSensors(){
  
  Serial.println("");
  Serial.println("Checking Sensors");
  Serial.println("");
  float ProximityReading = calculateProximity();
  float temperatureReading = calculateTemperature();
  String ProximityStatus = "";
  String temperatureStatus = "";
  String doorStatus = "";
  boolean warnStatus = false;
  
  if(checkProximity(ProximityReading)){
    warnStatus = true;
    Serial.println("WARNING");
    ProximityStatus = "WARNING";
  } else {
    Serial.println("OK");
    ProximityStatus = "OK";
  }
  
  if(checkTemperature(temperatureReading)){
    warnStatus = true;
    Serial.println("WARNING");
    temperatureStatus = "WARNING";
  } else {
    Serial.println("OK");
    temperatureStatus = "OK";
  }
  
  if(checkDoorSense()){
    warnStatus = true;
    Serial.println("OPEN");
    doorStatus = "OPEN";
  } else {
    Serial.println("CLOSED");
    doorStatus = "CLOSED";
  }

  if(warnStatus){
    digitalWrite(ledPin, HIGH);
  } else {
    digitalWrite(ledPin, LOW);
  }

  Serial.println("");
  Serial.println("Posting Sensor Data");
  Serial.println("");
  
  String postData = "{\"Sensor\": \"DoorSense\",\"Status\": \"" + doorStatus + "\"},{\"Sensor\": \"Proximity\",\"Value\": \"" + String(ProximityReading, 2) + "\",\"Status\": \"" + ProximityStatus + "\"},{\"Sensor\": \"Temperature\",\"Value\": \"" + String(temperatureReading, 2) + "\",\"Status\": \"" + temperatureStatus + "\"}";
  apiRequest("POST ",basePostURL,postURI,"application/json",postData);

}

float calculateTemperature(){

  int tempReading = analogRead(tempPin); 
  float voltage = tempReading * 3.3;
  voltage /= 1024.0; 
   
  float currentTempC = (voltage - 0.5) * 100 ; 
  float currentTempf = (currentTempC * 9.0 / 5.0) + 32.0;
  Serial.print("Temp: ");
  Serial.println(currentTempC);
  return currentTempC;
  
}

bool checkTemperature(float currentTemp){

  if(currentTemp > tempMax){
    
    return true;    
    
  }
  
  return false;
  
}

bool checkDoorSense(){

  if(digitalRead(doorSensePin)){
    
    return true;    
    
  }
  
  return false;
  
}

float calculateProximity(){

  float volts = analogRead(ProximityPin)*(3.3/1024);   
  float Proximity = 65*pow(volts, -1.10); 
  Serial.print("Proximity: ");
  Serial.println(Proximity);
  return Proximity;
  
}

bool checkProximity(float currentProximity){

  if(currentProximity < ProximityMax){
    
    return true;  
      
  }
  
  return false;
  
}

void setup() {
  
  Serial.begin(9600);
  while (!Serial) {
    ; 
  }
  
  pinMode(doorSensePin, INPUT_PULLUP);
  pinMode(ledPin, OUTPUT);
  
  initiateWifi();
  server.begin();
  Serial.println("Server Started");
  
}

void loop() {
  
  while(posting == true){
    ;
  }
  
  WiFiClient webServer = server.available();
  if (webServer) {
    String responseConcat;
    String result;
    boolean startRead = false;
    String readString = ""; 
    Serial.println("new client");
    boolean currentLineIsBlank = true;
    String ProximityStatus2 = "";
    String temperatureStatus2 = "";
    boolean warnStatus2 = false;
    while (webServer.connected()) {
      if (webServer.available()) {
        char response = webServer.read();
        
        if (readString.length() < 100) {
        
          readString += response;
        } 
        
        Serial.write(response);
        if (response == '\n' && currentLineIsBlank) {
          
          webServer.println("HTTP/1.1 200 OK");
          webServer.println("Content-Type: application/json");
          webServer.println("Connection: close"); 
          webServer.println();

          if(readString.indexOf("ReadData")>0) {
            
            webServer.println(checkSensors());
            
          } else if(readString.indexOf("MaxTemp=")>0) {
            
             int startCmd = readString.indexOf('=');
             int endCmd = readString.indexOf('S');
             String final = readString.substring(startCmd, endCmd);
             final.replace("S","");
             final.replace("=","");
             tempMax = final.toFloat();
            
            if(checkTemperature(calculateTemperature())){
              warnStatus2 = true;
              Serial.println("WARNING");
              temperatureStatus2 = "WARNING";
            } else {
              Serial.println("OK");
              temperatureStatus2 = "OK";
            }
            
             webServer.println("{\"Sensor\": \"Temperature\",\"Status\": \"Updated\",\"NewValue\": \"" + final + "\",\"Warn\": \"" + temperatureStatus2 + "\"}");
  
           } else if(readString.indexOf("MaxProximity=")>0) {
  
            if(checkProximity(calculateProximity())){
              warnStatus2 = true;
              Serial.println("WARNING");
              ProximityStatus2 = "WARNING";
            } else {
              Serial.println("OK");
              ProximityStatus2 = "OK";
            }
            
             int startCmd = readString.indexOf('=');
             int endCmd = readString.indexOf('S');
             String final = readString.substring(startCmd, endCmd);
             final.replace("S","");
             final.replace("=","");
             ProximityMax = final.toFloat();
             webServer.println("{\"Sensor\": \"Proximity\",\"Status\": \"Updated\",\"NewValue\": \"" + final + "\",\"Warn\": \"" + ProximityStatus2 + "\"}");
  
           }
          
          break;
        }
        if (response == '\n') {

          currentLineIsBlank = true;
          
        }
        else if (response != '\r') {

          currentLineIsBlank = false;
          
        }
      }
    }

    delay(1);
    webServer.stop();
    Serial.println("client disonnected");
  
  }
  postSensors();
}





