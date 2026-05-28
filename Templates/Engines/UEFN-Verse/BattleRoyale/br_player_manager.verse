using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /UnrealEngine.com/Temporary/Diagnostics }

br_player_manager := class(creative_device):
    @editable
    KillFeedDevice : kill_feed_device = kill_feed_device{}
    @editable
    StatDisplayDevice : stat_display_device = stat_display_device{}
    
    var AlivePlayers : [agent] = array{}
    var PlayerKills : [agent]int = map{}
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        GetPlayspace().PlayerRemovedEvent.Subscribe(OnPlayerLeft)
    
    OnPlayerJoined(Agent : agent) : void =
        set AlivePlayers += array{Agent}
        set PlayerKills[Agent] = 0
        Print("Player joined. Alive: {AlivePlayers.Length}")
    
    OnPlayerLeft(Agent : agent) : void =
        set AlivePlayers = AlivePlayers.RemoveElement(Agent)
        if (AlivePlayers.Length <= 1):
            DeclareWinner[AlivePlayers[0]]
    
    RegisterKill(Killer : agent, Victim : agent) : void =
        if (PlayerKills[Killer] <> 0):
            set PlayerKills[Killer] += 1
        else:
            set PlayerKills[Killer] = 1
        KillFeedDevice.AddKillEntry(Killer, Victim, "eliminated")
        set AlivePlayers = AlivePlayers.RemoveElement(Victim)
        if (AlivePlayers.Length <= 1):
            DeclareWinner[AlivePlayers[0]]
    
    DeclareWinner(Winner : agent) : void =
        Print("Winner: {Winner}")
        if (FortChar := Winner.GetFortCharacter[]):
            FortChar.AddGameResource(100)
