using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField] private float invokeRepeatTime;
    [SerializeField] private float spawnStartTime;
    [SerializeField] private Transform healthPackPrefab;
    [SerializeField] private Transform turretItemPrefab;
    private float xBorder;
    private float yBorder;

    // Start is called before the first frame update
    void Start()
    {
        xBorder = PlayerMovement.horizontalBorder - 1;
        yBorder = PlayerMovement.verticalBorder - 1;
        InvokeRepeating("SpawnCrate", spawnStartTime, invokeRepeatTime);
        InvokeRepeating("SpawnTurret", spawnStartTime, invokeRepeatTime);
    }


    void SpawnCrate()
    {
        float x = Random.Range(-xBorder, xBorder);
        float y = Random.Range(-yBorder, yBorder);

        Instantiate(healthPackPrefab, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
    }

    void SpawnTurret()
    {
        float x = Random.Range(-xBorder, xBorder);
        float y = Random.Range(-yBorder, yBorder);

        Instantiate(turretItemPrefab, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
    }
}
