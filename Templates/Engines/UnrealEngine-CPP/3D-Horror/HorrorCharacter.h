// Header for a horror character with flashlight, tension, and notes.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "HorrorCharacter.generated.h"

UCLASS()
class AHorrorCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    AHorrorCharacter();

    UPROPERTY(BlueprintReadOnly) float BatteryLife = 100.0f;
    UPROPERTY(BlueprintReadOnly) bool bFlashlightOn = true;
    UPROPERTY(BlueprintReadOnly) float TensionLevel = 0.0f;
    UPROPERTY(BlueprintReadOnly) TArray<FString> CollectedNotes;

    // Toggle the flashlight on/off.
    UFUNCTION(BlueprintCallable)
    void ToggleFlashlight();

    // Add tension (clamped to max).
    UFUNCTION(BlueprintCallable)
    void AddTension(float Amount);

    // Collect a note by its ID.
    UFUNCTION(BlueprintCallable)
    void CollectNote(const FString& NoteID);

protected:
    virtual void BeginPlay() override;
    virtual void Tick(float DT) override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    UPROPERTY(EditDefaultsOnly) float BatteryDrainRate = 2.0f;
    UPROPERTY(EditDefaultsOnly) float MaxTension = 100.0f;
    UPROPERTY(EditDefaultsOnly) UInputAction* FlashlightAction;

    UFUNCTION(Server, Reliable) void ServerToggleFlashlight();
};
