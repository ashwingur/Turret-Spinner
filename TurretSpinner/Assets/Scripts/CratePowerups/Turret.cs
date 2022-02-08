using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] protected float lifetime;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float range;
    [SerializeField] protected Transform bulletPrefab;
    protected float currentLife;
    protected float currentAttackCooldown;
    protected Transform currentTarget;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentLife = lifetime;
        currentAttackCooldown = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateLife();
        TargetClosestEnemy();
    }

    protected virtual void Shoot()
    {
        currentAttackCooldown -= Time.deltaTime;
        if (currentAttackCooldown <= 0)
        {
            AudioManager.GetInstance().Play("Minigun");
            Transform gun = transform.GetChild(1).GetChild(0);
            Instantiate(bulletPrefab, gun.position, gun.rotation);
            currentAttackCooldown = attackCooldown;
        }
    }

    protected virtual void TargetClosestEnemy()
    {
        // Also change target if it goes out of range
        if (currentTarget == null 
            || !currentTarget.gameObject.activeSelf
            || Vector3.Distance(currentTarget.transform.position, transform.position) > range)
        {
            // Find the closest game object within the Enemy layer and range
            Collider2D collider = Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Enemy"));
            if (collider != null)
            {
                currentTarget = collider.transform;
                FaceTarget(collider.transform.position);
            }
        } else
        {
            FaceTarget(currentTarget.transform.position);
            Shoot();
        }
        
    }

    private void UpdateLife()
    {
        currentLife -= Time.deltaTime;
        if (currentLife <= 0)
        {
            Destroy(gameObject);
        }

        transform.GetChild(0).transform.localScale = new Vector3(currentLife/lifetime, 1, 0);
    }

    private void UpdateHeadRotation(Vector3 v)
    {
        var angle = Vector3.Angle(v, Vector3.right);

        if (v.y < 0)
        {
            angle = -angle;
        }
        transform.GetChild(1).rotation = Quaternion.Euler(0, 0, angle);
    }

    protected void FaceTarget(Vector3 target)
    {
        UpdateHeadRotation(target - transform.position);
    }
}
