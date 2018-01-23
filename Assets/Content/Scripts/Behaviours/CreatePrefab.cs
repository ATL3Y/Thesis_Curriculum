using UnityEngine;
using System.Collections;

public class CreatePrefab : MonoBehaviour
{

    public GameObject itemToCreate;
    public Vector3 itemPosition;
    public Vector3 itemRotation;


    void Start()
    {
        // set the position of the prefab created relative to the triggering object
        itemPosition.x += transform.position.x;
        itemPosition.y += transform.position.y;
        itemPosition.z += transform.position.z;
        Debug.Log(itemToCreate);
    }

    void OnTriggerEnter(Collider other)
    {
        //Create a prefab
        // Instantiate(itemToCreate, itemPosition, Quaternion.Euler(itemRotation.x, itemRotation.y, itemRotation.z));
	    Instantiate(itemToCreate, itemPosition, Quaternion.Euler(itemRotation.x, itemRotation.y, itemRotation.z));

    	//Destroy the trigger (if you only want it to happen once)
	    Destroy(this);
    }
}
