# ESP32 WiFi HTTP Template

> ESP32-Sketch mit WLAN-Verbindung und HTTP-Server. / ESP32 sketch with Wi-Fi and HTTP server.

## Features
- WLAN-Verbindung mit Timeout / Wi-Fi connection with timeout
- Eingebauter HTTP-Server mit Beispielroute / Built-in HTTP server with example route
- Konfiguration via `config.h` / Configuration via `config.h`

## How to Use
1. Trage deine WLAN-Daten in `config.h` ein / Enter your Wi-Fi credentials in `config.h`
2. Öffne `sketch.ino` in Arduino IDE oder PlatformIO / Open `sketch.ino` in Arduino IDE or PlatformIO
3. Wähle dein ESP32-Board und lade hoch / Select your ESP32 board and upload

## Tech Stack
- ESP32 Arduino Core
- WiFi + WebServer

## Extension Ideas
- mDNS-Responder / mDNS responder
- OTA-Updates / OTA updates
- HTTPS mit WiFiClientSecure / HTTPS with WiFiClientSecure