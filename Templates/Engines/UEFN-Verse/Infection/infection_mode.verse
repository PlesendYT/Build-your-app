using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

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
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartGame[] }
    
    OnPlayerJoined(Agent : agent) : void =
        SurvivorTeam.AddToTeam(Agent)
        set SurvivorCount += 1
    
    StartGame()<suspends>() : void =
        Sleep(5.0)
        SelectInitialInfected[]
        set RoundActive = true
        spawn { RoundTimer[] }
    
    SelectInitialInfected() : void =
        Players := GetAllPlayers[]
        if (Players.Length >= 2):
            InfectedIndex := Random.UpperBound(Players.Length - 1)
            InfectPlayer(Players[InfectedIndex])
    
    InfectPlayer(Agent : agent) : void =
        SurvivorTeam.RemoveFromTeam(Agent)
        InfectedTeam.AddToTeam(Agent)
        set InfectedCount += 1
        set SurvivorCount -= 1
        Print("{Agent} is infected!")
    
    OnContactInfection(Attacker : agent, Victim : agent) : void =
        if (RoundActive):
            if (SurvivorTeam.IsMember(Victim) and InfectedTeam.IsMember(Attacker)):
                InfectPlayer(Victim)
                if (SurvivorCount <= 0):
                    EndRound(false)
    
    OnCurePickup(Agent : agent) : void =
        if (RoundActive and InfectedTeam.IsMember(Agent)):
            InfectedTeam.RemoveFromTeam(Agent)
            SurvivorTeam.AddToTeam(Agent)
            set InfectedCount -= 1
            set SurvivorCount += 1
            CurePickup.Destroy[]
            spawn { RespawnCure[] }
    
    RespawnCure()<suspends>() : void =
        Sleep(30.0)
        CurePickup.SpawnItem[]
    
    RoundTimer()<suspends>() : void =
        Sleep(RoundTime)
        if RoundActive:
            EndRound(true)
    
    EndRound(SurvivorsWin : logic) : void =
        set RoundActive = false
        if (SurvivorsWin):
            Print("Survivors win! {SurvivorCount} alive!")
        else:
            Print("Infected win! All infected!")
    
    GetAllPlayers() : []agent =
        GetPlayspace().GetPlayers()
