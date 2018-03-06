using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
	
	public GameObject followThis;
	public float distance = 5f;
	public float smoothing = 10f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Vector3.Distance(followThis.transform.position, transform.position) > distance) {
			StartCoroutine(follow());
		}
		
	}

	IEnumerator follow () {
		while (Vector3.Distance(followThis.transform.position, transform.position) > distance) {
			transform.position = Vector3.Lerp(transform.position, followThis.transform.position, Time.deltaTime / (10f * 2));
			yield return null;
		}

	}

}
