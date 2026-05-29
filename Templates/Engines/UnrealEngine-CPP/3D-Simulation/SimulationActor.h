// Header for a simulation actor with state machine and data logging.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "SimulationActor.generated.h"

// States for the simulation state machine.
UENUM()
enum class ESimState : uint8 { Idle, Active, Processing, Complete, Error };

// Delegate broadcast when simulation state changes.
DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnStateChanged, ESimState, NewState);

UCLASS()
class ASimulationActor : public AActor
{
    GENERATED_BODY()
public:
    ASimulationActor();

    UPROPERTY(BlueprintAssignable) FOnStateChanged OnStateChanged;
    UPROPERTY(BlueprintReadOnly) ESimState CurrentState = ESimState::Idle;
    UPROPERTY(BlueprintReadWrite) float SimulationValue = 0.0f;
    UPROPERTY(BlueprintReadWrite) float TargetValue = 100.0f;
    UPROPERTY(BlueprintReadWrite) float Speed = 1.0f;

    // Transition to a new state.
    UFUNCTION(BlueprintCallable)
    void SetState(ESimState NewState);

    // Reset simulation value and state to Idle.
    UFUNCTION(BlueprintCallable)
    void ResetSimulation();

    // Append a timestamped data point to the log.
    UFUNCTION(BlueprintCallable)
    void LogData(const FString& DataPoint);

protected:
    virtual void BeginPlay() override;
    virtual void Tick(float DT) override;

    UPROPERTY(EditDefaultsOnly) float ChangeRate = 10.0f;
    TArray<FString> DataLog;
};
