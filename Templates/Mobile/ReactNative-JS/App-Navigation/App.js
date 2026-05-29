import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { View, Text, Button, StyleSheet } from 'react-native';

// Navigator instances used throughout the app.
const Stack = createNativeStackNavigator();
const Tab = createBottomTabNavigator();

/** Screen shown on the Home tab with a link to the Details screen. */
function HomeScreen({ navigation }) {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Home Screen</Text>
      {/* Navigate to Details screen, passing a parameter */}
      <Button title="Go to Details" onPress={() => navigation.navigate('Details', { item: 'Hello from Home!' })} />
    </View>
  );
}

/** Displays the parameter received from the Home screen. */
function DetailsScreen({ route }) {
  // Extract the item passed via navigation params.
  const { item } = route.params;
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Details Screen</Text>
      <Text>{item}</Text>
    </View>
  );
}

/** Standalone settings screen accessible from the bottom tab. */
function SettingsScreen() {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Settings Screen</Text>
    </View>
  );
}

/** Wraps Home and Details in a stack navigator for push/pop transitions. */
function HomeStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="Home" component={HomeScreen} />
      <Stack.Screen name="Details" component={DetailsScreen} />
    </Stack.Navigator>
  );
}

/** Root component rendering the tab navigator inside a NavigationContainer. */
export default function App() {
  return (
    <NavigationContainer>
      <Tab.Navigator>
        <Tab.Screen name="HomeTab" component={HomeStack} options={{ headerShown: false, title: 'Home' }} />
        <Tab.Screen name="Settings" component={SettingsScreen} />
      </Tab.Navigator>
    </NavigationContainer>
  );
}

/** Shared styles for navigation screen layouts. */
const styles = StyleSheet.create({
  container: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  title: { fontSize: 24, marginBottom: 16 },
});
