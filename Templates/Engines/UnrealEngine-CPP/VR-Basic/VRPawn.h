// VR Pawn header - VR character with motion controllers and interaction.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Pawn.h"
#include "VRPawn.generated.h"

class UCameraComponent;
class UMotionControllerComponent;
class UVRInteractable;

UCLASS()
class AVRPawn : public APawn
{
    GENERATED_BODY()
public:
    AVRPawn();

    // Teleport the VR pawn to a world location.
    UFUNCTION(BlueprintCallable)
    void TeleportToLocation(FVector Location);

    // Grab an actor and attach it to a motion controller.
    UFUNCTION(BlueprintCallable)
    void GrabActor(UMotionControllerComponent* Controller, AActor* Target);

    // Release the actor held by the given controller.
    UFUNCTION(BlueprintCallable)
    void ReleaseActor(UMotionControllerComponent* Controller);

    // Interact with objects via line trace from controller.
    UFUNCTION(BlueprintCallable)
    void Interact(UMotionControllerComponent* Controller);

protected:
    virtual void BeginPlay() override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    UPROPERTY(VisibleAnywhere) UCameraComponent* VRCamera;
    UPROPERTY(VisibleAnywhere) UMotionControllerComponent* LeftController;
    UPROPERTY(VisibleAnywhere) UMotionControllerComponent* RightController;
    UPROPERTY(VisibleAnywhere) USceneComponent* VROrigin;

    UPROPERTY(EditDefaultsOnly) float TeleportRange = 1000.0f;
    UPROPERTY(EditDefaultsOnly) UInputAction* TeleportAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* GrabLeftAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* GrabRightAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* InteractAction;

    TMap<UMotionControllerComponent*, AActor*> GrabbedActors;

    void OnTeleport(const FInputActionValue& V);
    void OnGrabLeft(const FInputActionValue& V);
    void OnGrabRight(const FInputActionValue& V);
    void OnInteract(const FInputActionValue& V);

    UFUNCTION(Server, Reliable)
    void ServerTeleport(FVector Location);

    UFUNCTION(Server, Reliable)
    void ServerGrab(UMotionControllerComponent* Controller, AActor* Target);

    UFUNCTION(Server, Reliable)
    void ServerRelease(UMotionControllerComponent* Controller);
};
