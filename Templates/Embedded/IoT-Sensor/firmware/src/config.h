// Configuration header for the ESP32 IoT sensor firmware
// Set your WiFi credentials and MQTT broker details here.

#ifndef CONFIG_H
#define CONFIG_H

// DHT22 sensor data pin connected to GPIO4
#define DHT_PIN 4

// WiFi credentials
#define WIFI_SSID      "your_ssid"
#define WIFI_PASSWORD  "your_password"

// MQTT broker connection details
#define MQTT_BROKER    "broker.example.com"
#define MQTT_PORT      1883
#define MQTT_TOPIC     "sensor/environment"

#endif
