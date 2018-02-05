using UnityEngine;
using System.Collections;

public class ToggleActiveState : MonoBehaviour
{
    public GameObject itemToToggle;

	// Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        if (itemToToggle.activeSelf == true)
	    {
		    itemToToggle.SetActive(false);
		    Debug.Log("Deactivating.");
	    }
	    else
	    {
		    itemToToggle.SetActive(true);
		    Debug.Log("Activating.");
	    }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
