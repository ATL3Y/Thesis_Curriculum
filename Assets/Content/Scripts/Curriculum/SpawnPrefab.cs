using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Remember to break this by renaming the class
public class SpawnPrefab : MonoBehaviour
{

    public GameObject PrefabToSpawn;
    public List<CollisionTestSphere> colTestSpheres = new List<CollisionTestSphere>();

    public int NumberToSpawn;

    public bool RandomLocation = false;
    public float MinXSpawnLocation;
    public float MaxXSpawnLocation;
    public float MinYSpawnLocation;
    public float MaxYSpawnLocation;
    public float MinZSpawnLocation;
    public float MaxZSpawnLocation;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if ( colTestSpheres.Count < NumberToSpawn)
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
        CollisionTestSphere colTestInstance = newGameObject.GetComponent<CollisionTestSphere>();
        if(colTestInstance != null )
        {
            colTestInstance.mySpawner = this;
            colTestSpheres.Add ( colTestInstance );
        }
    }

    public void RemoveMe ( CollisionTestSphere colTestInstance )
    {
        colTestSpheres.Remove ( colTestInstance );
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(MinXSpawnLocation, MaxXSpawnLocation),
                                        Random.Range(MinYSpawnLocation, MaxYSpawnLocation),
                                        Random.Range(MinZSpawnLocation, MaxZSpawnLocation));

        return randomPosition;
    }

}
