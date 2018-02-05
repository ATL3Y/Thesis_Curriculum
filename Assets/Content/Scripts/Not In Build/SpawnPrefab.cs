using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Remember to break this by renaming the class
public class SpawnPrefab : MonoBehaviour
{

    public GameObject PrefabToSpawn;
    public List<GameObject> prefabs = new List<GameObject>();

    public int NumberToSpawn;

    public bool RandomLocation = false;
    public float MinXSpawnLocation;
    public float MaxXSpawnLocation;
    public float MinYSpawnLocation;
    public float MaxYSpawnLocation;
    public float MinZSpawnLocation;
    public float MaxZSpawnLocation;

    public GameObject explosionPrefab; // Mispell a variable name and include that as an error

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (prefabs.Count < NumberToSpawn)
        {
            InstantiatePrefab();
        }
    }

    
    void InstantiatePrefab()
    {
        Vector3 prefabPosition = new Vector3 (0, 0, 0);
        GameObject newGameObject;

        if (RandomLocation == true)
        {
            prefabPosition = GenerateRandomPosition();
        }
        newGameObject = Instantiate(PrefabToSpawn, prefabPosition, Quaternion.identity);
        prefabs.Add(newGameObject);

    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(MinXSpawnLocation, MaxXSpawnLocation),
                                        Random.Range(MinYSpawnLocation, MaxYSpawnLocation),
                                        Random.Range(MinZSpawnLocation, MaxZSpawnLocation));

        return randomPosition;
    }

}
