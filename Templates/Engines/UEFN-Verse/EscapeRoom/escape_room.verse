using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

escape_room := class(creative_device):
    @editable
    TotalPuzzles : int = 5
    @editable
    ExitDoor : item_spawner_device = item_spawner_device{}
    @editable
    PuzzleDevices : []item_spawner_device = array{}
    @editable
    TimerDevice : timer_device = timer_device{}
    @editable
    TimerLimit : float = 300.0
    
    var PuzzlesSolved : int = 0
    var GameActive : logic = false
    var KeysFound : [agent]int = map{}
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    OnPlayerJoined(Agent : agent) : void =
        set KeysFound[Agent] = 0
        if (not GameActive):
            StartGame[]
    
    StartGame() : void =
        set GameActive = true
        TimerDevice.Start()
        spawn { GameTimer[] }
        Print("Escape Room started! Solve {TotalPuzzles} puzzles to escape!")
    
    GameTimer()<suspends>() : void =
        Sleep(TimerLimit)
        if GameActive:
            Print("Time's up! You failed to escape!")
            set GameActive = false
    
    OnPuzzleSolved(Agent : agent, PuzzleID : int) : void =
        set PuzzlesSolved += 1
        Print("Puzzle {PuzzlesSolved}/{TotalPuzzles} solved!")
        
        if (PuzzlesSolved >= TotalPuzzles):
            OnAllPuzzlesSolved(Agent)
    
    OnAllPuzzlesSolved(Agent : agent) : void =
        ExitDoor.SpawnItem[]
        Print("All puzzles solved! Exit door is open!")
    
    OnKeyFound(Agent : agent) : void =
        if (KeysFound[Agent] <> 0):
            set KeysFound[Agent] += 1
        else:
            set KeysFound[Agent] = 1
    
    OnExitReached(Agent : agent) : void =
        if GameActive:
            set GameActive = false
            TimerDevice.Stop()
            FinalTime := TimerDevice.GetCurrentTime()
            Print("{Agent} escaped in {FinalTime} seconds!")
