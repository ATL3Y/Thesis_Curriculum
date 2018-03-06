using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_Collectables : MonoBehaviour
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
            CollectableController.instance.CreateCollectable ( leftHand.transform.position );
        }

        if ( rightHand.ButtonADown )
        {
            CollectableController.instance.CreateCollectable ( rightHand.transform.position );
        }

        if ( leftHand.ThumbClickDown )
        {
            CollectableController.instance.CreateAllCollectables ( leftHand.transform.position + 2.5f * leftHand.transform.forward );
        }

        if ( rightHand.ThumbClickDown )
        {
            CollectableController.instance.CreateAllCollectables ( rightHand.transform.position + 2.5f * leftHand.transform.forward );
        }
    }

    #endregion
}
