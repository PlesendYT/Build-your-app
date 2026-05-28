#ifndef DATASTRUCT_H
#define DATASTRUCT_H

#include <stdbool.h>
#include <stddef.h>

/* Dynamic Array */
typedef struct {
    int *data;
    size_t size;
    size_t capacity;
} ArrayList;

ArrayList *alist_create(void);
void alist_destroy(ArrayList *list);
bool alist_push(ArrayList *list, int value);
int alist_get(const ArrayList *list, size_t index);
bool alist_set(ArrayList *list, size_t index, int value);
size_t alist_size(const ArrayList *list);

/* Linked List Node */
typedef struct Node {
    int data;
    struct Node *next;
} Node;

Node *node_create(int value);
void list_destroy(Node *head);
Node *list_append(Node *head, int value);
Node *list_prepend(Node *head, int value);
void list_print(const Node *head);

/* Stack (Array-based) */
typedef struct {
    int *data;
    size_t top;
    size_t capacity;
} Stack;

Stack *stack_create(void);
void stack_destroy(Stack *s);
bool stack_push(Stack *s, int value);
int stack_pop(Stack *s);
int stack_peek(const Stack *s);
bool stack_is_empty(const Stack *s);

/* Queue (Array-based circular) */
typedef struct {
    int *data;
    size_t front;
    size_t rear;
    size_t size;
    size_t capacity;
} Queue;

Queue *queue_create(void);
void queue_destroy(Queue *q);
bool queue_enqueue(Queue *q, int value);
int queue_dequeue(Queue *q);
int queue_peek(const Queue *q);
bool queue_is_empty(const Queue *q);

#endif /* DATASTRUCT_H */
