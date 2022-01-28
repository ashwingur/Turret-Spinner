using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] private float healthPackRestorePercentage;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void HealthPack()
    {
        player.GetComponent<PlayerHealth>().HealthPack(healthPackRestorePercentage);
    }

    public void BasicTurret()
    {
        player.GetComponent<PlayerInventory>().SetCurrentTurret(TurretType.Basic);
    }
}
