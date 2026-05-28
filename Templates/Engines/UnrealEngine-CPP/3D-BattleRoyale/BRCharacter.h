#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "BRCharacter.generated.h"

USTRUCT(BlueprintType)
struct FLootItem
{
    GENERATED_BODY()
    UPROPERTY(BlueprintReadWrite) FName ItemID;
    UPROPERTY(BlueprintReadWrite) TSubclassOf<AActor> ItemClass;
    UPROPERTY(BlueprintReadWrite) int32 Rarity = 1;
};

UCLASS()
class ABRCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    ABRCharacter();

    UPROPERTY(Replicated, BlueprintReadOnly) int32 Kills = 0;
    UPROPERTY(Replicated, BlueprintReadOnly) int32 DamageDealt = 0;
    UPROPERTY(Replicated, BlueprintReadOnly) bool bIsAlive = true;
    UPROPERTY(Replicated, BlueprintReadOnly) TArray<FLootItem> Inventory;

    UFUNCTION(BlueprintCallable)
    void AddKill();

    UFUNCTION(BlueprintCallable)
    void AddDamage(int32 Amount);

    UFUNCTION(BlueprintCallable)
    void PickupLoot(FLootItem Item);

    UFUNCTION(BlueprintCallable)
    void DeployParachute();

    UFUNCTION(BlueprintCallable)
    bool IsInStorm() const;

protected:
    virtual void BeginPlay() override;
    virtual void Tick(float DT) override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    void Move(const FInputActionValue& V);
    void Look(const FInputActionValue& V);
    void Interact();

    UPROPERTY(EditDefaultsOnly) UInputMappingContext* InputMapping;
    UPROPERTY(EditDefaultsOnly) UInputAction* MoveAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* LookAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* InteractAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* JumpAction;

    UPROPERTY(EditDefaultsOnly) float ParachuteDescentSpeed = 200.0f;
    bool bIsParachuting = false;

    virtual float TakeDamage(float Damage, FDamageEvent const& Event, AController* Instigator, AActor* Causer) override;
};
