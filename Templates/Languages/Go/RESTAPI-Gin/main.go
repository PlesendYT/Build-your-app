// Package main implements a REST API for task management using Gin and GORM,
// backed by a SQLite database with full CRUD operations.
package main

import (
	"net/http"

	"github.com/gin-gonic/gin"
	"gorm.io/driver/sqlite"
	"gorm.io/gorm"
)

// Task represents a to-do item persisted in the SQLite database.
type Task struct {
	ID          uint   `json:"id" gorm:"primaryKey"`
	Title       string `json:"title" binding:"required"`
	Description string `json:"description"`
	Completed   bool   `json:"completed"`
}

// db is the global GORM database connection.
var db *gorm.DB

// initDB opens the SQLite database and auto-migrates the Task schema.
func initDB() {
	var err error
	db, err = gorm.Open(sqlite.Open("tasks.db"), &gorm.Config{})
	if err != nil {
		panic("DB connection failed: " + err.Error())
	}
	db.AutoMigrate(&Task{})
}

// getTasks returns all tasks from the database.
func getTasks(c *gin.Context) {
	var tasks []Task
	db.Find(&tasks)
	c.JSON(http.StatusOK, tasks)
}

// getTask returns a single task by its ID, or 404 if not found.
func getTask(c *gin.Context) {
	var task Task
	if err := db.First(&task, c.Param("id")).Error; err != nil {
		c.JSON(http.StatusNotFound, gin.H{"error": "Task not found"})
		return
	}
	c.JSON(http.StatusOK, task)
}

// createTask adds a new task after validating the JSON request body.
func createTask(c *gin.Context) {
	var task Task
	if err := c.ShouldBindJSON(&task); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}
	db.Create(&task)
	c.JSON(http.StatusCreated, task)
}

// updateTask modifies an existing task by ID with the provided fields.
func updateTask(c *gin.Context) {
	var task Task
	if err := db.First(&task, c.Param("id")).Error; err != nil {
		c.JSON(http.StatusNotFound, gin.H{"error": "Task not found"})
		return
	}
	var input Task
	if err := c.ShouldBindJSON(&input); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}
	db.Model(&task).Updates(input)
	c.JSON(http.StatusOK, task)
}

// deleteTask removes a task by ID, returning 204 on success.
func deleteTask(c *gin.Context) {
	if err := db.Delete(&Task{}, c.Param("id")).Error; err != nil {
		c.JSON(http.StatusNotFound, gin.H{"error": "Task not found"})
		return
	}
	c.JSON(http.StatusNoContent, nil)
}

// main initializes the database, configures Gin routes, and starts
// the HTTP server on :8080.
func main() {
	initDB()
	r := gin.Default()

	api := r.Group("/api")
	{
		api.GET("/tasks", getTasks)
		api.GET("/tasks/:id", getTask)
		api.POST("/tasks", createTask)
		api.PUT("/tasks/:id", updateTask)
		api.DELETE("/tasks/:id", deleteTask)
	}

	r.Run(":8080")
}
