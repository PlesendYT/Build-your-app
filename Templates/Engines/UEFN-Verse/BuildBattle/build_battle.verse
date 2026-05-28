using { /Fortnite.com/Devices }
using { /Fortnite.com/Game }
using { /Fortnite.com/Characters }
using { /Verse.org/Simulation }
using { /Verse.org/Time }
using { /Verse.org/Random }
using { /UnrealEngine.com/Temporary/Diagnostics }

build_battle := class(creative_device):
    @editable
    BuildTime : float = 180.0
    @editable
    VoteTime : float = 30.0
    @editable
    BuildingPlot : zone_device = zone_device{}
    @editable
    VoteDevices : []item_spawner_device = array{}
    
    var CurrentTheme : string = ""
    var BuildingPhase : logic = false
    var VotingPhase : logic = false
    var PlayerBuilds : [agent]string = map{}
    var VoteCounts : [agent]int = map{}
    var Themes : []string = array{"House", "Castle", "Animal", "Spaceship", "Treehouse", "Volcano", "Bridge", "Statue"}
    var RoundNumber : int = 1
    
    OnBegin<override>()<suspends>() : void =
        spawn { GameLoop[] }
    
    GameLoop()<suspends>() : void =
        loop:
            StartBuildRound[]
            Sleep(BuildTime)
            EndBuildPhase[]
            StartVotingPhase[]
            Sleep(VoteTime)
            EndVotingPhase[]
            set RoundNumber += 1
    
    StartBuildRound() : void =
        set CurrentTheme = GetRandomTheme[]
        set BuildingPhase = true
        set VotingPhase = false
        Print("Round {RoundNumber}! Theme: {CurrentTheme}. Build something amazing!")
        GiveBuildingBlocks[]
    
    GetRandomTheme() : string =
        Index := Random.UpperBound(Themes.Length - 1)
        Themes[Index]
    
    GiveBuildingBlocks() : void =
        for (Agent : GetPlayspace().GetPlayers()):
            if (FortChar := Agent.GetFortCharacter()):
                FortChar.AddItem("Stone", 999)
                FortChar.AddItem("Wood", 999)
                FortChar.AddItem("Glass", 64)
    
    EndBuildPhase() : void =
        set BuildingPhase = false
        Print("Build time is over!")
    
    StartVotingPhase() : void =
        set VotingPhase = true
        Print("Vote for the best build! (1-5 stars)")
    
    VoteForBuild(Voter : agent, Builder : agent, Stars : int) : void =
        if VotingPhase:
            if (VoteCounts[Builder] <> 0):
                set VoteCounts[Builder] = VoteCounts[Builder] + Stars
            else:
                set VoteCounts[Builder] = Stars
    
    EndVotingPhase() : void =
        set VotingPhase = false
        Winner := GetWinner[]
        Print("Winner: {Winner} with {VoteCounts[Winner]} stars!")
        
        if (FortChar := Winner.GetFortCharacter()):
            FortChar.AddGameResource(100)
        
        ResetAllBuilds[]
    
    GetWinner() : agent =
        var BestAgent : agent = agent{}
        var BestScore : int = 0
        for (Builder : VoteCounts, Score : VoteCounts[Builder]):
            if Score > BestScore:
                set BestAgent = Builder
                set BestScore = Score
        BestAgent
    
    ResetAllBuilds() : void =
        set VoteCounts = map{}
        Print("Get ready for the next round!")
