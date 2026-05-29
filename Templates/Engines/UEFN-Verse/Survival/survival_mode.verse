# Survival mode: players gather resources, build structures, and survive day/night cycles with monsters.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Survival device class managing resources, health, hunger, and day/night cycling.
survival_mode := class(creative_device):
    @editable
    DayLength : float = 300.0
    @editable
    NightLength : float = 120.0
    @editable
    ResourceNodes : []item_spawner_device = array{}
    @editable
    BuildingPlots : []zone_device = array{}
    @editable
    MonsterSpawner : []creature_spawner_device = array{}
    
    var PlayerResources : [agent]int = map{}
    var PlayerHealth : [agent]float = map{}
    var PlayerHunger : [agent]float = map{}
    var IsNightTime : logic = false
    var DayCount : int = 1
    
    # Subscribes player join events and starts the day/night cycle.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { DayNightCycle[] }
    
    # Initializes a new player's resources, health, and hunger.
    OnPlayerJoined(Agent : agent) : void =
        set PlayerResources[Agent] = 0
        set PlayerHealth[Agent] = 100.0
        set PlayerHunger[Agent] = 100.0
    
    # Cycles between day and night, spawning monsters during the night.
    DayNightCycle()<suspends>() : void =
        loop:
            Print("Day {DayCount} started!")
            set IsNightTime = false
            Sleep(DayLength)
            
            set IsNightTime = true
            Print("Night {DayCount} started! Monsters incoming!")
            spawn { SpawnMonsters[] }
            Sleep(NightLength)
            
            set DayCount += 1
    
    # Spawns monsters periodically while it is night time.
    SpawnMonsters()<suspends>() : void =
        loop:
            if IsNightTime:
                for (Spawner : MonsterSpawner):
                    Spawner.SpawnCreature[]
                Sleep(10.0)
            else:
                break
    
    # Adds resources to a player after gathering.
    GatherResource(Agent : agent, ResourceType : int, Amount : int) : void =
        if (Resources := PlayerResources[Agent]):
            set PlayerResources[Agent] = Resources + Amount
    
    # Spends resources to build a structure on the given plot.
    BuildStructure(Agent : agent, StructureType : int, PlotIndex : int) : void =
        Cost := 50
        if (Resources := PlayerResources[Agent]):
            if Resources >= Cost:
                set PlayerResources[Agent] = Resources - Cost
                Print("{Agent} built a structure on plot {PlotIndex}!")
    
    # Increases a player's hunger meter by the given food value.
    EatFood(Agent : agent, FoodValue : float) : void =
        if (Hunger := PlayerHunger[Agent]):
            set PlayerHunger[Agent] = Hunger + FoodValue
    
    # Applies damage to a player and prints a death message if health reaches zero.
    TakeSurvivalDamage(Agent : agent, Damage : float) : void =
        if (Health := PlayerHealth[Agent]):
            set PlayerHealth[Agent] = Health - Damage
            if PlayerHealth[Agent] <= 0.0:
                Print("{Agent} died!")
