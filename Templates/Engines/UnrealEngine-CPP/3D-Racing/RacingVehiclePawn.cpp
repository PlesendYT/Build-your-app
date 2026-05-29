// Racing vehicle pawn with physics-based driving, boost, and lap tracking.
#include "RacingVehiclePawn.h"
#include "GameFramework/SpringArmComponent.h"
#include "Camera/CameraComponent.h"
#include "ChaosWheeledVehicleMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "Net/UnrealNetwork.h"

// Initialize boost and enable tick.
ARacingVehiclePawn::ARacingVehiclePawn()
{
    PrimaryActorTick.bCanEverTick = true;
    CurrentBoost = MaxBoost;
}

// Find and sort checkpoints in the world.
void ARacingVehiclePawn::BeginPlay()
{
    Super::BeginPlay();
    TArray<AActor*> Found;
    UGameplayStatics::GetAllActorsWithTag(this, TEXT("Checkpoint"), Found);
    Checkpoints = Found;
    Checkpoints.Sort([](const AActor& A, const AActor& B) { return A.GetName() < B.GetName(); });
    TotalCheckpoints = Checkpoints.Num();
}

// Update race time and regenerate boost.
void ARacingVehiclePawn::Tick(float DeltaTime)
{
    Super::Tick(DeltaTime);
    if (!bRaceFinished) RaceTime += DeltaTime;
    if (CurrentBoost < MaxBoost) CurrentBoost = FMath::Min(MaxBoost, CurrentBoost + DeltaTime * 10.0f);
}

// Apply throttle input to the vehicle.
void ARacingVehiclePawn::MoveForward(const FInputActionValue& Value)
{
    float Axis = Value.Get<float>();
    if (UChaosWheeledVehicleMovementComponent* MoveComp = GetChaosWheeledVehicleMovementComponent())
    {
        MoveComp->SetThrottleInput(Axis);
    }
}

// Apply steering input to the vehicle.
void ARacingVehiclePawn::MoveRight(const FInputActionValue& Value)
{
    float Axis = Value.Get<float>();
    if (UChaosWheeledVehicleMovementComponent* MoveComp = GetChaosWheeledVehicleMovementComponent())
    {
        MoveComp->SetSteeringInput(Axis);
    }
}

// Apply handbrake input to the vehicle.
void ARacingVehiclePawn::Handbrake(const FInputActionValue& Value)
{
    if (UChaosWheeledVehicleMovementComponent* MoveComp = GetChaosWheeledVehicleMovementComponent())
    {
        MoveComp->SetHandbrakeInput(Value.Get<bool>());
    }
}

// Expend boost meter to apply a forward impulse.
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
