using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    #region private data

    [SerializeField] ParticleSystem fx_shrink;
    private bool debug = true;
    private bool shrink = false;
    private float speedMult = 1.5f;
    private float currentShrinkYLimit;
    private float finalShrinkLimit = 1.0f;
    private float shrinkMult = 0.7f;

    #endregion

    #region public functions

    public void OnHit ( )
    {
        if ( debug ) Debug.Log ( "In Shrink OnHit." );
        currentShrinkYLimit = shrinkMult * transform.localScale.y;
        fx_shrink.Play();
        shrink = true;
    }

    #endregion

    #region private functions

    void Update ( )
    {
        if ( shrink )
        {
            float speed = speedMult * Time.deltaTime;

            // If we're less than the final shrink limit, shrink the rest of the way and deactivate. 
            if ( transform.localScale.y < finalShrinkLimit )
            {
                speed *= 3.0f;
                transform.localScale *= ( 1.0f - speed );

                if ( transform.localScale.y < 0.1f )
                {
                    if( debug) Debug.Log ( "Registering a hit T." );
                    GameLord.instance.OpponentLord.RegisterHitT ( );
                    gameObject.SetActive ( false );
                }
                return;
            }

            // If we're still larger than the final shrink limit, shrink partially.
            transform.localScale *= ( 1.0f - speed );
            if ( debug ) Debug.Log ( "Shrinking." );

            if ( transform.localScale.y < currentShrinkYLimit )
            {
                if ( debug ) Debug.Log ( "Stopping shrinking." );
                shrink = false;
                fx_shrink.Stop ( );
            }
        }
    }

    #endregion
}
