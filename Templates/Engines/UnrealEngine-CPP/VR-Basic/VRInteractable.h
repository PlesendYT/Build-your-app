#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "VRInteractable.generated.h"

class AVRPawn;

UCLASS()
class AVRInteractable : public AActor
{
    GENERATED_BODY()
public:
    AVRInteractable();

    UFUNCTION(BlueprintNativeEvent)
    void OnInteract(AVRPawn* Interactor);

    UFUNCTION(BlueprintNativeEvent)
    void OnGrab(AVRPawn* Interactor);

    UFUNCTION(BlueprintNativeEvent)
    void OnRelease(AVRPawn* Interactor);

    UPROPERTY(BlueprintReadOnly) bool bIsGrabbed = false;
    UPROPERTY(BlueprintReadOnly) AVRPawn* CurrentInteractor = nullptr;

protected:
    UPROPERTY(VisibleAnywhere) UStaticMeshComponent* Mesh;
    UPROPERTY(EditAnywhere) bool bCanGrab = true;
};
