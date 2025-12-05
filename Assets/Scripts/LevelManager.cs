using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static int level = 0;
    public static int wave = 1;

    public static void Restart()
    {
        wave = 1;
        level = 0;
    }

    Dictionary<int, int> wavesPerLevel = new Dictionary<int, int>();
    List<string> levelNames = new List<string>();
    public GameObject basicEnemy;
    public GameObject bossEnemy;
    public GameObject intermEnemy;
    private GameObject player;
    private bool isSpawning = false;
    public EnemyManager enemyManager;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI waveText;
    private List<int> fullHPwaves = new List<int>();
    private List<int> halfHPwaves = new List<int>();

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        //add levels to HPwaves lists corresponding to how much HP players get when completing that level
        fullHPwaves.Add(1);
        fullHPwaves.Add(2);
        fullHPwaves.Add(7);

        halfHPwaves.Add(3);
        halfHPwaves.Add(4);
        halfHPwaves.Add(5);
        halfHPwaves.Add(6);

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

    private bool isWaitingForNextLevel = false;

    IEnumerator DelayTrigger()
    {
        isWaitingForNextLevel = true;
        yield return new WaitForSeconds(8f);
        Trigger();
        isWaitingForNextLevel = false;
    }

    void Update()
    {
        if (!isSpawning && enemyManager.AllEnemiesDead())
        {
            wave += 1;
            if (wave > wavesPerLevel[level])
            {
                //heal player
                AddPlayerHP(level);
                level++;
                wave = 1;

                // Debug.Log($"should be resetting..");
            }
            Trigger();
        }
        if (level == 0)
        {
            levelText.text = "Level Do";
        } else {
            levelText.text = $"Level {levelNames[level - 1]}";
        }
        waveText.text = $"Wave {wave}";
    }

    private void AddPlayerHP(int level)
    {
        //given level just beaten, adds corresponding HP to player as reward

        float maxHPtoAdd = 0;
        float playerMaxHP = player.GetComponent<Player>().maxHP;
        float playerCurrentHP = player.GetComponent<Player>().currentHP;

        //find full player HP or half player HP
        if (fullHPwaves.Contains(level))
        {
            maxHPtoAdd = player.GetComponent<Player>().maxHP;
        }
        else if (halfHPwaves.Contains(level))
        {
            maxHPtoAdd = player.GetComponent<Player>().maxHP / 2;
        }

        //figure out actual amount to add (to not exceed max)
        if (playerCurrentHP + maxHPtoAdd <= playerMaxHP)
        {
            //add full amount if it does not exceed max
            player.GetComponent<Player>().currentHP += maxHPtoAdd;
        }
        else
        {
            //if possible amount exceeds max, set player HP to max
            player.GetComponent<Player>().currentHP = playerMaxHP;
        }

        // Debug.Log("Added HP to player. current HP is now"+player.GetComponent<Player>().currentHP);
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
        GameObject spawnedEnemy = Instantiate(monster, transform.position, Quaternion.identity);
        spawnedEnemy.GetComponent<Enemy>().spawnPoint = transform.position;
    }

    void InstantiateMonster(GameObject monster, Vector3 delta)
    {
        Vector3 addedPos = transform.position + delta;
        Debug.Log(addedPos);
        GameObject spawnedEnemy = Instantiate(
            monster,
            transform.position + delta,
            Quaternion.identity
        );
        spawnedEnemy.GetComponent<Enemy>().spawnPoint = transform.position + delta;
    }
}
