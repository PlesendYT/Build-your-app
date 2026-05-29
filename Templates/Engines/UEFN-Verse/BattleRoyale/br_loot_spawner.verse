using { /Fortnite.com/Devices }
using { /Verse.org/Simulation }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Battle Royale loot spawner - manages loot spawning and respawning
br_loot_spawner := class(creative_device):
    @editable
    LootTable : []item_spawner_device = array{}
    @editable
    SpawnInterval : float = 30.0
    @editable
    MaxLootPerPlayer : int = 6
    
    var SpawnedItems : [item_spawner_device] = array{} # Track which items have been spawned
    
    # Initializes the spawner, spawns initial loot and starts the respawn loop
    OnBegin<override>()<suspends>() : void =
        SpawnInitialLoot[]
        loop:
            Sleep(SpawnInterval)
            RespawnLoot[]
    
    # Spawns items from all loot table entries at the start of the game
    SpawnInitialLoot() : void =
        for (Spawner : LootTable):
            if (Spawner <> item_spawner_device{}):
                Spawner.SpawnItem[]
                set SpawnedItems += array{Spawner}
    
    # Respawns items at empty spawner locations
    RespawnLoot() : void =
        for (Spawner : SpawnedItems):
            if (not Spawner.HasItem[]):
                Spawner.SpawnItem[]
    
    # Spawns a random weapon from the loot table at a given spawner
    SpawnRandomWeapon(Spawner : item_spawner_device) : void =
        RandomItem := GetRandomEntry(LootTable)
        if (RandomItem <> item_spawner_device{}):
            RandomItem.CopyProperties(Spawner)
            Spawner.SpawnItem[]
    
    # Returns a random entry from a list of item spawner devices
    GetRandomEntry(List : []item_spawner_device) : item_spawner_device =
        Index := Random.UpperBound(List.Length)
        List[Index]
