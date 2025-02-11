using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool enemyPool;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 3;
    private int waveNumber = 0;

    [Header("Dificultad Progresiva")]
    public float difficultyMultiplier = 1.1f; // Aumenta enemigos por oleada
    public float minTimeBetweenWaves = 2f; // Tiempo mínimo entre oleadas

    [Header("Efectos y Sonidos")]
    public GameObject spawnEffect; // Efecto de partículas al aparecer un enemigo
    public AudioClip spawnSound; // Sonido de aparición
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            waveNumber++;
            Debug.Log("🌊 Oleada " + waveNumber);

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }

            if (waveNumber % 3 == 0) // Cada 3 oleadas genera un jefe
            {
                SpawnBoss();
            }

            // Dificultad progresiva
            enemiesPerWave = Mathf.RoundToInt(enemiesPerWave * difficultyMultiplier);
            timeBetweenWaves = Mathf.Max(timeBetweenWaves * 0.9f, minTimeBetweenWaves);

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = enemyPool.GetEnemy();
        if (enemy != null)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            PlaySpawnEffect(spawnPoint.position);
        }
    }

    void SpawnBoss()
    {
        GameObject boss = enemyPool.GetBoss();
        if (boss != null)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            boss.transform.position = spawnPoint.position;
            boss.SetActive(true);
            PlaySpawnEffect(spawnPoint.position);
            Debug.Log("👹 ¡Jefe generado!");
        }
    }

    void PlaySpawnEffect(Vector3 position)
    {
        if (spawnEffect != null)
            Instantiate(spawnEffect, position, Quaternion.identity);

        if (spawnSound != null && audioSource != null)
            audioSource.PlayOneShot(spawnSound);
    }
}
