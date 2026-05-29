// RPG character with stats, leveling, inventory, quests, and damage reduction.
#include "RPGCharacter.h"
#include "Net/UnrealNetwork.h"
#include "Engine/DataTable.h"

// Initialize hit points, mana, and stats.
ARPGCharacter::ARPGCharacter()
{
    CurrentHealth = 100.0f;
    MaxHealth = 100.0f;
    MaxMana = 50.0f;
    CurrentMana = 50.0f;
}

void ARPGCharacter::BeginPlay()
{
    Super::BeginPlay();
    MaxHealth = 80.0f + Stats.Constitution * 2.0f;
    CurrentHealth = MaxHealth;
    MaxMana = 30.0f + Stats.Intelligence * 2.0f;
    CurrentMana = MaxMana;
}

void ARPGCharacter::AddExperience(int32 Amount)
{
    ServerAddExp(Amount);
}

void ARPGCharacter::ServerAddExp_Implementation(int32 Amount)
{
    Stats.Experience += Amount;
    while (Stats.Experience >= Stats.ExpToNext)
    {
        Stats.Experience -= Stats.ExpToNext;
        ServerLevelUp();
    }
}

void ARPGCharacter::ServerLevelUp_Implementation()
{
    Stats.Level++;
    Stats.ExpToNext = Stats.Level * 100;
    Stats.Strength += 2;
    Stats.Dexterity += 2;
    Stats.Intelligence += 2;
    Stats.Constitution += 2;
    MaxHealth = 80.0f + Stats.Constitution * 2.0f;
    CurrentHealth = MaxHealth;
    MaxMana = 30.0f + Stats.Intelligence * 2.0f;
    CurrentMana = MaxMana;
}

void ARPGCharacter::AddItem(FName ItemID, int32 Quantity)
{
    for (auto& Item : Inventory)
    {
        if (Item.ItemID == ItemID && Item.Quantity < Item.MaxStack)
        {
            int32 Space = Item.MaxStack - Item.Quantity;
            int32 Add = FMath::Min(Space, Quantity);
            Item.Quantity += Add;
            Quantity -= Add;
            if (Quantity <= 0) return;
        }
    }
    while (Quantity > 0)
    {
        int32 Add = FMath::Min(Quantity, 99);
        Inventory.Add({ItemID, Add, 99});
        Quantity -= Add;
    }
}

bool ARPGCharacter::RemoveItem(FName ItemID, int32 Quantity)
{
    if (!HasItem(ItemID, Quantity)) return false;
    for (int32 i = 0; i < Inventory.Num(); i++)
    {
        if (Inventory[i].ItemID == ItemID)
        {
            int32 Remove = FMath::Min(Inventory[i].Quantity, Quantity);
            Inventory[i].Quantity -= Remove;
            Quantity -= Remove;
            if (Inventory[i].Quantity <= 0)
            {
                Inventory.RemoveAt(i);
                i--;
            }
            if (Quantity <= 0) return true;
        }
    }
    return true;
}

bool ARPGCharacter::HasItem(FName ItemID, int32 Quantity) const
{
    int32 Count = 0;
    for (const auto& Item : Inventory)
    {
        if (Item.ItemID == ItemID) Count += Item.Quantity;
    }
    return Count >= Quantity;
}

void ARPGCharacter::AcceptQuest(FName QuestID)
{
    ActiveQuests.Add({QuestID, FText::FromName(QuestID), false});
}

void ARPGCharacter::CompleteQuest(FName QuestID)
{
    for (auto& Quest : ActiveQuests)
    {
        if (Quest.QuestID == QuestID)
        {
            Quest.bIsCompleted = true;
            AddExperience(50 * Stats.Level);
            break;
        }
    }
}

float ARPGCharacter::TakeDamage(float Damage, FDamageEvent const& Event, AController* Instigator, AActor* Causer)
{
    float Reduced = Damage * (100.0f / (100.0f + Stats.Constitution * 1.5f));
    CurrentHealth = FMath::Clamp(CurrentHealth - Reduced, 0.0f, MaxHealth);
    return Reduced;
}

void ARPGCharacter::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    DOREPLIFETIME(ARPGCharacter, Stats);
    DOREPLIFETIME(ARPGCharacter, ActiveQuests);
    DOREPLIFETIME(ARPGCharacter, Inventory);
}
