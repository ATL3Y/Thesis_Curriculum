using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTestSphere : MonoBehaviour
{
    public SpawnPrefab mySpawner;
    private float timer = 1.0f;
    private bool startTimer = false;
    public ParticleSystem fx_destroy;


    public void OnCollisionEnter ( Collision collision )
    {
        Debug.Log ( "colliding" );
        mySpawner.RemoveMe ( this );
        startTimer = true;
        fx_destroy.Play ( );
    }

    private void Update ( )
    {
        if ( startTimer )
        {
            timer -= Time.deltaTime;
            if ( timer < 0.0f )
            {
                Debug.Log ( "destroying" );
                
                Destroy ( this.gameObject );
            }
        }
    }
}
