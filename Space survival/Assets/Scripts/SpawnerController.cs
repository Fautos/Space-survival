using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] List<GameObject> Enemys = new List<GameObject>();
    private LevelManager LevelManager;
    [SerializeField] private int maxEnemysInPlay = 3;
    [SerializeField] private float spawnRate = 3.0f;
    [SerializeField] private Coroutine spawnCoroutine; 

    // Start is called before the first frame update
    void Start()
    {
        LevelManager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();
        //InvokeRepeating("spawnEnemy", 0, 2.5f);
        StartSpawnCoroutine();
    }

    #region Active & deactive
    // When the GameObject is enable the coroutine starts
    private void OnEnable()
    {
        // If there is any enemy left in the spawner it should be destroyed
        if (transform.childCount != 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        StartSpawnCoroutine();
    }

    // When the GameObject is disable the corutine must stop
    private void OnDisable()
    {
        StopSpawnCoroutine();
        
        // If there is any enemy left in the spawner it should be destroyed
        if (transform.childCount != 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }    
    }

    private void StartSpawnCoroutine()
    {
        // If there is no coroutine, it is started
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnEnemyTimer());
        }
    }

    private void StopSpawnCoroutine()
    {
        // If there is a coroutine it is stopped
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
    #endregion

    public void spawnEnemy()
    {
        // If the spawner has less than 5 enemies in play it can still spawn new enemies
        if (transform.childCount < maxEnemysInPlay && LevelManager.isGameActive)
        {
            Instantiate(Enemys[Random.Range(0, Enemys.Count)], transform);
        }
    }

    IEnumerator SpawnEnemyTimer()
    {
        while (gameObject.activeInHierarchy)
        {
            // Timer to spawn a new enemy
            yield return new WaitForSeconds(spawnRate);
            spawnEnemy();
        }
        
    }
    
}
