using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_m: MonoBehaviour
{
    #region public data

    public DrawingController_m drawController { get; set; }

    #endregion

    #region private data

    private bool debug = true;
    private TrailRenderer rend;

    #endregion

    #region public functions

    public void TurnTrailOff ( )
    {
        rend.enabled = false;
    }

    public void TurnTrailOn ( )
    {
        rend.enabled = true;
    }

    public void ChangeTrailColor( Color col )
    {
        rend.startColor = col;
        rend.endColor = col;
    }

    #endregion

    #region private functions

    private void SelfDestruct ( )
    {
        drawController.RemoveDraw ( this );
        Destroy ( this );
    }

    #endregion

    #region inherited functions

    // Update is called once per frame
    private void Update ( )
    {

    }

    // Use this for initialization
    private void Start ( )
    {
        rend = GetComponent<TrailRenderer> ( );
        if( rend == null )
        {
            Debug.LogError ( "This needs a line renderer." );
        }
        ChangeTrailColor ( 1.2f * new Color ( Random.value, Random.value, Random.value, 1.0f ) );
    }

    #endregion
}
