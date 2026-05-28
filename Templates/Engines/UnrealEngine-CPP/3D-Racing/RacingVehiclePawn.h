#pragma once
#include "CoreMinimal.h"
#include "WheeledVehiclePawn.h"
#include "RacingVehiclePawn.generated.h"

class UBoxComponent;
class UInputAction;
struct FInputActionValue;

UCLASS()
class ARacingVehiclePawn : public AWheeledVehiclePawn
{
    GENERATED_BODY()
public:
    ARacingVehiclePawn();

    UPROPERTY(BlueprintReadOnly) int32 CurrentLap = 1;
    UPROPERTY(BlueprintReadOnly) int32 TotalLaps = 3;
    UPROPERTY(BlueprintReadOnly) int32 CurrentCheckpoint = 0;
    UPROPERTY(BlueprintReadOnly) float RaceTime = 0.0f;
    UPROPERTY(BlueprintReadOnly) bool bRaceFinished = false;

    UFUNCTION(BlueprintCallable)
    void ApplyBoost(float BoostAmount);

protected:
    virtual void BeginPlay() override;
    virtual void Tick(float DeltaTime) override;
    virtual void SetupPlayerInputComponent(UInputComponent* PIC) override;

    void MoveForward(const FInputActionValue& Value);
    void MoveRight(const FInputActionValue& Value);
    void Handbrake(const FInputActionValue& Value);

    UPROPERTY(EditDefaultsOnly)
    float BoostForce = 5000.0f;

    UPROPERTY(EditDefaultsOnly)
    float MaxBoost = 100.0f;

    UPROPERTY(Replicated)
    float CurrentBoost;

    UPROPERTY(EditDefaultsOnly)
    UInputAction* ThrottleAction;

    UPROPERTY(EditDefaultsOnly)
    UInputAction* SteerAction;

    UPROPERTY(EditDefaultsOnly)
    UInputAction* HandbrakeAction;

    UPROPERTY(EditDefaultsOnly)
    UInputAction* BoostAction;

    TArray<AActor*> Checkpoints;
    int32 TotalCheckpoints;
};
