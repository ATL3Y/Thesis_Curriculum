using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_Collectables_m: MonoBehaviour
{
    #region private data

    [SerializeField] VRNodeMinion leftHand;
    [SerializeField] VRNodeMinion rightHand;

    #endregion

    #region private functions

    // Use this for initialization
    private void Start ()
    {
		
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if ( leftHand.ButtonADown )
        {
            CollectableController_m.instance.CreateCollectable ( leftHand.transform.position );
        }

        if ( rightHand.ButtonADown )
        {
            CollectableController_m.instance.CreateCollectable ( rightHand.transform.position );
        }

        if ( leftHand.ThumbClickDown )
        {
            CollectableController_m.instance.CreateAllCollectables ( leftHand.transform.position + 2.5f * leftHand.transform.forward );
        }

        if ( rightHand.ThumbClickDown )
        {
            CollectableController_m.instance.CreateAllCollectables ( rightHand.transform.position + 2.5f * leftHand.transform.forward );
        }
    }

    #endregion
}
