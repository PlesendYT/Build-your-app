#pragma once
#include "CoreMinimal.h"
#include "GameFramework/GameState.h"
#include "RacingGameState.generated.h"

USTRUCT()
struct FRacerInfo
{
    GENERATED_BODY()
    UPROPERTY() APlayerState* PlayerState = nullptr;
    UPROPERTY() int32 CurrentLap = 1;
    UPROPERTY() int32 CurrentCheckpoint = 0;
    UPROPERTY() float RaceTime = 0.0f;
    UPROPERTY() int32 Position = 0;
    UPROPERTY() bool bFinished = false;
};

UCLASS()
class ARacingGameState : public AGameState
{
    GENERATED_BODY()
public:
    UPROPERTY(ReplicatedUsing=OnRep_Racers) TArray<FRacerInfo> Racers;

    UFUNCTION()
    void OnRep_Racers();

    void UpdateRacerPosition(APlayerState* PS, int32 Lap, int32 Checkpoint, float Time, bool Finished);

    UFUNCTION(BlueprintPure)
    TArray<FRacerInfo> GetLeaderboard() const;

    UFUNCTION(BlueprintPure)
    int32 GetPlayerPosition(APlayerState* PS) const;
};
