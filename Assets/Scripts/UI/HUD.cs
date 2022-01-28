using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The HUD is on the GameScene canvas
public class HUD : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;

    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthSlider.value = currentHealth;
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue); 
        healthText.text = string.Format("{0}/{1}", (int)Mathf.Ceil(currentHealth), (int)maxHealth);
    }

    [Header("Rocket Bar")]
    [SerializeField] private Slider rocketSlider;

    public void SetMaxRocketCooldown(float cooldown)
    {
        rocketSlider.maxValue = cooldown;
    }

    public void UpdateRocketBar(float currentCooldown)
    {
        rocketSlider.value = rocketSlider.maxValue - currentCooldown;
    }

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    [Header("Wave")]
    [SerializeField] private TextMeshProUGUI waveText;

    public void UpdateWave(int wave, int subwave)
    {
        waveText.text = string.Format("Wave: {0}.{1}", wave, subwave);
    }

    [Header("Boss Text")]
    [SerializeField] private TextMeshProUGUI bossText;

    public void ShowBossText(string message, float duration)
    {
        bossText.text = message;
        bossText.gameObject.SetActive(true);
        Invoke("HideBossText", duration);
    }

    private void HideBossText()
    {
        bossText.gameObject.SetActive(false);
    }

}
