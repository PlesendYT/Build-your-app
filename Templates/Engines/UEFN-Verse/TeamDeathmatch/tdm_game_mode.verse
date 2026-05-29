# Team Deathmatch mode: two teams battle to reach the score limit first with wave respawns.
using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /UnrealEngine.com/Temporary/Diagnostics }

# Main Team Deathmatch device class managing teams, scores, and wave respawns.
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
    
    # Subscribes to player join events and starts the respawn wave loop.
    OnBegin<override>()<suspends>() : void =
        GetPlayspace().PlayerAddedEvent.Subscribe(OnPlayerJoined)
        spawn { RespawnWaveLoop[] }
    
    # Assigns a joining player to the team with fewer members to balance teams.
    OnPlayerJoined(Agent : agent) : void =
        if (GetTeamCount(BlueTeamDevice) <= GetTeamCount(RedTeamDevice)):
            set TeamAssignments[Agent] = BlueTeamDevice
            BlueTeamDevice.AddToTeam(Agent)
        else:
            set TeamAssignments[Agent] = RedTeamDevice
            RedTeamDevice.AddToTeam(Agent)
    
    # Marks a player as dead and awards a point to the eliminator's team; checks score limit.
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
    
    # Periodically respawns all dead players in waves.
    RespawnWaveLoop()<suspends>() : void =
        loop:
            Sleep(RespawnWaveTime)
            for (Agent : DeadPlayers, ShouldRespawn : DeadPlayers[Agent]):
                if ShouldRespawn:
                    RespawnDevice.Spawn(Agent)
                    set DeadPlayers[Agent] = false
    
    # Announces the winning team.
    EndGame(WinningTeam : team_device) : void =
        Print("Team {WinningTeam} wins!")
    
    # Counts how many players are assigned to a given team.
    GetTeamCount(TargetTeam : team_device) : int =
        var Count : int = 0
        for (Agent : TeamAssignments, AssignedTeam : TeamAssignments[Agent]):
            if AssignedTeam = TargetTeam:
                set Count += 1
        Count
