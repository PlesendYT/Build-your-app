#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "PlatformerCharacter.generated.h"

UCLASS()
class APlatformerCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    APlatformerCharacter();

    UPROPERTY(BlueprintReadOnly) int32 CoinCount = 0;
    UPROPERTY(BlueprintReadOnly) int32 KeyCount = 0;
    UPROPERTY(BlueprintReadOnly) bool bHasDoubleJump = false;
    UPROPERTY(BlueprintReadOnly) int32 CheckpointIndex = 0;

    UFUNCTION(BlueprintCallable)
    void AddCoin(int32 Amount);

    UFUNCTION(BlueprintCallable)
    void AddKey(int32 Amount);

    UFUNCTION(BlueprintCallable)
    void ActivateDoubleJump();

protected:
    virtual void BeginPlay() override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    void MoveHorizontal(const FInputActionValue& V);
    void PlayerJump();
    virtual void Landed(const FHitResult& Hit) override;

    UPROPERTY(EditDefaultsOnly) float JumpVelocity = 700.0f;
    UPROPERTY(EditDefaultsOnly) float AirControl = 0.6f;

    UPROPERTY(EditDefaultsOnly) UInputAction* MoveAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* JumpAction;

    int32 JumpCount = 0;
    int32 MaxJumps = 1;

    UFUNCTION(Server, Reliable)
    void ServerAddCoin(int32 A);
};
