// RTS strategy unit with move, attack, gather, and damage commands.
#include "StrategyUnit.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "AI/Navigation/NavigationTypes.h"

// Initialize unit with Hold command.
AStrategyUnit::AStrategyUnit()
{
    PrimaryActorTick.bCanEverTick = true;
    CurrentCommand = EUnitCommand::Hold;
}

void AStrategyUnit::BeginPlay() { Super::BeginPlay(); }

// Execute the current command each tick.
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

// Issue a command to the unit (move, attack, gather, etc.).
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

// Set movement speed and begin moving toward the target.
void AStrategyUnit::PerformMove()
{
    GetCharacterMovement()->MaxWalkSpeed = 300.0f;
    // AIController would handle MoveToLocation
}

// Attack the target if in range; otherwise move toward it.
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

// Gather resources from the target at the defined rate.
void AStrategyUnit::PerformGather(float DT)
{
    GatherTimer += DT;
    if (GatherTimer >= 1.0f && GatherTarget)
    {
        GatherTimer = 0.0f;
        // Gather resource logic
    }
}

// Apply damage to the unit; destroy if health reaches zero.
void AStrategyUnit::TakeUnitDamage(float Damage)
{
    Health -= Damage;
    if (Health <= 0) Destroy();
}

// Update the unit's selection state.
void AStrategyUnit::SetSelected(bool Selected) { bIsSelected = Selected; }
