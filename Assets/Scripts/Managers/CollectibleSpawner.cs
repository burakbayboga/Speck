using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{

    public static CollectibleSpawner instance;

    public Collectible[] Collectibles;
    public int MinSmallFryThreshold;
    public int MaxSmallFryThreshold;
    public int InitialSmallFryThreshold;
    public float ScreenFractionOffset;
    public float EdgeBouncerVelocity;
    public bool CollectibleSmallFryCountdownActive;

    int CurrentSmallFryThreshold;
    int CurrentSmallFryCount;

    Camera MainCamera;

    void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        MainCamera = Camera.main;
        CurrentSmallFryThreshold = InitialSmallFryThreshold;
        CurrentSmallFryCount = 0;
        SmallFryManager.instance.OnSmallFrySpawned += CheckCollectibleSpawn;
        CollectibleSmallFryCountdownActive = true;
    }

    void CheckCollectibleSpawn(SmallFry smallFry = null)
    {
        if (!CollectibleSmallFryCountdownActive)
        {
            return;
        }

        CurrentSmallFryCount++;
        if (CurrentSmallFryCount == CurrentSmallFryThreshold)
        {
            SpawnCollectible();
            CurrentSmallFryCount = 0;
            CurrentSmallFryThreshold = Random.Range(MinSmallFryThreshold, MaxSmallFryThreshold + 1);
        }
    }

    void SpawnCollectible()
    {
        Collectible toSpawn = Collectibles[Random.Range(0, Collectibles.Length)];
        Vector3 spawnPoint = GetRandomSpawnPoint();
        Collectible spawned = Instantiate(toSpawn, spawnPoint, Quaternion.identity);
        if (Random.Range(0.0f, 1.0f) < 0.33f)
        {
            spawned.gameObject.AddComponent<EdgeBouncer>();
            spawned.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle.normalized * EdgeBouncerVelocity;
        }

        CollectibleSmallFryCountdownActive = false;
    }

    Vector3 GetRandomSpawnPoint()
    {
        float horizontalOffset = Screen.width / ScreenFractionOffset;
        float spawnX = Random.Range(horizontalOffset, Screen.width - horizontalOffset);
        float verticalOffset = Screen.height / ScreenFractionOffset;
        float spawnY = Random.Range(verticalOffset, Screen.height - verticalOffset);
        Vector2 spawnPointScreenSpace = new Vector2(spawnX, spawnY);
        Vector3 spawnPointWorldSpace = MainCamera.ScreenToWorldPoint(spawnPointScreenSpace);
        spawnPointWorldSpace.Set(spawnPointWorldSpace.x, spawnPointWorldSpace.y, 25.0f);
        return spawnPointWorldSpace;
    }

}
