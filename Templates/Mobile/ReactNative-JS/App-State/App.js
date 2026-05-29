import React from 'react';
import { Provider, useSelector, useDispatch } from 'react-redux';
import { configureStore, createSlice } from '@reduxjs/toolkit';
import { View, Text, Button, FlatList, StyleSheet } from 'react-native';

/** Redux slice managing the todos array (add / remove actions). */
const todosSlice = createSlice({
  name: 'todos',
  initialState: { items: [] },
  reducers: {
    // Push a new todo item with a unique id and the provided text.
    addTodo: (state, action) => { state.items.push({ id: Date.now(), text: action.payload }); },
    // Filter out the todo matching the supplied id.
    removeTodo: (state, action) => { state.items = state.items.filter(t => t.id !== action.payload); },
  },
});

// Destructure action creators from the slice.
const { addTodo, removeTodo } = todosSlice.actions;

// Create the Redux store with the todos reducer.
const store = configureStore({ reducer: { todos: todosSlice.reducer } });

/** Displays the list of todos and handles add / remove interactions. */
function TodoList() {
  // Subscribe to the items array from the Redux store.
  const items = useSelector(state => state.todos.items);
  const dispatch = useDispatch();

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Redux Toolkit Todos</Text>
      {/* Dispatch an add action with a sequentially numbered label */}
      <Button title="Add Todo" onPress={() => dispatch(addTodo(`Todo ${items.length + 1}`))} />
      {/* Render each todo item with a remove button */}
      <FlatList
        data={items}
        keyExtractor={item => item.id.toString()}
        renderItem={({ item }) => (
          <View style={styles.todo}>
            <Text>{item.text}</Text>
            <Button title="X" onPress={() => dispatch(removeTodo(item.id))} />
          </View>
        )}
      />
    </View>
  );
}

/** Root component wrapping the app in the Redux Provider. */
export default function App() {
  return (
    <Provider store={store}>
      <TodoList />
    </Provider>
  );
}

/** Stylesheet for the todo list screen. */
const styles = StyleSheet.create({
  container: { flex: 1, paddingTop: 60, paddingHorizontal: 16 },
  title: { fontSize: 24, marginBottom: 16, textAlign: 'center' },
  todo: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', padding: 8, borderBottomWidth: 1 },
});
