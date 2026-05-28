#include "VRInteractable.h"
#include "VRPawn.h"

AVRInteractable::AVRInteractable()
{
    Mesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Mesh"));
    SetRootComponent(Mesh);
    Mesh->SetSimulatePhysics(true);
}

void AVRInteractable::OnInteract_Implementation(AVRPawn* Interactor)
{
    CurrentInteractor = Interactor;
}

void AVRInteractable::OnGrab_Implementation(AVRPawn* Interactor)
{
    bIsGrabbed = true;
    CurrentInteractor = Interactor;
}

void AVRInteractable::OnRelease_Implementation(AVRPawn* Interactor)
{
    bIsGrabbed = false;
    CurrentInteractor = nullptr;
}
