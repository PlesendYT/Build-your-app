// 2D top-down character with movement, mouse aiming, and projectile shooting.
#include "TopDownCharacter.h"
#include "TopDownProjectile.h"
#include "PaperFlipbookComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Kismet/GameplayStatics.h"
#include "Engine/World.h"

// Set up sprite and movement speed.
ATopDownCharacter::ATopDownCharacter()
{
    Sprite = CreateDefaultSubobject<UPaperFlipbookComponent>(TEXT("Sprite"));
    Sprite->SetupAttachment(GetRootComponent());
    GetCharacterMovement()->MaxWalkSpeed = MoveSpeed;
}

void ATopDownCharacter::BeginPlay()
{
    Super::BeginPlay();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
        if (auto* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
}

void ATopDownCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (auto* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(MoveAction, ETriggerEvent::Triggered, this, &ATopDownCharacter::Move);
        EIC->BindAction(ShootAction, ETriggerEvent::Started, this, &ATopDownCharacter::ShootProjectile);
    }
}

void ATopDownCharacter::Move(const FInputActionValue& V)
{
    FVector2D Vec = V.Get<FVector2D>();
    AddMovementInput(FVector::ForwardVector, Vec.Y);
    AddMovementInput(FVector::RightVector, Vec.X);
}

void ATopDownCharacter::LookAtMouse()
{
    APlayerController* PC = Cast<APlayerController>(GetController());
    if (!PC) return;
    FHitResult Hit;
    PC->GetHitResultUnderCursor(ECC_Visibility, false, Hit);
    if (Hit.bBlockingHit)
    {
        FVector Direction = Hit.Location - GetActorLocation();
        Direction.Z = 0;
        SetActorRotation(Direction.Rotation());
    }
}

void ATopDownCharacter::ShootProjectile()
{
    if (!ProjectileClass || !HasAuthority()) return;
    LookAtMouse();
    FActorSpawnParameters P;
    P.Owner = this;
    P.Instigator = this;
    FVector SpawnLoc = GetActorLocation() + GetActorForwardVector() * 50.0f;
    GetWorld()->SpawnActor<ATopDownProjectile>(ProjectileClass, SpawnLoc, GetActorRotation(), P);
}

void ATopDownCharacter::AddGold(int32 Amount) { Gold += Amount; }
void ATopDownCharacter::AddKey(int32 Amount) { Keys += Amount; }
void ATopDownCharacter::TakeDamage(float Amount) { Health = FMath::Max(0.0f, Health - Amount); }
