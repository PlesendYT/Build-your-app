// VR interactable actor - can be grabbed, interacted with, and released.
#include "VRInteractable.h"
#include "VRPawn.h"

// Set up mesh with physics simulation.
AVRInteractable::AVRInteractable()
{
    Mesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Mesh"));
    SetRootComponent(Mesh);
    Mesh->SetSimulatePhysics(true);
}

// Called when the interactable is interacted with (BlueprintNativeEvent).
void AVRInteractable::OnInteract_Implementation(AVRPawn* Interactor)
{
    CurrentInteractor = Interactor;
}

// Called when the interactable is grabbed (BlueprintNativeEvent).
void AVRInteractable::OnGrab_Implementation(AVRPawn* Interactor)
{
    bIsGrabbed = true;
    CurrentInteractor = Interactor;
}

// Called when the interactable is released (BlueprintNativeEvent).
void AVRInteractable::OnRelease_Implementation(AVRPawn* Interactor)
{
    bIsGrabbed = false;
    CurrentInteractor = nullptr;
}
