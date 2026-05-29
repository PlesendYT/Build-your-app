# Racing game mode: players race through checkpoints across multiple laps; fastest finisher wins.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Racing device class managing laps, checkpoints, boost pads, and leaderboard.
racing_game_mode := class(creative_device):
    @editable
    TotalLaps : int = 3
    @editable
    Checkpoints : []zone_device = array{}
    @editable
    FinishLine : zone_device = zone_device{}
    @editable
    BoostPadDevice : []boost_pad_device = array{}
    @editable
    LeaderboardDevice : leaderboard_device = leaderboard_device{}
    
    var PlayerLap : [agent]int = map{}
    var PlayerCheckpoint : [agent]int = map{}
    var PlayerRaceTime : [agent]float = map{}
    var PlayerFinished : [agent]logic = map{}
    var RaceStartTime : float = 0.0
    var RacingActive : logic = false
    
    # Subscribes to player join events when the device starts.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    # Initializes lap and checkpoint state for a new player.
    OnPlayerJoined(Agent : agent) : void =
        set PlayerLap[Agent] = 1
        set PlayerCheckpoint[Agent] = 0
        set PlayerFinished[Agent] = false
    
    # Starts the race, recording the start time.
    StartRace() : void =
        set RaceStartTime = GetWorldTime()
        set RacingActive = true
        Print("Race started! {TotalLaps} laps!")
    
    # Updates a player's checkpoint progress when they reach the next one in order.
    OnCheckpointReached(Agent : agent, CheckpointIndex : int) : void =
        if (not PlayerFinished[Agent] and RacingActive):
            if (CheckpointIndex = PlayerCheckpoint[Agent] + 1):
                set PlayerCheckpoint[Agent] = CheckpointIndex
    
    # Advances the player's lap count or finishes the race if all laps are done.
    OnLapCompleted(Agent : agent) : void =
        if (PlayerCheckpoint[Agent] = Checkpoints.Length):
            if (PlayerLap[Agent] < TotalLaps):
                set PlayerLap[Agent] += 1
                set PlayerCheckpoint[Agent] = 0
                Print("Lap {PlayerLap[Agent]} for {Agent}")
            else:
                FinishRace[Agent]
    
    # Records the player's finish time and adds it to the leaderboard.
    FinishRace(Agent : agent) : void =
        set PlayerFinished[Agent] = true
        Elapsed := GetWorldTime() - RaceStartTime
        set PlayerRaceTime[Agent] = Elapsed
        Print("{Agent} finished! Time: {Elapsed}s")
        LeaderboardDevice.AddEntry(Agent, Elapsed)
    
    # Launches the player forward when they activate a boost pad.
    OnBoostPadActivated(Agent : agent) : void =
        if (FortChar := Agent.GetFortCharacter[]):
            LaunchVector := FortChar.GetTransform().Translation + 
                           FortChar.GetTransform().Rotation.GetForwardVector() * 2000.0
            FortChar.LaunchCharacter(LaunchVector)
    
    # Placeholder for world time; should be replaced with actual time retrieval.
    GetWorldTime() : float = 0.0
