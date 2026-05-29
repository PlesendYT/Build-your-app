# Infection mode: one player starts infected and spreads the infection by contact; survivors try to outlast.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Infection device class managing teams, infection spread, and round logic.
infection_mode := class(creative_device):
    @editable
    RoundTime : float = 180.0
    @editable
    SurvivorTeam : team_device = team_device{}
    @editable
    InfectedTeam : team_device = team_device{}
    @editable
    CurePickup : item_spawner_device = item_spawner_device{}
    
    var RoundActive : logic = false
    var InfectedCount : int = 0
    var SurvivorCount : int = 0
    
    # Subscribes to player join events and starts the game loop.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartGame[] }
    
    # Adds a joining player to the survivor team.
    OnPlayerJoined(Agent : agent) : void =
        SurvivorTeam.AddToTeam(Agent)
        set SurvivorCount += 1
    
    # Delays then selects the initial infected and activates the round timer.
    StartGame()<suspends>() : void =
        Sleep(5.0)
        SelectInitialInfected[]
        set RoundActive = true
        spawn { RoundTimer[] }
    
    # Randomly picks a player to be the first infected.
    SelectInitialInfected() : void =
        Players := GetAllPlayers[]
        if (Players.Length >= 2):
            InfectedIndex := Random.UpperBound(Players.Length - 1)
            InfectPlayer(Players[InfectedIndex])
    
    # Moves a player from survivor to infected team and updates counts.
    InfectPlayer(Agent : agent) : void =
        SurvivorTeam.RemoveFromTeam(Agent)
        InfectedTeam.AddToTeam(Agent)
        set InfectedCount += 1
        set SurvivorCount -= 1
        Print("{Agent} is infected!")
    
    # Spreads infection when an infected player contacts a survivor; ends round if all infected.
    OnContactInfection(Attacker : agent, Victim : agent) : void =
        if (RoundActive):
            if (SurvivorTeam.IsMember(Victim) and InfectedTeam.IsMember(Attacker)):
                InfectPlayer(Victim)
                if (SurvivorCount <= 0):
                    EndRound(false)
    
    # Cures an infected player and respawns the cure pickup after a delay.
    OnCurePickup(Agent : agent) : void =
        if (RoundActive and InfectedTeam.IsMember(Agent)):
            InfectedTeam.RemoveFromTeam(Agent)
            SurvivorTeam.AddToTeam(Agent)
            set InfectedCount -= 1
            set SurvivorCount += 1
            CurePickup.Destroy[]
            spawn { RespawnCure[] }
    
    # Waits 30 seconds then respawns the cure item.
    RespawnCure()<suspends>() : void =
        Sleep(30.0)
        CurePickup.SpawnItem[]
    
    # Waits for the round time to elapse then ends the round with survivors winning.
    RoundTimer()<suspends>() : void =
        Sleep(RoundTime)
        if RoundActive:
            EndRound(true)
    
    # Ends the round and announces the winner based on the given flag.
    EndRound(SurvivorsWin : logic) : void =
        set RoundActive = false
        if (SurvivorsWin):
            Print("Survivors win! {SurvivorCount} alive!")
        else:
            Print("Infected win! All infected!")
    
    # Returns all players currently in the game.
    GetAllPlayers() : []agent =
        GetPlayspace().GetPlayers()
