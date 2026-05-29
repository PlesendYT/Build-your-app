// Data Structure test runner - demonstrates ArrayList, Linked List, Stack, and Queue usage.
#include "datastruct.h"
#include <stdio.h>
#include <stdlib.h>

// Test all ArrayList operations: create, push, get, size, destroy.
static void test_arraylist(void) {
    printf("=== ArrayList ===\n");
    ArrayList *list = alist_create();
    if (!list) { fprintf(stderr, "Failed to create list\n"); return; }

    for (int i = 0; i < 10; i++)
        alist_push(list, i * 10);

    printf("Size: %zu\n", alist_size(list));
    for (size_t i = 0; i < alist_size(list); i++)
        printf("  [%zu] = %d\n", i, alist_get(list, i));

    alist_destroy(list);
}

// Test all Linked List operations: append, prepend, print, destroy.
static void test_linkedlist(void) {
    printf("\n=== Linked List ===\n");
    Node *list = NULL;
    for (int i = 0; i < 5; i++)
        list = list_append(list, i);

    list = list_prepend(list, 99);
    printf("List: ");
    list_print(list);
    list_destroy(list);
}

// Test all Stack operations: push, peek, pop, is_empty, destroy.
static void test_stack(void) {
    printf("\n=== Stack ===\n");
    Stack *s = stack_create();
    for (int i = 0; i < 5; i++)
        stack_push(s, i);

    printf("Peek: %d\n", stack_peek(s));
    while (!stack_is_empty(s))
        printf("Pop: %d\n", stack_pop(s));

    stack_destroy(s);
}

// Test all Queue operations: enqueue, peek, dequeue, is_empty, destroy.
static void test_queue(void) {
    printf("\n=== Queue ===\n");
    Queue *q = queue_create();
    for (int i = 0; i < 5; i++)
        queue_enqueue(q, i);

    printf("Peek: %d\n", queue_peek(q));
    while (!queue_is_empty(q))
        printf("Dequeue: %d\n", queue_dequeue(q));

    queue_destroy(q);
}

// Entry point: runs all data structure tests.
int main(void) {
    test_arraylist();
    test_linkedlist();
    test_stack();
    test_queue();
    return 0;
}
