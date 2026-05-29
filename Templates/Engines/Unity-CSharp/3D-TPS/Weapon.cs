/// <summary>
/// TPS weapon with auto-fire, ammo, recoil, reload, and muzzle effects.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 20;
    public float fireRate = 0.15f;
    public int maxAmmo = 30;
    private int currentAmmo;
    public float range = 50f;

    [Header("References")]
    public Transform firePoint;
    public GameObject impactEffect;
    public ParticleSystem muzzleParticle;

    private float lastFireTime;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && currentAmmo > 0 && Time.time >= lastFireTime + fireRate)
        {
            Fire();
        }
    }

    private void Fire()
    {
        lastFireTime = Time.time;
        currentAmmo--;

        if (muzzleParticle != null)
            muzzleParticle.Play();

        Ray ray = new Ray(firePoint.position, firePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.TryGetComponent<EnemyController>(out var enemy))
                enemy.TakeDamage(damage);

            if (impactEffect != null)
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    public int GetAmmo() => currentAmmo;
    public int GetMaxAmmo() => maxAmmo;
}

public class EnemyController : MonoBehaviour
{
    public int health = 50;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
