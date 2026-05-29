# Hide and Seek game mode: hiders hide while a seeker tries to find them before time runs out.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Hide and Seek device class managing rounds, roles, and hiding spots.
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
    
    # Subscribes to player join events and starts the first round.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartRound[] }
    
    # Assigns the joining player to the hider team.
    OnPlayerJoined(Agent : agent) : void =
        AssignRoles[Agent]
    
    # Adds a player to the hider team and tracks them.
    AssignRoles(Agent : agent) : void =
        HiderTeam.AddToTeam(Agent)
        set HidersFound[Agent] = false
        set TotalHiders += 1
    
    # Delays then selects a seeker, starts the round, and ends after seek time.
    StartRound()<suspends>() : void =
        Sleep(15.0)
        SelectSeeker[]
        set RoundActive = true
        Print("Seeker, start finding!")
        Sleep(SeekTime)
        EndRound[]
    
    # Randomly selects a seeker from the player list.
    SelectSeeker() : void =
        Players := GetPlayspace().GetPlayers()
        if (Seeker := GetRandomPlayer[Players]):
            HiderTeam.RemoveFromTeam(Seeker)
            SeekerTeam.AddToTeam(Seeker)
            Print("{Seeker} is the seeker!")
    
    # Returns a random player from the given array.
    GetRandomPlayer(Players : []agent) : agent =
        Index := Random.UpperBound(Players.Length - 1)
        Players[Index]
    
    # Marks a hider as found and switches them to the seeker team; ends round if all found.
    OnHiderFound(FoundAgent : agent, SeekerAgent : agent) : void =
        if (RoundActive and HiderTeam.IsMember(FoundAgent)):
            set HidersFound[FoundAgent] = true
            HiderTeam.RemoveFromTeam(FoundAgent)
            SeekerTeam.AddToTeam(FoundAgent)
            Print("{FoundAgent} was found by {SeekerAgent}!")
            
            if (AllHidersFound[]):
                EndRound[]
    
    # Checks whether every hider has been found.
    AllHidersFound() : logic =
        for (Hider : HidersFound, Found : HidersFound[Hider]):
            if not Found:
                return false
        return true
    
    # Ends the round and announces whether the seeker or hiders win.
    EndRound() : void =
        set RoundActive = false
        FoundCount := CountFound[]
        if FoundCount >= TotalHiders:
            Print("Seeker wins! All hiders found!")
        else:
            Print("Hiders win! {TotalHiders - FoundCount} survived!")
    
    # Returns the number of hiders that have been found.
    CountFound() : int =
        var Count : int = 0
        for (Hider : HidersFound, Found : HidersFound[Hider]):
            if Found:
                set Count += 1
        Count
