using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingController_s : MonoBehaviour
{
    #region public data

    public static DrawingController_s instance;

    #endregion

    #region private data
    private Draw_s[] draws;  // #ATL
    private int constraint = 5;
    [SerializeField] GameObject prefab;
    private int countShown;

    public float baseRange = 2.0f;
    public float sinRange = 1.6f;

    private bool debug = false;

    #endregion

    #region public functions

    public void DisplayDraw ( )
    {
        if( countShown < constraint )
        {
            if ( debug ) Debug.Log ( "display " + countShown );
            draws [ countShown ].TurnTrailOn ( ); // index countShown is the next
            countShown++;
            if ( debug ) Debug.Log ( "new countShown: " + countShown );
        }
    }

    public void HideDraw ( )
    {
        if( countShown > 0 )
        {
            if ( debug ) Debug.Log ( "hide " + countShown );
            draws [ countShown - 1 ].TurnTrailOff ( ); // index countShown - 1 is the current
            countShown--;
            if ( debug ) Debug.Log ( "new countShown: " + countShown );
        }
    }

    // #ATL
    public void RemoveDraw ( Draw_s draw )
    {
        // Note that the array is still the same size.
        for ( int i = 0; i < draws.Length; i++ )
        {
            if( draws[i] == draw )
            {
                draws [ i ] = null;
            }
        }

        // Observe which now have values vs which are null 
        for ( int i = 0; i < draws.Length; i++ )
        {
            if ( debug ) Debug.Log ( "index " + i + " value is " + draws [ i ] );
        }
    }

    // #ATL
    public Draw_s CreateDraw ( float i = 1.0f )
    {
        GameObject newGameObject = Instantiate( prefab );
        Draw_s newDraw = newGameObject.GetComponent<Draw_s>();
        newDraw.drawController = this;
        newDraw.transform.SetParent ( this.transform );
        newDraw.transform.localPosition = Vector3.up * i * .1f;
        return newDraw;
    }

    #endregion

    #region inherited functions

    private void Update ( )
    {
        // update this position to oscillate towards and from the player's gaze
        Transform playerTrans = PlayerCurriculum.instance.GetHead();
        Vector3 pos = playerTrans.position + playerTrans.forward * baseRange;
        Quaternion rot = Quaternion.Inverse(playerTrans.rotation);
        pos += new Vector3 ( 0, 0, sinRange * Mathf.Sin ( Time.timeSinceLevelLoad * .5f ) );
        transform.position = Vector3.Lerp ( transform.position, pos, Time.deltaTime * 2f );
    }

    // Use this for initialization
    private void Start ( )
    {
        instance = this;
        draws = new Draw_s [ constraint ]; // #ATL

        // #ATL
        // Create the list of draws and parent them to the right hand.
        for ( int i=0; i < draws.Length; i++ )
        {
            draws[i] = CreateDraw ( (float) i );
            // draws [ i ] = CreateDraw ( );
        }
        countShown = constraint;
        transform.SetParent ( PlayerCurriculum.instance.GetRightHand ( ) );
    }

    #endregion
}
