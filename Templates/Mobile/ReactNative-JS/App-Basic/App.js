import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity } from 'react-native';

/** Root application component with a simple counter demo. */
export default function App() {
  // Local state tracking the current tap count.
  const [count, setCount] = useState(0);

  return (
    <View style={styles.container}>
      {/* Greeting and counter display */}
      <Text style={styles.title}>Hello, Build Your App!</Text>
      <Text style={styles.counter}>Count: {count}</Text>
      {/* Button increments the counter on each tap */}
      <TouchableOpacity style={styles.button} onPress={() => setCount(c => c + 1)}>
        <Text style={styles.buttonText}>Tap me</Text>
      </TouchableOpacity>
    </View>
  );
}

/** Stylesheet for the counter screen layout. */
const styles = StyleSheet.create({
  container: { flex: 1, justifyContent: 'center', alignItems: 'center', backgroundColor: '#fff' },
  title: { fontSize: 24, fontWeight: 'bold', marginBottom: 16 },
  counter: { fontSize: 20, marginBottom: 16 },
  button: { backgroundColor: '#007AFF', paddingHorizontal: 32, paddingVertical: 12, borderRadius: 8 },
  buttonText: { color: '#fff', fontSize: 16 },
});
