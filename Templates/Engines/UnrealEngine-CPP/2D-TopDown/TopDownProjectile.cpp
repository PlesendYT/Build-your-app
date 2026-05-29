// 2D projectile that moves forward and damages characters on hit.
#include "TopDownProjectile.h"
#include "Components/SphereComponent.h"
#include "GameFramework/ProjectileMovementComponent.h"
#include "PaperFlipbookComponent.h"
#include "GameFramework/Character.h"

// Set up collision sphere, sprite, and projectile movement.
ATopDownProjectile::ATopDownProjectile()
{
    Collision = CreateDefaultSubobject<USphereComponent>(TEXT("Collision"));
    SetRootComponent(Collision);
    Collision->SetSphereRadius(10.0f);
    Collision->SetCollisionEnabled(ECollisionEnabled::QueryAndPhysics);
    Collision->OnComponentHit.AddDynamic(this, &ATopDownProjectile::OnHit);

    Sprite = CreateDefaultSubobject<UPaperFlipbookComponent>(TEXT("Sprite"));
    Sprite->SetupAttachment(Collision);

    Movement = CreateDefaultSubobject<UProjectileMovementComponent>(TEXT("Movement"));
    Movement->InitialSpeed = Speed;
    Movement->MaxSpeed = Speed;
    Movement->bShouldBounce = false;
}

void ATopDownProjectile::BeginPlay() { Super::BeginPlay(); }

void ATopDownProjectile::OnHit(UPrimitiveComponent* HitComp, AActor* Other, UPrimitiveComponent* OtherComp, FVector NormalImpulse, const FHitResult& Hit)
{
    if (Other && Other != GetOwner())
    {
        if (ACharacter* Char = Cast<ACharacter>(Other))
        {
            // Apply damage
        }
        Destroy();
    }
}
