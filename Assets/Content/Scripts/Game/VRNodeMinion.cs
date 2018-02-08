using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR;

public class VRNodeMinion : MonoBehaviour
{
    #region public data

    public UnityEngine.XR.XRNode VRNode { get; set; }
    public Camera VRCamera { get; set; }
    public float Trigger { get; set; }
    public float Bumper { get; set; }
    public bool ButtonA { get; set; }
    public bool ButtonADown { get; set; }
    public bool ButtonAUp { get; set; }
    public bool TriggerTouch { get; set; }
    public bool TriggerTouchDown { get; set; }
    public bool TriggerTouchUp { get; set; }
    public bool ThumbTouch { get; set; }
    public bool ThumbTouchDown { get; set; }
    public bool ThumbTouchUp { get; set; }
    public bool ThumbClick { get; set; }
    public bool ThumbClickDown { get; set; }
    public bool ThumbClickUp { get; set; }
    public Vector2 ThumbPosition { get; set; }
    public Vector3 GetForce ( ) { return force; }
    public FillCircuit glove;
    public VRNodeLord VRNodeLord { get; set; }
    public bool GetIsLeftHand()
    {
        return isLeftHand;
    }
    #endregion

    #region private data

    private bool isHead = false;
    private bool isLeftHand = false;
    private bool isRightHand = false;
    private bool debug = false;
    private int index = -1;
    private Vector3 oldPos;
    private Vector3 force;

    #endregion

    #region public functions

    public void HapticPulse( int duration )
    {
        if ( GameLord.instance.GetDeviceType ( ) == GameLord.DeviceType.Vive )
        {
            // Do I need to send a "Both"
            if ( isLeftHand )
            {
                if ( index == -1 ) index = SteamVR_Controller.GetDeviceIndex ( SteamVR_Controller.DeviceRelation.Leftmost );
                SteamVR_Controller.Input ( index ).TriggerHapticPulse ( ( ushort ) duration );
            }
            if ( isRightHand )
            {
                if ( index == -1 ) index = SteamVR_Controller.GetDeviceIndex ( SteamVR_Controller.DeviceRelation.Rightmost );
                SteamVR_Controller.Input ( index ).TriggerHapticPulse ( ( ushort ) duration );
            }
        }
        else
        {
            // Do I need to send a "Both"
            if ( isLeftHand )
            {
                uint indexU;
                indexU = OpenVR.System.GetTrackedDeviceIndexForControllerRole ( ETrackedControllerRole.LeftHand );
                OpenVR.System.TriggerHapticPulse ( indexU, 0, ( char ) duration );
            }
            if ( isRightHand )
            {
                uint indexU;
                indexU = OpenVR.System.GetTrackedDeviceIndexForControllerRole ( ETrackedControllerRole.RightHand );
                OpenVR.System.TriggerHapticPulse ( indexU, 0, ( char ) duration );
            }
        }
    }

    #endregion

    #region private functions

    private void CalculateForce ( )
    {
        force = transform.position - oldPos;
        oldPos = transform.position;
    }

    #endregion

    #region inherited functions
    // Use this for initialization
    void Start ()
    {
        isHead = name.Contains ( "Head" );
        isLeftHand = name.Contains ( "Left" );
        isRightHand = name.Contains ( "Right" );
        if ( glove != null )
        {
            glove.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ( isHead )
            return;

        if ( isLeftHand )
        {
            Trigger = Input.GetAxis ( "LeftTrigger" );
            Bumper = Input.GetAxis ( "LeftBumper" );

            ButtonA = Input.GetButton ( "LeftButtonA" );
            ButtonADown = Input.GetButtonDown ( "LeftButtonA" );
            ButtonAUp = Input.GetButtonUp ( "LeftButtonA" );

            TriggerTouch = Input.GetButton ( "LeftTriggerTouch" );
            TriggerTouchDown = Input.GetButtonDown ( "LeftTriggerTouch" ); 
            TriggerTouchUp = Input.GetButtonUp ( "LeftTriggerTouch" );

            ThumbTouch = Input.GetButton ( "LeftThumbTouch" );
            ThumbTouchDown = Input.GetButtonDown ( "LeftThumbTouch" );
            ThumbTouchUp = Input.GetButtonUp ( "LeftThumbTouch" );

            ThumbClick = Input.GetButton ( "LeftThumbClick" );
            ThumbClickDown = Input.GetButtonDown ( "LeftThumbClick" );
            ThumbClickUp = Input.GetButtonUp ( "LeftThumbClick" );

            ThumbPosition = new Vector2 ( Input.GetAxis ( "LeftThumbHorizontal" ), Input.GetAxis ( "LeftThumbVertical" ) );
        }

        if ( isRightHand )
        {
            Trigger = Input.GetAxis ( "RightTrigger" ); 
            Bumper = Input.GetAxis ( "RightBumper" );

            ButtonA = Input.GetButton ( "RightButtonA" );
            ButtonADown = Input.GetButtonDown ( "RightButtonA" );
            ButtonAUp = Input.GetButtonUp ( "RightButtonA" );

            TriggerTouch = Input.GetButton ( "RightTriggerTouch" );
            TriggerTouchDown = Input.GetButtonDown ( "RightTriggerTouch" ); 
            TriggerTouchUp = Input.GetButtonUp ( "RightTriggerTouch" );

            ThumbTouch = Input.GetButton ( "RightThumbTouch" );
            ThumbTouchDown = Input.GetButtonDown ( "RightThumbTouch" );
            ThumbTouchUp = Input.GetButtonUp ( "RightThumbTouch" );

            ThumbClick = Input.GetButton ( "RightThumbClick" );
            ThumbClickDown = Input.GetButtonDown ( "RightThumbClick" );
            ThumbClickUp = Input.GetButtonUp ( "RightThumbClick" );

            ThumbPosition = new Vector2 ( Input.GetAxis ( "RightThumbHorizontal" ), Input.GetAxis ( "RightThumbVertical" ) );
        }

        if ( VRNodeLord.GlovesOn )
        {
            // Trigger goes from 0 (none) to 1 (down).
            glove.gameObject.SetActive ( Trigger > .5f ); 
        }

        CalculateForce ( );

        if ( debug )
        {
            // if ( Trigger > 0.0f ) Debug.Log ( "Trigger: " + Trigger );
            // if ( Bumper > 0.0f ) Debug.Log ( "Bumper: " + Bumper );
            if (ButtonA) Debug.Log ( "ButtonA" );
            if ( ButtonADown ) Debug.Log ( "ButtonADown" );
            if ( ButtonAUp ) Debug.Log ( "ButtonAUp" );
            if ( TriggerTouch ) Debug.Log ( "TriggerTouch" );
            if ( TriggerTouchDown ) Debug.Log ( "ThumbTouchDown" );
            if ( TriggerTouchUp ) Debug.Log ( "TriggerTouchUp" );
            if ( ThumbTouch ) Debug.Log ( "ThumbTouch" );
            if ( ThumbTouchDown ) Debug.Log ( "ThumbTouchDown" );
            if ( ThumbTouchUp ) Debug.Log ( "ThumbTouchUp" );
            if ( ThumbClick ) Debug.Log ( "ThumbClick" );
            if ( ThumbClickDown ) Debug.Log ( "ThumbClickDown" );
            if ( ThumbClickUp ) Debug.Log ( "ThumbClickUp" );
            // if ( ThumbPosition.x > 0.0f ) Debug.Log ( "ThumbPosition horizontal: " + ThumbPosition.x + " ThumbPosition vertical: " + ThumbPosition.y );
        }
    }

#endregion
}
