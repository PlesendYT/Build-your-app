/// <summary>
/// Provides crafting recipes and logic to combine resources into new items.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class CraftingSystem : MonoBehaviour
{
    [Header("Recipes")]
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

    [System.Serializable]
    public class CraftingRecipe
    {
        public string resultItem;
        public int resultQuantity = 1;
        public Ingredient[] ingredients;
    }

    [System.Serializable]
    public class Ingredient
    {
        public string itemName;
        public int quantityNeeded;
    }

    /// <summary>
    /// Initializes default crafting recipes.
    /// </summary>
    private void Start()
    {
        if (recipes.Count == 0)
            SetupDefaultRecipes();
    }

    /// <summary>
    /// Populates the recipe list with common survival crafting recipes.
    /// </summary>
    private void SetupDefaultRecipes()
    {
        recipes.Add(new CraftingRecipe
        {
            resultItem = "Plank",
            resultQuantity = 4,
            ingredients = new Ingredient[] { new Ingredient { itemName = "Wood", quantityNeeded = 1 } }
        });

        recipes.Add(new CraftingRecipe
        {
            resultItem = "Workbench",
            resultQuantity = 1,
            ingredients = new Ingredient[] { new Ingredient { itemName = "Plank", quantityNeeded = 10 } }
        });

        recipes.Add(new CraftingRecipe
        {
            resultItem = "Campfire",
            resultQuantity = 1,
            ingredients = new Ingredient[] {
                new Ingredient { itemName = "Wood", quantityNeeded = 5 },
                new Ingredient { itemName = "Stone", quantityNeeded = 3 }
            }
        });
    }

    /// <summary>
    /// Attempts to craft a recipe; returns true and consumes ingredients on success.
    /// </summary>
    public bool Craft(string resultItem, GameManager gameManager)
    {
        // Find the matching recipe
        CraftingRecipe recipe = recipes.Find(r => r.resultItem == resultItem);
        if (recipe == null)
        {
            Debug.Log("Recipe not found: " + resultItem);
            return false;
        }

        // Check if all ingredients are available
        foreach (Ingredient ing in recipe.ingredients)
        {
            GameManager.ItemStack stack = gameManager.inventory.Find(s => s.itemName == ing.itemName);
            if (stack == null || stack.quantity < ing.quantityNeeded)
            {
                Debug.Log("Missing ingredient: " + ing.itemName);
                return false;
            }
        }

        foreach (Ingredient ing in recipe.ingredients)
            gameManager.RemoveItem(ing.itemName, ing.quantityNeeded);

        gameManager.AddItem(recipe.resultItem, recipe.resultQuantity);
        Debug.Log("Crafted " + recipe.resultQuantity + "x " + resultItem);
        return true;
    }
}
