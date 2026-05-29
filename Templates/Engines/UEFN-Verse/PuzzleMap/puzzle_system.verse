# Puzzle System: players interact with switches and pressure plates to solve logic-gate puzzles and unlock doors.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Puzzle device class managing switches, gates, and puzzle progression.
puzzle_system := class(creative_device):
    @editable
    Switches : []item_spawner_device = array{}
    @editable
    Gates : []item_spawner_device = array{}
    @editable
    DoorDevices : []item_spawner_device = array{}
    
    var SwitchStates : [int]logic = map{}
    var GateOutputs : [int]logic = map{}
    var TotalPuzzles : int = 0
    var PuzzlesSolved : int = 0
    
    # Initializes switch states and subscribes to player join events.
    OnBegin<override>()<suspends>() : void =
        InitializePuzzles[]
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    # Sets all switch states to false (off) at startup.
    InitializePuzzles() : void =
        for (I : int = 0..Switches.Length):
            set SwitchStates[I] = false
    
    # Welcomes a joining player with a hint message.
    OnPlayerJoined(Agent : agent) : void =
        Print("Solve the puzzles to progress!")
    
    # Toggles a switch and re-evaluates all gates.
    OnSwitchActivated(SwitchIndex : int) : void =
        if (SwitchStates[SwitchIndex] = false):
            set SwitchStates[SwitchIndex] = true
            EvaluateGates[]
            Print("Switch {SwitchIndex} activated!")
        else:
            set SwitchStates[SwitchIndex] = false
            EvaluateGates[]
            Print("Switch {SwitchIndex} deactivated!")
    
    # Evaluates each gate based on its type (AND, OR, NOT, XOR) and updates visuals.
    EvaluateGates() : void =
        for (GateIndex : int = 0..Gates.Length):
            GateType := GateIndex % 4
            
            if GateType = 0:   # AND gate
                set GateOutputs[GateIndex] = SwitchStates[0] and SwitchStates[1]
            else if GateType = 1:  # OR gate
                set GateOutputs[GateIndex] = SwitchStates[0] or SwitchStates[1]
            else if GateType = 2:  # NOT gate
                set GateOutputs[GateIndex] = not SwitchStates[0]
            else:  # XOR gate
                set GateOutputs[GateIndex] = SwitchStates[0] xor SwitchStates[1]
            
            UpdateGateVisual(GateIndex, GateOutputs[GateIndex])
    
    # Shows or hides a gate based on its active state.
    UpdateGateVisual(GateIndex : int, Active : logic) : void =
        if Active:
            Gates[GateIndex].SpawnItem[]
            Print("Gate {GateIndex} opened!")
        else:
            Gates[GateIndex].Destroy[]
    
    # Tracks pressure plate puzzles and unlocks the final door when enough are solved.
    OnPressurePlateActivated(PlateIndex : int) : void =
        set PuzzlesSolved += 1
        Print("Pressure plate {PlateIndex} activated! ({PuzzlesSolved}/5)")
        
        if (PuzzlesSolved >= 5):
            UnlockFinalDoor[]
    
    # Spawns all door devices when every puzzle is completed.
    UnlockFinalDoor() : void =
        for (Door : DoorDevices):
            Door.SpawnItem[]
        Print("All puzzles solved! Final door unlocked!")
