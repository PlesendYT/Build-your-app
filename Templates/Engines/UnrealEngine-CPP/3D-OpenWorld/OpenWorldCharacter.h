#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "OpenWorldCharacter.generated.h"

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

    UFUNCTION(BlueprintCallable)
    void AddQuest(FWorldQuest Quest);

    UFUNCTION(BlueprintCallable)
    void CompleteQuest(FName QuestID);

    UFUNCTION(BlueprintCallable)
    void DiscoverLocation(FVector2D Location);

    UFUNCTION(BlueprintCallable)
    void SaveGame(FString SlotName);

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
