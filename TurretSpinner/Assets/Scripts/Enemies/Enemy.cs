using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float separationSpeed;
    [SerializeField] protected float separationDistance;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected int killReward;
    [SerializeField] protected int killScore;
    [SerializeField] protected ParticleSystem deathParticles;
    [SerializeField] protected string message;
    [SerializeField] protected string deathSound;
    [SerializeField] protected bool isBoss;
    protected float currentHealth;
    protected GameObject player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected void UpdateRotation(Vector3 v)
    {
        var angle = Vector3.Angle(v, Vector3.right);

        if (v.y < 0)
        {
            angle = -angle;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void MoveToPlayer()
    {
        Vector2 sum = Vector2.zero;
        int count = 0;

        var hits = Physics2D.OverlapCircleAll(transform.position, separationDistance);
        foreach (var hit in hits)
        {
            // If it is another enemy, try separate away from it to prevent bunching
            if (hit.GetComponent<Enemy>() != null && hit.transform != transform)
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
        } else
        {
            UpdateRotation(direction);
        }
    }

    protected virtual void MoveToLocation(Vector3 point, bool facePoint)
    {
        Vector3 direction = Vector3.Normalize(point - transform.position);
        transform.position += direction * speed * Time.deltaTime;

        if (facePoint)
        {
            UpdateRotation(point);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // If hit by a bullet, take damage and update the health bar
        if (collision.gameObject.CompareTag("AllyBullet"))
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().GetDamage());
        }
    }

    protected virtual void Die()
    {
        player.GetComponent<PlayerShoot>().GetKillReward(killReward, killScore);
        Instantiate(deathParticles, transform.position, transform.rotation);
        if (deathSound != "")
        {
            AudioManager.GetInstance().Play(deathSound);
        }
        if (isBoss)
        {
            print("Boss died, calling next wave");
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>().StartNextWave();
        }
        Destroy(gameObject);
    }


    public float GetDamage()
    {
        return damage;
    }

    protected void FacePlayer()
    {
        UpdateRotation(Vector3.Normalize(player.transform.position - transform.position));
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        transform.GetChild(0).transform.localScale =
            new Vector3(currentHealth / maxHealth, 1, 0);
    }

    // For bosses
    public string GetMessage()
    {
        return message;
    }


}
