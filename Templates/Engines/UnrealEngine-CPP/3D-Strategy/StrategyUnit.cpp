#include "StrategyUnit.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "AI/Navigation/NavigationTypes.h"

AStrategyUnit::AStrategyUnit()
{
    PrimaryActorTick.bCanEverTick = true;
    CurrentCommand = EUnitCommand::Hold;
}

void AStrategyUnit::BeginPlay() { Super::BeginPlay(); }

void AStrategyUnit::Tick(float DT)
{
    Super::Tick(DT);
    switch (CurrentCommand)
    {
        case EUnitCommand::Move: PerformMove(); break;
        case EUnitCommand::Attack: PerformAttack(DT); break;
        case EUnitCommand::Gather: PerformGather(DT); break;
        default: break;
    }
}

void AStrategyUnit::IssueCommand(EUnitCommand Command, FVector TargetLocation, AActor* TargetActor)
{
    CurrentCommand = Command;
    switch (Command)
    {
        case EUnitCommand::Move:
            MoveTarget = TargetLocation;
            PerformMove();
            break;
        case EUnitCommand::Attack:
            AttackTarget = TargetActor;
            break;
        case EUnitCommand::Gather:
            GatherTarget = TargetActor;
            break;
        default: break;
    }
}

void AStrategyUnit::PerformMove()
{
    GetCharacterMovement()->MaxWalkSpeed = 300.0f;
    // AIController would handle MoveToLocation
}

void AStrategyUnit::PerformAttack(float DT)
{
    if (AttackTarget && AttackTarget->IsValidLowLevel())
    {
        FVector ToTarget = AttackTarget->GetActorLocation() - GetActorLocation();
        float Dist = ToTarget.Size();
        if (Dist <= AttackRange)
        {
            // Attack logic
        }
        else
        {
            MoveTarget = AttackTarget->GetActorLocation();
            PerformMove();
        }
    }
}

void AStrategyUnit::PerformGather(float DT)
{
    GatherTimer += DT;
    if (GatherTimer >= 1.0f && GatherTarget)
    {
        GatherTimer = 0.0f;
        // Gather resource logic
    }
}

void AStrategyUnit::TakeUnitDamage(float Damage)
{
    Health -= Damage;
    if (Health <= 0) Destroy();
}

void AStrategyUnit::SetSelected(bool Selected) { bIsSelected = Selected; }
