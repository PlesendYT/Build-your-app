// Package main implements a Discord bot with slash commands
// (ping, roll, say) and a text-based !uptime command.
package main

import (
	"flag"
	"fmt"
	"log"
	"math/rand"
	"os"
	"os/signal"
	"syscall"
	"time"

	"github.com/bwmarrin/discordgo"
)

var (
	// token is the Discord bot token, read from flag or BOT_TOKEN env var.
	token    string
	// commands defines the slash commands registered on startup.
	commands = []*discordgo.ApplicationCommand{
		{Name: "ping", Description: "Pong!"},
		{Name: "roll", Description: "Würfeln (1-100)"},
		{Name: "say", Description: "Wiederhole einen Text",
			Options: []*discordgo.ApplicationCommandOption{
				{Type: discordgo.ApplicationCommandOptionString, Name: "text",
					Description: "Text", Required: true},
			}},
	}
)

// main initializes the bot session, registers handlers and slash commands,
// then blocks until SIGINT/SIGTERM.
func main() {
	flag.StringVar(&token, "token", os.Getenv("BOT_TOKEN"), "Bot token")
	flag.Parse()

	if token == "" {
		log.Fatal("Token erforderlich (BOT_TOKEN env oder -token flag)")
	}

	session, err := discordgo.New("Bot " + token)
	if err != nil {
		log.Fatal(err)
	}

	session.AddHandler(func(s *discordgo.Session, r *discordgo.Ready) {
		log.Printf("Bot eingeloggt als %s", r.User.String())
	})

	session.AddHandler(func(s *discordgo.Session, i *discordgo.InteractionCreate) {
		switch i.ApplicationCommandData().Name {
		case "ping":
			s.InteractionRespond(i.Interaction, &discordgo.InteractionResponse{
				Type: discordgo.InteractionResponseChannelMessageWithSource,
				Data: &discordgo.InteractionResponseData{
					Content: "Pong! " + time.Now().Format(time.RFC3339),
				},
			})
		case "roll":
			roll := rand.Intn(100) + 1
			s.InteractionRespond(i.Interaction, &discordgo.InteractionResponse{
				Type: discordgo.InteractionResponseChannelMessageWithSource,
				Data: &discordgo.InteractionResponseData{
					Embeds: []*discordgo.MessageEmbed{{
						Title:       "🎲 Würfelergebnis",
						Description: fmt.Sprintf("Du hast **%d** gewürfelt!", roll),
						Color:       0x00ff00,
					}},
				},
			})
		case "say":
			text := i.ApplicationCommandData().Options[0].StringValue()
			s.InteractionRespond(i.Interaction, &discordgo.InteractionResponse{
				Type: discordgo.InteractionResponseChannelMessageWithSource,
				Data: &discordgo.InteractionResponseData{
					Content: text,
				},
			})
		}
	})

	session.AddHandler(func(s *discordgo.Session, m *discordgo.MessageCreate) {
		if m.Author.ID == s.State.User.ID {
			return
		}
		if m.Content == "!uptime" {
			s.ChannelMessageSend(m.ChannelID, fmt.Sprintf(
				"🕐 Läuft seit %s", time.Since(startTime).Round(time.Second)))
		}
	})

	err = session.Open()
	if err != nil {
		log.Fatal(err)
	}
	defer session.Close()

	for _, cmd := range commands {
		_, err := session.ApplicationCommandCreate(session.State.User.ID, "", cmd)
		if err != nil {
			log.Printf("Command %s: %v", cmd.Name, err)
		}
	}

	sc := make(chan os.Signal, 1)
	signal.Notify(sc, syscall.SIGINT, syscall.SIGTERM)
	<-sc
	log.Println("Bot wird heruntergefahren...")
}

// startTime records when the bot started, used by the !uptime command.
var startTime = time.Now()
