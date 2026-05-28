#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "TopDownProjectile.generated.h"

class USphereComponent;
class UProjectileMovementComponent;

UCLASS()
class ATopDownProjectile : public AActor
{
    GENERATED_BODY()
public:
    ATopDownProjectile();

    UPROPERTY(EditDefaultsOnly) float Damage = 20.0f;
    UPROPERTY(EditDefaultsOnly) float Speed = 800.0f;

protected:
    UPROPERTY(VisibleAnywhere) USphereComponent* Collision;
    UPROPERTY(VisibleAnywhere) UProjectileMovementComponent* Movement;
    UPROPERTY(VisibleAnywhere) UPaperFlipbookComponent* Sprite;

    virtual void BeginPlay() override;
    UFUNCTION()
    void OnHit(UPrimitiveComponent* HitComp, AActor* Other, UPrimitiveComponent* OtherComp, FVector NormalImpulse, const FHitResult& Hit);
};
