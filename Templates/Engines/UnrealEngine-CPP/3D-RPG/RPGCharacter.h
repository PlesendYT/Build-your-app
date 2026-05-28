#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "RPGCharacter.generated.h"

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

USTRUCT(BlueprintType)
struct FQuestData
{
    GENERATED_BODY()
    UPROPERTY(EditAnywhere, BlueprintReadWrite) FName QuestID;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) FText QuestName;
    UPROPERTY(EditAnywhere, BlueprintReadWrite) bool bIsCompleted = false;
};

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

    UFUNCTION(BlueprintCallable)
    void AddExperience(int32 Amount);

    UFUNCTION(BlueprintCallable)
    void AddItem(FName ItemID, int32 Quantity = 1);

    UFUNCTION(BlueprintCallable)
    bool RemoveItem(FName ItemID, int32 Quantity = 1);

    UFUNCTION(BlueprintCallable)
    bool HasItem(FName ItemID, int32 Quantity = 1) const;

    UFUNCTION(BlueprintCallable)
    void AcceptQuest(FName QuestID);

    UFUNCTION(BlueprintCallable)
    void CompleteQuest(FName QuestID);

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
