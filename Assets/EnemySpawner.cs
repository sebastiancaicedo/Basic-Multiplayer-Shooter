using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public int numberOfEnemies;
    [Header("Potions")]
    public GameObject healthPotionPrefab;
    public int numberOfPotions;
    public Transform[] potionsSpawnPoints;

    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-8.0f, 8.0f), 0.0f, Random.Range(-8.0f, 8.0f));
            var spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);
            var enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);

            NetworkServer.Spawn(enemy);
        }

        for (int i = 0; i < numberOfPotions; i++)
        {
            var spawnPoint = potionsSpawnPoints[Random.Range(0, potionsSpawnPoints.Length)];
            var potion = Instantiate(healthPotionPrefab, spawnPoint.position, Quaternion.identity);
            NetworkServer.Spawn(potion);
        }
    }
}
