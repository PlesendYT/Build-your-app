#include "BRCharacter.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Net/UnrealNetwork.h"
#include "Components/CapsuleComponent.h"

ABRCharacter::ABRCharacter() { PrimaryActorTick.bCanEverTick = true; }

void ABRCharacter::BeginPlay()
{
    Super::BeginPlay();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
        if (auto* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
}

void ABRCharacter::Tick(float DT)
{
    Super::Tick(DT);
    if (bIsParachuting)
    {
        AddMovementInput(FVector::DownVector, ParachuteDescentSpeed * DT);
    }
    if (IsInStorm() && HasAuthority())
    {
        TakeDamage(2.0f * DT, FDamageEvent(), nullptr, nullptr);
    }
}

void ABRCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (auto* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(MoveAction, ETriggerEvent::Triggered, this, &ABRCharacter::Move);
        EIC->BindAction(LookAction, ETriggerEvent::Triggered, this, &ABRCharacter::Look);
        EIC->BindAction(JumpAction, ETriggerEvent::Started, this, &ABRCharacter::DeployParachute);
        EIC->BindAction(InteractAction, ETriggerEvent::Started, this, &ABRCharacter::Interact);
    }
}

void ABRCharacter::Move(const FInputActionValue& V)
{
    FVector2D Vec = V.Get<FVector2D>();
    AddMovementInput(GetActorForwardVector(), Vec.Y);
    AddMovementInput(GetActorRightVector(), Vec.X);
}

void ABRCharacter::Look(const FInputActionValue& V)
{
    FVector2D Vec = V.Get<FVector2D>();
    AddControllerYawInput(Vec.X);
    AddControllerPitchInput(Vec.Y);
}

void ABRCharacter::DeployParachute()
{
    if (!bIsParachuting)
    {
        bIsParachuting = true;
        GetCharacterMovement()->SetMovementMode(MOVE_Flying);
    }
}

void ABRCharacter::AddKill() { Kills++; }
void ABRCharacter::AddDamage(int32 Amount) { DamageDealt += Amount; }

void ABRCharacter::PickupLoot(FLootItem Item)
{
    if (Inventory.Num() < 6) Inventory.Add(Item);
}

bool ABRCharacter::IsInStorm() const { return false; }

void ABRCharacter::Interact() {}

float ABRCharacter::TakeDamage(float Damage, FDamageEvent const& Event, AController* Instigator, AActor* Causer)
{
    float Actual = Super::TakeDamage(Damage, Event, Instigator, Causer);
    if (HasAuthority() && GetHealth() - Actual <= 0)
    {
        bIsAlive = false;
        // Handle death
    }
    return Actual;
}

void ABRCharacter::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    DOREPLIFETIME(ABRCharacter, Kills);
    DOREPLIFETIME(ABRCharacter, DamageDealt);
    DOREPLIFETIME(ABRCharacter, bIsAlive);
    DOREPLIFETIME(ABRCharacter, Inventory);
}
