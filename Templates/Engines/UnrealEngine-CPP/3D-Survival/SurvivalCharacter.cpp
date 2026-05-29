// Survival character with hunger, thirst, stamina, inventory, and crafting.
#include "SurvivalCharacter.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Net/UnrealNetwork.h"

// Enable tick for vital stat drain.
ASurvivalCharacter::ASurvivalCharacter()
{
    PrimaryActorTick.bCanEverTick = true;
}

// Register enhanced input mapping context at start.
void ASurvivalCharacter::BeginPlay()
{
    Super::BeginPlay();
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
    {
        if (auto* S = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
            S->AddMappingContext(InputMapping, 0);
    }
}

// Drain hunger/thirst over time; apply damage if depleted; regenerate stamina.
void ASurvivalCharacter::Tick(float DT)
{
    Super::Tick(DT);
    if (!HasAuthority()) return;
    Vitals.Hunger = FMath::Max(0.0f, Vitals.Hunger - HungerDrainRate * DT);
    Vitals.Thirst = FMath::Max(0.0f, Vitals.Thirst - ThirstDrainRate * DT);
    if (Vitals.Hunger <= 0 || Vitals.Thirst <= 0)
    {
        Vitals.Health = FMath::Max(0.0f, Vitals.Health - DT * 5.0f);
    }
    Vitals.Stamina = FMath::Min(Vitals.MaxStamina, Vitals.Stamina + DT * 10.0f);
}

// Add gathered resources to the character's inventory.
void ASurvivalCharacter::GatherResource(FName ResourceType, int32 Amount)
{
    Inventory.FindOrAdd(ResourceType) += Amount;
}

// Attempt to craft an item: checks ingredients, consumes them, adds output.
bool ASurvivalCharacter::CraftItem(FName RecipeID)
{
    if (!Recipes.Contains(RecipeID)) return false;
    FCraftingRecipe& Recipe = Recipes[RecipeID];
    for (const auto& Ing : Recipe.Ingredients)
    {
        if ((Inventory.FindRef(Ing.Key)) < Ing.Value) return false;
    }
    for (const auto& Ing : Recipe.Ingredients)
    {
        Inventory[Ing.Key] -= Ing.Value;
        if (Inventory[Ing.Key] <= 0) Inventory.Remove(Ing.Key);
    }
    Inventory.FindOrAdd(Recipe.OutputItem) += Recipe.OutputQuantity;
    return true;
}

// Consume a food item from inventory to restore hunger and thirst.
void ASurvivalCharacter::Eat(FName FoodItem, float HungerRestore, float ThirstRestore)
{
    if (!Inventory.Contains(FoodItem) || Inventory[FoodItem] <= 0) return;
    Inventory[FoodItem]--;
    if (Inventory[FoodItem] <= 0) Inventory.Remove(FoodItem);
    Vitals.Hunger = FMath::Min(Vitals.MaxHealth, Vitals.Hunger + HungerRestore);
    Vitals.Thirst = FMath::Min(Vitals.MaxHealth, Vitals.Thirst + ThirstRestore);
}

// Spawn a building/object at the specified location (server only).
void ASurvivalCharacter::BuildStructure(TSubclassOf<AActor> StructureClass, FVector Location, FRotator Rotation)
{
    if (!HasAuthority() || !StructureClass) return;
    FActorSpawnParameters P;
    P.Owner = this;
    GetWorld()->SpawnActor<AActor>(StructureClass, Location, Rotation, P);
}

// Set up replication for vitals and inventory.
void ASurvivalCharacter::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    DOREPLIFETIME(ASurvivalCharacter, Vitals);
    DOREPLIFETIME(ASurvivalCharacter, Inventory);
}
