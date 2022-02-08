using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretItem : MonoBehaviour
{
    private GameObject powerupManager;
    private void Start()
    {
        powerupManager = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            powerupManager.GetComponent<PowerupManager>().PickupRandomTurret();
            Destroy(gameObject);
        }
    }
}
