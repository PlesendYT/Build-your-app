constexpr int LED_PIN = 13;
constexpr unsigned long BLINK_MS = 500;

unsigned long lastToggle = 0;
bool ledState = false;

void setup() {
    pinMode(LED_PIN, OUTPUT);
}

void loop() {
    const unsigned long now = millis();
    if (now - lastToggle >= BLINK_MS) {
        lastToggle = now;
        ledState = !ledState;
        digitalWrite(LED_PIN, ledState ? HIGH : LOW);
    }
}