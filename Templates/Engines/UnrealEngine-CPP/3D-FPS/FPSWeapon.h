// FPSWeapon.h
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

    virtual void Fire();
    virtual void Reload();
    virtual void StartFire();
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

    virtual void FireHitScan();
    virtual void FireProjectile();
    virtual void OnReloadFinished();
};
