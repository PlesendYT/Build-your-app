/// <summary>
/// Weapon with raycast firing, recoil, ammo, and reload mechanics.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public string weaponName = "Pistol";
    public float fireRate = 0.2f;
    public int damage = 25;
    public int maxAmmo = 12;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    public float range = 100f;

    [Header("References")]
    public Transform firePoint;
    public GameObject muzzleFlash;
    public GameObject bulletImpact;
    public Animator animator;

    private float lastFireTime;
    private bool isReloading;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && !isReloading && Time.time >= lastFireTime + fireRate)
        {
            if (currentAmmo > 0)
            {
                Fire();
            }
            else
            {
                Reload();
            }
        }
    }

    private void Fire()
    {
        lastFireTime = Time.time;
        currentAmmo--;

        if (muzzleFlash != null)
            muzzleFlash.SetActive(true);

        Ray ray = new Ray(firePoint.position, firePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.TryGetComponent<EnemyController>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }

            if (bulletImpact != null)
                Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
        }

        if (animator != null)
            animator.SetTrigger("Fire");

        Invoke(nameof(DisableMuzzleFlash), 0.05f);
    }

    private void DisableMuzzleFlash()
    {
        if (muzzleFlash != null)
            muzzleFlash.SetActive(false);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed && !isReloading && currentAmmo < maxAmmo)
        {
            Reload();
        }
    }

    private void Reload()
    {
        isReloading = true;
        if (animator != null)
            animator.SetTrigger("Reload");
        Invoke(nameof(FinishReload), reloadTime);
    }

    private void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public int GetAmmo() => currentAmmo;
    public int GetMaxAmmo() => maxAmmo;
    public bool IsReloading() => isReloading;
}
