using UnityEngine;

public class PositionWaveSpawner : MonoBehaviour
{
    [Header("Activacion por posicion")]
    public Transform player;
    public float activationX = 68f;

    [Header("Oleada")]
    public GameObject obstaclePrefab;
    public Transform[] spawnPoints;

    private bool hasSpawned;

    private void Update()
    {
        if (hasSpawned || player == null || obstaclePrefab == null)
        {
            return;
        }

        if (player.position.x >= activationX)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        hasSpawned = true;

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
