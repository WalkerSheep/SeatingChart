using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolController : MonoBehaviour
{
    public static ObjectPoolController instance;
    public PoolObject[] objectPool;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private List<ObjectTimer> objectTimers = new List<ObjectTimer>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        foreach (PoolObject poolObject in objectPool)
        {
            for (int i = 0; i < poolObject.amount; i++)
            {
                GameObject spawned = Instantiate(poolObject.prefab, transform);
                spawned.transform.localPosition = Vector3.zero;
                spawned.name = poolObject.prefab.name;
                spawned.SetActive(false);
                spawnedObjects.Add(spawned);
            }

        }
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation, float offTime = Mathf.Infinity)
    {

        GameObject chosen = null;

        foreach (GameObject gameObject in spawnedObjects)
        {
            if (!gameObject.activeSelf && gameObject.name == prefab.name)
            {
                chosen = gameObject;
                break;
            }
        }

        if (!chosen)
        {
            foreach (GameObject gameObject in spawnedObjects)
            {
                if (gameObject.name == prefab.name)
                {
                    chosen = gameObject;
                    break;
                }
            }
        }

        RemoveObjectTimer(chosen);

        chosen.transform.position = position;
        chosen.transform.rotation = rotation;
        chosen.SetActive(true);

        ObjectTimer createdObjectTimer = new ObjectTimer();
        createdObjectTimer.target = chosen;
        createdObjectTimer.timer = offTime;
        objectTimers.Add(createdObjectTimer);

        return chosen;

    }
    
    public void RemoveObjectTimer(GameObject gameObject)
    {
        ObjectTimer found = null;
        foreach (ObjectTimer objectTimer in objectTimers)
        {
            if (objectTimer.target == gameObject)
            {
                found = objectTimer;
                break;
            }
        }
        if (found != null)
        {
            objectTimers.Remove(found);
        }
    }
    
    void FixedUpdate()
    {
        List<ObjectTimer> completedTimers = new List<ObjectTimer>();
        foreach (ObjectTimer objectTimer in objectTimers)
        {
            objectTimer.timer -= Time.fixedDeltaTime;
            if (objectTimer.timer <= 0)
            {
                completedTimers.Add(objectTimer);
            }
        }
        foreach (ObjectTimer objectTimer in completedTimers)
        {
            objectTimer.target.SetActive(false);
            objectTimer.target.transform.SetParent(transform);
            objectTimers.Remove(objectTimer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class PoolObject
{
    public int amount;
    public GameObject prefab;
}

[System.Serializable]
public class ObjectTimer
{
    public float timer;
    public GameObject target;
}
