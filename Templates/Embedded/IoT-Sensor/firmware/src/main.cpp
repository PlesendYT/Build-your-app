// ESP32 IoT sensor firmware: reads a DHT22 sensor, publishes data via
// MQTT, and enters deep sleep between readings to conserve battery.

#include <WiFi.h>
#include <PubSubClient.h>
#include <DHT.h>
#include "config.h"

// Create the DHT sensor instance and the WiFi/MQTT clients
DHT dht(DHT_PIN, DHT22);
WiFiClient espClient;
PubSubClient mqttClient(espClient);
char payload[128];

void setup() {
  Serial.begin(115200);
  delay(100);
  Serial.println(F("IoT Sensor waking up..."));

  // Initialize the DHT22 sensor
  dht.begin();

  // Connect to WiFi using credentials from config.h
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print(F("Connecting to WiFi"));
  for (int i = 0; i < 20 && WiFi.status() != WL_CONNECTED; i++) {
    delay(500);
    Serial.print(F("."));
  }

  if (WiFi.status() != WL_CONNECTED) {
    Serial.println(F("\nWiFi failed, going to deep sleep"));
    esp_deep_sleep_start();
  }
  Serial.println(F(" connected"));

  // Connect to the MQTT broker
  mqttClient.setServer(MQTT_BROKER, MQTT_PORT);
  if (!mqttClient.connect("IoTSensor")) {
    Serial.println(F("MQTT connection failed"));
    esp_deep_sleep_start();
  }

  // Read temperature (Celsius) and humidity from the DHT22
  float temperature = dht.readTemperature();
  float humidity = dht.readHumidity();

  // Validate the readings; DHT22 returns NaN on error
  if (isnan(temperature) || isnan(humidity)) {
    Serial.println(F("DHT22 read failed"));
    esp_deep_sleep_start();
  }

  // Build a JSON payload with the sensor readings
  snprintf(payload, sizeof(payload),
    "{\"temperature\":%.1f,\"humidity\":%.1f}", temperature, humidity);

  Serial.print(F("Publishing: "));
  Serial.println(payload);
  mqttClient.publish(MQTT_TOPIC, payload);

  // Allow the MQTT client to flush the outgoing buffer
  delay(100);
  mqttClient.loop();

  Serial.println(F("Entering deep sleep"));
  esp_deep_sleep_start();
}

void loop() {
  // The firmware never reaches here because deep sleep is entered in setup
}
