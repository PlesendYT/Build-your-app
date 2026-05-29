# SkyWars game mode: players spawn on islands, loot chests, and fight until one remains.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main SkyWars device class managing spawns, kills, and game flow.
skywars_mode := class(creative_device):
    @editable
    IslandSpawnPoints : []respawn_device = array{}
    @editable
    ChestDevices : []item_spawner_device = array{}
    @editable
    CenterChest : item_spawner_device = item_spawner_device{}
    @editable
    VoidZone : zone_device = zone_device{}
    
    # Tracks alive state and kill count per player.
    var AlivePlayers : [agent]logic = map{}
    # Player kill count tracking.
    var PlayerKills : [agent]int = map{}
    var GameActive : logic = false
    
    # Called when the device starts; subscribes to player join events.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    # Initializes player state and starts the game if enough players are present.
    OnPlayerJoined(Agent : agent) : void =
        set AlivePlayers[Agent] = true
        set PlayerKills[Agent] = 0
        SpawnOnIsland[Agent]
        
        if (TotalAlivePlayers[] >= 2 and not GameActive):
            StartGame[]
    
    # Activates the game and begins the center chest timer.
    StartGame() : void =
        set GameActive = true
        Print("SkyWars started! Last player standing wins!")
        spawn { CenterChestTimer[] }
    
    # Waits 60 seconds then spawns a chest at center.
    CenterChestTimer()<suspends>() : void =
        Sleep(60.0)
        CenterChest.SpawnItem[]
        Print("Center chest spawned!")
    
    # Teleports a player to a random island and gives starter gear.
    SpawnOnIsland(Agent : agent) : void =
        IslandIndex := Random.UpperBound(IslandSpawnPoints.Length - 1)
        IslandSpawnPoints[IslandIndex].Spawn(Agent)
        GiveStarterGear[Agent]
    
    # Gives the player a sword and blocks.
    GiveStarterGear(Agent : agent) : void =
        if (FortChar := Agent.GetFortCharacter[]):
            FortChar.AddItem("Sword")
            FortChar.AddItem("Blocks", 64)
    
    # Marks a player as eliminated and checks if the game should end.
    OnPlayerEliminated(Eliminated : agent, Eliminator : agent) : void =
        set AlivePlayers[Eliminated] = false
        if (Kills := PlayerKills[Eliminator]):
            set PlayerKills[Eliminator] = Kills + 1
        else:
            set PlayerKills[Eliminator] = 1
        
        if (TotalAlivePlayers[] <= 1):
            DeclareWinner[Eliminator]
    
    # Ends the game and announces the winner with their kill count.
    DeclareWinner(Winner : agent) : void =
        set GameActive = false
        Print("Winner: {Winner} with {PlayerKills[Winner]} kills!")
    
    # Counts how many players are still alive.
    TotalAlivePlayers() : int =
        var Count : int = 0
        for (Agent : AlivePlayers, Alive : AlivePlayers[Agent]):
            if Alive: set Count += 1
        Count
    
    # Handles falling into the void during an active game.
    OnVoidFall(Agent : agent) : void =
        if GameActive:
            OnPlayerEliminated(Agent, agent{})
