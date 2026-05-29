/// <summary>
/// Crafting system with recipes, ingredient validation, and inventory integration.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingRecipe
{
    public string resultName;
    public Sprite resultIcon;
    public int resultCount = 1;
    public Ingredient[] ingredients;
}

[System.Serializable]
public class Ingredient
{
    public string itemName;
    public int quantity;
}

public class CraftingSystem : MonoBehaviour
{
    public CraftingRecipe[] recipes;
    public Inventory inventory;

    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (Ingredient ingredient in recipe.ingredients)
        {
            if (inventory.GetItemCount(ingredient.itemName) < ingredient.quantity)
                return false;
        }
        return true;
    }

    public bool Craft(int recipeIndex)
    {
        if (recipeIndex < 0 || recipeIndex >= recipes.Length) return false;
        CraftingRecipe recipe = recipes[recipeIndex];

        if (!CanCraft(recipe)) return false;

        foreach (Ingredient ingredient in recipe.ingredients)
        {
            inventory.RemoveItem(ingredient.itemName, ingredient.quantity);
        }

        Item result = new Item
        {
            itemName = recipe.resultName,
            icon = recipe.resultIcon,
            quantity = recipe.resultCount,
            type = Item.ItemType.Material
        };
        inventory.AddItem(result);
        return true;
    }
}

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    public int value;
    public ItemType type;
    public Sprite icon;
    public int quantity;

    public enum ItemType { Weapon, Armor, Consumable, Material, Tool }
}

public class Inventory : MonoBehaviour
{
    public int maxSlots = 30;
    public List<Item> items = new List<Item>();

    public bool AddItem(Item item)
    {
        if (items.Count >= maxSlots) return false;
        Item existing = items.Find(i => i.itemName == item.itemName);
        if (existing != null) existing.quantity += item.quantity;
        else items.Add(item);
        return true;
    }

    public bool RemoveItem(string itemName, int quantity = 1)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item == null || item.quantity < quantity) return false;
        item.quantity -= quantity;
        if (item.quantity <= 0) items.Remove(item);
        return true;
    }

    public int GetItemCount(string itemName)
    {
        Item item = items.Find(i => i.itemName == itemName);
        return item?.quantity ?? 0;
    }
}
