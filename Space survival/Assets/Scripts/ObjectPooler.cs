using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
    }

    public GameObject GetPooledObject()
    {
        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        // otherwise, return null   
        return null;
    }

    // If there is any active object return true, else return false
    public bool CheckAnyObjectActive()
    {
        bool anyBall = false;

        foreach (GameObject obj in pooledObjects)
        {
            if (obj.activeInHierarchy == true)
            {
                anyBall = true;
            }
        }

        return anyBall;
    }

    // Deactivate all objects in the pooler
    public void DeactiveAllObjects()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (obj.activeInHierarchy == true)
            {
                obj.SetActive(false);
            }
        }
    }

}
*/

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public Dictionary<string, List<GameObject>> pooledObjects = new Dictionary<string, List<GameObject>>(); // Para manejar varios tipos de objetos
    public GameObject[] objectsToPool; // Para diferentes tipos de objetos
    public int amountToPool = 10; // Puedes tener el mismo valor de cantidad de objetos para cada tipo

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        // Loop through list of pooled objects, deactivating them and adding them to the dictionary
        for (int i = 0; i < objectsToPool.Length; i++)
        {
            pooledObjects.Add(objectsToPool[i].name, new List<GameObject>());
            for (int j = 0; j < amountToPool; j++)
            {
                GameObject obj = Instantiate(objectsToPool[i]);
                obj.SetActive(false);
                pooledObjects[objectsToPool[i].name].Add(obj);
                obj.transform.SetParent(this.transform); // set as children of the pooler
            }
        }
    }

    // GetPooledObject now accepts an object type name to pull the correct object
    public GameObject GetPooledObject(string objectType)
    {
        if (pooledObjects.ContainsKey(objectType))
        {
            for (int i = 0; i < pooledObjects[objectType].Count; i++)
            {
                if (!pooledObjects[objectType][i].activeInHierarchy)
                {
                    return pooledObjects[objectType][i];
                }
            }
        }

        // if no inactive object is found, return null
        return null;
    }

    public bool CheckAnyObjectActive(string objectType)
    {
        if (pooledObjects.ContainsKey(objectType))
        {
            foreach (GameObject obj in pooledObjects[objectType])
            {
                if (obj.activeInHierarchy)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void DeactivateAllObjects(string objectType)
    {
        if (pooledObjects.ContainsKey(objectType))
        {
            foreach (GameObject obj in pooledObjects[objectType])
            {
                if (obj.activeInHierarchy)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}