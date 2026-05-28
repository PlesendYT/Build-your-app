using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /UnrealEngine.com/Temporary/Diagnostics }

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
    
    OnBegin<override>()<suspends>() : void =
        InitializePuzzles[]
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
    
    InitializePuzzles() : void =
        for (I : int = 0..Switches.Length):
            set SwitchStates[I] = false
    
    OnPlayerJoined(Agent : agent) : void =
        Print("Solve the puzzles to progress!")
    
    OnSwitchActivated(SwitchIndex : int) : void =
        if (SwitchStates[SwitchIndex] = false):
            set SwitchStates[SwitchIndex] = true
            EvaluateGates[]
            Print("Switch {SwitchIndex} activated!")
        else:
            set SwitchStates[SwitchIndex] = false
            EvaluateGates[]
            Print("Switch {SwitchIndex} deactivated!")
    
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
    
    UpdateGateVisual(GateIndex : int, Active : logic) : void =
        if Active:
            Gates[GateIndex].SpawnItem[]
            Print("Gate {GateIndex} opened!")
        else:
            Gates[GateIndex].Destroy[]
    
    OnPressurePlateActivated(PlateIndex : int) : void =
        set PuzzlesSolved += 1
        Print("Pressure plate {PlateIndex} activated! ({PuzzlesSolved}/5)")
        
        if (PuzzlesSolved >= 5):
            UnlockFinalDoor[]
    
    UnlockFinalDoor() : void =
        for (Door : DoorDevices):
            Door.SpawnItem[]
        Print("All puzzles solved! Final door unlocked!")
