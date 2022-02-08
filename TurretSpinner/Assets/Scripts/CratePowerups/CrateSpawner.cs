using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    [Header("Crate Spawn Timings")]
    [SerializeField] private float minSpawnStartTime;
    [SerializeField] private float maxSpawnStartTime;
    [SerializeField] private float repeatMinTime;
    [SerializeField] private float repeatMaxTime;
    [SerializeField] private float turretRepeatMinTime;
    [SerializeField] private float turretRepeatMaxTime;
    [Header("Crate Prefabs")]
    [SerializeField] private Transform healthPackPrefab;
    [SerializeField] private Transform turretItemPrefab;
    private float xBorder;
    private float yBorder;

    // Start is called before the first frame update
    void Start()
    {
        xBorder = PlayerMovement.horizontalBorder - 1;
        yBorder = PlayerMovement.verticalBorder - 1;
        Invoke("SpawnHealthPack", Random.Range(minSpawnStartTime, maxSpawnStartTime));
        Invoke("SpawnTurret", Random.Range(minSpawnStartTime, maxSpawnStartTime));
    }


    private void SpawnHealthPack()
    {
        float x = Random.Range(-xBorder, xBorder);
        float y = Random.Range(-yBorder, yBorder);

        Instantiate(healthPackPrefab, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
        Invoke("SpawnHealthPack", Random.Range(repeatMinTime, repeatMaxTime));
    }

    private void SpawnTurret()
    {
        float x = Random.Range(-xBorder, xBorder);
        float y = Random.Range(-yBorder, yBorder);

        Instantiate(turretItemPrefab, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
        Invoke("SpawnTurret", Random.Range(turretRepeatMinTime, turretRepeatMaxTime));
    }
}
