#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "TPSCharacter.generated.h"

class UCameraComponent;
class USpringArmComponent;
class ATPSWeapon;
class UInputMappingContext;
class UInputAction;
struct FInputActionValue;

UCLASS()
class ATPSCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    ATPSCharacter();

protected:
    virtual void BeginPlay() override;
    virtual void SetupPlayerInputComponent(UInputComponent* InputComponent) override;

    void Move(const FInputActionValue& Value);
    void Look(const FInputActionValue& Value);
    void StartJump();
    void StartFire();
    void StopFire();
    void Reload();
    void StartAim();
    void StopAim();
    void StartCover();
    void StopCover();
    void SwitchWeapon(const FInputActionValue& Value);

    UPROPERTY(VisibleAnywhere)
    UCameraComponent* Camera;

    UPROPERTY(VisibleAnywhere)
    USpringArmComponent* SpringArm;

    UPROPERTY(VisibleDefaultsOnly)
    USkeletalMeshComponent* GunMesh;

    UPROPERTY(EditDefaultsOnly)
    float MaxHealth = 100.0f;

    UPROPERTY(Replicated)
    float CurrentHealth;

    UPROPERTY(Replicated)
    ATPSWeapon* CurrentWeapon;

    UPROPERTY(EditDefaultsOnly)
    TArray<TSubclassOf<ATPSWeapon>> WeaponClasses;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputMappingContext* InputMapping;

    UPROPERTY(EditDefaultsOnly)
    UInputAction* MoveAction;
    UPROPERTY(EditDefaultsOnly)
    UInputAction* LookAction;
    UPROPERTY(EditDefaultsOnly)
    UInputAction* JumpAction;
    UPROPERTY(EditDefaultsOnly)
    UInputAction* FireAction;
    UPROPERTY(EditDefaultsOnly)
    UInputAction* ReloadAction;
    UPROPERTY(EditDefaultsOnly)
    UInputAction* AimAction;
    UPROPERTY(EditDefaultsOnly)
    UInputAction* CoverAction;
    UPROPERTY(EditDefaultsOnly)
    UInputAction* SwitchWeaponAction;

    float DefaultFOV;
    bool bIsAiming;
    bool bIsInCover;

    UFUNCTION(Server, Reliable)
    void ServerSpawnWeapons();

    virtual float TakeDamage(float Damage, FDamageEvent const& Event, AController* Instigator, AActor* Causer) override;
};
