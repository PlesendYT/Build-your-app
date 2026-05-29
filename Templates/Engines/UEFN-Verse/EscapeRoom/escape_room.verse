# Escape Room mode: players solve puzzles and find keys to escape within a time limit.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Escape Room device class managing puzzles, keys, timer, and exit conditions.
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
    
    # Subscribes to player join events when the device starts.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    # Initializes a new player's key count and starts the game if not already active.
    OnPlayerJoined(Agent : agent) : void =
        set KeysFound[Agent] = 0
        if (not GameActive):
            StartGame[]
    
    # Activates the game, starts the timer, and announces the objective.
    StartGame() : void =
        set GameActive = true
        TimerDevice.Start()
        spawn { GameTimer[] }
        Print("Escape Room started! Solve {TotalPuzzles} puzzles to escape!")
    
    # Waits for the time limit then ends the game if still active.
    GameTimer()<suspends>() : void =
        Sleep(TimerLimit)
        if GameActive:
            Print("Time's up! You failed to escape!")
            set GameActive = false
    
    # Increments the solved puzzle count and triggers the exit when all are solved.
    OnPuzzleSolved(Agent : agent, PuzzleID : int) : void =
        set PuzzlesSolved += 1
        Print("Puzzle {PuzzlesSolved}/{TotalPuzzles} solved!")
        
        if (PuzzlesSolved >= TotalPuzzles):
            OnAllPuzzlesSolved(Agent)
    
    # Spawns the exit door when all puzzles are completed.
    OnAllPuzzlesSolved(Agent : agent) : void =
        ExitDoor.SpawnItem[]
        Print("All puzzles solved! Exit door is open!")
    
    # Tracks how many keys a player has found.
    OnKeyFound(Agent : agent) : void =
        if (KeysFound[Agent] <> 0):
            set KeysFound[Agent] += 1
        else:
            set KeysFound[Agent] = 1
    
    # Handles a player reaching the exit: stops the timer and announces their time.
    OnExitReached(Agent : agent) : void =
        if GameActive:
            set GameActive = false
            TimerDevice.Stop()
            FinalTime := TimerDevice.GetCurrentTime()
            Print("{Agent} escaped in {FinalTime} seconds!")
