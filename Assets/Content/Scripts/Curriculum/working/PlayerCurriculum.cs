using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurriculum : MonoBehaviour
{
    public static PlayerCurriculum instance;

    [SerializeField]
    bool isVR = false;

    [SerializeField]
    GameObject player_VR;

    [SerializeField]
    GameObject player_PC;

    private VRNodeMinion HeadVR;
    private VRNodeMinion LeftHandVR;
    private VRNodeMinion RightHandVR;

    private GameObject HeadPC;
    private GameObject LeftHandPC;
    private GameObject RightHandPC;

    public bool GetLeftTriggerDown ( ) { return LeftTriggerDown; }
    private bool LeftTriggerDown = false;

    public bool GetRightTriggerDown ( ) { return RightTriggerDown; }
    private bool RightTriggerDown = false;
    private bool debug = true;

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

    public Transform GetHead ( )
    {
        if ( isVR )
        {
            return HeadVR.transform;
        }
        else
        {
            return HeadPC.transform;
        }
    }

    public bool ThisTriggerDown ( GameObject hand )
    {
        if ( debug ) Debug.Log ( (hand == GetLeftHand ( ).gameObject) + ", " + GetLeftTriggerDown ( ) );
        if ( debug ) Debug.Log ( ( hand == GetRightHand ( ).gameObject ) + ", " + GetRightTriggerDown ( ) );

        if ( ( hand == GetLeftHand ( ).gameObject && GetLeftTriggerDown ( ) )
            || ( hand == GetRightHand ( ).gameObject && GetRightTriggerDown ( ) ) )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool LeftADown ( )
    {
        if ( isVR )
        {
            if ( LeftHandVR.ButtonADown )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if ( Input.GetKeyDown ( KeyCode.E ) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool RightADown ( )
    {
        if ( isVR )
        {
            if ( RightHandVR.ButtonADown )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if ( Input.GetKeyDown ( KeyCode.L ) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool LeftThumbClickDown ( )
    {
        if ( isVR )
        {
            if ( RightHandVR.ThumbClickDown )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if ( Input.GetKeyDown ( KeyCode.LeftShift ) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool RightThumbClickDown ( )
    {
        if ( isVR )
        {
            if ( RightHandVR.ThumbClickDown )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if ( Input.GetKeyDown( KeyCode.RightShift ) )
            {
                return true;
            }
            else
            {
                return false;
            }
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
        instance = this;

        if ( isVR )
        {
            player_VR.SetActive ( true );
            player_PC.SetActive ( false );
            HeadVR = player_VR.GetComponent<VRNodeLord> ( ).head;
            LeftHandVR = player_VR.GetComponent<VRNodeLord> ( ).leftHand;
            RightHandVR = player_VR.GetComponent<VRNodeLord> ( ).rightHand;
        }
        else
        {
            player_VR.SetActive ( false );
            player_PC.SetActive ( true );
            HeadPC = player_PC.GetComponent<PlayerCurriculumPC> ( ).Head;
            LeftHandPC = player_PC.GetComponent<PlayerCurriculumPC> ( ).LeftHand;
            RightHandPC = player_PC.GetComponent<PlayerCurriculumPC> ( ).RightHand;
        }
    }
}
