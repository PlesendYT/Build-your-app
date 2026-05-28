using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

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
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        BlueFlag.SpawnItem[]
        RedFlag.SpawnItem[]
    
    OnPlayerJoined(Agent : agent) : void =
        Print("Player joined CTF")
    
    OnFlagPickup(Agent : agent, Flag : item_spawner_device) : void =
        set FlagCarriers[Agent] = Flag
        Flag.SetOwner(Agent)
        Print("{Agent} picked up the flag!")
    
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
    
    RespawnFlag(Flag : item_spawner_device)<suspends>() : void =
        Sleep(10.0)
        Flag.SpawnItem[]
    
    OnFlagDrop(Agent : agent) : void =
        if (Flag := FlagCarriers[Agent]):
            set FlagCarriers[Agent] = item_spawner_device{}
    
    EndGame() : void =
        if (BlueScore > RedScore):
            Print("Blue Team wins!")
        else if (RedScore > BlueScore):
            Print("Red Team wins!")
        else:
            Print("It's a tie!")
