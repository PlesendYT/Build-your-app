using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Among Us style social deduction game mode
# Players complete tasks while identifying the impostor among them
among_us_mode := class(creative_device):
    @editable
    TaskDevices : []item_spawner_device = array{} # Devices representing tasks to complete
    @editable
    MeetingRoom : zone_device = zone_device{} # Zone for emergency meetings
    @editable
    VotingTime : float = 30.0 # Duration of voting phase in seconds
    
    var PlayerTasks : [agent]int = map{} # Tracks tasks completed by each player
    var TotalTasks : int = 0 # Total number of tasks available
    var Impostors : [agent]logic = map{} # Tracks which players are impostors
    var IsMeeting : logic = false # Whether a meeting is currently active
    var AlivePlayers : [agent]logic = map{} # Tracks which players are alive
    var Votes : [agent]int = map{} # Track votes cast by players
    
    # Called when the game starts - initializes tasks and starts the game loop
    OnBegin<override>()<suspends>() : void =
        set TotalTasks = TaskDevices.Length
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { StartGame[] }
    
    # Called when a new player joins the game - initializes their state
    OnPlayerJoined(Agent : agent) : void =
        set AlivePlayers[Agent] = true
        set PlayerTasks[Agent] = 0
    
    # Delays then starts the game by assigning impostor roles
    StartGame()<suspends>() : void =
        Sleep(10.0)
        AssignImpostors[]
        Print("Game started! Complete tasks or find the impostor!")
    
    # Randomly assigns one player as impostor
    AssignImpostors() : void =
        Players := GetPlayspace().GetPlayers()
        if (Players.Length >= 3):
            ImpostorIndex := Random.UpperBound(Players.Length - 1)
            set Impostors[Players[ImpostorIndex]] = true
            Print("Impostor has been chosen...")
    
    # Called when a player completes a task - tracks progress and checks for crewmate win
    OnTaskCompleted(Agent : agent) : void =
        if (Tasks := PlayerTasks[Agent]):
            set PlayerTasks[Agent] = Tasks + 1
            Print("{Agent} completed task {Tasks + 1}/{TotalTasks}")
            
            if (PlayerTasks[Agent] >= TotalTasks):
                CrewmateWin[]
    
    # Declares crewmates as winners when all tasks are completed
    CrewmateWin() : void =
        Print("Crewmates win! All tasks completed!")
        set IsMeeting = true
    
    # Initiates an emergency meeting called by a player
    CallMeeting(Caller : agent) : void =
        set IsMeeting = true
        Print("{Caller} called an emergency meeting!")
        spawn { VotingPhase[] }
    
    # Waits for voting time to elapse then resolves votes
    VotingPhase()<suspends>() : void =
        Sleep(VotingTime)
        ResolveVotes[]
    
    # Tallies all votes and determines if the most voted player is the impostor
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
    
    # Casts a vote from one player against another during a meeting
    Vote(Agent : agent, Target : agent) : void =
        if IsMeeting:
            set Votes[Agent] = Target
    
    # Finds the player with the highest vote count
    GetMostVoted() : agent =
        var MaxVotes : int = 0
        var MostVoted : agent = agent{}
        for (Player : Votes, VoteCount : Votes[Player]):
            if VoteCount > MaxVotes:
                set MaxVotes = VoteCount
                set MostVoted = Player
        MostVoted
    
    # Allows an impostor to activate a sabotage event
    Sabotage(Impostor : agent, SabotageType : int) : void =
        if (Impostors[Impostor] = true and not IsMeeting):
            Print("Sabotage actived!")
            
    # Handles a player being eliminated and checks if impostor wins
    OnPlayerEliminated(Agent : agent) : void =
        set AlivePlayers[Agent] = false
        AliveCount := CountAlive[]
        if AliveCount <= 2 and HasAliveImpostor[]:
            Print("Impostor wins!")
    
    # Counts how many players are still alive
    CountAlive() : int =
        var Count : int = 0
        for (Agent : AlivePlayers, Alive : AlivePlayers[Agent]):
            if Alive: set Count += 1
        Count
    
    # Checks whether at least one impostor is still alive
    HasAliveImpostor() : logic =
        for (Agent : Impostors, IsImp : Impostors[Agent]):
            if IsImp and AlivePlayers[Agent]:
                return true
        return false
