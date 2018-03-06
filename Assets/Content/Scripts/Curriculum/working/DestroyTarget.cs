using UnityEngine;
using System.Collections;

public class DestroyTarget : MonoBehaviour
{
    public GameObject itemToDestroy;

    void OnTriggerEnter(Collider other)
    {
        //Destroy the object specified in the Inspector
        Destroy(itemToDestroy);

        //Destroy the trigger (if you only want it to happen once)
        Destroy(this);
    }
}
