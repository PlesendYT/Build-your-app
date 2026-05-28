#include "datastruct.h"
#include <stdio.h>
#include <stdlib.h>

#define INITIAL_CAPACITY 16

/* ArrayList */
ArrayList *alist_create(void) {
    ArrayList *list = calloc(1, sizeof(ArrayList));
    if (!list) return NULL;
    list->data = malloc(INITIAL_CAPACITY * sizeof(int));
    if (!list->data) { free(list); return NULL; }
    list->capacity = INITIAL_CAPACITY;
    list->size = 0;
    return list;
}

void alist_destroy(ArrayList *list) {
    if (list) { free(list->data); free(list); }
}

bool alist_push(ArrayList *list, int value) {
    if (list->size >= list->capacity) {
        size_t new_cap = list->capacity * 2;
        int *tmp = realloc(list->data, new_cap * sizeof(int));
        if (!tmp) return false;
        list->data = tmp;
        list->capacity = new_cap;
    }
    list->data[list->size++] = value;
    return true;
}

int alist_get(const ArrayList *list, size_t index) {
    return list->data[index];
}

bool alist_set(ArrayList *list, size_t index, int value) {
    if (index >= list->size) return false;
    list->data[index] = value;
    return true;
}

size_t alist_size(const ArrayList *list) { return list->size; }

/* Linked List */
Node *node_create(int value) {
    Node *node = calloc(1, sizeof(Node));
    if (node) { node->data = value; }
    return node;
}

void list_destroy(Node *head) {
    while (head) {
        Node *next = head->next;
        free(head);
        head = next;
    }
}

Node *list_append(Node *head, int value) {
    Node *new_node = node_create(value);
    if (!head) return new_node;
    Node *cur = head;
    while (cur->next) cur = cur->next;
    cur->next = new_node;
    return head;
}

Node *list_prepend(Node *head, int value) {
    Node *new_node = node_create(value);
    new_node->next = head;
    return new_node;
}

void list_print(const Node *head) {
    printf("[");
    while (head) {
        printf("%d", head->data);
        if (head->next) printf(", ");
        head = head->next;
    }
    printf("]\n");
}

/* Stack */
Stack *stack_create(void) {
    Stack *s = calloc(1, sizeof(Stack));
    if (!s) return NULL;
    s->data = malloc(INITIAL_CAPACITY * sizeof(int));
    if (!s->data) { free(s); return NULL; }
    s->capacity = INITIAL_CAPACITY;
    s->top = 0;
    return s;
}

void stack_destroy(Stack *s) { if (s) { free(s->data); free(s); } }

bool stack_push(Stack *s, int value) {
    if (s->top >= s->capacity) {
        size_t new_cap = s->capacity * 2;
        int *tmp = realloc(s->data, new_cap * sizeof(int));
        if (!tmp) return false;
        s->data = tmp;
        s->capacity = new_cap;
    }
    s->data[s->top++] = value;
    return true;
}

int stack_pop(Stack *s) { return s->data[--s->top]; }
int stack_peek(const Stack *s) { return s->data[s->top - 1]; }
bool stack_is_empty(const Stack *s) { return s->top == 0; }

/* Queue */
Queue *queue_create(void) {
    Queue *q = calloc(1, sizeof(Queue));
    if (!q) return NULL;
    q->data = malloc(INITIAL_CAPACITY * sizeof(int));
    if (!q->data) { free(q); return NULL; }
    q->capacity = INITIAL_CAPACITY;
    q->front = 0;
    q->rear = 0;
    q->size = 0;
    return q;
}

void queue_destroy(Queue *q) { if (q) { free(q->data); free(q); } }

bool queue_enqueue(Queue *q, int value) {
    if (q->size >= q->capacity) {
        size_t new_cap = q->capacity * 2;
        int *tmp = malloc(new_cap * sizeof(int));
        if (!tmp) return false;
        for (size_t i = 0; i < q->size; i++)
            tmp[i] = q->data[(q->front + i) % q->capacity];
        free(q->data);
        q->data = tmp;
        q->capacity = new_cap;
        q->front = 0;
        q->rear = q->size;
    }
    q->data[q->rear] = value;
    q->rear = (q->rear + 1) % q->capacity;
    q->size++;
    return true;
}

int queue_dequeue(Queue *q) {
    int val = q->data[q->front];
    q->front = (q->front + 1) % q->capacity;
    q->size--;
    return val;
}

int queue_peek(const Queue *q) { return q->data[q->front]; }
bool queue_is_empty(const Queue *q) { return q->size == 0; }
