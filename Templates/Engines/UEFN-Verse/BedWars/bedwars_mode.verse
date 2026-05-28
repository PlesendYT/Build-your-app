using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

bedwars_mode := class(creative_device):
    @editable
    TeamIslands : []zone_device = array{}
    @editable
    BedDevices : []item_spawner_device = array{}
    @editable
    ResourceGenerators : []item_spawner_device = array{}
    @editable
    ShopDevices : []item_spawner_device = array{}
    
    var TeamAlive : [team_device]logic = map{}
    var BedAlive : [team_device]logic = map{}
    var TeamAssignments : [agent]team_device = map{}
    var TeamGold : [team_device]int = map{}
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { ResourceLoop[] }
        spawn { CheckWinCondition[] }
    
    OnPlayerJoined(Agent : agent) : void =
        AssignTeamToPlayer[Agent]
    
    AssignTeamToPlayer(Agent : agent) : void =
        TeamIndex := TeamAssignments.Length % TeamIslands.Length
        if (TeamDevice := GetTeamDeviceForIndex[TeamIndex]):
            set TeamAssignments[Agent] = TeamDevice
            set TeamAlive[TeamDevice] = true
            set BedAlive[TeamDevice] = true
            set TeamGold[TeamDevice] = 0
            Print("{Agent} assigned to team {TeamIndex}")
    
    GetTeamDeviceForIndex(Index : int) : team_device =
        team_device{}
    
    ResourceLoop()<suspends>() : void =
        loop:
            Sleep(2.0)
            for (Generator : ResourceGenerators):
                Generator.SpawnItem[]
    
    OnBedDestroyed(Team : team_device) : void =
        set BedAlive[Team] = false
        Print("Team {Team} bed destroyed!")
    
    OnPlayerEliminated(Agent : agent, Team : team_device) : void =
        if (BedAlive[Team]):
            spawn { RespawnPlayer[Agent] }
        else:
            set TeamAlive[Team] = false
            Print("Team {Team} eliminated!")
    
    RespawnPlayer(Agent : agent)<suspends>() : void =
        Sleep(5.0)
        if (FortChar := Agent.GetFortCharacter[]):
            FortChar.SetHealth(100.0)
    
    BuyItem(Agent : agent, Cost : int, Item : item_spawner_device) : void =
        if (Team := TeamAssignments[Agent]):
            if (Gold := TeamGold[Team]):
                if Gold >= Cost:
                    set TeamGold[Team] = Gold - Cost
                    Item.SpawnItem[]
    
    CheckWinCondition()<suspends>() : void =
        loop:
            Sleep(5.0)
            AliveTeams := CountAliveTeams[]
            if AliveTeams <= 1:
                for (Team : TeamAlive, Alive : TeamAlive[Team]):
                    if Alive:
                        Print("Team {Team} wins BedWars!")
                break
    
    CountAliveTeams() : int =
        var Count : int = 0
        for (Team : TeamAlive, Alive : TeamAlive[Team]):
            if Alive: set Count += 1
        Count
