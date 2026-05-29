# Zombie Survival wave mode: players survive increasingly difficult waves of zombies, earning points per kill.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Zombie Survival device class managing waves, spawners, and player points.
zombie_survival := class(creative_device):
    @editable
    ZombieSpawner : []creature_spawner_device = array{}
    @editable
    WaveDelay : float = 30.0
    @editable
    MaxWaves : int = 20
    @editable
    UpgradeStationDevice : item_spawner_device = item_spawner_device{}
    @editable
    HealthPickupDevice : item_spawner_device = item_spawner_device{}
    
    var CurrentWave : int = 0
    var ZombiesAlive : int = 0
    var PlayerPoints : [agent]int = map{}
    var GameActive : logic = false
    
    # Activates the game, subscribes to player join events, and starts the wave loop.
    OnBegin<override>()<suspends>() : void =
        GameActive = true
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartWaveLoop[] }
    
    # Initializes a new player's points to zero.
    OnPlayerJoined(Agent : agent) : void =
        set PlayerPoints[Agent] = 0
    
    # Loops through each wave, spawning zombies and waiting between waves.
    StartWaveLoop()<suspends>() : void =
        while GameActive and CurrentWave < MaxWaves:
            set CurrentWave += 1
            Print("Wave {CurrentWave} starting!")
            BroadcastWaveMessage[CurrentWave]
            SpawnZombieWave[CurrentWave]
            Sleep(WaveDelay)
        
        if (CurrentWave >= MaxWaves):
            Print("All waves complete! Victory!")
    
    # Spawns zombies for a given wave, with extra on every 5th wave.
    SpawnZombieWave(WaveNumber : int)<suspends>() : void =
        ZombieCount := WaveNumber * 2 + 5
        if (WaveNumber % 5 = 0):
            set ZombieCount += 10
        
        for (I : int = 0..ZombieCount):
            if (Random.SpawnChance(0.8)):
                SpawnZombie[]
            Sleep(0.5)
    
    # Spawns a single zombie from a random spawner.
    SpawnZombie() : void =
        if (Spawner := GetRandomSpawner[]):
            Spawner.SpawnCreature[]
            set ZombiesAlive += 1
    
    # Returns a random zombie spawner device.
    GetRandomSpawner() : creature_spawner_device =
        Index := Random.UpperBound(ZombieSpawner.Length)
        ZombieSpawner[Index]
    
    # Awards points to the player who eliminated a zombie and decrements alive count.
    OnZombieEliminated(Eliminator : agent) : void =
        set ZombiesAlive -= 1
        if (PlayerPoints[Eliminator] <> 0):
            set PlayerPoints[Eliminator] += 10
        else:
            set PlayerPoints[Eliminator] = 10
    
    # Prints a message about the current wave number.
    BroadcastWaveMessage(WaveNumber : int) : void =
        Print("Wave {WaveNumber} of {MaxWaves}")
