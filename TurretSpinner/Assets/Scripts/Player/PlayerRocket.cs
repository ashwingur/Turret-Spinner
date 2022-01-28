using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : Bullet
{
    [SerializeField] private float collateralDamage;
    [SerializeField] private float explosionRadius;
    private int layerMask;

    protected override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Enemy");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Explode();
            KillSelf();
        }
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerMask);

        foreach (Collider2D c in colliders)
        {
            c.GetComponent<Enemy>().TakeDamage(collateralDamage);
        }

    }
}
