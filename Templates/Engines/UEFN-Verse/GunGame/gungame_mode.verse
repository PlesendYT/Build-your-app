# GunGame mode: players advance through weapon tiers by eliminating opponents; first to max tier wins.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main GunGame device class managing weapon tiers and player progression.
gungame_mode := class(creative_device):
    @editable
    WeaponTiers : []item_spawner_device = array{}
    @editable
    FinalTierWeapon : item_spawner_device = item_spawner_device{}
    
    var PlayerTiers : [agent]int = map{}
    var MaxTier : int = 0
    
    # Sets the max tier count and subscribes to player join events.
    OnBegin<override>()<suspends>() : void =
        set MaxTier = WeaponTiers.Length
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    # Gives a new player the first tier weapon.
    OnPlayerJoined(Agent : agent) : void =
        set PlayerTiers[Agent] = 0
        GiveWeapon(Agent, 0)
    
    # Advances the eliminator's tier when they eliminate another player.
    OnPlayerEliminated(Eliminated : agent, Eliminator : agent) : void =
        if (Eliminated <> Eliminator and PlayerTiers[Eliminator] < MaxTier):
            AdvanceTier(Eliminator)
    
    # Moves the player to the next tier, awarding the corresponding weapon or the final weapon.
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
    
    # Copies properties from the weapon tier device to the player's character.
    GiveWeapon(Agent : agent, TierIndex : int) : void =
        if (TierIndex < WeaponTiers.Length):
            WeaponTiers[TierIndex].CopyProperties(Agent.GetFortCharacter())
    
    # Gives the player the final tier weapon when they reach max tier.
    GiveFinalWeapon(Agent : agent) : void =
        FinalTierWeapon.CopyProperties(Agent.GetFortCharacter())
