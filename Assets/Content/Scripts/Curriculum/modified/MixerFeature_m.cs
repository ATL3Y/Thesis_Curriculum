using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerFeature_m: MonoBehaviour
{
    #region public data

    public MixerController_m mixerController { get; set; }

    #endregion

    #region protected data

    protected bool debug = true;
    protected float currentFillAmount; 

    #endregion

    #region private data

    [SerializeField] Renderer[] rends;
    private Material[] mats;
    private float speed = 0.3f;
    private float acceleration = 0.02f;
    private float targetFillAmount;

    #endregion

    #region public functions

    #endregion

    #region private functions

    private void Fill ( )
    {
        float deltaTime = Time.deltaTime;

        if ( mats.Length > 0 )
        {
            currentFillAmount = mats [ 0 ].GetFloat ( "_FillAmount" );
        }

        // If we are outside .01f of the target, fill 
        if ( currentFillAmount < targetFillAmount - .01f || currentFillAmount > targetFillAmount + .01f )
        {
            if ( debug ) Debug.Log ( "filling currentAmount " + currentFillAmount + " to fillAmount " + targetFillAmount );
            speed += acceleration * deltaTime;
            currentFillAmount += Mathf.Sign ( targetFillAmount - currentFillAmount ) * speed * deltaTime;

            for ( int i = 0; i < mats.Length; i++ )
            {
                mats [ i ].SetFloat ( "_FillAmount", currentFillAmount );
            }
        }
    }

    #endregion

    #region virtual functions

    public virtual void FillTo ( float fillAmount )
    {
        targetFillAmount = Mathf.Clamp01 ( fillAmount );
        Fill ( );
    }

    // Update is called once per frame
    private void Update ( )
    {

    }

    // Use this for initialization
    private void Start ( )
    {
        mixerController = FindObjectOfType<MixerController_m> ( );
        if( mixerController == null )
        {
            Debug.LogError ( "Cannot find mixer controller." );
        }
        else
        {
            mixerController.AddMe ( this );
        }

        currentFillAmount = 0.0f;

        if ( rends.Length == 0 )
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
    }

    #endregion
}
