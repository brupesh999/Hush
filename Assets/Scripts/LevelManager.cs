using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static int level;
    public static int wave = 1;
    Dictionary<int, int> wavesPerLevel = new Dictionary<int, int>();
    List<string> levelNames = new List<string>();
    public GameObject basicEnemy;
    public GameObject bossEnemy;
    public GameObject intermEnemy;
    private bool isSpawning = false;
    public EnemyManager enemyManager;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI waveText;

    void Start()
    {
        wavesPerLevel.Add(0, 0);
        wavesPerLevel.Add(1, 1);
        wavesPerLevel.Add(2, 1);
        wavesPerLevel.Add(3, 2);
        wavesPerLevel.Add(4, 1);
        wavesPerLevel.Add(5, 3);
        wavesPerLevel.Add(6, 2);
        wavesPerLevel.Add(7, 4);
        wavesPerLevel.Add(8, 1);
        levelNames = new List<string> { "Do", "Re", "Mi", "Fa", "Sol", "La", "Ti", "Do" };
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"level {level} wave {wave}");
        if (!isSpawning && enemyManager.AllEnemiesDead())
        {
            wave += 1;
            if (wave > wavesPerLevel[level])
            {
                level++;
                wave = 1;
                // Debug.Log($"should be resetting..");
            }
            Trigger();
        }
        levelText.text = $"Level {levelNames[level - 1]}";
        waveText.text = $"Wave {wave}";
    }

    IEnumerator SpawnMonstersWave2()
    {
        isSpawning = true;
        for (int i = 0; i < 3; i++)
        {
            InstantiateMonster(basicEnemy);
            yield return new WaitForSeconds(3f);
        }
        isSpawning = false;
    }

    IEnumerator TiWave2()
    {
        isSpawning = true;
        for (int i = 0; i < 3; i++)
        {
            InstantiateMonster(basicEnemy);
            yield return new WaitForSeconds(3f);
        }
        InstantiateMonster(intermEnemy);
        yield return new WaitForSeconds(3f);
        isSpawning = false;
    }

    IEnumerator TiWave4()
    {
        isSpawning = true;

        for (int i = 0; i < 5; i++)
        {
            if (i % 2 == 0)
            {
                InstantiateMonster(basicEnemy);
            }
            else
            {
                InstantiateMonster(intermEnemy);
            }
            yield return new WaitForSeconds(3f);
        }
        InstantiateMonster(intermEnemy);
        yield return new WaitForSeconds(3f);
        isSpawning = false;
    }

    IEnumerator SpawnBasics(int n)
    {
        for (int i = 0; i < n; i++)
        {
            InstantiateMonster(basicEnemy);
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator SpawnIntermediate(int n)
    {
        for (int i = 0; i < n; i++)
        {
            InstantiateMonster(basicEnemy);
            yield return new WaitForSeconds(5f);
        }
    }

    void Trigger()
    {
        if (level == 1)
        {
            InstantiateMonster(basicEnemy);
        }
        else if (level == 2)
        {
            StartCoroutine(SpawnMonstersWave2());
        }
        else if (level == 3)
        {
            InstantiateMonster(basicEnemy);
            InstantiateMonster(basicEnemy, new Vector3(-3, 0, 0));
        }
        else if (level == 4)
        {
            // wave 1
            InstantiateMonster(intermEnemy);
        }
        else if (level == 5)
        {
            if (wave == 1)
            {
                StartCoroutine(SpawnMonstersWave2());
            }
            if (wave == 2)
            {
                InstantiateMonster(intermEnemy);
                InstantiateMonster(basicEnemy, new Vector3(-3, 0, 0));
            }
            if (wave == 3)
            {
                StartCoroutine(SpawnIntermediate(2));
            }
            // wave 3
        }
        else if (level == 6)
        {
            // wave 1
            StartCoroutine(SpawnBasics(2));
            StartCoroutine(SpawnIntermediate(1));
            // wave 2
            StartCoroutine(SpawnIntermediate(3));
        }
        else if (level == 7)
        {
            StartCoroutine(SpawnIntermediate(1));

            // wave 2
            StartCoroutine(TiWave2());

            // wave 3
            StartCoroutine(SpawnIntermediate(3));

            StartCoroutine(TiWave4());
        }
        else if (level == 8)
        {
            InstantiateMonster(bossEnemy);
        }
    }

    void InstantiateMonster(GameObject monster)
    {
        Instantiate(monster, transform.position, Quaternion.identity);
    }

    void InstantiateMonster(GameObject monster, Vector3 delta)
    {
        Instantiate(monster, transform.position + delta, Quaternion.identity);
    }
}
