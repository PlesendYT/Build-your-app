#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "StrategyUnit.generated.h"

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

    UFUNCTION(BlueprintCallable)
    void IssueCommand(EUnitCommand Command, FVector TargetLocation, AActor* TargetActor = nullptr);

    UFUNCTION(BlueprintCallable)
    void TakeUnitDamage(float Damage);

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
