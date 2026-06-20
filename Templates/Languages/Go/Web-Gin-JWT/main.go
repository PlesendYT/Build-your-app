package main

import (
    "net/http"
    "time"

    "github.com/gin-gonic/gin"
    "github.com/golang-jwt/jwt/v5"
)

var jwtKey = []byte("super-secret-demo-key-change-me")

type Credentials struct {
    User string `json:"user" binding:"required"`
    Pass string `json:"pass" binding:"required"`
}

var users = map[string]string{"alice": "wonderland"}

func issueToken(user string) (string, error) {
    claims := jwt.MapClaims{
        "sub": user,
        "exp": time.Now().Add(time.Hour).Unix(),
    }
    tok := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
    return tok.SignedString(jwtKey)
}

func authRequired() gin.HandlerFunc {
    return func(c *gin.Context) {
        h := c.GetHeader("Authorization")
        if len(h) < 8 || h[:7] != "Bearer " {
            c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"error": "missing bearer token"})
            return
        }
        _, err := jwt.Parse(h[7:], func(t *jwt.Token) (interface{}, error) { return jwtKey, nil })
        if err != nil {
            c.AbortWithStatusJSON(http.StatusUnauthorized, gin.H{"error": err.Error()})
            return
        }
        c.Next()
    }
}

func main() {
    r := gin.Default()
    r.POST("/login", func(c *gin.Context) {
        var creds Credentials
        if err := c.ShouldBindJSON(&creds); err != nil {
            c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
            return
        }
        if users[creds.User] != creds.Pass {
            c.JSON(http.StatusUnauthorized, gin.H{"error": "invalid credentials"})
            return
        }
        tok, _ := issueToken(creds.User)
        c.JSON(http.StatusOK, gin.H{"token": tok})
    })

    api := r.Group("/api", authRequired())
    api.GET("/me", func(c *gin.Context) {
        c.JSON(http.StatusOK, gin.H{"user": "alice", "role": "admin"})
    })

    r.Run(":8080")
}