using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBoss : Enemy
{
    [SerializeField] private float chargeTime;
    [SerializeField] private float turnTime;
    private BasicBossMode mode;
    private float currentModeTime;
    private Vector3 chargeDirection;

    protected override void Start()
    {
        base.Start();
        mode = BasicBossMode.Charging;
        currentModeTime = chargeTime;
        chargeDirection = Vector3.Normalize(player.transform.position - transform.position);
    }

    protected override void Update()
    {
        currentModeTime -= Time.deltaTime;
        // Switch to the other mode
        if (currentModeTime <= 0)
        {
            if (mode == BasicBossMode.Charging)
            {
                mode = BasicBossMode.Turning;
                currentModeTime = turnTime;
            } else
            {
                mode = BasicBossMode.Charging;
                chargeDirection = Vector3.Normalize(player.transform.position - transform.position);
                currentModeTime = chargeTime;
            }
        }

        if (mode == BasicBossMode.Charging)
        {
            transform.position += chargeDirection * speed * Time.deltaTime;
        } else
        {
            FacePlayer();
        }
    }
}

enum BasicBossMode
{
    Charging,
    Turning,
}
