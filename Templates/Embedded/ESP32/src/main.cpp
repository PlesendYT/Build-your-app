// ESP32 Arduino core application: WiFi connection, web server, MQTT
// Connects to a WiFi network, serves a simple web page, and publishes
// sensor data to an MQTT broker at a regular interval.

#include <WiFi.h>
#include <WebServer.h>
#include <PubSubClient.h>
#include "config.h"

// Create instances for the web server on port 80 and the MQTT client
WebServer server(80);
WiFiClient espClient;
PubSubClient mqttClient(espClient);

// Forward declarations for the MQTT callback and reconnect function
void mqttCallback(char* topic, byte* payload, unsigned int length);
void reconnectMQTT();

void setup() {
  Serial.begin(115200);
  delay(100);
  Serial.println(F("ESP32 starting..."));

  // Connect to WiFi using credentials defined in config.h
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print(F("Connecting to WiFi"));
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(F("."));
  }
  Serial.println(F(" connected"));
  Serial.print(F("IP address: "));
  Serial.println(WiFi.localIP());

  // Configure the MQTT broker and callback function
  mqttClient.setServer(MQTT_BROKER, MQTT_PORT);
  mqttClient.setCallback(mqttCallback);

  // Define the root URL handler that returns a simple status page
  server.on("/", []() {
    server.send(200, "text/html",
      "<h1>ESP32 Web Server</h1>"
      "<p>Status: Running</p>"
      "<p>Uptime: " + String(millis() / 1000) + " s</p>");
  });

  // Start the web server
  server.begin();
  Serial.println(F("HTTP server started"));
}

void loop() {
  // Reconnect to MQTT broker if the connection drops
  if (!mqttClient.connected()) {
    reconnectMQTT();
  }
  // Process incoming MQTT messages
  mqttClient.loop();
  // Handle incoming HTTP requests
  server.handleClient();

  // Publish a dummy sensor reading every PUBLISH_INTERVAL milliseconds
  static unsigned long lastPublish = 0;
  if (millis() - lastPublish > PUBLISH_INTERVAL) {
    lastPublish = millis();
    String payload = "{\"value\":" + String(analogRead(34)) + "}";
    mqttClient.publish(MQTT_TOPIC, payload.c_str());
    Serial.println(F("Published sensor data"));
  }
}

// Callback invoked when an MQTT message is received on a subscribed topic
void mqttCallback(char* topic, byte* payload, unsigned int length) {
  Serial.print(F("Message arrived on topic: "));
  Serial.println(topic);
  Serial.print(F("Payload: "));
  for (unsigned int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
  }
  Serial.println();
}

// Blocking reconnect loop for the MQTT client
void reconnectMQTT() {
  while (!mqttClient.connected()) {
    Serial.print(F("Connecting to MQTT..."));
    if (mqttClient.connect("ESP32Client")) {
      Serial.println(F(" connected"));
      mqttClient.subscribe(MQTT_TOPIC);
    } else {
      Serial.print(F(" failed, rc="));
      Serial.print(mqttClient.state());
      Serial.println(F(" retrying in 5s"));
      delay(5000);
    }
  }
}
