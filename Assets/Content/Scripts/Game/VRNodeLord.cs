using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.PostProcessing;

public class VRNodeLord : MonoBehaviour
{
    #region public data

    // dropped in from prefab in inspector
    public VRNodeMinion head;
    public VRNodeMinion leftHand;
    public VRNodeMinion rightHand;
    public bool GlovesOn { get; set; }

    #endregion

    #region private data

    private List<UnityEngine.XR.XRNodeState> vrNodeStates = new List<UnityEngine.XR.XRNodeState>();
    private float PlayerHealth { get; set; }
    private bool debug = false;
    [SerializeField] FX_Damage fx_Damage;
    private Vector3 destination;

    #endregion

    #region public functions

    public void OnDamage ( Vector3 hitPosition )
    {
        if ( debug ) Debug.Log ( "hit received" );
        GameLord.instance.MusicLord.PlayErrorClip ( hitPosition );
        PlayerHealth *= 0.9f;
        fx_Damage.OnDamage ( );
        SetHealthState ( );
    }

    // Called from GameLord.instance.IterateState()
    public void ForceGlovesOn ( )
    {
        GlovesOn = true;
        leftHand.glove.gameObject.SetActive ( true );
        leftHand.glove.FillTo ( PlayerHealth );
        rightHand.glove.gameObject.SetActive ( true );
        rightHand.glove.FillTo ( PlayerHealth );
    }

    #endregion

    #region private functions

    private void SetHealthState ( )
    {
        // Set glove fill amount to the player's health. 
        leftHand.glove.FillTo ( PlayerHealth );
        rightHand.glove.FillTo ( PlayerHealth );
    }

    private bool IsTracking ( UnityEngine.XR.XRNode vrNode )
    {
        for( int i = 0; i < vrNodeStates.Count; i++ )
        {
            if( vrNodeStates[i].nodeType == vrNode )
            {
                return vrNodeStates [ i ].tracked;
            }
        }
        return false;
    }

    private void SetTransforms ( )
    {
        if ( !UnityEngine.XR.XRDevice.isPresent )
            return;

        if ( IsTracking ( UnityEngine.XR.XRNode.Head ) )
        {
            head.transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition ( UnityEngine.XR.XRNode.Head );
            head.transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation ( UnityEngine.XR.XRNode.Head );
        }

        if ( IsTracking ( UnityEngine.XR.XRNode.LeftHand ) )
        {
            leftHand.transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition ( UnityEngine.XR.XRNode.LeftHand );
            leftHand.transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation ( UnityEngine.XR.XRNode.LeftHand );
        }

        if ( IsTracking ( UnityEngine.XR.XRNode.RightHand ) )
        {
            rightHand.transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition ( UnityEngine.XR.XRNode.RightHand );
            rightHand.transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation ( UnityEngine.XR.XRNode.RightHand );
        }
    }

    #endregion

    #region inherited functions

    void Start ( )
    {
        PlayerHealth = 1.0f;
        GlovesOn = false;

        if( head.GetComponent<Camera> ( ) != null )
        {
            head.VRCamera = head.GetComponent<Camera> ( );
            if(GameLord.instance != null )
            {
                head.gameObject.tag = "MainCamera";
            }
        }

        leftHand.VRNode = UnityEngine.XR.XRNode.LeftHand;
        leftHand.VRNodeLord = this;
        rightHand.VRNode = UnityEngine.XR.XRNode.RightHand;
        rightHand.VRNodeLord = this;

        ForceGlovesOn ( );
    }

    void Update ( )
    {
        UnityEngine.XR.InputTracking.GetNodeStates ( vrNodeStates );
        SetTransforms ( );
    }

    #endregion
}
