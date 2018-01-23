using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillCircuit : MonoBehaviour
{
    #region private data

    [SerializeField] Renderer[] rends;
	private Material[] mats;
    private bool fill;
    private float fillAmount;
    private float speed = 0.3f;
    private float acceleration = 0.02f;
    private float flickerTimer = 0.0f;
    private float pulseTimer = 0.0f;
    private bool debug = false;

    #endregion

    #region public data

    #endregion

    #region public functions

    public void FillTo ( float health, float mySpeed = 0.3f )
    {
        if( debug ) Debug.Log ( "in fillcircuit fillto" );
        fillAmount = health;
        fill = true;
        speed = mySpeed;
    }

    public void FlickerFor ( float duration )
    {
        flickerTimer = duration;
    }

    public void PulseFor ( float duration )
    {
        pulseTimer = duration;
    }

    #endregion

    #region private functions

    private void Fill ( )
    {
        float deltaTime = Time.deltaTime;

        float currentAmount = 0.0f;

        if ( mats.Length > 0 )
        {
            currentAmount = mats [ 0 ].GetFloat ( "_FillAmount" );
        }

        // If we are outside .01f of the target, fill 
        if( currentAmount < fillAmount - .01f || currentAmount > fillAmount + .01f )
        {
            if ( debug ) Debug.Log ( "in fill conditional currAmt is " + currentAmount + " and fillAmt is " + fillAmount );
            // Debug.Log ( "filling speed " + speed + " amount " + amount );
            speed += acceleration * deltaTime;
            currentAmount += Mathf.Sign ( fillAmount - currentAmount ) * speed * deltaTime;

            for ( int i = 0; i < mats.Length; i++ )
            {
                if ( debug ) Debug.Log ( "setting mats to " + currentAmount );
                mats [ i ].SetFloat ( "_FillAmount", currentAmount );
            }
        }
        else
        {
            fill = false;
        }
    }

    private void Flicker ( )
    {
        float flicker = Random.Range( 0.0f, 1.0f ); 
        for ( int i = 0; i < mats.Length; i++ )
        {
            mats [ i ].SetFloat ( "_Flicker", flicker );
        }
    }

    private void Pulse ( )
    {

    }

    private void ResetFlicker ( )
    {
        for ( int i = 0; i < mats.Length; i++ )
        {
            mats [ i ].SetFloat ( "_Flicker", 0.0f );
        }
    }

    #endregion

    #region inherited functions

    void Update ()
    {
        if ( pulseTimer > 0.0f )
        {
            pulseTimer -= Time.deltaTime;
            Pulse ( );
            if( pulseTimer <= 0.0f )
            {
                pulseTimer = 0.0f;
            }
        }

        if ( flickerTimer > 0.0f )
        {
            flickerTimer -= Time.deltaTime;

            if( Mathf.Sin( 3.0f * Time.timeSinceLevelLoad ) > 0.7f )
            {
                Flicker ( );
            }
            
            if ( flickerTimer <= 0.0f )
            {
                ResetFlicker ( );
                flickerTimer = 0.0f;
            }
        }

        if ( fill )
        {
            Fill ( );
        }
	}

    void Start ( )
    {
        if( rends.Length == 0 )
        {
            mats = new Material [ 1 ];
            if ( debug ) Debug.Log ( mats.Length );
            mats [ 0 ] = GetComponent<Renderer> ( ).material;
        }
        else
        {
            mats = new Material [ rends.Length ];
            if ( debug ) Debug.Log ( mats.Length );
            for ( int i = 0; i < mats.Length; i++ )
            {
                mats [ i ] = rends [ i ].material;
            }
        }

        fillAmount = 0.0f;
        fill = true;
    }

    #endregion
}
