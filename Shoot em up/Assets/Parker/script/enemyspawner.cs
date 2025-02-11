using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public int poolSize = 10;

    private List<GameObject> enemyPool;
    private List<GameObject> bossPool;

    void Awake()
    {
        enemyPool = new List<GameObject>();
        bossPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }

        for (int i = 0; i < poolSize / 2; i++) // Menos jefes en el pool
        {
            GameObject boss = Instantiate(bossPrefab);
            boss.SetActive(false);
            bossPool.Add(boss);
        }
    }

    public GameObject GetEnemy()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        // Si todos están ocupados, crear uno nuevo (puede ajustarse según rendimiento)
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(false);
        enemyPool.Add(newEnemy);
        return newEnemy;
    }

    public GameObject GetBoss()
    {
        foreach (var boss in bossPool)
        {
            if (!boss.activeInHierarchy)
            {
                return boss;
            }
        }

        GameObject newBoss = Instantiate(bossPrefab);
        newBoss.SetActive(false);
        bossPool.Add(newBoss);
        return newBoss;
    }
}
