using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    #region public data

    public DrawingController drawController { get; set; }

    #endregion

    #region private data

    private bool debug = true;
    private LineRenderer rend;

    #endregion

    #region public functions

    public void TurnLineOff ( )
    {
        rend.enabled = false;
    }

    public void TurnLineOn ( )
    {
        rend.enabled = true;
    }

    public void ChangeLineColor( Color col )
    {
        rend.material.color = col;
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
        rend = GetComponent<LineRenderer> ( );
        if( rend == null )
        {
            Debug.LogError ( "This needs a line renderer." );
        }
    }

    #endregion
}
