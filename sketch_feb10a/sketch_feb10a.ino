#include <Servo.h>
#include <RFID.h>
#include <SPI.h>
RFID rfid(10,9);

Servo myservo;  // create servo object to control a servo
// twelve servo objects can be created on most boards

int pos = 0;    // variable to store the servo position

void setup()
{
  myservo.attach(7);  // attaches the servo on pin 9 to the servo object
  Serial.begin(9600);
  SPI.begin();
  rfid.init();
  myservo.write(pos);  
}
void loop()
{
  if(rfid.isCard())
  {
    if(rfid.readCardSerial())
    {
      Serial.print(rfid.serNum[0]);
      Serial.print(rfid.serNum[1]);
      Serial.print(rfid.serNum[2]);
      Serial.print(rfid.serNum[3]);
      Serial.println(rfid.serNum[4]);
      delay(3000);
      
    }
    
    rfid.halt();  
  }
  
if (Serial.available()>0){
 
int gelenVeri=Serial.read();
//Serial.print(gelenVeri);
 if(gelenVeri==49){
 for (pos = 0; pos <= 80; pos += 2) { // goes from 0 degrees to 180 degrees
    // in steps of 1 degree
    myservo.write(pos);              // tell servo to go to position in variable 'pos'
     delay(15);                  // waits 15ms for the servo to reach the position
  }
      delay(5000); 
  for (pos = 80; pos >= 0; pos -= 2) { // goes from 180 degrees to 0 degrees
    myservo.write(pos);              // tell servo to go to position in variable 'pos'                     // waits 15ms for the servo to reach the position
  delay(15);
  }
  Serial.write(0);
  }

}

}


