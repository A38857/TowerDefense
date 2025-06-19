using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class EnermySpawnConfig
{
    public GameObject prefab;
    public float weight;
}

public class EnermySpawner : MonoBehaviour
{
    public static EnermySpawner main;

    [SerializeField] private GameObject[] EnermyPrefabList;
    [SerializeField] private int BaseEnermy = 8;
    [SerializeField] private float EnermyPerSecond = 0.3f;
    [SerializeField] private float TimeBetweenWave = 1f;
    [SerializeField] private float DifficultyScalingFactor = 0.75f;
    [SerializeField] private Slider WaveBar;

    private int CurrentWave = 0;
    private int CountWave = 1;
    private float TimeSinceLastSpawn;
    private int EnermyAlive;
    private int EnermyLeftToSpawn;
    private bool IsSpawning = false;
    private Vector3 currentWaveStartPos;
    private List<List<Transform>> currentWavePaths = new List<List<Transform>>();

    public static UnityEvent OnEnermyDestroy = new UnityEvent();

    private void Awake()
    {
        OnEnermyDestroy.AddListener(EnermyDestroy);
    }

    private void Start()
    {
        main = this;
        CountWave = LeverManager.main.CountWave;
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (CurrentWave == CountWave && EnermyLeftToSpawn == 0 && EnermyAlive == 0 && !LeverManager.main.GetWin() && !LeverManager.main.GetLose())
        {
            LeverManager.main.SetWin(true);
        }

        if (LeverManager.main.GetWin() || LeverManager.main.GetLose()) return;
        if (!IsSpawning || (CurrentWave == CountWave && EnermyLeftToSpawn == 0)) return;

        TimeSinceLastSpawn += Time.deltaTime;
        if (TimeSinceLastSpawn >= 1f / EnermyPerSecond && EnermyLeftToSpawn > 0)
        {
            SpawnEnermy();
            EnermyLeftToSpawn--;
            EnermyAlive++;
            TimeSinceLastSpawn = 0;
        }

        if (EnermyAlive == 0 && EnermyLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(TimeBetweenWave);

        IsSpawning = true;
        CurrentWave++;
        EnermyLeftToSpawn = EnermyPerWave();
        WaveBar.value = (float)(CurrentWave) / CountWave;

        var allPaths = LeverManager.main.ListPath;
        var randomPath = allPaths[Random.Range(0, allPaths.Count)];

        currentWaveStartPos = randomPath[0].position;

        currentWavePaths = allPaths.FindAll(path =>
            path.Count > 0 &&
            Vector2.Distance(path[0].position, currentWaveStartPos) < 0.01f
        );
    }

    private void EndWave()
    {
        IsSpawning = false;
        TimeSinceLastSpawn = 0;
        StartCoroutine(StartWave());
    }

    private void EnermyDestroy()
    {
        EnermyAlive--;
    }

    private void SpawnEnermy()
    {
        var config = GetSpawnConfigForWave(CurrentWave);
        GameObject prefab = GetRandomEnermyPrefab(config);
        var chosenPath = currentWavePaths[Random.Range(0, currentWavePaths.Count)];
        GameObject enemy = Instantiate(prefab, chosenPath[0].position, Quaternion.identity);
        enemy.transform.localScale = new Vector3(
            LeverManager.main.Scale * enemy.transform.localScale.x,
            LeverManager.main.Scale * enemy.transform.localScale.y,
            1
        );
        enemy.GetComponent<Enermy>().SetPath(chosenPath);
    }

    private int EnermyPerWave()
    {
        return Mathf.RoundToInt(BaseEnermy * Mathf.Pow(CurrentWave, DifficultyScalingFactor));
    }

    public int GetEnermyALive()
    {
        return EnermyAlive;
    }

    // Enemy per wave
    private List<EnermySpawnConfig> GetSpawnConfigForWave(int wave)
    {
        List<EnermySpawnConfig> config = new List<EnermySpawnConfig>();

        if (wave <= 2)
        {
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[0], weight = 0.7f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[1], weight = 0.3f });
        }
        else if (wave <= 4)
        {
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[0], weight = 0.1f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[1], weight = 0.15f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[2], weight = 0.35f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[3], weight = 0.4f });
        }
        else
        {
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[0], weight = 0.05f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[1], weight = 0.1f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[2], weight = 0.15f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[3], weight = 0.2f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[4], weight = 0.25f });
            config.Add(new EnermySpawnConfig { prefab = EnermyPrefabList[5], weight = 0.25f });
        }

        return config;
    }

    private GameObject GetRandomEnermyPrefab(List<EnermySpawnConfig> config)
    {
        float totalWeight = 0f;
        foreach (var e in config) totalWeight += e.weight;

        float rand = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var e in config)
        {
            cumulative += e.weight;
            if (rand <= cumulative)
                return e.prefab;
        }

        return config[config.Count - 1].prefab; // fallback
    }
}
