// FPSWeapon.cpp
#include "FPSWeapon.h"
#include "Engine/World.h"
#include "Engine/HitResult.h"
#include "CollisionQueryParams.h"
#include "DrawDebugHelpers.h"
#include "Kismet/GameplayStatics.h"
#include "Particles/ParticleSystemComponent.h"
#include "TimerManager.h"

AFPSWeapon::AFPSWeapon()
{
    PrimaryActorTick.bCanEverTick = false;
    SetReplicates(true);

    WeaponMesh = CreateDefaultSubobject<USkeletalMeshComponent>(TEXT("WeaponMesh"));
    SetRootComponent(WeaponMesh);
    WeaponMesh->SetCollisionEnabled(ECollisionEnabled::NoCollision);

    CurrentAmmo = MaxAmmo;
    bIsReloading = false;
}

void AFPSWeapon::StartFire()
{
    if (bIsReloading || CurrentAmmo <= 0) return;
    Fire();
    GetWorldTimerManager().SetTimer(FireTimerHandle, this, &AFPSWeapon::Fire, FireRate, true);
}

void AFPSWeapon::StopFire()
{
    GetWorldTimerManager().ClearTimer(FireTimerHandle);
}

void AFPSWeapon::Fire()
{
    if (CurrentAmmo <= 0 || bIsReloading) return;
    CurrentAmmo--;

    if (MuzzleFlash)
    {
        UGameplayStatics::SpawnEmitterAttached(MuzzleFlash, WeaponMesh, TEXT("MuzzleSocket"));
    }
    if (FireSound)
    {
        UGameplayStatics::SpawnSoundAttached(FireSound, WeaponMesh, TEXT("MuzzleSocket"));
    }

    if (HasAuthority())
    {
        if (ProjectileClass)
        {
            FireProjectile();
        }
        else
        {
            FireHitScan();
        }
    }

    OnFire.Broadcast();
}

void AFPSWeapon::FireHitScan()
{
    APlayerCameraManager* CameraManager = UGameplayStatics::GetPlayerCameraManager(this, 0);
    if (!CameraManager) return;

    FVector Start = CameraManager->GetCameraLocation();
    FVector End = Start + CameraManager->GetCameraRotation().Vector() * WeaponRange;

    FHitResult Hit;
    FCollisionQueryParams Params;
    Params.AddIgnoredActor(this);
    Params.AddIgnoredActor(GetOwner());

    if (GetWorld()->LineTraceSingleByChannel(Hit, Start, End, ECC_Visibility, Params))
    {
        UGameplayStatics::ApplyDamage(Hit.GetActor(), WeaponDamage, GetOwner()->GetInstigatorController(), this, DamageType);
        if (UParticleSystem* ImpactEffect = GetImpactEffect(Hit))
        {
            UGameplayStatics::SpawnEmitterAtLocation(this, ImpactEffect, Hit.Location, Hit.Normal.Rotation());
        }
    }
}

void AFPSWeapon::FireProjectile()
{
    if (!ProjectileClass) return;
    FVector MuzzleLocation = WeaponMesh->GetSocketLocation(TEXT("MuzzleSocket"));
    FRotator MuzzleRotation = WeaponMesh->GetSocketRotation(TEXT("MuzzleSocket"));
    FActorSpawnParameters SpawnParams;
    SpawnParams.Owner = this;
    SpawnParams.Instigator = GetInstigator();
    GetWorld()->SpawnActor<AActor>(ProjectileClass, MuzzleLocation, MuzzleRotation, SpawnParams);
}

void AFPSWeapon::Reload()
{
    if (bIsReloading || CurrentAmmo == MaxAmmo) return;
    bIsReloading = true;
    if (ReloadSound)
    {
        UGameplayStatics::SpawnSoundAttached(ReloadSound, WeaponMesh);
    }
    GetWorldTimerManager().SetTimer(ReloadTimerHandle, this, &AFPSWeapon::OnReloadFinished, 2.0f, false);
}

void AFPSWeapon::OnReloadFinished()
{
    CurrentAmmo = MaxAmmo;
    bIsReloading = false;
}
