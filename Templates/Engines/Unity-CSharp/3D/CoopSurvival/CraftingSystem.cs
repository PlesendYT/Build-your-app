/// <summary>
/// Recipe-based crafting system for co-op survival: tools, weapons, building materials, and food.
/// </summary>
using UnityEngine;

[System.Serializable]
public class CraftingRecipe
{
    public string recipeName, resultItemID;
    public int resultCount = 1;
    [System.Serializable] public struct Ingredient { public string itemID; public int count; }
    public Ingredient[] ingredients;
    public float craftTime = 2f;
    public int requiredLevel = 1;
}

public class CraftingSystem : MonoBehaviour
{
    [Header("Recipes")]
    public CraftingRecipe[] availableRecipes;
    public bool isCrafting;
    private float currentCraftTime;
    private CraftingRecipe currentRecipe;

    [Header("UI")]
    public GameObject craftingPanel;
    public TMPro.TextMeshProUGUI recipeListText, craftProgressText;

    private void Start() { if (craftingPanel != null) craftingPanel.SetActive(false); }

    private void Update()
    {
        if (!isCrafting) return;
        currentCraftTime -= Time.deltaTime;
        if (craftProgressText != null) craftProgressText.text = "Crafting: " + Mathf.FloorToInt((1f - currentCraftTime / currentRecipe.craftTime) * 100f) + "%";
        if (currentCraftTime <= 0f) { isCrafting = false; if (craftProgressText != null) craftProgressText.text = "Crafted!"; }
    }

    public bool CanCraft(CraftingRecipe r) => r.requiredLevel <= GameManager.Instance.dayCount;
    public void StartCrafting(CraftingRecipe r) { if (isCrafting || !CanCraft(r)) return; isCrafting = true; currentRecipe = r; currentCraftTime = r.craftTime; }
    public void CancelCrafting() { isCrafting = false; currentRecipe = null; currentCraftTime = 0f; }

    public void ToggleCraftingPanel()
    {
        if (craftingPanel == null) return;
        craftingPanel.SetActive(!craftingPanel.activeSelf);
        if (!craftingPanel.activeSelf || recipeListText == null) return;
        string list = "";
        foreach (CraftingRecipe r in availableRecipes)
        {
            list += (CanCraft(r) ? "<color=white>" : "<color=gray>") + r.recipeName + (CanCraft(r) ? "" : " (Locked)") + "</color>\n";
            foreach (CraftingRecipe.Ingredient i in r.ingredients) list += "  - " + i.itemID + " x" + i.count + "\n";
            list += "\n";
        }
        recipeListText.text = list;
    }
}
