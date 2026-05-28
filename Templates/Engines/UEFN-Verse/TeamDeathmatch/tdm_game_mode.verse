using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

tdm_game_mode := class(creative_device):
    @editable
    ScoreLimit : int = 50
    @editable
    RespawnWaveTime : float = 10.0
    @editable
    BlueTeamDevice : team_device = team_device{}
    @editable
    RedTeamDevice : team_device = team_device{}
    @editable
    RespawnDevice : respawn_device = respawn_device{}
    
    var BlueScore : int = 0
    var RedScore : int = 0
    var TeamAssignments : [agent]team_device = map{}
    var DeadPlayers : [agent]logic = map{}
    
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { RespawnWaveLoop[] }
    
    OnPlayerJoined(Agent : agent) : void =
        if (GetTeamCount(BlueTeamDevice) <= GetTeamCount(RedTeamDevice)):
            set TeamAssignments[Agent] = BlueTeamDevice
            BlueTeamDevice.AddToTeam(Agent)
        else:
            set TeamAssignments[Agent] = RedTeamDevice
            RedTeamDevice.AddToTeam(Agent)
    
    OnPlayerEliminated(Eliminated : agent, Eliminator : agent) : void =
        set DeadPlayers[Eliminated] = true
        if (Eliminator <> Eliminated):
            if (TeamAssignments[Eliminator] = BlueTeamDevice):
                set BlueScore += 1
            else:
                set RedScore += 1
            if (BlueScore >= ScoreLimit):
                EndGame(BlueTeamDevice)
            else if (RedScore >= ScoreLimit):
                EndGame(RedTeamDevice)
    
    RespawnWaveLoop()<suspends>() : void =
        loop:
            Sleep(RespawnWaveTime)
            for (Agent : DeadPlayers, ShouldRespawn : DeadPlayers[Agent]):
                if ShouldRespawn:
                    RespawnDevice.Spawn(Agent)
                    set DeadPlayers[Agent] = false
    
    EndGame(WinningTeam : team_device) : void =
        Print("Team {WinningTeam} wins!")
    
    GetTeamCount(TargetTeam : team_device) : int =
        var Count : int = 0
        for (Agent : TeamAssignments, AssignedTeam : TeamAssignments[Agent]):
            if AssignedTeam = TargetTeam:
                set Count += 1
        Count
