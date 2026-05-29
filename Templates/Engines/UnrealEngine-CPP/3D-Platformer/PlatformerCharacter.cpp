// 3D platformer character with double jump, coins, and keys.
#include "PlatformerCharacter.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Net/UnrealNetwork.h"

// Configure jump velocity, air control, and walk speed.
APlatformerCharacter::APlatformerCharacter()
{
    GetCharacterMovement()->JumpZVelocity = JumpVelocity;
    GetCharacterMovement()->AirControl = AirControl;
    GetCharacterMovement()->MaxWalkSpeed = 600.0f;
}

void APlatformerCharacter::BeginPlay()
{
    Super::BeginPlay();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
        if (UEnhancedInputLocalPlayerSubsystem* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
}

void APlatformerCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (UEnhancedInputComponent* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(MoveAction, ETriggerEvent::Triggered, this, &APlatformerCharacter::MoveHorizontal);
        EIC->BindAction(JumpAction, ETriggerEvent::Started, this, &APlatformerCharacter::PlayerJump);
    }
}

void APlatformerCharacter::MoveHorizontal(const FInputActionValue& V)
{
    AddMovementInput(GetActorForwardVector(), V.Get<float>());
}

void APlatformerCharacter::PlayerJump()
{
    if (JumpCount < MaxJumps)
    {
        LaunchCharacter(FVector(0, 0, JumpVelocity), false, true);
        JumpCount++;
    }
}

void APlatformerCharacter::Landed(const FHitResult& Hit)
{
    Super::Landed(Hit);
    JumpCount = 0;
}

void APlatformerCharacter::AddCoin(int32 A) { ServerAddCoin(A); }
void APlatformerCharacter::ServerAddCoin_Implementation(int32 A) { CoinCount += A; }
void APlatformerCharacter::AddKey(int32 A) { KeyCount += A; }
void APlatformerCharacter::ActivateDoubleJump() { MaxJumps = 2; }
