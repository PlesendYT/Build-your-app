#include "HorrorCharacter.h"
#include "Camera/CameraComponent.h"
#include "Components/SpotLightComponent.h"
#include "EnhancedInputComponent.h"

AHorrorCharacter::AHorrorCharacter()
{
    PrimaryActorTick.bCanEverTick = true;
}

void AHorrorCharacter::BeginPlay() { Super::BeginPlay(); }

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

void AHorrorCharacter::ToggleFlashlight() { ServerToggleFlashlight(); }
void AHorrorCharacter::ServerToggleFlashlight_Implementation()
{
    bFlashlightOn = !bFlashlightOn;
}

void AHorrorCharacter::AddTension(float Amount)
{
    TensionLevel = FMath::Min(MaxTension, TensionLevel + Amount);
}

void AHorrorCharacter::CollectNote(const FString& NoteID)
{
    CollectedNotes.AddUnique(NoteID);
}
