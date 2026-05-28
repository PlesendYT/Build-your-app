using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

tower_defense := class(creative_device):
    @editable
    Waypoints : []zone_device = array{}
    @editable
    TowerSlots : []zone_device = array{}
    @editable
    EnemySpawner : creature_spawner_device = creature_spawner_device{}
    @editable
    MaxWaves : int = 15
    
    var CurrentWave : int = 0
    var PlayerGold : [agent]int = map{}
    var TowerLevels : [int]int = map{}
    var EnemiesAlive : int = 0
    var GameActive : logic = false
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartGame[] }
    
    OnPlayerJoined(Agent : agent) : void =
        set PlayerGold[Agent] = 200
    
    StartGame()<suspends>() : void =
        set GameActive = true
        for (Wave : int = 1..MaxWaves):
            set CurrentWave = Wave
            Print("Wave {Wave} / {MaxWaves}")
            SpawnEnemyWave[Wave]
            Sleep(WaitForWaveClear[])
        
        if GameActive:
            Print("Victory! All waves survived!")
    
    SpawnEnemyWave(WaveNumber : int) : void =
        EnemyCount := WaveNumber * 2
        for (I : int = 0..EnemyCount):
            EnemySpawner.SpawnCreature[]
            set EnemiesAlive += 1
    
    WaitForWaveClear()<suspends>() : float =
        loop:
            Sleep(1.0)
            if EnemiesAlive <= 0:
                return 5.0
    
    BuildTower(Agent : agent, SlotIndex : int, TowerType : int) : void =
        Cost := 100 + TowerType * 50
        if (Gold := PlayerGold[Agent]):
            if Gold >= Cost:
                set PlayerGold[Agent] = Gold - Cost
                set TowerLevels[SlotIndex] = 1
                Print("Tower built at slot {SlotIndex}!")
    
    UpgradeTower(Agent : agent, SlotIndex : int) : void =
        if (Level := TowerLevels[SlotIndex]):
            UpgradeCost := Level * 150
            if (Gold := PlayerGold[Agent]):
                if Gold >= UpgradeCost and Level < 3:
                    set PlayerGold[Agent] = Gold - UpgradeCost
                    set TowerLevels[SlotIndex] = Level + 1
                    Print("Tower upgraded to level {Level + 1}!")
    
    OnEnemyKilled(Killer : agent, EnemyValue : int) : void =
        set EnemiesAlive -= 1
        if (Gold := PlayerGold[Killer]):
            set PlayerGold[Killer] = Gold + EnemyValue
