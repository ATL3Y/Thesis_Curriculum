using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerController : MonoBehaviour
{

    #region public data

    #endregion

    #region private data

    private List<MixerFeature> mixerFeatures;
    private AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    private bool debug = true;

    #endregion

    #region public functions

    public void AddMe ( MixerFeature mixerFeature )
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
        // SetVolume ( 1.0f );
        if( GameController_Mixer.instance.leftHand.Trigger > 0.5f )
        {
            if ( debug ) Debug.Log ( "leftHand.Trigger > 0.5f" );
            RaycastHit hit = new RaycastHit();
            Vector3 fwd = GameController_Mixer.instance.leftHand.transform.forward;
            Vector3 pos = GameController_Mixer.instance.leftHand.transform.position + fwd;

            Ray ray = new Ray(pos, fwd);
            if ( Physics.Raycast ( ray, out hit, 50.0f ) )
            {
                if ( debug ) Debug.Log ( "Raycast: " + hit.transform.gameObject.name );
                MixerVolume mixerVolume = hit.transform.gameObject.GetComponent<MixerVolume>();
                if ( mixerVolume != null )
                {
                    if ( debug ) Debug.Log ( "mixerFeature.Fill to: " + hit.point.y / hit.collider.transform.localScale.y ); 
                    mixerVolume.FillTo ( hit.point.y / hit.collider.transform.localScale.y );
                }
            }
        }

        if ( GameController_Mixer.instance.rightHand.Trigger > 0.5f )
        {
            if ( debug ) Debug.Log ( "rightHand.Trigger > 0.5f" );
            RaycastHit hit = new RaycastHit();
            Vector3 fwd = GameController_Mixer.instance.rightHand.transform.forward;
            Vector3 pos = GameController_Mixer.instance.rightHand.transform.position + fwd;

            Ray ray = new Ray(pos, fwd);
            if ( Physics.Raycast ( ray, out hit, 50.0f ) )
            {
                if ( debug ) Debug.Log ( "Raycast: " + hit.transform.gameObject.name );
                MixerVolume mixerVolume = hit.transform.gameObject.GetComponent<MixerVolume>();
                if ( mixerVolume != null )
                {
                    if ( debug ) Debug.Log ( "mixerFeature.Fill to: " + hit.point.y / hit.collider.transform.localScale.y ); 
                    mixerVolume.FillTo ( hit.point.y / hit.collider.transform.localScale.y );
                }
            }
        }
    }

    private void Awake ( )
    {
        mixerFeatures = new List<MixerFeature> ( );
    }

    // Use this for initialization
    private void Start ( )
    {
        // mixerFeatures = new List<MixerFeature> ( ); // Exercise: initialize in start and run the debugger
        audioSource = gameObject.AddComponent<AudioSource> ( );
        audioSource.clip = audioClip;
        audioSource.volume = 0.0f;
        audioSource.spatialBlend = 0.0f;
        audioSource.loop = true;
        audioSource.Play ( );
    }

    #endregion
}
