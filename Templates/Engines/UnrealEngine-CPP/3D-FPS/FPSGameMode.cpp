// FPS game mode with kill tracking and player respawning.
#include "FPSGameMode.h"
#include "FPSCharacter.h"
#include "GameFramework/PlayerStart.h"
#include "Engine/PlayerStartActor.h"
#include "Kismet/GameplayStatics.h"

// Set the default pawn class to the FPS character.
AFPSGameMode::AFPSGameMode()
{
    DefaultPawnClass = AFPSCharacter::StaticClass();
}

void AFPSGameMode::BeginPlay()
{
    Super::BeginPlay();
}

void AFPSGameMode::OnPlayerKilled(AController* Victim, AController* Killer)
{
    if (Victim && Killer && Victim != Killer)
    {
        // Award kill score logic
    }
    GetWorldTimerManager().SetTimer(RespawnTimerHandle, FTimerDelegate::CreateWeakLambda(this, [this, Victim]()
    {
        if (Victim) RespawnPlayer(Victim);
    }), RespawnDelay, false);
}

void AFPSGameMode::RespawnPlayer(AController* Controller)
{
    if (!Controller) return;
    TArray<AActor*> PlayerStarts;
    UGameplayStatics::GetAllActorsOfClass(this, APlayerStart::StaticClass(), PlayerStarts);
    if (PlayerStarts.Num() > 0)
    {
        AActor* Start = PlayerStarts[FMath::RandRange(0, PlayerStarts.Num() - 1)];
        if (APawn* OldPawn = Controller->GetPawn())
        {
            OldPawn->Destroy();
        }
        if (APawn* NewPawn = SpawnDefaultPawnFor(Controller, Start))
        {
            Controller->Possess(NewPawn);
        }
    }
}
