using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [SerializeField] private float bulletUpgradeDamage;
    [SerializeField] private float bulletUpgradeSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            KillSelf();
        }
    }

    public void UpgradeDamage()
    {
        damage += bulletUpgradeDamage;
    }

    public void UpgradeBulletSpeed()
    {
        bulletSpeed += bulletUpgradeSpeed;
    }
}
