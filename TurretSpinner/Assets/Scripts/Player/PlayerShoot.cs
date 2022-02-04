using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform rocket;
    [SerializeField] private Transform rocketHead;
    [Header("Cooldowns")]
    [SerializeField] private float baseShootingCooldown;
    [SerializeField] private float rocketShootingCooldown;
    [Header("Other")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private int maxGunCount;
    [SerializeField] private int playerScore;
    private HUD hud;
    private AudioManager audioManager;
    private float currentBulletCooldown = 0;
    private int gunCount = 1;

    private float upgradedShootingCooldown;
    private int firerateLevel = 0;

    private float currentRocketCooldown = 0;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        upgradedShootingCooldown = baseShootingCooldown;
        hud = canvas.GetComponent<HUD>();
        hud.SetMaxRocketCooldown(rocketShootingCooldown);
        hud.UpdateScore(0);
    }

    void Update()
    {
        currentBulletCooldown -= Time.deltaTime;
        currentRocketCooldown -= Time.deltaTime;
        if (!UpgradeMenu.UpgradeMenuOpen)
        {
            Shoot();
        }
        hud.UpdateRocketBar(currentRocketCooldown);
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            if (currentBulletCooldown <= 0)
            {
                audioManager.Play("PlayerShoot");
                for (int i = 0; i < gunCount; i++)
                {
                    Instantiate(bullet, transform.GetChild(i).position, transform.GetChild(i).rotation);
                }
                currentBulletCooldown = upgradedShootingCooldown;
            }
            if (currentRocketCooldown <= 0)
            {
                audioManager.Play("Missile");
                Instantiate(rocket, rocketHead.position, rocketHead.rotation);
                currentRocketCooldown = rocketShootingCooldown;
            }
            
        }
    }

    // Called from the UpgradeMenu
    public void AddGun()
    {
        if (gunCount < maxGunCount)
        {
            transform.GetChild(gunCount).gameObject.SetActive(true);
            gunCount++;
        }
    }

    // Called from UpgradeMenu
    public void UpgradeFireRate()
    {
        firerateLevel++;
        upgradedShootingCooldown *= 0.85f;
    }

    public void UpgradeBulletDamage()
    {
        bullet.GetComponent<PlayerBullet>().UpgradeDamage();
    }

    public void UpgradeBulletSpeed()
    {
        bullet.GetComponent<PlayerBullet>().UpgradeBulletSpeed();
    }

    public void GetKillReward(int moneyAmount, int scoreAmount)
    {
        canvas.GetComponent<UpgradeMenu>().AddMoney(moneyAmount);
        playerScore += scoreAmount;
        hud.UpdateScore(playerScore);
    }

    public int GetScore() => playerScore;
}
