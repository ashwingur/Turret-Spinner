using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeMenu : MonoBehaviour
{
    public static bool UpgradeMenuOpen = false;
    [SerializeField] private GameObject upgradeMenuUI;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private GameOver gameOver;
    [SerializeField] private GameObject player;
    private int currentBalance = 0;

    [SerializeField] private Upgrade moreGunsUpgrade;
    [SerializeField] private Upgrade healthRegenUpgrade;
    [SerializeField] private Upgrade fireRateUpgrade;
    [SerializeField] private Upgrade movementSpeedUpgrade;
    [SerializeField] private Upgrade bulletDamageUpgrade;
    [SerializeField] private Upgrade bulletSpeedUpgrade;

    private void Start()
    {
        moreGunsUpgrade.Initialise();
        healthRegenUpgrade.Initialise();
        fireRateUpgrade.Initialise();
        movementSpeedUpgrade.Initialise();
        bulletDamageUpgrade.Initialise();
        bulletSpeedUpgrade.Initialise();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !gameOver.gameOver)
        {
            if (UpgradeMenuOpen)
            {
                Resume();
            } else
            {
                OpenUpgradeMenu();
            }
        } else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    private void OpenUpgradeMenu()
    {
        UpgradeMenuOpen = true;
        upgradeMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        UpgradeMenuOpen = false;
        upgradeMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AddMoney(int amount)
    {
        currentBalance += amount;
        balanceText.text = "$" + currentBalance.ToString();
    }

    private bool AttemptPurchase(Upgrade upgrade)
    {
        if (upgrade.amountBought < upgrade.maxAmount)
        {
            int cost = upgrade.GetUpgradeCost();
            if (cost > currentBalance)
            {
                return false;
            }
            else
            {
                currentBalance -= cost;
                balanceText.text = "$" + currentBalance.ToString();
                upgrade.UpgradeUpdate();
                return true;
            }
        }
        return false;
    }

    public void BuyMoreGuns()
    {
        if (AttemptPurchase(moreGunsUpgrade))
        {
            player.GetComponent<PlayerShoot>().AddGun();
        }
    }

    public void BuyHealthRegen()
    {
        if (AttemptPurchase(healthRegenUpgrade))
        {
            player.GetComponent<PlayerHealth>().UpgradeHealth();
        }
    }

    public void BuyFireRate()
    {
        if (AttemptPurchase(fireRateUpgrade))
        {
            player.GetComponent<PlayerShoot>().UpgradeFireRate();
        }
    }

    public void BuyMovementSpeed()
    {
        if (AttemptPurchase(movementSpeedUpgrade))
        {
            player.GetComponent<PlayerMovement>().UpgradeMovementSpeed();
        }
    }
    
    public void BuyBulletDamage()
    {
        if (AttemptPurchase(bulletDamageUpgrade))
        {
            player.GetComponent<PlayerShoot>().UpgradeBulletDamage();
        }
    }

    public void BuyBulletSpeed()
    {
        if (AttemptPurchase(bulletSpeedUpgrade))
        {
            player.GetComponent<PlayerShoot>().UpgradeBulletSpeed();
        }
    }

}

[System.Serializable]
class Upgrade
{
    [SerializeField] public Button buyButton;
    private TextMeshProUGUI costText;
    private TextMeshProUGUI upgradeAmountText;
    [SerializeField] public int baseCost;
    [SerializeField] public int maxAmount;
    [SerializeField] private float costMultiplier;
    public int amountBought;

    public Upgrade()
    {
        amountBought = 0;
    }

    public void Initialise()
    {
        costText = buyButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        upgradeAmountText = buyButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        costText.text = "$" + GetUpgradeCost();
        upgradeAmountText.text = string.Format("{0}/{1}", amountBought, maxAmount);
    }

    public void UpgradeUpdate()
    {
        amountBought++;
        costText.text = "$" + GetUpgradeCost();
        upgradeAmountText.text = string.Format("{0}/{1}", amountBought, maxAmount);
    }

    public int GetUpgradeCost()
    {
        return (int)(baseCost * Mathf.Pow(costMultiplier, amountBought));
    }
}


