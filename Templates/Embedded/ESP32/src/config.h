// WiFi and MQTT configuration for the ESP32 application
// Update these values to match your network environment.

#ifndef CONFIG_H
#define CONFIG_H

// WiFi credentials
#define WIFI_SSID      "your_ssid"
#define WIFI_PASSWORD  "your_password"

// MQTT broker connection details
#define MQTT_BROKER    "broker.example.com"
#define MQTT_PORT      1883
#define MQTT_TOPIC     "sensor/data"

// Interval between sensor data publications in milliseconds
#define PUBLISH_INTERVAL  5000

#endif
