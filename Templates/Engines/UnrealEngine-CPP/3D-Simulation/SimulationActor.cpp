#include "SimulationActor.h"

ASimulationActor::ASimulationActor()
{
    PrimaryActorTick.bCanEverTick = true;
}

void ASimulationActor::BeginPlay() { Super::BeginPlay(); }

void ASimulationActor::Tick(float DT)
{
    Super::Tick(DT);
    if (CurrentState == ESimState::Active)
    {
        SimulationValue = FMath::FInterpTo(SimulationValue, TargetValue, DT, ChangeRate * Speed);
        if (FMath::IsNearlyEqual(SimulationValue, TargetValue, 0.1f))
        {
            SimulationValue = TargetValue;
            SetState(ESimState::Complete);
        }
    }
}

void ASimulationActor::SetState(ESimState NewState)
{
    CurrentState = NewState;
    OnStateChanged.Broadcast(NewState);
}

void ASimulationActor::ResetSimulation()
{
    SimulationValue = 0.0f;
    SetState(ESimState::Idle);
}

void ASimulationActor::LogData(const FString& DataPoint)
{
    DataLog.Add(FString::Printf(TEXT("[%f] %s"), GetWorld()->GetTimeSeconds(), *DataPoint));
}
