# Capture the Flag mode: two teams compete to capture the enemy flag and return it to their base for points.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Capture the Flag device class managing flags, bases, scoring, and respawns.
ctf_game_mode := class(creative_device):
    @editable
    ScoreToWin : int = 3
    @editable
    BlueBase : zone_device = zone_device{}
    @editable
    RedBase : zone_device = zone_device{}
    @editable
    BlueFlag : item_spawner_device = item_spawner_device{}
    @editable
    RedFlag : item_spawner_device = item_spawner_device{}
    
    var BlueScore : int = 0
    var RedScore : int = 0
    var FlagCarriers : [agent]item_spawner_device = map{}
    
    # Subscribes to player join events and spawns both flags at the start.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        BlueFlag.SpawnItem[]
        RedFlag.SpawnItem[]
    
    # Notifies a joining player that they are in CTF mode.
    OnPlayerJoined(Agent : agent) : void =
        Print("Player joined CTF")
    
    # Assigns the picked-up flag to the player and sets its owner.
    OnFlagPickup(Agent : agent, Flag : item_spawner_device) : void =
        set FlagCarriers[Agent] = Flag
        Flag.SetOwner(Agent)
        Print("{Agent} picked up the flag!")
    
    # Scores a point when a flag is captured at the correct enemy base; checks win condition.
    OnFlagCapture(Agent : agent, CapturedAt : zone_device) : void =
        if (CarriedFlag := FlagCarriers[Agent]):
            if ((CarriedFlag = BlueFlag and CapturedAt = RedBase) or 
                (CarriedFlag = RedFlag and CapturedAt = BlueBase)):
                if (CapturedAt = BlueBase):
                    set BlueScore += 1
                else:
                    set RedScore += 1
                set FlagCarriers[Agent] = item_spawner_device{}
                CarriedFlag.Destroy[]
                spawn { RespawnFlag[CarriedFlag] }
                
                if (BlueScore >= ScoreToWin or RedScore >= ScoreToWin):
                    EndGame[]
    
    # Waits 10 seconds then respawns a captured flag at its base.
    RespawnFlag(Flag : item_spawner_device)<suspends>() : void =
        Sleep(10.0)
        Flag.SpawnItem[]
    
    # Clears the flag carrier entry when a player drops the flag.
    OnFlagDrop(Agent : agent) : void =
        if (Flag := FlagCarriers[Agent]):
            set FlagCarriers[Agent] = item_spawner_device{}
    
    # Announces the winning team or a tie at the end of the game.
    EndGame() : void =
        if (BlueScore > RedScore):
            Print("Blue Team wins!")
        else if (RedScore > BlueScore):
            Print("Red Team wins!")
        else:
            Print("It's a tie!")
