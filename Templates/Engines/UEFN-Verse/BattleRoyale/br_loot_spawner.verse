using { /Fortnite.com/Devices }
using { /Verse.org/Simulation }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

br_loot_spawner := class(creative_device):
    @editable
    LootTable : []item_spawner_device = array{}
    @editable
    SpawnInterval : float = 30.0
    @editable
    MaxLootPerPlayer : int = 6
    
    var SpawnedItems : [item_spawner_device] = array{}
    
    OnBegin<override>()<suspends>() : void =
        SpawnInitialLoot[]
        loop:
            Sleep(SpawnInterval)
            RespawnLoot[]
    
    SpawnInitialLoot() : void =
        for (Spawner : LootTable):
            if (Spawner <> item_spawner_device{}):
                Spawner.SpawnItem[]
                set SpawnedItems += array{Spawner}
    
    RespawnLoot() : void =
        for (Spawner : SpawnedItems):
            if (not Spawner.HasItem[]):
                Spawner.SpawnItem[]
    
    SpawnRandomWeapon(Spawner : item_spawner_device) : void =
        RandomItem := GetRandomEntry(LootTable)
        if (RandomItem <> item_spawner_device{}):
            RandomItem.CopyProperties(Spawner)
            Spawner.SpawnItem[]
    
    GetRandomEntry(List : []item_spawner_device) : item_spawner_device =
        Index := Random.UpperBound(List.Length)
        List[Index]
