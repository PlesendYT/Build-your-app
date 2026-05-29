// Header for a sports ball with physics, launching, and hit tracking.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "SportsBall.generated.h"

class USphereComponent;
class UProjectileMovementComponent;

UCLASS()
class ASportsBall : public AActor
{
    GENERATED_BODY()
public:
    ASportsBall();

    UPROPERTY(BlueprintReadOnly) AActor* LastHitter = nullptr;
    UPROPERTY(BlueprintReadOnly) float LastHitTime = 0.0f;

    // Launch the ball in a direction with a given force.
    UFUNCTION(BlueprintCallable)
    void LaunchBall(FVector Direction, float Force);

    // Reset the ball to a specific location.
    UFUNCTION(BlueprintCallable)
    void ResetBall(FVector Location);

protected:
    UPROPERTY(VisibleAnywhere) USphereComponent* SphereCollision;
    UPROPERTY(VisibleAnywhere) UStaticMeshComponent* BallMesh;
    UPROPERTY(VisibleAnywhere) UProjectileMovementComponent* MovementComp;

    virtual void BeginPlay() override;
    virtual void NotifyHit(UPrimitiveComponent* MyComp, AActor* Other, UPrimitiveComponent* OtherComp,
        bool bSelfMoved, FVector HitLocation, FVector HitNormal, FVector NormalImpulse, const FHitResult& Hit) override;
};
