// Header for a 2D platformer character with double jump and collectibles.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "TwoDPlatformerCharacter.generated.h"

class UPaperFlipbookComponent;

UCLASS()
class ATwoDPlatformerCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    ATwoDPlatformerCharacter();

    UPROPERTY(BlueprintReadOnly) int32 CoinCount = 0;
    UPROPERTY(BlueprintReadOnly) int32 GemCount = 0;
    UPROPERTY(BlueprintReadOnly) int32 Lives = 3;
    UPROPERTY(BlueprintReadOnly) bool bIsFacingRight = true;

    // Add coins to the character.
    UFUNCTION(BlueprintCallable)
    void AddCoin(int32 Amount);

    // Add gems to the character.
    UFUNCTION(BlueprintCallable)
    void AddGem(int32 Amount);

    // Handle character death (lose a life, respawn or destroy).
    UFUNCTION(BlueprintCallable)
    void Die();

    // Respawn at the last checkpoint.
    UFUNCTION(BlueprintCallable)
    void Respawn();

protected:
    virtual void BeginPlay() override;
    virtual void Tick(float DT) override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    void MoveHorizontal(const FInputActionValue& V);
    void PlayerJump();
    virtual void Landed(const FHitResult& Hit) override;

    UPROPERTY(VisibleAnywhere) UPaperFlipbookComponent* Sprite;

    UPROPERTY(EditDefaultsOnly) float MoveSpeed = 400.0f;
    UPROPERTY(EditDefaultsOnly) UInputAction* MoveAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* JumpAction;
    UPROPERTY(EditDefaultsOnly) UInputMappingContext* InputMapping;

    int32 JumpCount = 0;
    int32 MaxJumps = 2;
    FVector RespawnLocation;
};
