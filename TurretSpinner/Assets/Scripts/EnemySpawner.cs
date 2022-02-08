using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int currentWave;
    [SerializeField] private HUD hud;
    [SerializeField] private float bossTextDisplayTime;
    [SerializeField] private float minSpawnDistance;
    private List<Wave> waves;
    private int spawnpointCount;
    private Transform player;

    [Header("Enemy Prefabs")]
    [SerializeField] private Transform basicEnemy;
    [SerializeField] private Transform basicSniper;
    [Header("Boss Prefabs")]
    [SerializeField] private Transform basicBoss;
    [SerializeField] private Transform gunnerBoss;
    [SerializeField] private Transform necromancerBoss;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnpointCount = transform.childCount;
        InitialiseWaves();
        StartCoroutine(StartWave(currentWave));
    }

    private void InitialiseWaves()
    {
        int wavesCapacity = 20;
        // Initialise spawn amounts for first 20 waves
        int[] spawnAmounts = new int[wavesCapacity];
        for (int i = 0; i < spawnAmounts.Length; i++)
        {
            spawnAmounts[i] = 3 + i;
        }
        // Initialise spawn growths for first 20 waves
        double[] spawnGrowths = new double[wavesCapacity];
        for (int i = 0; i < spawnAmounts.Length; i++)
        {
            spawnGrowths[i] = 1.2 + 0.1 * i;
        }

        // Array of delays
        int[] delays1 = new int[] { 5, 5 };

        // WaveSpawnItem array of bosses
        WaveSpawnItem[] bosses = new WaveSpawnItem[] {
                new WaveSpawnItem(basicBoss, 2),
                new WaveSpawnItem(gunnerBoss, 2),
                new WaveSpawnItem(necromancerBoss, 100),
        };

        // WaveSpawnItem array of basic enemies
        WaveSpawnItem[] minions1 = new WaveSpawnItem[] {
                new WaveSpawnItem(basicEnemy, 7),
                new WaveSpawnItem(basicSniper, 3)
        };

        WaveSpawnItem[] minions2 = new WaveSpawnItem[] {
                new WaveSpawnItem(basicEnemy, 1),
                new WaveSpawnItem(basicSniper, 1)
        };


        waves = new List<Wave>();

        // For first 4 waves make the basic enemy dominant
        for (int i = 0; i < 4; i++)
        {
            waves.Add(new(minions1, bosses, delays1, spawnAmounts[i], spawnGrowths[i]));
        }

        // For first 4 waves make the basic enemy dominant
        for (int i = 4; i < wavesCapacity; i++)
        {
            waves.Add(new(minions2, bosses, delays1, spawnAmounts[i], spawnGrowths[i]));
        }
    }

    IEnumerator StartWave(int waveIndex)
    {
        if (waveIndex >= waves.Count)
        {
            yield break;
        }
        Wave wave = waves[waveIndex];
        int length = wave.delays.Length;
        for (int i = 0; i < length; i++)
        {
            hud.UpdateWave(waveIndex + 1, i);

            // Delay
            if (wave.delays[i] > 0)
            {
                yield return new WaitForSeconds(wave.delays[i]);
            }


            if (i == length - 1)
            {
                // Spawn a boss since we're at the end of the wave
                Transform spawnpoint = GetRandomSpawnPoint();
                Transform boss = wave.GetRandomBoss();
                Instantiate(boss, spawnpoint.position, spawnpoint.rotation);
                hud.ShowBossText(boss.GetComponent<Enemy>().GetMessage(), bossTextDisplayTime);
            }
            else
            {
                // Spawn minions
                for (int j = 0; j < wave.GetSpawnAmount(i); j++)
                {
                    Transform spawnpoint = GetRandomSpawnPoint();
                    Instantiate(wave.getRandomMinion(), spawnpoint.position, spawnpoint.rotation);
                }
            }
        }

        yield return null;
    }

    public void StartNextWave()
    {
        StackTrace stackTrace = new StackTrace();
        // Get calling method name
        print(stackTrace.GetFrame(1).GetMethod().Name);
        currentWave++;
        StartCoroutine(StartWave(currentWave));
    }

    private Transform GetRandomSpawnPoint()
    {
        // Get a random spawn point that is not near the player
        // To prevent an enemy spawning on top of the player
        int random = Random.Range(0, spawnpointCount - 1);
        Transform spawnpoint = transform.GetChild(random);

        if (minSpawnDistance >= Vector3.Distance(player.position, spawnpoint.position))
        {
            // Spawnpoint unsuitable, find another
            return GetRandomSpawnPoint();
        }
        else
            return spawnpoint;
    }
}

[System.Serializable]
public class Wave
{
    public WaveSpawnItem[] minions;
    public int[] delays;
    public WaveSpawnItem[] bosses;
    public int spawnAmount;
    public double spawnGrowth;
    private int minionWeightSum;
    private int bossWeightSum;

    public Wave(WaveSpawnItem[] minions, WaveSpawnItem[] bosses, int[] delays, int spawnAmount, double spawnGrowth)
    {
        this.minions = minions;
        this.delays = delays;
        this.bosses = bosses;
        this.spawnAmount = spawnAmount;
        this.spawnGrowth = spawnGrowth;
        minionWeightSum = 0;
        bossWeightSum = 0;
        foreach (var i in minions)
        {
            minionWeightSum += i.weight;
        }
        foreach (var i in bosses)
        {
            bossWeightSum += i.weight;
        }
    }

    // Gets a random minion taking into account its spawn chance
    public Transform getRandomMinion()
    {
        int random = Random.Range(0, minionWeightSum);
        foreach(var v in minions)
        {
            if (random < v.weight)
            {
                return v.prefab;
            }
            random -= v.weight;
        }
        return null;
    }

    public Transform GetRandomBoss()
    {
        int random = Random.Range(0, bossWeightSum);
        //Debug.Log("Random is " + random + " total weight is " + bossWeightSum);
        foreach (var v in bosses)
        {
            if (random < v.weight)
            {
                return v.prefab;
            }
            random -= v.weight;
        }
        return null;
    }

    public int GetSpawnAmount(int subwave)
    {
        return Mathf.CeilToInt(spawnAmount * Mathf.Pow((float)spawnGrowth, subwave));
    }
}


public class WaveSpawnItem
{
    public Transform prefab;
    public int weight;

    public WaveSpawnItem(Transform prefab, int weight)
    {
        this.prefab = prefab;
        this.weight = weight;
    }
}
