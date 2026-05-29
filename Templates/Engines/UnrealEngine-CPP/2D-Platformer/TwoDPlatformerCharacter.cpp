// 2D platformer character with double jump, facing direction, coins/gems, and lives.
#include "TwoDPlatformerCharacter.h"
#include "PaperFlipbookComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"

// Set up sprite, enable tick, configure move speed.
ATwoDPlatformerCharacter::ATwoDPlatformerCharacter()
{
    Sprite = CreateDefaultSubobject<UPaperFlipbookComponent>(TEXT("Sprite"));
    Sprite->SetupAttachment(GetRootComponent());
    PrimaryActorTick.bCanEverTick = true;
    GetCharacterMovement()->MaxWalkSpeed = MoveSpeed;
}

// Store respawn point and register input mapping context.
void ATwoDPlatformerCharacter::BeginPlay()
{
    Super::BeginPlay();
    RespawnLocation = GetActorLocation();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
        if (auto* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
}

// Face movement direction by rotating the sprite.
void ATwoDPlatformerCharacter::Tick(float DT)
{
    Super::Tick(DT);
    FVector Vel = GetVelocity();
    if (FMath::Abs(Vel.X) > 10.0f)
    {
        bIsFacingRight = Vel.X > 0.0f;
        SetActorRotation(FRotator(0, bIsFacingRight ? 0 : 180, 0));
    }
}

// Bind enhanced input actions for horizontal movement and jump.
void ATwoDPlatformerCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (auto* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(MoveAction, ETriggerEvent::Triggered, this, &ATwoDPlatformerCharacter::MoveHorizontal);
        EIC->BindAction(JumpAction, ETriggerEvent::Started, this, &ATwoDPlatformerCharacter::PlayerJump);
    }
}

// Move horizontally.
void ATwoDPlatformerCharacter::MoveHorizontal(const FInputActionValue& V)
{
    AddMovementInput(FVector::RightVector, V.Get<float>());
}

// Perform a jump (supports double jump).
void ATwoDPlatformerCharacter::PlayerJump()
{
    if (JumpCount < MaxJumps)
    {
        FVector JumpVel = FVector(0, 0, 600.0f);
        LaunchCharacter(JumpVel, false, true);
        JumpCount++;
    }
}

// Reset jump count on landing.
void ATwoDPlatformerCharacter::Landed(const FHitResult& Hit)
{
    Super::Landed(Hit);
    JumpCount = 0;
}

// Add coins collected.
void ATwoDPlatformerCharacter::AddCoin(int32 Amount) { CoinCount += Amount; }
// Add gems collected.
void ATwoDPlatformerCharacter::AddGem(int32 Amount) { GemCount += Amount; }

// Handle death: decrement lives, respawn or destroy.
void ATwoDPlatformerCharacter::Die()
{
    Lives--;
    if (Lives > 0) Respawn();
    else Destroy();
}

// Move character back to respawn location.
void ATwoDPlatformerCharacter::Respawn()
{
    SetActorLocation(RespawnLocation);
    GetCharacterMovement()->SetMovementMode(MOVE_Walking);
}
