#include "StealthCharacter.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Kismet/GameplayStatics.h"

AStealthCharacter::AStealthCharacter() {}

void AStealthCharacter::BeginPlay()
{
    Super::BeginPlay();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
        if (auto* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
}

void AStealthCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (auto* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(MoveAction, ETriggerEvent::Triggered, this, &AStealthCharacter::Move);
        EIC->BindAction(LookAction, ETriggerEvent::Triggered, this, &AStealthCharacter::Look);
        EIC->BindAction(CrouchAction, ETriggerEvent::Started, this, &AStealthCharacter::ToggleCrouch);
        EIC->BindAction(InteractAction, ETriggerEvent::Started, this, &AStealthCharacter::Interact);
        EIC->BindAction(DistractAction, ETriggerEvent::Started, this, &AStealthCharacter::DistractEnemy);
    }
}

void AStealthCharacter::Move(const FInputActionValue& V)
{
    FVector2D Vec = V.Get<FVector2D>();
    AddMovementInput(GetActorForwardVector(), Vec.Y);
    AddMovementInput(GetActorRightVector(), Vec.X);
}

void AStealthCharacter::Look(const FInputActionValue& V)
{
    FVector2D Vec = V.Get<FVector2D>();
    AddControllerYawInput(Vec.X);
    AddControllerPitchInput(Vec.Y);
}

void AStealthCharacter::MakeNoise(float Loudness)
{
    NoiseLevel = Loudness;
    if (Loudness > 0.1f)
    {
        UGameplayStatics::PlaySoundAtLocation(this, nullptr, GetActorLocation(), Loudness);
    }
}

void AStealthCharacter::ToggleCrouch()
{
    bIsCrouching = !bIsCrouching;
    if (bIsCrouching) GetCharacterMovement()->MaxWalkSpeed = 150.0f;
    else GetCharacterMovement()->MaxWalkSpeed = 400.0f;
}

void AStealthCharacter::DistractEnemy(FVector Location)
{
    MakeNoise(0.8f);
}

void AStealthCharacter::Interact() {}
