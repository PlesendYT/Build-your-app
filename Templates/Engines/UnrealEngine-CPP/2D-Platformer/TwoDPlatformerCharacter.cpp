#include "TwoDPlatformerCharacter.h"
#include "PaperFlipbookComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"

ATwoDPlatformerCharacter::ATwoDPlatformerCharacter()
{
    Sprite = CreateDefaultSubobject<UPaperFlipbookComponent>(TEXT("Sprite"));
    Sprite->SetupAttachment(GetRootComponent());
    PrimaryActorTick.bCanEverTick = true;
    GetCharacterMovement()->MaxWalkSpeed = MoveSpeed;
}

void ATwoDPlatformerCharacter::BeginPlay()
{
    Super::BeginPlay();
    RespawnLocation = GetActorLocation();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
        if (auto* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
}

void ATwoDPlatformerCharacter::Tick(float DT)
{
    Super::Tick(DT);
    FVector Vel = GetVelocity();
    if (FMath::Abs(Vel.X) > 10.0f)
    {
        bIsFacingRight = Vel.X > 0.0f;
        SetActorRotation(FRotator(0, bIsFacingRight ? 0 : 180, 0));
    }
}

void ATwoDPlatformerCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (auto* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(MoveAction, ETriggerEvent::Triggered, this, &ATwoDPlatformerCharacter::MoveHorizontal);
        EIC->BindAction(JumpAction, ETriggerEvent::Started, this, &ATwoDPlatformerCharacter::PlayerJump);
    }
}

void ATwoDPlatformerCharacter::MoveHorizontal(const FInputActionValue& V)
{
    AddMovementInput(FVector::RightVector, V.Get<float>());
}

void ATwoDPlatformerCharacter::PlayerJump()
{
    if (JumpCount < MaxJumps)
    {
        FVector JumpVel = FVector(0, 0, 600.0f);
        LaunchCharacter(JumpVel, false, true);
        JumpCount++;
    }
}

void ATwoDPlatformerCharacter::Landed(const FHitResult& Hit)
{
    Super::Landed(Hit);
    JumpCount = 0;
}

void ATwoDPlatformerCharacter::AddCoin(int32 Amount) { CoinCount += Amount; }
void ATwoDPlatformerCharacter::AddGem(int32 Amount) { GemCount += Amount; }

void ATwoDPlatformerCharacter::Die()
{
    Lives--;
    if (Lives > 0) Respawn();
    else Destroy();
}

void ATwoDPlatformerCharacter::Respawn()
{
    SetActorLocation(RespawnLocation);
    GetCharacterMovement()->SetMovementMode(MOVE_Walking);
}
