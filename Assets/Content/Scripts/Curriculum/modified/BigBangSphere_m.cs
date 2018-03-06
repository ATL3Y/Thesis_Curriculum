using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBangSphere_m: MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Fix or fill this in (#ATL).
        if ( transform.position.y < 3.0f )
        {
            Destroy( this.gameObject );
        }
	}
}
