using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float baseHealthRegen;
    [SerializeField] private float healthRegen;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private HUD hud;
    private float healthRegenLevel = 0;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        } else
        {
            RegenerateHealth();
            hud.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    private void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        hud.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            TakeDamage(collision.GetComponent<Enemy>().GetDamage());
        }
    }

    public void UpgradeHealth()
    {
        healthRegenLevel++;
        healthRegen = healthRegenLevel * baseHealthRegen;
    }

    private void RegenerateHealth()
    {
        currentHealth += healthRegen * Time.deltaTime;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }


    // Should be a value between 0-1
    public void HealthPack(float percentageRestore)
    {
        currentHealth += percentageRestore * maxHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
        Time.timeScale = 0.1f;
        gameObject.SetActive(false);
        Invoke("GoToStartMenu", 0.5f);
    }

    private void GoToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1;
    }




}
