/// <summary>
/// Weapon with stats, rarity tiers, and weapon mod attachments for looter shooter.
/// </summary>
using UnityEngine;

[System.Serializable]
public class WeaponMod
{
    public string modName;
    public int damageBonus;
    public float fireRateBonus;
    public int magazineBonus;
}

public class WeaponSystem : MonoBehaviour
{
    [Header("Base Stats")]
    public string weaponName = "Pistol";
    public int baseDamage = 25;
    public float baseFireRate = 0.2f;
    public int baseMagazineSize = 12;
    public float reloadTime = 1.5f;

    [Header("Rarity")]
    public int rarityTier; // 0=Common, 1=Uncommon, 2=Rare, 3=Epic, 4=Legendary
    public float rarityMultiplier = 1f;

    [Header("Mods")]
    public WeaponMod[] equippedMods;
    public int maxMods = 3;

    [Header("State")]
    public int currentAmmo;
    public bool isReloading;

    private float nextFireTime;

    private void Start()
    {
        currentAmmo = baseMagazineSize;
        rarityMultiplier = 1f + (rarityTier * 0.25f);
    }

    public int GetDamage()
    {
        int damage = Mathf.RoundToInt(baseDamage * rarityMultiplier);
        foreach (WeaponMod mod in equippedMods)
        {
            if (mod != null) damage += mod.damageBonus;
        }
        return damage;
    }

    public float GetFireRate()
    {
        float rate = baseFireRate;
        foreach (WeaponMod mod in equippedMods)
        {
            if (mod != null) rate -= mod.fireRateBonus;
        }
        return Mathf.Max(0.05f, rate);
    }

    public int GetMagazineSize()
    {
        int size = baseMagazineSize;
        foreach (WeaponMod mod in equippedMods)
        {
            if (mod != null) size += mod.magazineBonus;
        }
        return size;
    }

    public bool TryShoot()
    {
        if (isReloading || Time.time < nextFireTime) return false;
        if (currentAmmo <= 0)
        {
            StartReload();
            return false;
        }

        currentAmmo--;
        nextFireTime = Time.time + GetFireRate();
        return true;
    }

    public void StartReload()
    {
        if (isReloading || currentAmmo >= GetMagazineSize()) return;
        isReloading = true;
        Invoke(nameof(FinishReload), reloadTime);
    }

    private void FinishReload()
    {
        currentAmmo = GetMagazineSize();
        isReloading = false;
    }

    public void EquipMod(WeaponMod mod, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= maxMods) return;
        equippedMods[slotIndex] = mod;
    }
}
