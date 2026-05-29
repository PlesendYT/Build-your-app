// Header for an FPS weapon with fire modes, ammo, reload, and effects.
#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "FPSWeapon.generated.h"

class USkeletalMeshComponent;
class UDamageType;
class UParticleSystem;
class USoundBase;

UCLASS(Blueprintable, Abstract)
class AFPSWeapon : public AActor
{
    GENERATED_BODY()

public:
    AFPSWeapon();

    // Fire a single shot.
    virtual void Fire();
    // Start reload sequence.
    virtual void Reload();
    // Begin automatic fire.
    virtual void StartFire();
    // Stop automatic fire.
    virtual void StopFire();

    UFUNCTION(BlueprintPure)
    int32 GetCurrentAmmo() const { return CurrentAmmo; }

    UFUNCTION(BlueprintPure)
    int32 GetMaxAmmo() const { return MaxAmmo; }

    UFUNCTION(BlueprintPure)
    float GetFireRate() const { return FireRate; }

    UFUNCTION(BlueprintPure)
    bool IsReloading() const { return bIsReloading; }

protected:
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
    USkeletalMeshComponent* WeaponMesh;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    int32 MaxAmmo = 30;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    float FireRate = 0.1f;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    float WeaponDamage = 20.0f;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    float WeaponRange = 5000.0f;

    UPROPERTY(EditDefaultsOnly)
    TSubclassOf<UDamageType> DamageType;

    UPROPERTY(EditDefaultsOnly)
    UParticleSystem* MuzzleFlash;

    UPROPERTY(EditDefaultsOnly)
    USoundBase* FireSound;

    UPROPERTY(EditDefaultsOnly)
    USoundBase* ReloadSound;

    UPROPERTY(EditDefaultsOnly)
    TSubclassOf<AActor> ProjectileClass;

    int32 CurrentAmmo;
    bool bIsReloading;
    FTimerHandle FireTimerHandle;
    FTimerHandle ReloadTimerHandle;

    // Perform a hitscan trace from the camera center.
    virtual void FireHitScan();
    // Spawn a projectile actor from the muzzle socket.
    virtual void FireProjectile();
    // Called when reload timer completes - refills ammo.
    virtual void OnReloadFinished();
};
