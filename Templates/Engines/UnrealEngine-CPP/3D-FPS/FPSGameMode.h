// Header for FPS game mode with kill score and respawn logic.
#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameMode.h"
#include "FPSGameMode.generated.h"

UCLASS()
class AFPSGameMode : public AGameMode
{
    GENERATED_BODY()

public:
    AFPSGameMode();

protected:
    virtual void BeginPlay() override;

    // Called when a player is killed; handles score and respawn timer.
    virtual void OnPlayerKilled(AController* Victim, AController* Killer);

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    int32 KillScore = 100;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    float RespawnDelay = 5.0f;

    FTimerHandle RespawnTimerHandle;

    // Respawn a player at a random PlayerStart.
    virtual void RespawnPlayer(AController* Controller);
};
