// Header for a 2D top-down character with gold/keys/health and shooting.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "TopDownCharacter.generated.h"

class UPaperFlipbookComponent;
class ATopDownProjectile;
class UInputAction;

UCLASS()
class ATopDownCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    ATopDownCharacter();

    UPROPERTY(BlueprintReadOnly) int32 Gold = 0;
    UPROPERTY(BlueprintReadOnly) int32 Keys = 0;
    UPROPERTY(BlueprintReadOnly) float Health = 100.0f;

    // Add gold to inventory.
    UFUNCTION(BlueprintCallable)
    void AddGold(int32 Amount);

    // Add keys to inventory.
    UFUNCTION(BlueprintCallable)
    void AddKey(int32 Amount);

    // Reduce health by the given amount.
    UFUNCTION(BlueprintCallable)
    void TakeDamage(float Amount);

    // Fire a projectile toward the mouse cursor.
    UFUNCTION(BlueprintCallable)
    void ShootProjectile();

protected:
    virtual void BeginPlay() override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    void Move(const FInputActionValue& V);
    void LookAtMouse();

    UPROPERTY(VisibleAnywhere) UPaperFlipbookComponent* Sprite;
    UPROPERTY(EditDefaultsOnly) TSubclassOf<ATopDownProjectile> ProjectileClass;
    UPROPERTY(EditDefaultsOnly) float MoveSpeed = 350.0f;
    UPROPERTY(EditDefaultsOnly) UInputAction* MoveAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* ShootAction;
    UPROPERTY(EditDefaultsOnly) UInputMappingContext* InputMapping;
};
