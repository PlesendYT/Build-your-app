using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

# BedWars game mode - teams defend beds while gathering resources to eliminate rivals
bedwars_mode := class(creative_device):
    @editable
    TeamIslands : []zone_device = array{}
    @editable
    BedDevices : []item_spawner_device = array{}
    @editable
    ResourceGenerators : []item_spawner_device = array{}
    @editable
    ShopDevices : []item_spawner_device = array{}
    
    var TeamAlive : [team_device]logic = map{} # Tracks which teams are still alive
    var BedAlive : [team_device]logic = map{} # Tracks which team beds are intact
    var TeamAssignments : [agent]team_device = map{} # Maps each player to their team
    var TeamGold : [team_device]int = map{} # Gold resource per team
    
    # Initializes the game - subscribes to player events and starts core loops
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { ResourceLoop[] }
        spawn { CheckWinCondition[] }
    
    # Assigns a team to a newly joined player
    OnPlayerJoined(Agent : agent) : void =
        AssignTeamToPlayer[Agent]
    
    # Assigns a player to a team by cycling through available team islands
    AssignTeamToPlayer(Agent : agent) : void =
        TeamIndex := TeamAssignments.Length % TeamIslands.Length
        if (TeamDevice := GetTeamDeviceForIndex[TeamIndex]):
            set TeamAssignments[Agent] = TeamDevice
            set TeamAlive[TeamDevice] = true
            set BedAlive[TeamDevice] = true
            set TeamGold[TeamDevice] = 0
            Print("{Agent} assigned to team {TeamIndex}")
    
    # Placeholder - returns a team device for a given index
    GetTeamDeviceForIndex(Index : int) : team_device =
        team_device{}
    
    # Periodic resource generation loop that spawns items from generators
    ResourceLoop()<suspends>() : void =
        loop:
            Sleep(2.0)
            for (Generator : ResourceGenerators):
                    Generator.SpawnItem[]
    
    # Marks a team's bed as destroyed, preventing respawn
    OnBedDestroyed(Team : team_device) : void =
        set BedAlive[Team] = false
        Print("Team {Team} bed destroyed!")
    
    # Respawns player if bed is alive, otherwise eliminates team
    OnPlayerEliminated(Agent : agent, Team : team_device) : void =
        if (BedAlive[Team]):
            spawn { RespawnPlayer[Agent] }
        else:
            set TeamAlive[Team] = false
            Print("Team {Team} eliminated!")
    
    # Respawns a player after a short delay with full health
    RespawnPlayer(Agent : agent)<suspends>() : void =
        Sleep(5.0)
        if (FortChar := Agent.GetFortCharacter[]):
            FortChar.SetHealth(100.0)
    
    # Processes a shop purchase - deducts gold and spawns the item
    BuyItem(Agent : agent, Cost : int, Item : item_spawner_device) : void =
        if (Team := TeamAssignments[Agent]):
            if (Gold := TeamGold[Team]):
                if Gold >= Cost:
                    set TeamGold[Team] = Gold - Cost
                    Item.SpawnItem[]
    
    # Periodically checks if only one team remains and declares them winner
    CheckWinCondition()<suspends>() : void =
        loop:
            Sleep(5.0)
            AliveTeams := CountAliveTeams[]
            if AliveTeams <= 1:
                for (Team : TeamAlive, Alive : TeamAlive[Team]):
                    if Alive:
                        Print("Team {Team} wins BedWars!")
                break
    
    # Counts how many teams are still alive in the game
    CountAliveTeams() : int =
        var Count : int = 0
        for (Team : TeamAlive, Alive : TeamAlive[Team]):
            if Alive: set Count += 1
        Count
