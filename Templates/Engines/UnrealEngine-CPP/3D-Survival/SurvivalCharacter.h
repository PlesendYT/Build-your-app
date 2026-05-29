// Header for a survival character with vitals, inventory, crafting, and building.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "SurvivalCharacter.generated.h"

// Vital stats for the survival character.
USTRUCT(BlueprintType)
struct FVitalStats
{
    GENERATED_BODY()
    UPROPERTY(BlueprintReadWrite) float Health = 100.0f;
    UPROPERTY(BlueprintReadWrite) float MaxHealth = 100.0f;
    UPROPERTY(BlueprintReadWrite) float Hunger = 100.0f;
    UPROPERTY(BlueprintReadWrite) float Thirst = 100.0f;
    UPROPERTY(BlueprintReadWrite) float Stamina = 100.0f;
    UPROPERTY(BlueprintReadWrite) float MaxStamina = 100.0f;
};

// A crafting recipe with ingredients and output.
USTRUCT(BlueprintType)
struct FCraftingRecipe
{
    GENERATED_BODY()
    UPROPERTY(BlueprintReadWrite) FName OutputItem;
    UPROPERTY(BlueprintReadWrite) int32 OutputQuantity = 1;
    UPROPERTY(BlueprintReadWrite) TMap<FName, int32> Ingredients;
};

UCLASS()
class ASurvivalCharacter : public ACharacter
{
    GENERATED_BODY()
public:
    ASurvivalCharacter();

    UPROPERTY(Replicated, BlueprintReadWrite) FVitalStats Vitals;
    UPROPERTY(Replicated, BlueprintReadWrite) TMap<FName, int32> Inventory;

    // Add gathered resources to inventory.
    UFUNCTION(BlueprintCallable)
    void GatherResource(FName ResourceType, int32 Amount);

    // Attempt to craft an item from the recipe. Returns true on success.
    UFUNCTION(BlueprintCallable)
    bool CraftItem(FName RecipeID);

    // Consume a food item to restore hunger and thirst.
    UFUNCTION(BlueprintCallable)
    void Eat(FName FoodItem, float HungerRestore, float ThirstRestore);

    // Spawn a structure actor at the given location.
    UFUNCTION(BlueprintCallable)
    void BuildStructure(TSubclassOf<AActor> StructureClass, FVector Location, FRotator Rotation);

protected:
    virtual void BeginPlay() override;
    virtual void Tick(float DT) override;
    virtual void SetupPlayerInputComponent(UInputComponent*) override;

    UPROPERTY(EditDefaultsOnly) float HungerDrainRate = 0.5f;
    UPROPERTY(EditDefaultsOnly) float ThirstDrainRate = 0.8f;
    UPROPERTY(EditDefaultsOnly) TMap<FName, FCraftingRecipe> Recipes;

    void Move(const FInputActionValue& V);
    void Look(const FInputActionValue& V);
    void Interact();

    UPROPERTY(EditDefaultsOnly) UInputMappingContext* InputMapping;
    UPROPERTY(EditDefaultsOnly) UInputAction* MoveAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* LookAction;
    UPROPERTY(EditDefaultsOnly) UInputAction* InteractAction;
};
