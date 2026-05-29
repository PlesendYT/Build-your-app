// Simulation actor with state machine, value interpolation, and data logging.
#include "SimulationActor.h"

// Enable tick for simulation updates.
ASimulationActor::ASimulationActor()
{
    PrimaryActorTick.bCanEverTick = true;
}

void ASimulationActor::BeginPlay() { Super::BeginPlay(); }

// Interpolate simulation value toward target when Active.
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

// Transition to a new state and broadcast the change.
void ASimulationActor::SetState(ESimState NewState)
{
    CurrentState = NewState;
    OnStateChanged.Broadcast(NewState);
}

// Reset simulation to initial state.
void ASimulationActor::ResetSimulation()
{
    SimulationValue = 0.0f;
    SetState(ESimState::Idle);
}

// Append a timestamped data log entry.
void ASimulationActor::LogData(const FString& DataPoint)
{
    DataLog.Add(FString::Printf(TEXT("[%f] %s"), GetWorld()->GetTimeSeconds(), *DataPoint));
}
