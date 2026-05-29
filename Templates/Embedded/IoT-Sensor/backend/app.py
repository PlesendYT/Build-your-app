"""Python backend for IoT sensor data ingestion.

Subscribes to an MQTT topic and stores incoming sensor readings in an
in-memory list with timestamps. Exposes a simple REST API via Flask
to retrieve the collected data.
"""

import json
import time
from threading import Thread

import paho.mqtt.client as mqtt
from flask import Flask, jsonify, request

# Configuration constants
MQTT_BROKER = "broker.example.com"
MQTT_PORT = 1883
MQTT_TOPIC = "sensor/environment"
FLASK_PORT = 5000

# In-memory store for received sensor readings
readings = []

app = Flask(__name__)


def on_message(client, userdata, msg):
    """MQTT callback: decodes the payload and appends it to the store."""
    try:
        data = json.loads(msg.payload.decode())
        data["timestamp"] = time.time()
        readings.append(data)
        print(f"Received: {data}")
    except Exception as e:
        print(f"Failed to parse message: {e}")


@app.route("/api/readings", methods=["GET"])
def get_readings():
    """Return all sensor readings, optionally filtered by count."""
    count = request.args.get("count", default=None, type=int)
    if count:
        return jsonify(readings[-count:])
    return jsonify(readings)


@app.route("/api/readings/latest", methods=["GET"])
def get_latest():
    """Return the single most recent sensor reading."""
    if not readings:
        return jsonify({"error": "No readings available"}), 404
    return jsonify(readings[-1])


def start_mqtt():
    """Connect to the MQTT broker and listen for sensor data."""
    client = mqtt.Client()
    client.on_message = on_message
    client.connect(MQTT_BROKER, MQTT_PORT, 60)
    client.subscribe(MQTT_TOPIC)
    client.loop_forever()


if __name__ == "__main__":
    # Run the MQTT subscriber in a background thread
    mqtt_thread = Thread(target=start_mqtt, daemon=True)
    mqtt_thread.start()
    print(f"Starting Flask API on port {FLASK_PORT}")
    app.run(host="0.0.0.0", port=FLASK_PORT)
