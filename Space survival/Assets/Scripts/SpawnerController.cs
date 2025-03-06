using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] List<GameObject> Enemys = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("spawnEnemy", 0, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnEnemy()
    {
        Instantiate(Enemys[Random.Range(0, Enemys.Count)], transform.position, Quaternion.identity);
    }
}
