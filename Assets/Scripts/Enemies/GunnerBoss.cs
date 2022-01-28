using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBoss : Enemy
{
    [SerializeField] private float smallBulletCooldown;
    [SerializeField] private float bigBulletCooldown;
    [SerializeField] private Transform smallBulletPrefab;
    [SerializeField] private Transform bigBulletPrefab;
    [SerializeField] private float minimumModeTime;
    [SerializeField] private float maximumModeTime;
    private float currentAttackCooldown;
    private float currentModeTime;
    private GunnerBossMode mode;
    private int spinDirection;


    protected override void Start()
    {
        base.Start();
        currentAttackCooldown = 0;
        currentModeTime = Random.Range(minimumModeTime, maximumModeTime);
        mode = GunnerBossMode.MoveShooting;

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
        throw new System.NotImplementedException();
    }

    private void StationaryShooting()
    {
        throw new System.NotImplementedException();
    }

    private void SpinShooting()
    {
        
    }

    private void StartRandomMode()
    {
        mode = (GunnerBossMode)Random.Range(0, 2);
        if (mode == GunnerBossMode.SpinShooting)
        {
            spinDirection = Random.Range(0, 1);
        } 
        else if (mode == GunnerBossMode.StationaryShooting)
        {

        } 
        else if (mode == GunnerBossMode.MoveShooting)
        {

        }
    }

    protected override void Die()
    {
        GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>().StartNextWave();
        base.Die();
    }


}

enum GunnerBossMode
{
    SpinShooting, // Not moving, but spinning with heaps of small bullets
    StationaryShooting, // Not moving, facing player and shooting a small amount of big bullets
    MoveShooting, // Moving and shooting a small amount of big bullets
}
