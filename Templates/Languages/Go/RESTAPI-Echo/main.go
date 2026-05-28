package main

import (
	"net/http"
	"sync"
	"time"

	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
)

type User struct {
	ID        int       `json:"id"`
	Name      string    `json:"name" validate:"required"`
	Email     string    `json:"email" validate:"required,email"`
	CreatedAt time.Time `json:"created_at"`
}

var (
	users   = []User{{ID: 1, Name: "Alice", Email: "alice@example.com", CreatedAt: time.Now()}}
	nextID  = 2
	usersMu sync.RWMutex
)

func main() {
	e := echo.New()
	e.Use(middleware.Logger())
	e.Use(middleware.Recover())
	e.Use(middleware.CORS())

	e.GET("/api/users", func(c echo.Context) error {
		usersMu.RLock()
		defer usersMu.RUnlock()
		return c.JSON(http.StatusOK, users)
	})

	e.GET("/api/users/:id", func(c echo.Context) error {
		usersMu.RLock()
		defer usersMu.RUnlock()
		for _, u := range users {
			if u.ID == c.Get("id").(int) {
				return c.JSON(http.StatusOK, u)
			}
		}
		return c.JSON(http.StatusNotFound, map[string]string{"error": "not found"})
	})

	e.POST("/api/users", func(c echo.Context) error {
		var u User
		if err := c.Bind(&u); err != nil {
			return c.JSON(http.StatusBadRequest, map[string]string{"error": err.Error()})
		}
		if err := c.Validate(u); err != nil {
			return c.JSON(http.StatusBadRequest, map[string]string{"error": err.Error()})
		}
		usersMu.Lock()
		u.ID = nextID
		nextID++
		u.CreatedAt = time.Now()
		users = append(users, u)
		usersMu.Unlock()
		return c.JSON(http.StatusCreated, u)
	})

	e.DELETE("/api/users/:id", func(c echo.Context) error {
		usersMu.Lock()
		defer usersMu.Unlock()
		for i, u := range users {
			if u.ID == c.Get("id").(int) {
				users = append(users[:i], users[i+1:]...)
				return c.NoContent(http.StatusNoContent)
			}
		}
		return c.JSON(http.StatusNotFound, map[string]string{"error": "not found"})
	})

	e.Logger.Fatal(e.Start(":8080"))
}
