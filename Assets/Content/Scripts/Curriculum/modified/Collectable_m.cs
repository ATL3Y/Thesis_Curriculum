using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_m: MonoBehaviour
{
    #region public data

    public CollectableController_m collectableController { get; set; }

    #endregion

    #region private data

    private VRNodeMinion owner;
    private Vector3 offset;
    private Quaternion rotOffset;

    [SerializeField] AudioClip pickUpSound;
    private AudioSource pickUpSoundSource;
    [SerializeField] AudioClip putDownSound;
    private AudioSource putDownSoundSource;
    [SerializeField] AudioClip storySoundR;

    private bool debug = true;

    #endregion

    #region private functions

    private void SelfDestruct ( )
    {
        collectableController.RemoveCollectable ( this );
        Destroy ( this );
    }

    private void PickUp ( )
    {
        if ( debug ) Debug.Log ( "PickUp: owner is " + owner.gameObject.name );

        rotOffset = Quaternion.Inverse ( owner.transform.rotation ) * transform.rotation;
        offset = owner.transform.InverseTransformDirection ( transform.position - owner.transform.position );
        if ( pickUpSound )
        {
            pickUpSoundSource.Play ( );
        }

        // transform.GetComponentInChildren<ParticleSystem> ( ).Play ( );
    }

    private void PutDown ( )
    {
        owner = null;

        if ( putDownSound )
        {
            putDownSoundSource.Play ( );
        }
    }

    private void InPickUp ( )
    {
        Vector3 targetPos = owner.transform.position + owner.transform.TransformDirection(offset);
        Vector3 newScale = transform.localScale;

        // stick
        float distanceToNewPos = Vector3.Magnitude(targetPos - transform.position);
        Quaternion newRotation = owner.transform.rotation * rotOffset;

        float t = Mathf.Clamp01(1.0f / (distanceToNewPos * distanceToNewPos * 100f));
        t = CubicEase ( t, 0f, 1f, 1f );

        newScale = Vector3.Lerp ( transform.localScale, newScale, t );
        newRotation = Quaternion.Slerp ( transform.rotation, newRotation, t );
        targetPos = Vector3.Lerp ( transform.position, targetPos, t );

        transform.localScale = newScale;
        transform.rotation = newRotation;
        transform.position = targetPos;
    }

    private float CubicEase ( float t, float b, float c, float d )
    {
        t /= d / 2;
        if ( t < 1 )
        {
            return c / 2 * t * t * t + b;
        }
        t -= 2;
        return c / 2 * ( t * t * t + 2 ) + b;
    }

    #endregion

    #region inherited functions

    // Update is called once per frame
    private void Update ( )
    {
        if ( owner == null )
        {
            return;
        }

        // If the owner is holding the trigger, stick to the owner.
        if ( owner.Trigger > 0.5f )
        {
            InPickUp ( );
        }
        // If the owner releases the trigger, stay in place.
        else
        {
            PutDown ( );
        }
    }

    public void OnCollisionStay ( Collision collision )
    {
        // We can only get picked up if we don't have an owner. 
        if ( owner != null )
        {
            return;
        }

        if ( debug ) Debug.Log ( collision.gameObject.name );

        // Check that we are colliding with a hand, and that the hand's trigger is down. 
        VRNodeMinion handTemp = collision.gameObject.GetComponent<VRNodeMinion> ( );
        if ( handTemp != null && handTemp.gameObject.layer == LayerMask.NameToLayer ( "Hand" ) 
            && handTemp.Trigger > .5f )
        {
            owner = handTemp;
            PickUp ( );
        }
    }

    // Use this for initialization
    private void Start ( )
    {
        // Add pickup sound.
        if ( pickUpSound != null )
        {
            pickUpSoundSource = gameObject.AddComponent<AudioSource> ( );
            pickUpSoundSource.playOnAwake = false;
            pickUpSoundSource.loop = false;
            pickUpSoundSource.volume = 1f;
            pickUpSoundSource.spatialBlend = 1f;
            pickUpSoundSource.clip = pickUpSound;
        }
        // Add putdown sound.
        if ( putDownSound != null )
        {
            putDownSoundSource = gameObject.AddComponent<AudioSource> ( );
            putDownSoundSource.playOnAwake = false;
            putDownSoundSource.loop = false;
            putDownSoundSource.volume = 1f;
            putDownSoundSource.spatialBlend = 1f;
            putDownSoundSource.clip = putDownSound;
        }
    }

    #endregion
}
