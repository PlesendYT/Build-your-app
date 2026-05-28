#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "StealthCharacter.generated.h"

UCLASS()
class AStealthCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    AStealthCharacter();

    UPROPERTY(BlueprintReadOnly) float StealthMeter = 1.0f;
    UPROPERTY(BlueprintReadOnly) float NoiseLevel = 0.0f;
    UPROPERTY(BlueprintReadOnly) bool bIsInShadow = false;
    UPROPERTY(BlueprintReadOnly) bool bIsCrouching = false;

    UFUNCTION(BlueprintCallable)
    void MakeNoise(float Loudness);

    UFUNCTION(BlueprintCallable)
    void ToggleCrouch();

    UFUNCTION(BlueprintCallable)
    void DistractEnemy(FVector Location);

protected:
    virtual void BeginPlay() override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    void Move(const FInputActionValue& V);
    void Look(const FInputActionValue& V);
    void Interact();

    UPROPERTY(EditDefaultsOnly) UInputMappingContext* InputMapping;
    UPROPERTY(EditDefaultsOnly) UInputAction* MoveAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* LookAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* CrouchAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* InteractAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* DistractAction;
};
