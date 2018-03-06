using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingController : MonoBehaviour
{
    #region public data

    public static DrawingController instance;

    #endregion

    #region private data

    private List<Draw> draws;
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

    public void RemoveDraw ( Draw draw )
    {
        draws.Remove ( draw );
    }

    public void CreateDraw ( float i )
    {
        GameObject newGameObject = Instantiate( prefab );
        Draw newDraw = newGameObject.GetComponent<Draw>();
        newDraw.drawController = this;
        newDraw.transform.SetParent ( this.transform );
        newDraw.transform.localPosition = Vector3.up * i * .1f;
        draws.Add ( newDraw );
    }

    #endregion

    #region inherited functions

    private void Update ( )
    {
        // update this position to oscillate towards and from the player's gaze
        Transform playerTrans = GameController_Drawing.instance.player.head.transform;
        Vector3 pos = playerTrans.position + playerTrans.forward * baseRange;
        Quaternion rot = Quaternion.Inverse(playerTrans.rotation);
        pos += new Vector3 ( 0, 0, sinRange * Mathf.Sin ( Time.timeSinceLevelLoad * .5f ) );
        transform.position = Vector3.Lerp ( transform.position, pos, Time.deltaTime * 2f );
    }

    // Use this for initialization
    private void Start ( )
    {
        instance = this;
        draws = new List<Draw> ( );

        // Create the list of draws and parent them to the right hand.
        for( int i=0; i < constraint; i++ )
        {
            CreateDraw ( (float) i );
        }
        countShown = constraint;
        transform.SetParent ( GameController_Drawing.instance.player.rightHand.transform );
    }

    #endregion
}
