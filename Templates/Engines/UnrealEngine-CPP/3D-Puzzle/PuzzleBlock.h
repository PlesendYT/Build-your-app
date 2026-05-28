#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "PuzzleBlock.generated.h"

class UBoxComponent;

DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnBlockActivated, bool, bActivated);

UCLASS()
class APuzzleBlock : public AActor
{
    GENERATED_BODY()
public:
    APuzzleBlock();

    UPROPERTY(BlueprintAssignable) FOnBlockActivated OnActivated;
    UPROPERTY(BlueprintReadOnly) bool bActivated = false;

    UFUNCTION(BlueprintCallable)
    void Activate(bool bNewState);

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
