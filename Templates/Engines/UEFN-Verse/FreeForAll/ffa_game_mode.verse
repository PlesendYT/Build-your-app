# Free-For-All mode: every player fights for themselves; first to the score limit or last alive wins.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Free-For-All device class managing scores, timer, and respawns.
ffa_game_mode := class(creative_device):
    @editable
    ScoreLimit : int = 30
    @editable
    TimeLimit : float = 300.0
    @editable
    RespawnDelay : float = 3.0
    @editable
    RespawnDevice : respawn_device = respawn_device{}
    
    var PlayerScores : [agent]int = map{}
    var GameActive : logic = false
    var ElapsedTime : float = 0.0
    
    # Activates the game, subscribes to player joins, and starts the game timer.
    OnBegin<override>()<suspends>() : void =
        GameActive = true
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { GameTimer[] }
    
    # Tracks elapsed time and ends the game when the time limit is reached.
    GameTimer()<suspends>() : void =
        while GameActive:
            Sleep(1.0)
            set ElapsedTime += 1.0
            if ElapsedTime >= TimeLimit:
                EndGame[]
    
    # Initializes a new player's score to zero.
    OnPlayerJoined(Agent : agent) : void =
        set PlayerScores[Agent] = 0
        Print("Player joined FFA")
    
    # Awards a kill point to the eliminator and checks if they reached the score limit.
    OnPlayerEliminated(Eliminated : agent, Eliminator : agent) : void =
        if (Eliminator <> Eliminated):
            if (PlayerScores[Eliminator] <> 0):
                set PlayerScores[Eliminator] += 1
            else:
                set PlayerScores[Eliminator] = 1
            if (PlayerScores[Eliminator] >= ScoreLimit):
                EndGame[]
                Print("Winner: {Eliminator} with {PlayerScores[Eliminator]} kills")
        
        spawn { DelayedRespawn[Eliminated] }
    
    # Respawns a player after the configured delay.
    DelayedRespawn(Agent : agent)<suspends>() : void =
        Sleep(RespawnDelay)
        RespawnDevice.Spawn(Agent)
    
    # Ends the game and awards a bonus to the best player.
    EndGame() : void =
        set GameActive = false
        Print("Game Over!")
        BestPlayer := GetBestPlayer[]
        if (FortChar := BestPlayer.GetFortCharacter[]):
            FortChar.AddGameResource(50)
    
    # Returns the player with the highest score.
    GetBestPlayer() : agent =
        var BestAgent : agent = agent{}
        var BestScore : int = 0
        for (Player : PlayerScores, Score : PlayerScores[Player]):
            if (Score > BestScore):
                set BestAgent = Player
                set BestScore = Score
        BestAgent
