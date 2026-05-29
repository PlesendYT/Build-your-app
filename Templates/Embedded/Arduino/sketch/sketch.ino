// Arduino starter sketch: LED blink, analog sensor read, serial output
// Reads an analog sensor on A0 and blinks the built-in LED at a rate
// proportional to the sensor value. Results are printed to Serial.

#include "config.h"

void setup() {
  // Initialize the serial port for debugging at 9600 baud
  Serial.begin(9600);
  // Wait briefly to allow the serial connection to stabilize
  delay(100);
  Serial.println(F("Arduino sketch started"));

  // Configure the built-in LED pin as an output
  pinMode(LED_BUILTIN, OUTPUT);
  // Configure the sensor pin as an input (no pull-up needed for analog)
  pinMode(SENSOR_PIN, INPUT);
}

void loop() {
  // Read the raw analog value from the sensor (0–1023)
  int sensorValue = analogRead(SENSOR_PIN);
  // Map the 10-bit sensor reading to a blink delay in milliseconds
  int blinkDelay = map(sensorValue, 0, 1023, MIN_DELAY, MAX_DELAY);

  // Print current sensor reading and computed delay to the serial monitor
  Serial.print(F("Sensor: "));
  Serial.print(sensorValue);
  Serial.print(F("  Delay: "));
  Serial.print(blinkDelay);
  Serial.println(F(" ms"));

  // Blink the LED with the computed delay
  digitalWrite(LED_BUILTIN, HIGH);
  delay(blinkDelay);
  digitalWrite(LED_BUILTIN, LOW);
  delay(blinkDelay);
}
