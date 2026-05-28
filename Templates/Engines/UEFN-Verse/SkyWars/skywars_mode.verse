using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

skywars_mode := class(creative_device):
    @editable
    IslandSpawnPoints : []respawn_device = array{}
    @editable
    ChestDevices : []item_spawner_device = array{}
    @editable
    CenterChest : item_spawner_device = item_spawner_device{}
    @editable
    VoidZone : zone_device = zone_device{}
    
    var AlivePlayers : [agent]logic = map{}
    var PlayerKills : [agent]int = map{}
    var GameActive : logic = false
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    OnPlayerJoined(Agent : agent) : void =
        set AlivePlayers[Agent] = true
        set PlayerKills[Agent] = 0
        SpawnOnIsland[Agent]
        
        if (TotalAlivePlayers[] >= 2 and not GameActive):
            StartGame[]
    
    StartGame() : void =
        set GameActive = true
        Print("SkyWars started! Last player standing wins!")
        spawn { CenterChestTimer[] }
    
    CenterChestTimer()<suspends>() : void =
        Sleep(60.0)
        CenterChest.SpawnItem[]
        Print("Center chest spawned!")
    
    SpawnOnIsland(Agent : agent) : void =
        IslandIndex := Random.UpperBound(IslandSpawnPoints.Length - 1)
        IslandSpawnPoints[IslandIndex].Spawn(Agent)
        GiveStarterGear[Agent]
    
    GiveStarterGear(Agent : agent) : void =
        if (FortChar := Agent.GetFortCharacter[]):
            FortChar.AddItem("Sword")
            FortChar.AddItem("Blocks", 64)
    
    OnPlayerEliminated(Eliminated : agent, Eliminator : agent) : void =
        set AlivePlayers[Eliminated] = false
        if (Kills := PlayerKills[Eliminator]):
            set PlayerKills[Eliminator] = Kills + 1
        else:
            set PlayerKills[Eliminator] = 1
        
        if (TotalAlivePlayers[] <= 1):
            DeclareWinner[Eliminator]
    
    DeclareWinner(Winner : agent) : void =
        set GameActive = false
        Print("Winner: {Winner} with {PlayerKills[Winner]} kills!")
    
    TotalAlivePlayers() : int =
        var Count : int = 0
        for (Agent : AlivePlayers, Alive : AlivePlayers[Agent]):
            if Alive: set Count += 1
        Count
    
    OnVoidFall(Agent : agent) : void =
        if GameActive:
            OnPlayerEliminated(Agent, agent{})
