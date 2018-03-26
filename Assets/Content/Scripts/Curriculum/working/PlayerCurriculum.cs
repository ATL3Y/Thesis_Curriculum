using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurriculum : MonoBehaviour
{
    [SerializeField]
    bool isVR = false;

    [SerializeField]
    GameObject player_VR;

    [SerializeField]
    GameObject player_PC;

    private VRNodeMinion LeftHandVR;
    private VRNodeMinion RightHandVR;

    private GameObject LeftHandPC;
    private GameObject RightHandPC;

    public bool GetLeftTriggerDown ( ) { return LeftTriggerDown; }
    private bool LeftTriggerDown = false;

    public bool GetRightTriggerDown ( ) { return RightTriggerDown; }
    private bool RightTriggerDown = false;

    public Transform GetLeftHand ( )
    {
        if ( isVR )
        {
            return LeftHandVR.transform;
        }
        else
        {
            return LeftHandPC.transform;
        }
    }

    public Transform GetRightHand ( )
    {
        if ( isVR )
        {
            return RightHandVR.transform;
        }
        else
        {
            return RightHandPC.transform;
        }
    }

    private void Update ()
    {
        if ( isVR )
        {
            LeftTriggerDown = LeftHandVR.Trigger > 0.8f;
            RightTriggerDown = RightHandVR.Trigger > 0.8f;
        }
        else
        {
            LeftTriggerDown = Input.GetMouseButton ( 0 );
            RightTriggerDown = Input.GetMouseButton ( 1 );
        }
	}

    private void Start ( )
    {
        if ( isVR )
        {
            player_VR.SetActive ( true );
            player_PC.SetActive ( false );
            LeftHandVR = player_VR.GetComponent<VRNodeLord> ( ).leftHand;
            RightHandVR = player_VR.GetComponent<VRNodeLord> ( ).rightHand;
        }
        else
        {
            player_VR.SetActive ( false );
            player_PC.SetActive ( true );
            LeftHandPC = player_PC.GetComponent<PlayerCurriculumPC> ( ).LeftHand;
            RightHandPC = player_PC.GetComponent<PlayerCurriculumPC> ( ).RightHand;
        }
    }
}
