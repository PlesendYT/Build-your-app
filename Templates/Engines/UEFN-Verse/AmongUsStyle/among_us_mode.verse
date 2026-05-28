using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

among_us_mode := class(creative_device):
    @editable
    TaskDevices : []item_spawner_device = array{}
    @editable
    MeetingRoom : zone_device = zone_device{}
    @editable
    VotingTime : float = 30.0
    
    var PlayerTasks : [agent]int = map{}
    var TotalTasks : int = 0
    var Impostors : [agent]logic = map{}
    var IsMeeting : logic = false
    var AlivePlayers : [agent]logic = map{}
    var Votes : [agent]int = map{}
    
    OnBegin<override>()<suspends>() : void =
        set TotalTasks = TaskDevices.Length
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartGame[] }
    
    OnPlayerJoined(Agent : agent) : void =
        set AlivePlayers[Agent] = true
        set PlayerTasks[Agent] = 0
    
    StartGame()<suspends>() : void =
        Sleep(10.0)
        AssignImpostors[]
        Print("Game started! Complete tasks or find the impostor!")
    
    AssignImpostors() : void =
        Players := GetPlayspace().GetPlayers()
        if (Players.Length >= 3):
            ImpostorIndex := Random.UpperBound(Players.Length - 1)
            set Impostors[Players[ImpostorIndex]] = true
            Print("Impostor has been chosen...")
    
    OnTaskCompleted(Agent : agent) : void =
        if (Tasks := PlayerTasks[Agent]):
            set PlayerTasks[Agent] = Tasks + 1
            Print("{Agent} completed task {Tasks + 1}/{TotalTasks}")
            
            if (PlayerTasks[Agent] >= TotalTasks):
                CrewmateWin[]
    
    CrewmateWin() : void =
        Print("Crewmates win! All tasks completed!")
        set IsMeeting = true
    
    CallMeeting(Caller : agent) : void =
        set IsMeeting = true
        Print("{Caller} called an emergency meeting!")
        spawn { VotingPhase[] }
    
    VotingPhase()<suspends>() : void =
        Sleep(VotingTime)
        ResolveVotes[]
    
    ResolveVotes() : void =
        for (Voter : Votes, Target : Votes[Voter]):
            if (Votes[Target] <> 0):
                set Votes[Target] = Votes[Target] + 1
            else:
                set Votes[Target] = 1
        
        MostVoted := GetMostVoted[]
        if (Impostors[MostVoted] = true):
            Print("{MostVoted} was the impostor! Crewmates win!")
        else:
            Print("{MostVoted} was innocent! Impostor wins!")
        
        set IsMeeting = false
    
    Vote(Agent : agent, Target : agent) : void =
        if IsMeeting:
            set Votes[Agent] = Target
    
    GetMostVoted() : agent =
        var MaxVotes : int = 0
        var MostVoted : agent = agent{}
        for (Player : Votes, VoteCount : Votes[Player]):
            if VoteCount > MaxVotes:
                set MaxVotes = VoteCount
                set MostVoted = Player
        MostVoted
    
    Sabotage(Impostor : agent, SabotageType : int) : void =
        if (Impostors[Impostor] = true and not IsMeeting):
            Print("Sabotage actived!")
            
    OnPlayerEliminated(Agent : agent) : void =
        set AlivePlayers[Agent] = false
        AliveCount := CountAlive[]
        if AliveCount <= 2 and HasAliveImpostor[]:
            Print("Impostor wins!")
    
    CountAlive() : int =
        var Count : int = 0
        for (Agent : AlivePlayers, Alive : AlivePlayers[Agent]):
            if Alive: set Count += 1
        Count
    
    HasAliveImpostor() : logic =
        for (Agent : Impostors, IsImp : Impostors[Agent]):
            if IsImp and AlivePlayers[Agent]:
                return true
        return false
