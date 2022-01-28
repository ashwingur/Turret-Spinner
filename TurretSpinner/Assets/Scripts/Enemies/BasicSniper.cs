using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSniper : Enemy
{
    [SerializeField] private float attackSpeedCooldown;
    [SerializeField] private Transform bulletPrefab;
    private float currentAttackCooldown;

    protected override void Start()
    {
        base.Start();
        currentAttackCooldown = Random.Range(0, attackSpeedCooldown);
    }

    protected override void Update()
    {
        Move();
        Shoot();
    }

    protected override void Move()
    {
        Vector2 sum = Vector2.zero;
        int count = 0;

        var hits = Physics2D.OverlapCircleAll(transform.position, separationDistance);
        foreach (var hit in hits)
        {
            // If it is another enemy, try separate away from it to prevent bunching
            if (hit.GetComponent<BasicSniper>() != null && hit.transform != transform)
            {
                Vector2 difference = transform.position - hit.transform.position;

                // Weight by distance so closer enemies lead to bigger separation
                difference = difference.normalized / Mathf.Abs(difference.magnitude);

                sum += difference;
                count++;
            }

        }

        Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);
        transform.position += direction * speed * Time.deltaTime;

        if (count > 0)
        {
            sum = sum.normalized * separationSpeed * Time.deltaTime;
            transform.position += new Vector3(sum.x, sum.y, 0);
        }
        
        UpdateRotation(direction);
    }

    private void Shoot()
    {
        currentAttackCooldown -= Time.deltaTime;
        if (currentAttackCooldown <= 0)
        {
            Transform gun = transform.GetChild(1);
            Instantiate(bulletPrefab, gun.position, gun.rotation);
            currentAttackCooldown = attackSpeedCooldown;
        }
    }

}
