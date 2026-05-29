// Sports ball with physics, launching, reset, and hit tracking.
#include "SportsBall.h"
#include "Components/SphereComponent.h"
#include "GameFramework/ProjectileMovementComponent.h"

// Set up sphere collision, mesh, and projectile movement with bouncing.
ASportsBall::ASportsBall()
{
    SphereCollision = CreateDefaultSubobject<USphereComponent>(TEXT("SphereCollision"));
    SetRootComponent(SphereCollision);
    SphereCollision->SetSphereRadius(22.0f);
    SphereCollision->SetSimulatePhysics(true);
    SphereCollision->SetCollisionEnabled(ECollisionEnabled::QueryAndPhysics);

    BallMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("BallMesh"));
    BallMesh->SetupAttachment(SphereCollision);

    MovementComp = CreateDefaultSubobject<UProjectileMovementComponent>(TEXT("MovementComp"));
    MovementComp->InitialSpeed = 0.0f;
    MovementComp->MaxSpeed = 3000.0f;
    MovementComp->bShouldBounce = true;
    MovementComp->Bounciness = 0.6f;

    SetReplicates(true);
}

void ASportsBall::BeginPlay() { Super::BeginPlay(); }

// Launch the ball in the given direction with the specified force.
void ASportsBall::LaunchBall(FVector Direction, float Force)
{
    if (MovementComp)
    {
        MovementComp->Velocity = Direction * Force;
        MovementComp->SetActive(true);
        LastHitTime = GetWorld()->GetTimeSeconds();
    }
}

// Reset the ball to the given location and stop its movement.
void ASportsBall::ResetBall(FVector Location)
{
    SetActorLocation(Location);
    if (MovementComp)
    {
        MovementComp->Velocity = FVector::ZeroVector;
        MovementComp->SetActive(false);
    }
    LastHitter = nullptr;
}

// Track the last actor that hit this ball.
void ASportsBall::NotifyHit(UPrimitiveComponent* MyComp, AActor* Other, UPrimitiveComponent* OtherComp,
    bool bSelfMoved, FVector HitLocation, FVector HitNormal, FVector NormalImpulse, const FHitResult& Hit)
{
    Super::NotifyHit(MyComp, Other, OtherComp, bSelfMoved, HitLocation, HitNormal, NormalImpulse, Hit);
    if (Other && Other != this) LastHitter = Other;
}
