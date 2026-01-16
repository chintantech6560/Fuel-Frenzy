using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField]private float minX;
    [SerializeField]private float maxX;
    [SerializeField]private float spawnY;
    [SerializeField]private float padding = 1f;

    [Header("Timing")]
    [SerializeField]private float baseSpawnDelay = 2f;
    private float nextSpawnTime;

    [Header("Pool Tags")]
   [SerializeField]public string[] obstacleTags = { "Obstacle_1", "Obstacle_3", "Obstacle_4" };
   [SerializeField]public string fuelTag = "Fuel";

    [Range(0, 100)]
    [SerializeField]public int fuelChance = 20;

    void Start()
    {
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;

        minX = -halfWidth + padding;
        maxX = halfWidth - padding;

        spawnY = cam.orthographicSize + 3f;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && GameManager.instance.currentState == GameState.Playing)
        {
            SpawnObject();
            CalculateNextSpawnTime();
        }
    }

    void SpawnObject()
    {
        string tagToSpawn;
        if (Random.Range(0, 100) < fuelChance)
        {
            tagToSpawn = fuelTag;
        }
        else
        {
            tagToSpawn = obstacleTags[Random.Range(0, obstacleTags.Length)];
        }

        Vector3 spawnPos = GetValidSpawnPosition();

        if (spawnPos != Vector3.zero && GameManager.instance.currentState == GameState.Playing)
        {
            ObjectPooler.Instance.SpawnFromPool(tagToSpawn, spawnPos, Quaternion.identity);
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(minX, maxX);
            Vector3 potentialPos = new Vector3(randomX, spawnY, 0);
            if (!Physics2D.OverlapCircle(potentialPos, 1.2f))
            {
                return potentialPos;
            }
        }

        return Vector3.zero;
    }

    void CalculateNextSpawnTime()
    {
        float speed = GameManager.instance.globalSpeed;
        if (speed <= 0) speed = 1f;

        float adjustedDelay = baseSpawnDelay / (speed * 0.2f);
        adjustedDelay = Mathf.Clamp(adjustedDelay, 0.5f, 3f);

        nextSpawnTime = Time.time + adjustedDelay;
    }
}