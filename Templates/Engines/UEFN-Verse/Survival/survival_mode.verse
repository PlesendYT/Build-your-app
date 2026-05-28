using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

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
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { DayNightCycle[] }
    
    OnPlayerJoined(Agent : agent) : void =
        set PlayerResources[Agent] = 0
        set PlayerHealth[Agent] = 100.0
        set PlayerHunger[Agent] = 100.0
    
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
    
    SpawnMonsters()<suspends>() : void =
        loop:
            if IsNightTime:
                for (Spawner : MonsterSpawner):
                    Spawner.SpawnCreature[]
                Sleep(10.0)
            else:
                break
    
    GatherResource(Agent : agent, ResourceType : int, Amount : int) : void =
        if (Resources := PlayerResources[Agent]):
            set PlayerResources[Agent] = Resources + Amount
    
    BuildStructure(Agent : agent, StructureType : int, PlotIndex : int) : void =
        Cost := 50
        if (Resources := PlayerResources[Agent]):
            if Resources >= Cost:
                set PlayerResources[Agent] = Resources - Cost
                Print("{Agent} built a structure on plot {PlotIndex}!")
    
    EatFood(Agent : agent, FoodValue : float) : void =
        if (Hunger := PlayerHunger[Agent]):
            set PlayerHunger[Agent] = Hunger + FoodValue
    
    TakeSurvivalDamage(Agent : agent, Damage : float) : void =
        if (Health := PlayerHealth[Agent]):
            set PlayerHealth[Agent] = Health - Damage
            if PlayerHealth[Agent] <= 0.0:
                Print("{Agent} died!")
