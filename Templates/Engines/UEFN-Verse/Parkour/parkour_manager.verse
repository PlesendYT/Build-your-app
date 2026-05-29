# Parkour manager: players traverse checkpoints in sequence with the goal of reaching the finish with the fastest time.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Parkour device class managing checkpoints, timers, and leaderboard tracking.
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
    
    # Stores checkpoint count and subscribes to player join events.
    OnBegin<override>()<suspends>() : void =
        set TotalCheckpoints = Checkpoints.Length
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    # Initializes a joining player's progress and displays the objective.
    OnPlayerJoined(Agent : agent) : void =
        set PlayerProgress[Agent] = 0
        set PlayerTimes[Agent] = 0.0
        Print("Player joined Parkour. Complete {TotalCheckpoints} checkpoints!")
    
    # Updates checkpoint progress and records start time on first checkpoint; checks for finish.
    OnCheckpointReached(Agent : agent, CheckpointIndex : int) : void =
        if (PlayerProgress[Agent] = CheckpointIndex - 1):
            set PlayerProgress[Agent] = CheckpointIndex
            Print("Checkpoint {CheckpointIndex}/{TotalCheckpoints} reached!")
            if (CheckpointIndex = 1):
                set PlayerStartTimes[Agent] = GetWorldTime()
        
        if (PlayerProgress[Agent] = TotalCheckpoints - 1 and 
            PlayerProgress[Agent] <> -1):
            OnFinishReached[Agent]
    
    # Calculates elapsed time and records the player on the leaderboard upon finishing.
    OnFinishReached(Agent : agent) : void =
        if (StartTime := PlayerStartTimes[Agent]):
            Elapsed := GetWorldTime() - StartTime
            set PlayerTimes[Agent] = Elapsed
            set PlayerProgress[Agent] = -1
            Print("Finished! Time: {Elapsed}s")
            BestTimeDevice.AddEntry(Agent, Elapsed)
    
    # Returns the current time from the timer device.
    GetWorldTime() : float =
        TimerDevice.GetCurrentTime()
    
    # Resets a player's progress and start time back to zero.
    ResetPlayer(Agent : agent) : void =
        set PlayerProgress[Agent] = 0
        set PlayerStartTimes[Agent] = 0.0
