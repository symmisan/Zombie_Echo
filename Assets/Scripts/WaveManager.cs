using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Zombies")]
    public GameObject zombiePrefab;

    public int startingZombies = 4;
    public int extraZombiesPerWave = 1;
    public float timeBetweenWaves = 3f;

    [Header("Zombie Difficulty")]
    public float baseZombieSpeed = 1.5f;
    public float speedIncreaseEveryFiveWaves = 0.25f;

    [Header("Arena")]
    public Vector2 arenaMin = new Vector2(-11.5f, -11.5f);
    public Vector2 arenaMax = new Vector2(11.5f, 11.5f);

    public float minimumDistanceFromPlayer = 3f;

    private Transform player;
    private int currentWave = 1;
    private bool waitingForNextWave;
    private float waveTimer;

    public int CurrentWave => currentWave;

    void Start()
    {
        // Find the player using the Player tag
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        SpawnWave();
    }

    void Update()
    {
        // Check whether all zombies have been defeated
        if (!waitingForNextWave &&
            FindObjectsByType<ZombieHealth>().Length == 0)
        {
            waitingForNextWave = true;
            waveTimer = timeBetweenWaves;

            Debug.Log("Wave cleared!");
        }

        // Wait before starting the next wave
        if (waitingForNextWave)
        {
            waveTimer -= Time.deltaTime;

            if (waveTimer <= 0f)
            {
                currentWave++;
                waitingForNextWave = false;

                SpawnWave();
            }
        }
    }

    void SpawnWave()
    {
        // Calculate the number of zombies for this wave
        int zombiesThisWave =
            startingZombies +
            (currentWave - 1) * extraZombiesPerWave;

        // Increase zombie speed every five waves
        int speedIncreaseCount = currentWave / 5;

        float currentZombieSpeed =
            baseZombieSpeed +
            speedIncreaseCount *
            speedIncreaseEveryFiveWaves;

        Debug.Log(
            "Starting Wave " +
            currentWave +
            " with " +
            zombiesThisWave +
            " zombies."
        );

        Debug.Log(
            "Zombie Speed: " +
            currentZombieSpeed
        );

        // Spawn the zombies
        for (int i = 0; i < zombiesThisWave; i++)
        {
            Vector2 spawnPosition =
                GetRandomSpawnPosition();

            GameObject newZombie =
                Instantiate(
                    zombiePrefab,
                    spawnPosition,
                    Quaternion.identity
                );

            // Apply the current wave's zombie speed
            ZombieMovement zombieMovement =
                newZombie.GetComponent<ZombieMovement>();

            if (zombieMovement != null)
            {
                zombieMovement.moveSpeed =
                    currentZombieSpeed;
            }
        }

        // Weapon pickups are no longer spawned here.
        // Zombies now have a chance to drop them when they die.
    }

    // Finds a random position inside the arena
    Vector2 GetRandomSpawnPosition()
    {
        for (int attempt = 0; attempt < 30; attempt++)
        {
            float randomX =
                Random.Range(
                    arenaMin.x,
                    arenaMax.x
                );

            float randomY =
                Random.Range(
                    arenaMin.y,
                    arenaMax.y
                );

            Vector2 randomPosition =
                new Vector2(
                    randomX,
                    randomY
                );

            // Keep zombies and pickups away from the player
            if (player == null ||
                Vector2.Distance(
                    randomPosition,
                    player.position
                ) >= minimumDistanceFromPlayer)
            {
                return randomPosition;
            }
        }

        // Backup position if a valid position is not found
        return new Vector2(5f, 5f);
    }
}