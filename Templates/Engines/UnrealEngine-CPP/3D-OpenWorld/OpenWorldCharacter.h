// Header for an open world character with quests, discovery, and save/load.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "OpenWorldCharacter.generated.h"

// A quest that can be active or completed.
USTRUCT(BlueprintType)
struct FWorldQuest
{
    GENERATED_BODY()
    UPROPERTY(BlueprintReadWrite) FName QuestID;
    UPROPERTY(BlueprintReadWrite) FText QuestName;
    UPROPERTY(BlueprintReadWrite) FText Description;
    UPROPERTY(BlueprintReadWrite) FVector TargetLocation;
    UPROPERTY(BlueprintReadWrite) bool bIsActive = false;
    UPROPERTY(BlueprintReadWrite) bool bIsCompleted = false;
};

UCLASS()
class AOpenWorldCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    AOpenWorldCharacter();

    UPROPERTY(Replicated, BlueprintReadWrite) TArray<FWorldQuest> Quests;
    UPROPERTY(Replicated, BlueprintReadWrite) TArray<FVector2D> DiscoveredLocations;
    UPROPERTY(BlueprintReadOnly) float GameTimeHours = 12.0f;

    // Add a new quest to the character's quest log.
    UFUNCTION(BlueprintCallable)
    void AddQuest(FWorldQuest Quest);

    // Mark a quest as completed by its ID.
    UFUNCTION(BlueprintCallable)
    void CompleteQuest(FName QuestID);

    // Add a discovered location to the map.
    UFUNCTION(BlueprintCallable)
    void DiscoverLocation(FVector2D Location);

    // Save the character's data to a save slot.
    UFUNCTION(BlueprintCallable)
    void SaveGame(FString SlotName);

    // Load the character's data from a save slot.
    UFUNCTION(BlueprintCallable)
    void LoadGame(FString SlotName);

protected:
    virtual void BeginPlay() override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    void Move(const FInputActionValue& V);
    void Look(const FInputActionValue& V);
    void Interact();
    void OpenMap();

    UPROPERTY(EditDefaultsOnly) UInputMappingContext* InputMapping;
    UPROPERTY(EditDefaultsOnly) UInputAction* MoveAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* LookAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* InteractAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* MapAction;
};
