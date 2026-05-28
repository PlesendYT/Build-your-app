using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /UnrealEngine.com/Temporary/Diagnostics }

gungame_mode := class(creative_device):
    @editable
    WeaponTiers : []item_spawner_device = array{}
    @editable
    FinalTierWeapon : item_spawner_device = item_spawner_device{}
    
    var PlayerTiers : [agent]int = map{}
    var MaxTier : int = 0
    
    OnBegin<override>()<suspends>() : void =
        set MaxTier = WeaponTiers.Length
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    OnPlayerJoined(Agent : agent) : void =
        set PlayerTiers[Agent] = 0
        GiveWeapon(Agent, 0)
    
    OnPlayerEliminated(Eliminated : agent, Eliminator : agent) : void =
        if (Eliminated <> Eliminator and PlayerTiers[Eliminator] < MaxTier):
            AdvanceTier(Eliminator)
    
    AdvanceTier(Agent : agent) : void =
        if (CurrentTier := PlayerTiers[Agent]):
            set PlayerTiers[Agent] = CurrentTier + 1
            NewTier := CurrentTier + 1
            Print("{Agent} advanced to tier {NewTier}!")
            
            if (NewTier >= MaxTier):
                GiveFinalWeapon(Agent)
                Print("{Agent} wins GunGame!")
            else:
                GiveWeapon(Agent, NewTier)
    
    GiveWeapon(Agent : agent, TierIndex : int) : void =
        if (TierIndex < WeaponTiers.Length):
            WeaponTiers[TierIndex].CopyProperties(Agent.GetFortCharacter())
    
    GiveFinalWeapon(Agent : agent) : void =
        FinalTierWeapon.CopyProperties(Agent.GetFortCharacter())
