#include "config.h"
#include <WiFi.h>
#include <WebServer.h>

WebServer server(80);

void handleRoot() { server.send(200, "text/plain", "Hello from ESP32!\n"); }

void setup() {
    Serial.begin(115200);
    WiFi.mode(WIFI_STA);
    WiFi.begin(WIFI_SSID, WIFI_PASS);
    while (WiFi.status() != WL_CONNECTED) { delay(500); Serial.print("."); }
    Serial.println(WiFi.localIP());
    server.on("/", handleRoot);
    server.begin();
}

void loop() { server.handleClient(); }