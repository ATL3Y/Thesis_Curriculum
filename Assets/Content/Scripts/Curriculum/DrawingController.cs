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
    [SerializeField] VRNodeMinion rightHand;
    private int countShown;

    #endregion

    #region public functions

    public void DisplayDraw ( )
    {
        if( countShown < constraint )
        {
            draws [ countShown ].TurnLineOn ( );
            countShown++;
        }
    }

    public void HideDraw ( )
    {
        if( countShown > 0 )
        {
            draws [ countShown ].TurnLineOff ( );
            countShown--;
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
        newDraw.transform.SetParent ( rightHand.transform );
        newDraw.transform.localPosition = new Vector3 ( i / 2.0f, i / 2.0f, 3.0f );
        newDraw.ChangeLineColor( 1.2f * new Color ( Random.value, Random.value, Random.value ) );
        draws.Add ( newDraw );
    }

    #endregion

    #region inherited functions

    // Use this for initialization
    private void Start ( )
    {
        instance = this;
        draws = new List<Draw> ( );

        // Create the list of draws and parent them to the right hand.
        for( int i=0; i<constraint; i++ )
        {
            CreateDraw ( (float) i );
        }
        countShown = constraint;
    }

    #endregion
}
