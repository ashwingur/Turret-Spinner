using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int currentWave;
    [SerializeField] private HUD hud;
    [SerializeField] private float bossTextDisplayTime;
    private List<Wave> waves;
    private int spawnpointCount;

    [Header("Enemy Prefabs")]
    [SerializeField] private Transform basicEnemy;
    [SerializeField] private Transform basicSniper;
    [SerializeField] private Transform basicBoss;


    // Start is called before the first frame update
    void Start()
    {
        spawnpointCount = transform.childCount;
        InitialiseWaves();
        StartCoroutine(StartWave(currentWave));
    }

    private void InitialiseWaves()
    {
        waves = new List<Wave>();
        Wave wave1 = new(new WaveSpawnItem[] {
                new WaveSpawnItem(basicEnemy, 70),
                new WaveSpawnItem(basicSniper, 30)
            }, 
            new WaveSpawnItem[] {
                new WaveSpawnItem(basicBoss, 1),
            },
            new int[] { 2, 5, 5, 5, 5 }, 3, 1.4);
        waves.Add(wave1);

        Wave wave2 = new(new WaveSpawnItem[] {
                new WaveSpawnItem(basicEnemy, 10),
                new WaveSpawnItem(basicSniper, 90)
            },
            new WaveSpawnItem[] {
                new WaveSpawnItem(basicBoss, 1),
            },
            new int[] { 3, 10, 10, 10 }, 5, 1.5);
        waves.Add(wave2);
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


            if (i == length - 1 && wave.bosses.Length != 0)
            {
                // Spawn a boss since we're at the end of the wave
                int random = Random.Range(0, spawnpointCount - 1);
                Transform spawnpoint = transform.GetChild(random);
                Transform boss = wave.GetRandomBoss();
                Instantiate(boss, spawnpoint.position, spawnpoint.rotation);
                hud.ShowBossText(boss.GetComponent<Enemy>().GetMessage(), bossTextDisplayTime);
            }
            else
            {
                // Spawn minions
                for (int j = 0; j < wave.GetSpawnAmount(i); j++)
                {
                    int random = Random.Range(0, spawnpointCount - 1);
                    Transform spawnpoint = transform.GetChild(random);
                    Instantiate(wave.getRandomMinion(), spawnpoint.position, spawnpoint.rotation);
                }

                if (i == length - 1)
                {
                    StartNextWave();
                }
            }
        }

        yield return null;
    }

    public void StartNextWave()
    {
        currentWave++;
        StartCoroutine(StartWave(currentWave));
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
            minionWeightSum += i.weight;
        }
    }

    // Gets a random minion taking into account its spawn chance
    public Transform getRandomMinion()
    {
        int random = Random.Range(0, minionWeightSum - 1);
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
        int random = Random.Range(0, bossWeightSum - 1);
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
