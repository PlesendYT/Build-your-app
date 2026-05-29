/// <summary>
/// Manages the unit shop: offering, refreshing, and purchasing units for the auto-battler.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Settings")]
    public int gold = 10;
    public int refreshCost = 2;
    public int shopSize = 5;

    [Header("Unit Pool")]
    public Unit[] availableUnitPrefabs;
    public List<Unit> shopOffers = new List<Unit>();

    [Header("References")]
    public BattleManager battleManager;

    /// <summary>
    /// Generates the initial shop offer.
    /// </summary>
    private void Start()
    {
        if (battleManager == null)
            battleManager = FindObjectOfType<BattleManager>();
        RefreshShop();
    }

    /// <summary>
    /// Rerolls the shop offer by selecting random units from the available pool.
    /// </summary>
    public void RefreshShop()
    {
        if (gold < refreshCost)
        {
            Debug.Log("Not enough gold to refresh!");
            return;
        }

        gold -= refreshCost;
        shopOffers.Clear();

        // Pick random units for each shop slot
        for (int i = 0; i < shopSize; i++)
        {
            Unit randomUnit = availableUnitPrefabs[Random.Range(0, availableUnitPrefabs.Length)];
            shopOffers.Add(randomUnit);
        }

        Debug.Log("Shop refreshed for " + refreshCost + " gold.");
    }

    /// <summary>
    /// Purchases a unit from the shop at the given index and places it on the board.
    /// </summary>
    public bool BuyUnit(int shopIndex, int boardSlot)
    {
        if (shopIndex < 0 || shopIndex >= shopOffers.Count)
            return false;

        Unit selected = shopOffers[shopIndex];
        if (gold < selected.cost)
        {
            Debug.Log("Not enough gold to buy " + selected.unitName);
            return false;
        }

        gold -= selected.cost;
        shopOffers.RemoveAt(shopIndex);

        // Instantiate the unit and place it on the board
        Unit newUnit = Instantiate(selected);
        bool placed = battleManager.PlaceUnit(newUnit, boardSlot);

        Debug.Log("Purchased " + selected.unitName + " for " + selected.cost + " gold.");
        return placed;
    }

    /// <summary>
    /// Sells a unit from the board, refunding half its cost.
    /// </summary>
    public void SellUnit(Unit unit)
    {
        int refund = Mathf.FloorToInt(unit.cost * 0.5f);
        gold += refund;
        battleManager.playerUnits.Remove(unit);
        Destroy(unit.gameObject);
        Debug.Log("Sold " + unit.unitName + " for " + refund + " gold.");
    }
}
