// Racing game state with leaderboard, lap tracking, and position sorting.
#include "RacingGameState.h"
#include "Net/UnrealNetwork.h"

// Set up replication for the Racers array.
void ARacingGameState::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    DOREPLIFETIME(ARacingGameState, Racers);
}

void ARacingGameState::OnRep_Racers() {}

void ARacingGameState::UpdateRacerPosition(APlayerState* PS, int32 Lap, int32 Checkpoint, float Time, bool Finished)
{
    for (auto& R : Racers)
    {
        if (R.PlayerState == PS)
        {
            R.CurrentLap = Lap;
            R.CurrentCheckpoint = Checkpoint;
            R.RaceTime = Time;
            R.bFinished = Finished;
            return;
        }
    }
    Racers.Add({PS, Lap, Checkpoint, Time, 0, Finished});
    Racers.Sort([](const FRacerInfo& A, const FRacerInfo& B)
    {
        if (A.bFinished != B.bFinished) return A.bFinished > B.bFinished;
        if (A.CurrentLap != B.CurrentLap) return A.CurrentLap > B.CurrentLap;
        if (A.CurrentCheckpoint != B.CurrentCheckpoint) return A.CurrentCheckpoint > B.CurrentCheckpoint;
        return A.RaceTime < B.RaceTime;
    });
    for (int32 i = 0; i < Racers.Num(); i++) Racers[i].Position = i + 1;
}

int32 ARacingGameState::GetPlayerPosition(APlayerState* PS) const
{
    for (const auto& R : Racers)
        if (R.PlayerState == PS) return R.Position;
    return 0;
}
