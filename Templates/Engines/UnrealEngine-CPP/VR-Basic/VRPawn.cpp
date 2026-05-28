#include "VRPawn.h"
#include "Camera/CameraComponent.h"
#include "MotionControllerComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "VRInteractable.h"
#include "Net/UnrealNetwork.h"

AVRPawn::AVRPawn()
{
    VROrigin = CreateDefaultSubobject<USceneComponent>(TEXT("VROrigin"));
    SetRootComponent(VROrigin);

    VRCamera = CreateDefaultSubobject<UCameraComponent>(TEXT("VRCamera"));
    VRCamera->SetupAttachment(VROrigin);

    LeftController = CreateDefaultSubobject<UMotionControllerComponent>(TEXT("LeftController"));
    LeftController->SetupAttachment(VROrigin);
    LeftController->SetTrackingSource(EControllerHand::Left);

    RightController = CreateDefaultSubobject<UMotionControllerComponent>(TEXT("RightController"));
    RightController->SetupAttachment(VROrigin);
    RightController->SetTrackingSource(EControllerHand::Right);
}

void AVRPawn::BeginPlay()
{
    Super::BeginPlay();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
        if (auto* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
}

void AVRPawn::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (auto* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(TeleportAction, ETriggerEvent::Started, this, &AVRPawn::OnTeleport);
        EIC->BindAction(GrabLeftAction, ETriggerEvent::Started, this, &AVRPawn::OnGrabLeft);
        EIC->BindAction(GrabRightAction, ETriggerEvent::Started, this, &AVRPawn::OnGrabRight);
        EIC->BindAction(InteractAction, ETriggerEvent::Started, this, &AVRPawn::OnInteract);
    }
}

void AVRPawn::TeleportToLocation(FVector Location)
{
    ServerTeleport(Location);
}

void AVRPawn::ServerTeleport_Implementation(FVector Location)
{
    SetActorLocation(Location);
}

void AVRPawn::GrabActor(UMotionControllerComponent* Controller, AActor* Target)
{
    if (Target)
    {
        GrabbedActors.Add(Controller, Target);
        Target->AttachToComponent(Controller, FAttachmentTransformRules::SnapToTargetNotIncludingScale);
    }
}

void AVRPawn::ReleaseActor(UMotionControllerComponent* Controller)
{
    if (AActor* Grabbed = GrabbedActors.FindRef(Controller))
    {
        Grabbed->DetachFromActor(FDetachmentTransformRules::KeepWorldTransform);
        GrabbedActors.Remove(Controller);
    }
}

void AVRPawn::Interact(UMotionControllerComponent* Controller)
{
    FVector Start = Controller->GetComponentLocation();
    FVector End = Start + Controller->GetForwardVector() * 200.0f;
    FHitResult Hit;
    if (GetWorld()->LineTraceSingleByChannel(Hit, Start, End, ECC_Visibility))
    {
        if (auto* Interactable = Cast<AVRInteractable>(Hit.GetActor()))
        {
            Interactable->OnInteract(this);
        }
    }
}

void AVRPawn::OnTeleport(const FInputActionValue& V) {}
void AVRPawn::OnGrabLeft(const FInputActionValue& V) {}
void AVRPawn::OnGrabRight(const FInputActionValue& V) {}
void AVRPawn::OnInteract(const FInputActionValue& V) {}
