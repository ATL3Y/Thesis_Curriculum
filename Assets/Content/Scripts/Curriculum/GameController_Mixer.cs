using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_Mixer : MonoBehaviour
{

    #region public data

    public static GameController_Mixer instance;
    public VRNodeMinion leftHand;
    public VRNodeMinion rightHand;

    #endregion

    #region private data

    #endregion

    #region private functions

    // Use this for initialization
    private void Start ( )
    {
        instance = this;
    }

    // Update is called once per frame
    private void Update ( )
    {

    }

    #endregion
}
