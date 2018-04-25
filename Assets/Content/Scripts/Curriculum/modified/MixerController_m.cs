using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerController_m : MonoBehaviour
{

    #region public data

    #endregion

    #region private data

    private List<MixerFeature_m> mixerFeatures;
    private AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    private bool debug = true;
    [SerializeField] GameObject pointerPrefab;
    private GameObject pointer;

    #endregion

    #region public functions

    public void AddMe ( MixerFeature_m mixerFeature )
    {
        mixerFeatures.Add ( mixerFeature );
    }

    public void SetVolume ( float volume )
    {
        audioSource.volume = volume;
    }

    public void SetSpatialBlend ( float spatialBlend )
    {
        audioSource.spatialBlend = spatialBlend;
    }

    public void SetFilter ( float filter )
    {
        // Will this slow time?  What is the range? 
        audioSource.pitch = filter;
    }

    #endregion

    #region inherited functions

    private void Update ( )
    {
        Vector3 fwd = PlayerCurriculum.instance.GetRightHand().transform.forward;
        Vector3 pos = PlayerCurriculum.instance.GetRightHand().transform.position + fwd;
        pointer.transform.position = pos + 10.0f * fwd;

        // SetVolume ( 1.0f );
        if ( PlayerCurriculum.instance.GetLeftTriggerDown ( ) )
        {
            if ( debug ) Debug.Log ( "leftHand.Trigger > 0.5f" );
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(pos, fwd);
            if ( Physics.Raycast ( ray, out hit, 50.0f ) )
            {
                if ( debug ) Debug.Log ( "Raycast: " + hit.transform.gameObject.name );
                MixerFeature_m mixerFeature = hit.transform.gameObject.GetComponent<MixerFeature_m>();
                if ( mixerFeature != null )
                {
                    if ( debug ) Debug.Log ( "mixerFeature.Fill to: " + hit.point.y / hit.collider.transform.localScale.y );
                    mixerFeature.FillTo ( hit.point.y / hit.collider.transform.localScale.y );
                    pointer.transform.position = hit.point;
                }
            }
        }

        if ( PlayerCurriculum.instance.GetRightTriggerDown ( ) )
        {
            if ( debug ) Debug.Log ( "rightHand.Trigger > 0.5f" );
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(pos, fwd);
            if ( Physics.Raycast ( ray, out hit, 50.0f ) )
            {
                if ( debug ) Debug.Log ( "Raycast: " + hit.transform.gameObject.name );
                MixerFeature_m mixerFeature = hit.transform.gameObject.GetComponent<MixerFeature_m>();
                if ( mixerFeature != null )
                {
                    if ( debug ) Debug.Log ( "mixerFeature.Fill to: " + hit.point.y / hit.collider.transform.localScale.y );
                    mixerFeature.FillTo ( hit.point.y / hit.collider.transform.localScale.y );
                    pointer.transform.position = hit.point;
                }
            }
        }
    }

    private void Awake ( )
    {
        mixerFeatures = new List<MixerFeature_m> ( );
    }

    // Use this for initialization
    private void Start ( )
    {
        // mixerFeatures = new List<MixerFeature> ( ); // ATL: initialize in start and run the debugger
        audioSource = gameObject.AddComponent<AudioSource> ( );
        audioSource.clip = audioClip;
        audioSource.volume = 0.0f;
        audioSource.spatialBlend = 0.0f;
        audioSource.loop = true;
        audioSource.Play ( );
        pointer = GameObject.Instantiate ( pointerPrefab );
    }

    #endregion
}
