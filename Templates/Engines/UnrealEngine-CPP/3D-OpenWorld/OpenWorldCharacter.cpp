// Open world character with quests, location discovery, and save/load.
#include "OpenWorldCharacter.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Kismet/GameplayStatics.h"
#include "Net/UnrealNetwork.h"

AOpenWorldCharacter::AOpenWorldCharacter() {}

// Register the enhanced input mapping context at start.
void AOpenWorldCharacter::BeginPlay()
{
    Super::BeginPlay();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
        if (auto* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
}

// Bind enhanced input actions.
void AOpenWorldCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (auto* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(MoveAction, ETriggerEvent::Triggered, this, &AOpenWorldCharacter::Move);
        EIC->BindAction(LookAction, ETriggerEvent::Triggered, this, &AOpenWorldCharacter::Look);
        EIC->BindAction(InteractAction, ETriggerEvent::Started, this, &AOpenWorldCharacter::Interact);
        EIC->BindAction(MapAction, ETriggerEvent::Started, this, &AOpenWorldCharacter::OpenMap);
    }
}

// Handle movement input.
void AOpenWorldCharacter::Move(const FInputActionValue& V)
{
    FVector2D Vec = V.Get<FVector2D>();
    AddMovementInput(GetActorForwardVector(), Vec.Y);
    AddMovementInput(GetActorRightVector(), Vec.X);
}

// Handle mouse look input.
void AOpenWorldCharacter::Look(const FInputActionValue& V)
{
    FVector2D Vec = V.Get<FVector2D>();
    AddControllerYawInput(Vec.X);
    AddControllerPitchInput(Vec.Y);
}

void AOpenWorldCharacter::Interact() {}
void AOpenWorldCharacter::OpenMap() {}

// Add a new quest to the character's quest log.
void AOpenWorldCharacter::AddQuest(FWorldQuest Quest)
{
    Quests.Add(Quest);
}

// Mark a quest as completed.
void AOpenWorldCharacter::CompleteQuest(FName QuestID)
{
    for (auto& Q : Quests)
        if (Q.QuestID == QuestID) Q.bIsCompleted = true;
}

// Add a discovered location to the list.
void AOpenWorldCharacter::DiscoverLocation(FVector2D Location)
{
    DiscoveredLocations.AddUnique(Location);
}

// Save game data to a slot.
void AOpenWorldCharacter::SaveGame(FString SlotName)
{
    if (auto* SG = UGameplayStatics::CreateSaveGameObject(USaveGame::StaticClass()))
    {
        UGameplayStatics::SaveGameToSlot(SG, SlotName, 0);
    }
}

// Load game data from a slot.
void AOpenWorldCharacter::LoadGame(FString SlotName)
{
    if (UGameplayStatics::DoesSaveGameExist(SlotName, 0))
    {
        UGameplayStatics::LoadGameFromSlot(SlotName, 0);
    }
}

void AOpenWorldCharacter::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    DOREPLIFETIME(AOpenWorldCharacter, Quests);
    DOREPLIFETIME(AOpenWorldCharacter, DiscoveredLocations);
}
