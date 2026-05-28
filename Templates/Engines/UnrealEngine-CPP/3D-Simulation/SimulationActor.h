#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "SimulationActor.generated.h"

UENUM()
enum class ESimState : uint8 { Idle, Active, Processing, Complete, Error };

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

    UFUNCTION(BlueprintCallable)
    void SetState(ESimState NewState);

    UFUNCTION(BlueprintCallable)
    void ResetSimulation();

    UFUNCTION(BlueprintCallable)
    void LogData(const FString& DataPoint);

protected:
    virtual void BeginPlay() override;
    virtual void Tick(float DT) override;

    UPROPERTY(EditDefaultsOnly) float ChangeRate = 10.0f;
    TArray<FString> DataLog;
};
