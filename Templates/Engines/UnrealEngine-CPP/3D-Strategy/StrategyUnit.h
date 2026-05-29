// Header for an RTS strategy unit with command system.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "StrategyUnit.generated.h"

// Possible commands for a strategy unit.
UENUM()
enum class EUnitCommand : uint8 { Move, Attack, Patrol, Hold, Gather };

UCLASS()
class AStrategyUnit : public ACharacter
{
    GENERATED_BODY()
public:
    AStrategyUnit();

    UPROPERTY(BlueprintReadOnly) int32 TeamID = 1;
    UPROPERTY(BlueprintReadOnly) float Health = 50.0f;
    UPROPERTY(BlueprintReadOnly) float AttackDamage = 10.0f;
    UPROPERTY(BlueprintReadOnly) float AttackRange = 200.0f;
    UPROPERTY(BlueprintReadOnly) float GatherRate = 5.0f;
    UPROPERTY(BlueprintReadOnly) bool bIsSelected = false;

    // Issue a command to the unit (move, attack, gather, etc.).
    UFUNCTION(BlueprintCallable)
    void IssueCommand(EUnitCommand Command, FVector TargetLocation, AActor* TargetActor = nullptr);

    // Apply damage to this unit.
    UFUNCTION(BlueprintCallable)
    void TakeUnitDamage(float Damage);

    // Set the unit's selection state.
    UFUNCTION(BlueprintCallable)
    void SetSelected(bool Selected);

protected:
    virtual void BeginPlay() override;
    virtual void Tick(float DT) override;

    EUnitCommand CurrentCommand;
    FVector MoveTarget;
    AActor* AttackTarget;
    AActor* GatherTarget;
    float GatherTimer = 0.0f;

    void PerformMove();
    void PerformAttack(float DT);
    void PerformGather(float DT);
};
