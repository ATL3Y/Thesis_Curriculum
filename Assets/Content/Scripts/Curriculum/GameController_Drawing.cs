using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_Drawing : MonoBehaviour
{

    #region private data

    [SerializeField] VRNodeMinion leftHand;
    [SerializeField] VRNodeMinion rightHand;

    #endregion

    #region private functions

    // Use this for initialization
    private void Start ( )
    {

    }

    // Update is called once per frame
    private void Update ( )
    {
        if( rightHand.Bumper > 0.99f )
        {
            DrawingController.instance.HideDraw ( );
        }
        else if ( leftHand.Trigger > 0.99f )
        {
            DrawingController.instance.DisplayDraw ( );
        }
    }

    #endregion
}
