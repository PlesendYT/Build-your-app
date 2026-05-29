// Header for a puzzle block with activation, chaining, and pressure plate detection.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "PuzzleBlock.generated.h"

class UBoxComponent;

// Delegate broadcast when the block's activation state changes.
DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnBlockActivated, bool, bActivated);

UCLASS()
class APuzzleBlock : public AActor
{
    GENERATED_BODY()
public:
    APuzzleBlock();

    UPROPERTY(BlueprintAssignable) FOnBlockActivated OnActivated;
    UPROPERTY(BlueprintReadOnly) bool bActivated = false;

    // Activate or deactivate this block and its connected blocks.
    UFUNCTION(BlueprintCallable)
    void Activate(bool bNewState);

    // Toggle the current activation state.
    UFUNCTION(BlueprintCallable)
    void Toggle();

    UPROPERTY(EditAnywhere) TArray<APuzzleBlock*> ConnectedBlocks;
    UPROPERTY(EditAnywhere) bool bIsPressurePlate = false;
    UPROPERTY(EditAnywhere) bool bInvertOutput = false;

protected:
    UPROPERTY(VisibleAnywhere) UBoxComponent* CollisionBox;
    UPROPERTY(VisibleAnywhere) UStaticMeshComponent* Mesh;

    virtual void BeginPlay() override;
    virtual void NotifyActorBeginOverlap(AActor* Other) override;
    virtual void NotifyActorEndOverlap(AActor* Other) override;
};
