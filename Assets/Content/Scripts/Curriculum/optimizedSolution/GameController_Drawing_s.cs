using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_Drawing_s : MonoBehaviour
{
    #region public data

    public static GameController_Drawing_s instance;
    public VRNodeLord player;

    #endregion

    #region private data

    private bool debug = true;
    private float displayTimer;
    private float hideTimer;
    private float delay = 2.0f;

    #endregion

    #region private functions

    // Use this for initialization
    private void Start ( )
    {
        instance = this;
        displayTimer = delay;
        hideTimer = delay;
    }

    // Update is called once per frame
    private void Update ( )
    {
        displayTimer -= Time.deltaTime;
        hideTimer -= Time.deltaTime;

        if( displayTimer < 0.0f )
        {
            if ( player.leftHand.Trigger > 0.99f )
            {
                if ( debug ) Debug.Log ( "Call display" );
                DrawingController_s.instance.DisplayDraw ( );
                displayTimer = delay;
                if ( debug ) Debug.Log ( "new displayTimer: " + displayTimer );
            }
        }
        
        if( hideTimer < 0.0f )
        {
            if ( player.rightHand.Trigger > 0.99f )
            {
                if ( debug ) Debug.Log ( "Call hide" );
                DrawingController_s.instance.HideDraw ( );
                hideTimer = delay;
                if ( debug ) Debug.Log ( "new hideTimer: " + hideTimer );
            }
        }

    }

    #endregion
}
