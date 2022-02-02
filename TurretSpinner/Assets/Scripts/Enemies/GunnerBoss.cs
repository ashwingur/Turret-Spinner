using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBoss : Enemy
{
    [SerializeField] private float smallBulletCooldown;
    [SerializeField] private float largeBulletCooldown;
    [SerializeField] private Transform smallBulletPrefab;
    [SerializeField] private Transform largeBulletPrefab;
    [SerializeField] private float minimumModeTime;
    [SerializeField] private float maximumModeTime;
    [SerializeField] private float rotationSpeed;
    private float currentAttackCooldown;
    private float currentModeTime;
    private GunnerBossMode mode;
    private int spinDirection = -1;


    protected override void Start()
    {
        base.Start();
        currentAttackCooldown = 0.5f;
        currentModeTime = Random.Range(minimumModeTime, maximumModeTime);
        mode = GunnerBossMode.SpinShooting;

    }

    protected override void Update()
    {
        currentModeTime -= Time.deltaTime;
        if (currentModeTime <= 0)
        {
            StartRandomMode();
        }

        if (mode == GunnerBossMode.SpinShooting)
        {
            SpinShooting();
        }
        else if (mode == GunnerBossMode.StationaryShooting)
        {
            StationaryShooting();
        }
        else if (mode == GunnerBossMode.MoveShooting)
        {
            MoveShooting();
        }
    }

    private void MoveShooting()
    {
        Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);
        transform.position += direction * speed * Time.deltaTime;
        FacePlayer();
        Shoot(false);
    }

    private void StationaryShooting()
    {
        FacePlayer();
        Shoot(false);
    }

    private void SpinShooting()
    {
        transform.Rotate(0, 0, spinDirection * rotationSpeed * Time.deltaTime);
        Shoot(true);
    }

    private void StartRandomMode()
    {
        mode = (GunnerBossMode)Random.Range(0, 3);
        if (mode == GunnerBossMode.SpinShooting)
        {
            // Either 1 or -1
            spinDirection = Random.Range(0, 1) == 0 ? 1 : -1;
        } 
        else if (mode == GunnerBossMode.StationaryShooting)
        {

        } 
        else if (mode == GunnerBossMode.MoveShooting)
        {

        }
        currentAttackCooldown = 0.2f;
        currentModeTime = Random.Range(minimumModeTime, maximumModeTime);
    }

    private void Shoot(bool isSmall)
    {
        currentAttackCooldown -= Time.deltaTime;

        if (currentAttackCooldown <= 0)
        {
            if (isSmall)
            {
                // Fire the small bullets
                for (int i = 2; i < 6; i++)
                {
                    Transform gun = transform.GetChild(i);
                    Instantiate(smallBulletPrefab, gun.position, gun.rotation);
                }

                currentAttackCooldown = smallBulletCooldown;
            } 
            else
            {
                // Fire the large bullet
                Transform gun = transform.GetChild(1);
                Instantiate(largeBulletPrefab, gun.position, gun.rotation);

                currentAttackCooldown = largeBulletCooldown;
            }
        }

    }

}

enum GunnerBossMode
{
    SpinShooting, // Not moving, but spinning with heaps of small bullets
    StationaryShooting, // Not moving, facing player and shooting a small amount of big bullets
    MoveShooting, // Moving and shooting a small amount of big bullets
}
