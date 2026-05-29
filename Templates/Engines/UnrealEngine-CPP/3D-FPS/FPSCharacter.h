// Header for an FPS character with first-person camera, sprint, weapons, and armor.
#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "FPSCharacter.generated.h"

class UCameraComponent;
class USpringArmComponent;
class AFPSWeapon;
class UInputMappingContext;
class UInputAction;
struct FInputActionValue;

// Delegate for weapon change events.
// (FOnWeaponChanged should be declared elsewhere in the project.)

UCLASS()
class AFPSCharacter : public ACharacter
{
    GENERATED_BODY()

public:
    AFPSCharacter();

    UPROPERTY(BlueprintAssignable)
    FOnWeaponChanged OnWeaponChanged;

protected:
    virtual void BeginPlay() override;
    virtual void SetupPlayerInputComponent(UInputComponent* InputComponent) override;

    void Move(const FInputActionValue& Value);
    void Look(const FInputActionValue& Value);
    void StartJump();
    void StopJump();
    void StartSprint();
    void StopSprint();
    void StartFire();
    void StopFire();
    void ReloadWeapon();

    UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
    UCameraComponent* FirstPersonCamera;

    UPROPERTY(VisibleDefaultsOnly)
    USkeletalMeshComponent* FP_Mesh;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    TSubclassOf<AFPSWeapon> DefaultWeaponClass;

    UPROPERTY(EditDefaultsOnly)
    float MaxHealth = 100.0f;

    UPROPERTY(EditDefaultsOnly)
    float MaxArmor = 100.0f;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    float WalkSpeed = 600.0f;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    float SprintSpeed = 1000.0f;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputMappingContext* InputMapping;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputAction* MoveAction;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputAction* LookAction;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputAction* JumpAction;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputAction* SprintAction;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputAction* FireAction;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputAction* ReloadAction;

    UPROPERTY(Replicated, BlueprintReadWrite)
    float CurrentHealth;

    UPROPERTY(Replicated, BlueprintReadWrite)
    float CurrentArmor;

    UPROPERTY(Replicated)
    AFPSWeapon* CurrentWeapon;

    UFUNCTION(Server, Reliable)
    void ServerFire();

    UFUNCTION(Server, Reliable)
    void ServerReload();

    UFUNCTION(Server, Reliable)
    void ServerSpawnDefaultWeapon();

    virtual float TakeDamage(float Damage, FDamageEvent const& DamageEvent, AController* EventInstigator, AActor* DamageCauser) override;
};
