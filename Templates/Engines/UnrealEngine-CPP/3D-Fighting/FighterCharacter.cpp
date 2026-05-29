// Fighting game character with combo system, input buffering, and special meter.
#include "FighterCharacter.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"

// Enable tick for input buffer timing.
AFighterCharacter::AFighterCharacter()
{
    PrimaryActorTick.bCanEverTick = true;
}

void AFighterCharacter::Tick(float DT)
{
    Super::Tick(DT);
    if (BufferTimer > 0)
    {
        BufferTimer -= DT;
        if (BufferTimer <= 0) InputBuffer.Empty();
    }
}

void AFighterCharacter::SetupPlayerInputComponent(UInputComponent* PIC)
{
    if (auto* EIC = Cast<UEnhancedInputComponent>(PIC))
    {
        EIC->BindAction(LightPunchAction, ETriggerEvent::Started, this, &AFighterCharacter::LightAttack);
        EIC->BindAction(HeavyPunchAction, ETriggerEvent::Started, this, &AFighterCharacter::HeavyAttack);
        EIC->BindAction(KickAction, ETriggerEvent::Started, this, &AFighterCharacter::Kick);
        EIC->BindAction(BlockAction, ETriggerEvent::Started, this, &AFighterCharacter::Block);
        EIC->BindAction(SpecialAction, ETriggerEvent::Started, this, &AFighterCharacter::PerformSpecialAttack);
    }
}

void AFighterCharacter::LightAttack()
{
    InputBuffer.Add(FName("Light"));
    BufferTimer = InputBufferTime;
}

void AFighterCharacter::HeavyAttack()
{
    InputBuffer.Add(FName("Heavy"));
    BufferTimer = InputBufferTime;
}

void AFighterCharacter::Kick()
{
    InputBuffer.Add(FName("Kick"));
    BufferTimer = InputBufferTime;
}

void AFighterCharacter::Block() {}

bool AFighterCharacter::TryCombo(FName ComboID)
{
    if (!Combos.Contains(ComboID)) return false;
    auto& Combo = Combos[ComboID];
    if (InputBuffer.Num() < Combo.InputSequence.Num()) return false;
    for (int32 i = 0; i < Combo.InputSequence.Num(); i++)
        if (InputBuffer[i] != Combo.InputSequence[i]) return false;
    InputBuffer.Empty();
    AddSpecial(Combo.SpecialGain);
    return true;
}

void AFighterCharacter::TakeHitDamage(float Damage)
{
    Health = FMath::Max(0.0f, Health - Damage);
    AddSpecial(Damage * 0.5f);
}

void AFighterCharacter::AddSpecial(float Amount)
{
    SpecialMeter = FMath::Min(MaxSpecial, SpecialMeter + Amount);
}

void AFighterCharacter::PerformSpecialAttack()
{
    if (SpecialMeter >= 30.0f)
    {
        SpecialMeter -= 30.0f;
        TakeHitDamage(25.0f);
    }
}
