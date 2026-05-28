import React from 'react';
import { Provider, useSelector, useDispatch } from 'react-redux';
import { configureStore, createSlice } from '@reduxjs/toolkit';
import { View, Text, Button, FlatList, StyleSheet } from 'react-native';

const todosSlice = createSlice({
  name: 'todos',
  initialState: { items: [] },
  reducers: {
    addTodo: (state, action) => { state.items.push({ id: Date.now(), text: action.payload }); },
    removeTodo: (state, action) => { state.items = state.items.filter(t => t.id !== action.payload); },
  },
});

const { addTodo, removeTodo } = todosSlice.actions;

const store = configureStore({ reducer: { todos: todosSlice.reducer } });

function TodoList() {
  const items = useSelector(state => state.todos.items);
  const dispatch = useDispatch();

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Redux Toolkit Todos</Text>
      <Button title="Add Todo" onPress={() => dispatch(addTodo(`Todo ${items.length + 1}`))} />
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

export default function App() {
  return (
    <Provider store={store}>
      <TodoList />
    </Provider>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, paddingTop: 60, paddingHorizontal: 16 },
  title: { fontSize: 24, marginBottom: 16, textAlign: 'center' },
  todo: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', padding: 8, borderBottomWidth: 1 },
});
