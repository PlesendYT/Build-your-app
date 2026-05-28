using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

parkour_manager := class(creative_device):
    @editable
    Checkpoints : []zone_device = array{}
    @editable
    FinishZone : zone_device = zone_device{}
    @editable
    TimerDevice : timer_device = timer_device{}
    @editable
    BestTimeDevice : leaderboard_device = leaderboard_device{}
    
    var PlayerProgress : [agent]int = map{}
    var PlayerTimes : [agent]float = map{}
    var PlayerStartTimes : [agent]float = map{}
    var TotalCheckpoints : int = 0
    
    OnBegin<override>()<suspends>() : void =
        set TotalCheckpoints = Checkpoints.Length
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    OnPlayerJoined(Agent : agent) : void =
        set PlayerProgress[Agent] = 0
        set PlayerTimes[Agent] = 0.0
        Print("Player joined Parkour. Complete {TotalCheckpoints} checkpoints!")
    
    OnCheckpointReached(Agent : agent, CheckpointIndex : int) : void =
        if (PlayerProgress[Agent] = CheckpointIndex - 1):
            set PlayerProgress[Agent] = CheckpointIndex
            Print("Checkpoint {CheckpointIndex}/{TotalCheckpoints} reached!")
            if (CheckpointIndex = 1):
                set PlayerStartTimes[Agent] = GetWorldTime()
        
        if (PlayerProgress[Agent] = TotalCheckpoints - 1 and 
            PlayerProgress[Agent] <> -1):
            OnFinishReached[Agent]
    
    OnFinishReached(Agent : agent) : void =
        if (StartTime := PlayerStartTimes[Agent]):
            Elapsed := GetWorldTime() - StartTime
            set PlayerTimes[Agent] = Elapsed
            set PlayerProgress[Agent] = -1
            Print("Finished! Time: {Elapsed}s")
            BestTimeDevice.AddEntry(Agent, Elapsed)
    
    GetWorldTime() : float =
        TimerDevice.GetCurrentTime()
    
    ResetPlayer(Agent : agent) : void =
        set PlayerProgress[Agent] = 0
        set PlayerStartTimes[Agent] = 0.0
