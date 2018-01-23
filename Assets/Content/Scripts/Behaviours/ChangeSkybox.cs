using UnityEngine;
using System.Collections;

public class ChangeSkybox : MonoBehaviour
{
    public Material newSkybox;

	// Use this for initialization
	void OnTriggerEnter(Collider other)
    {
	    RenderSettings.skybox = newSkybox;
    }

}
