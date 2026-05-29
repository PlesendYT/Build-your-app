// Puzzle block with activation/toggle, pressure plate support, and chained blocks.
#include "PuzzleBlock.h"
#include "Components/BoxComponent.h"
#include "GameFramework/Character.h"

// Set up collision box and visual mesh.
APuzzleBlock::APuzzleBlock()
{
    CollisionBox = CreateDefaultSubobject<UBoxComponent>(TEXT("CollisionBox"));
    SetRootComponent(CollisionBox);

    Mesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Mesh"));
    Mesh->SetupAttachment(CollisionBox);
}

void APuzzleBlock::BeginPlay() { Super::BeginPlay(); }

void APuzzleBlock::Activate(bool bNewState)
{
    bActivated = bInvertOutput ? !bNewState : bNewState;
    OnActivated.Broadcast(bActivated);
    for (auto* Block : ConnectedBlocks)
    {
        if (Block) Block->Activate(bActivated);
    }
}

void APuzzleBlock::Toggle() { Activate(!bActivated); }

void APuzzleBlock::NotifyActorBeginOverlap(AActor* Other)
{
    Super::NotifyActorBeginOverlap(Other);
    if (bIsPressurePlate && Other->IsA(ACharacter::StaticClass()))
    {
        Activate(true);
    }
}

void APuzzleBlock::NotifyActorEndOverlap(AActor* Other)
{
    Super::NotifyActorEndOverlap(Other);
    if (bIsPressurePlate && Other->IsA(ACharacter::StaticClass()))
    {
        Activate(false);
    }
}
