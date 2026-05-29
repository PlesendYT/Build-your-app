# Prop Hunt mode: props disguise as objects while hunters try to find and eliminate them within a time limit.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Prop Hunt device class managing rounds, teams, and prop transformations.
prop_hunt := class(creative_device):
    @editable
    RoundTime : float = 120.0
    @editable
    PropTeam : team_device = team_device{}
    @editable
    HunterTeam : team_device = team_device{}
    @editable
    PropDevices : []item_spawner_device = array{}
    @editable
    HunterSpawn : respawn_device = respawn_device{}
    
    var CurrentRound : int = 1
    var RoundActive : logic = false
    var AliveHiders : [agent]logic = map{}
    
    # Subscribes to player join events and starts the first round.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartRound[] }
    
    # Assigns a joining player to a team based on the current round number.
    OnPlayerJoined(Agent : agent) : void =
        AssignTeam[Agent]
    
    # Alternates team assignment each round; props are transformed on odd rounds.
    AssignTeam(Agent : agent) : void =
        if (CurrentRound % 2 = 0):
            HunterTeam.AddToTeam(Agent)
        else:
            PropTeam.AddToTeam(Agent)
            set AliveHiders[Agent] = true
            TransformToProp[Agent]
    
    # Randomly selects a prop device and applies its appearance to the player.
    TransformToProp(Agent : agent) : void =
        if (Prop := GetRandomProp[]):
            if (FortChar := Agent.GetFortCharacter[]):
                Prop.CopyProperties(FortChar)
                FortChar.SetScale(Random.Range(0.5, 1.5))
        Print("{Agent} transformed into a prop!")
    
    # Returns a random prop device from the available array.
    GetRandomProp() : item_spawner_device =
        Index := Random.UpperBound(PropDevices.Length)
        PropDevices[Index]
    
    # Starts a round and ends it after the configured time.
    StartRound()<suspends>() : void =
        Print("Round {CurrentRound} started!")
        set RoundActive = true
        Sleep(RoundTime)
        EndRound[]
    
    # Ends the round and announces whether hiders or hunters won.
    EndRound() : void =
        set RoundActive = false
        HidersAlive := CountAliveHiders[]
        if (HidersAlive > 0):
            Print("Hiders win! {HidersAlive} survived!")
        else:
            Print("Hunters win! All hiders found!")
        set CurrentRound += 1
    
    # Counts how many hiders are still alive.
    CountAliveHiders() : int =
        var Count : int = 0
        for (Hider : AliveHiders, IsAlive : AliveHiders[Hider]):
            if IsAlive:
                set Count += 1
        Count
    
    # Marks a hider as found and moves them to the hunter team.
    OnHiderFound(Hider : agent) : void =
        set AliveHiders[Hider] = false
        HunterTeam.AddToTeam(Hider)
