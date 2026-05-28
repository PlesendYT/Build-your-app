using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

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
    
    OnBegin<override>()<suspends>() : void =
        GameActive = true
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartWaveLoop[] }
    
    OnPlayerJoined(Agent : agent) : void =
        set PlayerPoints[Agent] = 0
    
    StartWaveLoop()<suspends>() : void =
        while GameActive and CurrentWave < MaxWaves:
            set CurrentWave += 1
            Print("Wave {CurrentWave} starting!")
            BroadcastWaveMessage[CurrentWave]
            SpawnZombieWave[CurrentWave]
            Sleep(WaveDelay)
        
        if (CurrentWave >= MaxWaves):
            Print("All waves complete! Victory!")
    
    SpawnZombieWave(WaveNumber : int)<suspends>() : void =
        ZombieCount := WaveNumber * 2 + 5
        if (WaveNumber % 5 = 0):
            set ZombieCount += 10
        
        for (I : int = 0..ZombieCount):
            if (Random.SpawnChance(0.8)):
                SpawnZombie[]
            Sleep(0.5)
    
    SpawnZombie() : void =
        if (Spawner := GetRandomSpawner[]):
            Spawner.SpawnCreature[]
            set ZombiesAlive += 1
    
    GetRandomSpawner() : creature_spawner_device =
        Index := Random.UpperBound(ZombieSpawner.Length)
        ZombieSpawner[Index]
    
    OnZombieEliminated(Eliminator : agent) : void =
        set ZombiesAlive -= 1
        if (PlayerPoints[Eliminator] <> 0):
            set PlayerPoints[Eliminator] += 10
        else:
            set PlayerPoints[Eliminator] = 10
    
    BroadcastWaveMessage(WaveNumber : int) : void =
        Print("Wave {WaveNumber} of {MaxWaves}")
