// Domain model for an Order in a hexagonal (ports & adapters) architecture.
// The domain is the innermost layer with zero external dependencies.

package domain

import (
	"time"
	"errors"
)

// Status represents the lifecycle state of an order.
type Status string

const (
	StatusPending   Status = "PENDING"
	StatusConfirmed Status = "CONFIRMED"
	StatusShipped   Status = "SHIPPED"
	StatusDelivered Status = "DELIVERED"
)

// OrderItem is a single line item within an order.
type OrderItem struct {
	ProductID string
	Quantity  int
	Price     float64
}

// Order is the core domain entity representing a customer purchase.
type Order struct {
	ID        string
	CustomerID string
	Items     []OrderItem
	Total     float64
	Status    Status
	CreatedAt time.Time
	UpdatedAt time.Time
}

// NewOrder creates a new Order with the given customer and items.
func NewOrder(customerID string, items []OrderItem) (*Order, error) {
	if customerID == "" {
		return nil, errors.New("customer ID is required")
	}
	if len(items) == 0 {
		return nil, errors.New("order must have at least one item")
	}
	var total float64
	for _, item := range items {
		total += item.Price * float64(item.Quantity)
	}
	now := time.Now()
	return &Order{
		ID:         generateID(),
		CustomerID: customerID,
		Items:      items,
		Total:      total,
		Status:     StatusPending,
		CreatedAt:  now,
		UpdatedAt:  now,
	}, nil
}

// generateID returns a simple unique identifier for the order.
func generateID() string {
	return time.Now().Format("20060102150405") + "-" + randomString(8)
}

// randomString returns a pseudo-random alphanumeric string of length n.
func randomString(n int) string {
	const letters = "abcdefghijklmnopqrstuvwxyz0123456789"
	result := make([]byte, n)
	for i := range result {
		result[i] = letters[time.Now().UnixNano()%int64(len(letters))]
		time.Sleep(1) // ensure different seeds
	}
	return string(result)
}
