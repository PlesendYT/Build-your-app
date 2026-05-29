// Header for an RPG character with stats, inventory, quests, and leveling.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "RPGCharacter.generated.h"

// Core character stats (level, experience, attributes).
USTRUCT(BlueprintType)
struct FCharacterStats
{
    GENERATED_BODY()
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 Level = 1;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 Experience = 0;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 ExpToNext = 100;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 Strength = 10;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 Dexterity = 10;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 Intelligence = 10;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 Constitution = 10;
};

// A quest entry with completion status.
USTRUCT(BlueprintType)
struct FQuestData
{
    GENERATED_BODY()
    UPROPERTY(EditAnywhere, BlueprintReadWrite) FName QuestID;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) FText QuestName;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) bool bIsCompleted = false;
};

// An inventory item with stack support.
USTRUCT(BlueprintType)
struct FInventoryItem
{
    GENERATED_BODY()
    UPROPERTY(EditAnywhere, BlueprintReadWrite) FName ItemID;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 Quantity = 1;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) int32 MaxStack = 99;
};

UCLASS()
class ARPGCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    ARPGCharacter();

    UPROPERTY(Replicated, BlueprintReadWrite) FCharacterStats Stats;
    UPROPERTY(Replicated, BlueprintReadWrite) TArray<FQuestData> ActiveQuests;
    UPROPERTY(Replicated, BlueprintReadWrite) TArray<FInventoryItem> Inventory;

    // Add experience and check for level-up.
    UFUNCTION(BlueprintCallable)
    void AddExperience(int32 Amount);

    // Add an item to inventory (with stacking).
    UFUNCTION(BlueprintCallable)
    void AddItem(FName ItemID, int32 Quantity = 1);

    // Remove items from inventory. Returns false if insufficient quantity.
    UFUNCTION(BlueprintCallable)
    bool RemoveItem(FName ItemID, int32 Quantity = 1);

    // Check if the character has at least Quantity of an item.
    UFUNCTION(BlueprintCallable)
    bool HasItem(FName ItemID, int32 Quantity = 1) const;

    // Accept a new quest.
    UFUNCTION(BlueprintCallable)
    void AcceptQuest(FName QuestID);

    // Complete a quest and grant experience reward.
    UFUNCTION(BlueprintCallable)
    void CompleteQuest(FName QuestID);

    // Get a stat bonus based on stat type.
    UFUNCTION(BlueprintCallable)
    int32 GetStatBonus(EStatType StatType) const;

    UPROPERTY(EditDefaultsOnly)
    UDataTable* ItemDataTable;

    UPROPERTY(EditDefaultsOnly)
    UDataTable* QuestDataTable;

protected:
    virtual void BeginPlay() override;
    virtual void SetupPlayerInputComponent(UInputComponent* PIC) override;

    UFUNCTION(Server, Reliable)
    void ServerAddExp(int32 Amount);

    UFUNCTION(Server, Reliable)
    void ServerLevelUp();

    virtual float TakeDamage(float Damage, FDamageEvent const& Event, AController* Instigator, AActor* Causer) override;
};
