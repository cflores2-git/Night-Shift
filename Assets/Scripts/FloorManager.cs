using UnityEngine;
using TMPro;

public class FloorManager : MonoBehaviour
{
    public Transform player;
    public Enemies lowEnemyPrefab;
    public Enemies highEnemyPrefab;
    
    public Transform lowSpawnPoint;
    public Transform highSpawnPoint;

    public float spawnInterval = 3f;
    public float enemySpeed = 1.5f;
    public int spawnLimit = 10;

    private int spawned;
    
    public TMP_Text scoreText;
    private int cleaned;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCounter();
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        if (spawned >= spawnLimit)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }
        bool spawnHigh = spawned % 2 == 1;
        Enemies prefab = spawnHigh ? highEnemyPrefab : lowEnemyPrefab;
        Transform spawnPoint = spawnHigh ? highSpawnPoint : lowSpawnPoint;

        Enemies enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        enemy.player = player;
        enemy.moveSpeed = enemySpeed;
        enemy.floorManager = this;

        spawned++;
    }

    public bool AllCleaned()
    {
        return cleaned >= spawnLimit;
    }
    public void AddCleaned()
    {
        cleaned++;
        UpdateCounter();
    }
    void UpdateCounter()
    {
        scoreText.text = "Mess Cleaned: " + cleaned + " / " +spawnLimit;
        if((AllCleaned()))
        {
            scoreText.text += " - Exit open";
        }
    }

}
