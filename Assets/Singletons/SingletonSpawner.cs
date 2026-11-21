using UnityEngine;

public class SingletonSpawner : MonoBehaviour
{
    public static bool spawned;
    public GameObject[] singletons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        foreach (GameObject singleton in singletons)
        {
            Instantiate(singleton, new Vector3(0,0,0), Quaternion.identity);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
