// Header for a fighting game character with combo moves and special meter.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "FighterCharacter.generated.h"

// A combo move defined by an input sequence.
USTRUCT(BlueprintType)
struct FComboMove
{
    GENERATED_BODY()
    UPROPERTY(BlueprintReadWrite) TArray<FName> InputSequence;
    UPROPERTY(BlueprintReadWrite) FName MontageSection;
    UPROPERTY(BlueprintReadWrite) float Damage = 10.0f;
    UPROPERTY(BlueprintReadWrite) float SpecialGain = 5.0f;
};

UCLASS()
class AFighterCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    AFighterCharacter();

    UPROPERTY(BlueprintReadWrite) float Health = 100.0f;
    UPROPERTY(BlueprintReadWrite) float MaxHealth = 100.0f;
    UPROPERTY(BlueprintReadWrite) float SpecialMeter = 0.0f;
    UPROPERTY(BlueprintReadWrite) float MaxSpecial = 100.0f;
    UPROPERTY(BlueprintReadOnly) int32 Wins = 0;

    // Apply damage and gain special meter.
    UFUNCTION(BlueprintCallable)
    void TakeHitDamage(float Damage);

    // Add to the special meter (clamped to max).
    UFUNCTION(BlueprintCallable)
    void AddSpecial(float Amount);

    // Check if the input buffer matches a combo sequence.
    UFUNCTION(BlueprintCallable)
    bool TryCombo(FName ComboID);

    // Spend special meter to perform a special attack.
    UFUNCTION(BlueprintCallable)
    void PerformSpecialAttack();

protected:
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    void LightAttack();
    void HeavyAttack();
    void Kick();
    void Block();

    UPROPERTY(EditDefaultsOnly) TMap<FName, FComboMove> Combos;
    UPROPERTY(EditDefaultsOnly) UInputMappingContext* InputMapping;
    UPROPERTY(EditDefaultsOnly) UInputAction* LightPunchAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* HeavyPunchAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* KickAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* BlockAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* SpecialAction;

    TArray<FName> InputBuffer;
    float InputBufferTime = 0.3f;
    float BufferTimer = 0.0f;

    virtual void Tick(float DT) override;
};
