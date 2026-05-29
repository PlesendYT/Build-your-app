# Tower Defense mode: players build and upgrade towers to defend against enemy waves.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Tower Defense device class handling waves, gold, and tower management.
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
    
    # Subscribes to player join events and starts the game loop.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartGame[] }
    
    # Grants starting gold to a new player.
    OnPlayerJoined(Agent : agent) : void =
        set PlayerGold[Agent] = 200
    
    # Runs through each wave, spawning enemies and waiting for them to be cleared.
    StartGame()<suspends>() : void =
        set GameActive = true
        for (Wave : int = 1..MaxWaves):
            set CurrentWave = Wave
            Print("Wave {Wave} / {MaxWaves}")
            SpawnEnemyWave[Wave]
            Sleep(WaitForWaveClear[])
        
        if GameActive:
            Print("Victory! All waves survived!")
    
    # Spawns enemies for a given wave number.
    SpawnEnemyWave(WaveNumber : int) : void =
        EnemyCount := WaveNumber * 2
        for (I : int = 0..EnemyCount):
            EnemySpawner.SpawnCreature[]
            set EnemiesAlive += 1
    
    # Waits until all enemies in the current wave are dead, then returns a delay.
    WaitForWaveClear()<suspends>() : float =
        loop:
            Sleep(1.0)
            if EnemiesAlive <= 0:
                return 5.0
    
    # Builds a tower at a given slot if the player has enough gold.
    BuildTower(Agent : agent, SlotIndex : int, TowerType : int) : void =
        Cost := 100 + TowerType * 50
        if (Gold := PlayerGold[Agent]):
            if Gold >= Cost:
                set PlayerGold[Agent] = Gold - Cost
                set TowerLevels[SlotIndex] = 1
                Print("Tower built at slot {SlotIndex}!")
    
    # Upgrades an existing tower if the player has enough gold and it is below max level.
    UpgradeTower(Agent : agent, SlotIndex : int) : void =
        if (Level := TowerLevels[SlotIndex]):
            UpgradeCost := Level * 150
            if (Gold := PlayerGold[Agent]):
                if Gold >= UpgradeCost and Level < 3:
                    set PlayerGold[Agent] = Gold - UpgradeCost
                    set TowerLevels[SlotIndex] = Level + 1
                    Print("Tower upgraded to level {Level + 1}!")
    
    # Decrements alive enemies and rewards the killer with gold.
    OnEnemyKilled(Killer : agent, EnemyValue : int) : void =
        set EnemiesAlive -= 1
        if (Gold := PlayerGold[Killer]):
            set PlayerGold[Killer] = Gold + EnemyValue
