using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletLifetime;
    [SerializeField] private ParticleSystem destroyParticle;
    private float lifetimeLeft;

    protected virtual void Start()
    {
        lifetimeLeft = bulletLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeLeft -= Time.deltaTime;
        if (lifetimeLeft <= 0)
        {
            KillSelf();
        }

        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    public float GetDamage()
    {
        return damage;
    }

    protected void KillSelf()
    {
        Instantiate(destroyParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
