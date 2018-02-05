using UnityEngine;
using System.Collections;

public class Translate : MonoBehaviour
{

	public float speed = 0.1f;
	
	// Update is called once per frame
	void Update ()
    {
		transform.Translate(Vector3.up * speed);
	}
}
