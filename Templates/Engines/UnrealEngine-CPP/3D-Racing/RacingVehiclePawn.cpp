#include "RacingVehiclePawn.h"
#include "GameFramework/SpringArmComponent.h"
#include "Camera/CameraComponent.h"
#include "ChaosWheeledVehicleMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "Net/UnrealNetwork.h"

ARacingVehiclePawn::ARacingVehiclePawn()
{
    PrimaryActorTick.bCanEverTick = true;
    CurrentBoost = MaxBoost;
}

void ARacingVehiclePawn::BeginPlay()
{
    Super::BeginPlay();
    TArray<AActor*> Found;
    UGameplayStatics::GetAllActorsWithTag(this, TEXT("Checkpoint"), Found);
    Checkpoints = Found;
    Checkpoints.Sort([](const AActor& A, const AActor& B) { return A.GetName() < B.GetName(); });
    TotalCheckpoints = Checkpoints.Num();
}

void ARacingVehiclePawn::Tick(float DeltaTime)
{
    Super::Tick(DeltaTime);
    if (!bRaceFinished) RaceTime += DeltaTime;
    if (CurrentBoost < MaxBoost) CurrentBoost = FMath::Min(MaxBoost, CurrentBoost + DeltaTime * 10.0f);
}

void ARacingVehiclePawn::MoveForward(const FInputActionValue& Value)
{
    float Axis = Value.Get<float>();
    if (UChaosWheeledVehicleMovementComponent* MoveComp = GetChaosWheeledVehicleMovementComponent())
    {
        MoveComp->SetThrottleInput(Axis);
    }
}

void ARacingVehiclePawn::MoveRight(const FInputActionValue& Value)
{
    float Axis = Value.Get<float>();
    if (UChaosWheeledVehicleMovementComponent* MoveComp = GetChaosWheeledVehicleMovementComponent())
    {
        MoveComp->SetSteeringInput(Axis);
    }
}

void ARacingVehiclePawn::Handbrake(const FInputActionValue& Value)
{
    if (UChaosWheeledVehicleMovementComponent* MoveComp = GetChaosWheeledVehicleMovementComponent())
    {
        MoveComp->SetHandbrakeInput(Value.Get<bool>());
    }
}

void ARacingVehiclePawn::ApplyBoost(float BoostAmount)
{
    if (CurrentBoost <= 0) return;
    CurrentBoost = FMath::Max(0.0f, CurrentBoost - BoostAmount);
    if (UChaosWheeledVehicleMovementComponent* MoveComp = GetChaosWheeledVehicleMovementComponent())
    {
        FVector BoostDir = GetActorForwardVector();
        MoveComp->AddTorqueInRadians(BoostDir * BoostForce);
    }
}
