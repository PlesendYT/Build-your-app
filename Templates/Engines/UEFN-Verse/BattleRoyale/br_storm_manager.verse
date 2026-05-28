using { /Fortnite.com/Devices }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }

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
    
    CurrentRadius : float = InitialZoneRadius
    CurrentPhase : int = 0
    StormActive : logic = false
    
    OnBegin<override>()<suspends> := :
        if:
            FortCharacter := GetPlayspace().GetFortCharacter[]
        then:
            StormActive = true
            SpawnStorm[]
    
    SpawnStorm()<suspends> := :
        while StormActive:
            CurrentPhase += 1
            Sleep(ShrinkInterval)
            if CurrentRadius > FinalZoneRadius:
                CurrentRadius = CurrentRadius - ShrinkAmount
                SafeZoneDevice.SetZoneRadius(CurrentRadius)
                DamageZoneDevice.SetZoneRadius(CurrentRadius + 200.0)
                Print("Storm shrinks to {CurrentRadius}")
    
    ApplyStormDamage(Agent : agent) : void =
        if:
            FortChar := agent.GetFortCharacter[]
            InStorm := not DamageZoneDevice.IsInside(FortChar.GetTransform().Translation)
        then:
            if InStorm:
                FortChar.ApplyDamage(StormDamagePerSecond * 0.1)
    
    OnPlayerJoined(Agent : agent) : void =
        Print("Player joined Battle Royale!")
