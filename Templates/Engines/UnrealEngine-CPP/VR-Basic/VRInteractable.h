// VR interactable actor header - supports interaction, grab, and release.
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

    // Called when the player interacts with this object.
    UFUNCTION(BlueprintNativeEvent)
    void OnInteract(AVRPawn* Interactor);

    // Called when the player grabs this object.
    UFUNCTION(BlueprintNativeEvent)
    void OnGrab(AVRPawn* Interactor);

    // Called when the player releases this object.
    UFUNCTION(BlueprintNativeEvent)
    void OnRelease(AVRPawn* Interactor);

    UPROPERTY(BlueprintReadOnly) bool bIsGrabbed = false;
    UPROPERTY(BlueprintReadOnly) AVRPawn* CurrentInteractor = nullptr;

protected:
    UPROPERTY(VisibleAnywhere) UStaticMeshComponent* Mesh;
    UPROPERTY(EditAnywhere) bool bCanGrab = true;
};
