using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private readonly List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        enemies.Remove(enemy);
    }

    public float TotalMaxHP()
    {
        float total = 0f;

        foreach (Enemy e in enemies)
        {
            if (e != null)
                total += e.maxHP;
        }

        return total;
    }

    public float TotalCurrentHP()
    {
        float total = 0f;

        foreach (Enemy e in enemies)
        {
            if (e != null)
                total += e.currentHP;
        }

        return total;
    }

    public int EnemyCount() => enemies.Count;

    public bool AllEnemiesDead() => enemies.Count == 0;

    public float HealthRatio()
    {
        float max = TotalMaxHP();
        if (max <= 0)
            return 0;
        return TotalCurrentHP() / max;
    }
}
