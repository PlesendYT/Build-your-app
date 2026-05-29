// Horror character with flashlight battery drain, tension level, and note collection.
#include "HorrorCharacter.h"
#include "Camera/CameraComponent.h"
#include "Components/SpotLightComponent.h"
#include "EnhancedInputComponent.h"

// Enable tick for battery drain and tension decay.
AHorrorCharacter::AHorrorCharacter()
{
    PrimaryActorTick.bCanEverTick = true;
}

void AHorrorCharacter::BeginPlay() { Super::BeginPlay(); }

// Drain battery when flashlight is on; decay tension over time.
void AHorrorCharacter::Tick(float DT)
{
    Super::Tick(DT);
    if (bFlashlightOn && BatteryLife > 0)
    {
        BatteryLife = FMath::Max(0.0f, BatteryLife - BatteryDrainRate * DT);
        if (BatteryLife <= 0) bFlashlightOn = false;
    }
    if (TensionLevel > 0) TensionLevel = FMath::Max(0.0f, TensionLevel - DT * 1.0f);
}

// Toggle flashlight on/off via server RPC.
void AHorrorCharacter::ToggleFlashlight() { ServerToggleFlashlight(); }
// Server-side: toggle the flashlight state.
void AHorrorCharacter::ServerToggleFlashlight_Implementation()
{
    bFlashlightOn = !bFlashlightOn;
}

// Increase the tension level (clamped to max).
void AHorrorCharacter::AddTension(float Amount)
{
    TensionLevel = FMath::Min(MaxTension, TensionLevel + Amount);
}

// Add a note ID to the collected notes list (duplicates prevented).
void AHorrorCharacter::CollectNote(const FString& NoteID)
{
    CollectedNotes.AddUnique(NoteID);
}
