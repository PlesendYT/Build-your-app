using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

hide_and_seek := class(creative_device):
    @editable
    SeekTime : float = 90.0
    @editable
    HiderTeam : team_device = team_device{}
    @editable
    SeekerTeam : team_device = team_device{}
    @editable
    HidingSpotDevices : []zone_device = array{}
    
    var RoundActive : logic = false
    var HidersFound : [agent]logic = map{}
    var TotalHiders : int = 0
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartRound[] }
    
    OnPlayerJoined(Agent : agent) : void =
        AssignRoles[Agent]
    
    AssignRoles(Agent : agent) : void =
        HiderTeam.AddToTeam(Agent)
        set HidersFound[Agent] = false
        set TotalHiders += 1
    
    StartRound()<suspends>() : void =
        Sleep(15.0)
        SelectSeeker[]
        set RoundActive = true
        Print("Seeker, start finding!")
        Sleep(SeekTime)
        EndRound[]
    
    SelectSeeker() : void =
        Players := GetPlayspace().GetPlayers()
        if (Seeker := GetRandomPlayer[Players]):
            HiderTeam.RemoveFromTeam(Seeker)
            SeekerTeam.AddToTeam(Seeker)
            Print("{Seeker} is the seeker!")
    
    GetRandomPlayer(Players : []agent) : agent =
        Index := Random.UpperBound(Players.Length - 1)
        Players[Index]
    
    OnHiderFound(FoundAgent : agent, SeekerAgent : agent) : void =
        if (RoundActive and HiderTeam.IsMember(FoundAgent)):
            set HidersFound[FoundAgent] = true
            HiderTeam.RemoveFromTeam(FoundAgent)
            SeekerTeam.AddToTeam(FoundAgent)
            Print("{FoundAgent} was found by {SeekerAgent}!")
            
            if (AllHidersFound[]):
                EndRound[]
    
    AllHidersFound() : logic =
        for (Hider : HidersFound, Found : HidersFound[Hider]):
            if not Found:
                return false
        return true
    
    EndRound() : void =
        set RoundActive = false
        FoundCount := CountFound[]
        if FoundCount >= TotalHiders:
            Print("Seeker wins! All hiders found!")
        else:
            Print("Hiders win! {TotalHiders - FoundCount} survived!")
    
    CountFound() : int =
        var Count : int = 0
        for (Hider : HidersFound, Found : HidersFound[Hider]):
            if Found:
                set Count += 1
        Count
