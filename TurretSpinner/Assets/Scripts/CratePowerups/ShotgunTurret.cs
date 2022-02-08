using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTurret : Turret
{
    protected override void Shoot()
    {
        currentAttackCooldown -= Time.deltaTime;
        if (currentAttackCooldown <= 0)
        {
            AudioManager.GetInstance().Play("Minigun");
            // The shotgun shoots 5 shells
            for (int i = 0; i < 5; i++)
            {
                Transform gun = transform.GetChild(1).GetChild(i);
                Instantiate(bulletPrefab, gun.position, gun.rotation);
            }
            currentAttackCooldown = attackCooldown;
        }
    }

}
