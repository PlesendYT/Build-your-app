using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

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
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartRound[] }
    
    OnPlayerJoined(Agent : agent) : void =
        AssignTeam[Agent]
    
    AssignTeam(Agent : agent) : void =
        if (CurrentRound % 2 = 0):
            HunterTeam.AddToTeam(Agent)
        else:
            PropTeam.AddToTeam(Agent)
            set AliveHiders[Agent] = true
            TransformToProp[Agent]
    
    TransformToProp(Agent : agent) : void =
        if (Prop := GetRandomProp[]):
            if (FortChar := Agent.GetFortCharacter[]):
                Prop.CopyProperties(FortChar)
                FortChar.SetScale(Random.Range(0.5, 1.5))
        Print("{Agent} transformed into a prop!")
    
    GetRandomProp() : item_spawner_device =
        Index := Random.UpperBound(PropDevices.Length)
        PropDevices[Index]
    
    StartRound()<suspends>() : void =
        Print("Round {CurrentRound} started!")
        set RoundActive = true
        Sleep(RoundTime)
        EndRound[]
    
    EndRound() : void =
        set RoundActive = false
        HidersAlive := CountAliveHiders[]
        if (HidersAlive > 0):
            Print("Hiders win! {HidersAlive} survived!")
        else:
            Print("Hunters win! All hiders found!")
        set CurrentRound += 1
    
    CountAliveHiders() : int =
        var Count : int = 0
        for (Hider : AliveHiders, IsAlive : AliveHiders[Hider]):
            if IsAlive:
                set Count += 1
        Count
    
    OnHiderFound(Hider : agent) : void =
        set AliveHiders[Hider] = false
        HunterTeam.AddToTeam(Hider)
