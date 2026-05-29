using { /Fortnite.com/Devices }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }

# Battle Royale storm manager - handles storm shrinking and damage over time
br_storm_manager := class(creative_device):
    @editable
    StormDamagePerSecond : float = 5.0
    @editable
    ShrinkInterval : float = 60.0
    @editable
    ShrinkAmount : float = 1000.0
    @editable
    InitialZoneRadius : float = 5000.0
    @editable
    FinalZoneRadius : float = 500.0
    @editable
    SafeZoneDevice : zone_device = zone_device{}
    @editable
    DamageZoneDevice : zone_device = zone_device{}
    
    CurrentRadius : float = InitialZoneRadius # Current storm radius
    CurrentPhase : int = 0 # Which phase of shrinking the storm is in
    StormActive : logic = false # Whether the storm is active
    
    # Initializes storm activity and starts the shrinking loop
    OnBegin<override>()<suspends> := :
        if:
            FortCharacter := GetPlayspace().GetFortCharacter[]
        then:
            StormActive = true
            SpawnStorm[]
    
    # Periodic storm shrinking loop - reduces safe zone radius each interval
    SpawnStorm()<suspends> := :
        while StormActive:
            CurrentPhase += 1
            Sleep(ShrinkInterval)
            if CurrentRadius > FinalZoneRadius:
                CurrentRadius = CurrentRadius - ShrinkAmount
                SafeZoneDevice.SetZoneRadius(CurrentRadius)
                DamageZoneDevice.SetZoneRadius(CurrentRadius + 200.0)
                Print("Storm shrinks to {CurrentRadius}")
    
    # Applies damage over time to a player if they are outside the safe zone
    ApplyStormDamage(Agent : agent) : void =
        if:
            FortChar := agent.GetFortCharacter[]
            InStorm := not DamageZoneDevice.IsInside(FortChar.GetTransform().Translation)
        then:
            if InStorm:
                FortChar.ApplyDamage(StormDamagePerSecond * 0.1)
    
    # Welcomes a new player to the Battle Royale match
    OnPlayerJoined(Agent : agent) : void =
        Print("Player joined Battle Royale!")
