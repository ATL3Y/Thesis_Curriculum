using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentA : Opponent
{
    #region public data

    #endregion

    #region private data

    private Vector3 reboundDir;
    private float reboundMod;
    private bool damage;
    private Color origRewardMatEmisCol;

    #endregion

    #region public functions

    #endregion

    #region private functions

    // The actual target
    private Vector3 TargetPos ( )
    {
        Vector3 offsetY = 0.5f * ( GameLord.instance.Player.head.transform.position.y - GameLord.instance.FloorCenter.y ) * Vector3.up;

        // We want the last beat to pulse right in front of the player before being considered passed the player.
        float dist = speed * cycleDuration * 1.5f;
        Vector3 offsetZ = -dist * Vector3.forward;
        Vector3 offsetX = 0.4f * GameLord.instance.Player.head.transform.position.x * Vector3.right;

        // Offset is between -1/2 and 1/2
        offsetX += Offset * Vector3.right; 

        Vector3 target = offsetX + offsetY + offsetZ + GameLord.instance.FloorCenter;

        return target;
    }

    // Target 5 units past the player
    private Vector3 TargetPosMod ( )
    {
        Vector3 offsetY = 0.5f * ( GameLord.instance.Player.head.transform.position.y - GameLord.instance.FloorCenter.y ) * Vector3.up;

        // We want the last beat to pulse right in front of the player before being considered passed the player.
        float dist = speed * cycleDuration * 1.5f;
        Vector3 offsetZ = -dist * Vector3.forward;
        Vector3 offsetX = 0.4f * GameLord.instance.Player.head.transform.position.x * Vector3.right;

        // Offset is between -1/2 and 1/2
        offsetX += Offset * Vector3.right; 

        Vector3 target = offsetX + offsetY + offsetZ + GameLord.instance.FloorCenter;
        // if ( debug ) Debug.Log ( " x: " + offsetX + " y: " + offsetY + " z: " + offsetZ + " fc: " + GameLord.instance.FloorCenter);

        // Set target 5.0 units past the real target
        // This direction can only depend on the target up until we pass the target
        if( state != OpponentState.Passed && Vector3.Magnitude ( target - transform.localPosition ) > 1.0f )
        {
            transform.rotation = Quaternion.LookRotation ( target - transform.position );
        }
        target += 5.0f * transform.forward;

        return target;
    }

    #endregion

    #region inherited functions

    private void OnCollisionEnter ( Collision collision )
    {
        if ( state == OpponentState.Untouched )
        {
            VRNodeMinion vrNodeMinion = collision.gameObject.GetComponent<VRNodeMinion>();
            if( vrNodeMinion != null )
            {
                var sit = new HandSituation();
                HandSituations.Add(sit);
                sit.hand = vrNodeMinion;

                reboundDir = sit.hand.GetForce( ).normalized; 
                reboundMod = sit.hand.GetForce ( ).magnitude / Time.deltaTime * 2.0f; // This range is about 0.0f to 4.0f.

                if( debug ) Debug.Log ( "Hit by hand reboundMod: " + reboundMod );

                // Can only collide with "gloves on" 
                if ( sit.hand.glove != null && sit.hand.glove.gameObject.activeInHierarchy )
                {
                    state = OpponentState.Hit;
                    GameLord.instance.MusicLord.PlayResponseClip ( transform.localPosition );
                    SetRends ( rewardMat );
                    fx_hitHand_one.SetActive ( true );
                    OpponentLord.RegisterHit ( this );

                    // Flatten
                    transform.localScale = new Vector3 ( transform.localScale.x, transform.localScale.y, 0.1f * transform.localScale.z );

                    // Turn light 
                    rewardMat.SetColor ( "_EmissionColor", rewardMat.color * 0.25f * reboundMod );
                }
                else
                {
                    state = OpponentState.Missed;
                    SetRends ( punishMat );
                    timer = 0.5f;
                }
            }
        }
        else if ( state == OpponentState.Passed )
        {
            // Can only damage the player once
            if ( !damage && collision.gameObject == GameLord.instance.Player.head )
            {
                damage = true;
                timer = 0.5f; // Give the fx time
                GameLord.instance.Player.OnDamage ( transform.position );
            }
        }
        else if ( state == OpponentState.Hit )
        {
            if ( debug ) Debug.Log ( "OnCollisionEnter: Hit by other: " + collision.gameObject.name );
            if ( !finished )
            {
                Shrink shrink = collision.gameObject.GetComponent<Shrink>();
                if( shrink != null )
                {
                    if ( debug ) Debug.Log ( "OnCollisionEnter: Hit by Shrink" );
                    shrink.OnHit ( );
                    fx_hitTrigger.SetActive ( true );
                    timer = 1.0f; // Give the fx time
                    finished = true;
                }
            }
        }
    }

    protected void Update ( )
    {
        // Whatever the state, travel past the player
        Vector3 target = TargetPosMod();

        if ( state == OpponentState.Untouched )
        {
            // Check if we are passed the player. The target is 5 units passed the player. 
            if ( Vector3.Magnitude ( target - transform.localPosition ) < 5.0f ) 
            {
                SetRends ( punishMat );
                state = OpponentState.Passed;
            }

            transform.localPosition = Vector3.MoveTowards ( transform.localPosition, target, speed * Time.deltaTime );
        }
        else if ( state == OpponentState.Passed )
        {
            // If untouched, travel past the player
            transform.localPosition = Vector3.MoveTowards ( transform.localPosition, target, speed * Time.deltaTime );

            // Store disable this opponent after a given time
            timer -= Time.deltaTime;
            if ( timer < 0.0f )
                ToDisable ( );
        }
        else if ( state == OpponentState.Missed )
        {
            // If missed, rebound with less force
            transform.localPosition = Vector3.MoveTowards ( transform.localPosition, reboundDir + transform.localPosition, 0.5f * reboundMod * Time.deltaTime );

            if ( !damage )
            {
                GameLord.instance.Player.OnDamage ( transform.position );
                damage = true;
            }

            // Store disable this opponent after a given time
            timer -= Time.deltaTime;
            if ( timer < 0.0f )
                ToDisable ( );
        }
        else if ( state == OpponentState.Hit )
        {
            
            // We are finished if we hit a shrink object.
            if ( finished )
            {
                // Iterate through the list of hands
                for (int i = 0; i < HandSituations.Count; i++)
                {
                    // Pulse
                    int pulseIntensity = Mathf.RoundToInt(1999.0f * reboundMod); // High is 3999
                    // Pulse intentity increases with hit force
                    HandSituations[i].hand.HapticPulse(pulseIntensity);

                    if (debug) Debug.Log(i + " pulseIntensity " + pulseIntensity);
                }

                // Sink.
                transform.localPosition = Vector3.MoveTowards ( transform.localPosition, -Vector3.up + transform.localPosition, reboundMod * Time.deltaTime );
            }
            else
            {
                // If simply rebounding, rebound from the hand.
                transform.localPosition = Vector3.MoveTowards ( transform.localPosition, reboundDir + transform.localPosition, reboundMod * Time.deltaTime );
            }

            // Store disable this opponent after a given time.
            timer -= Time.deltaTime;
            if ( timer < 0.0f )
                ToDisable ( );
        }
    }

    private void OnDisable ( )
    {
        if( state == OpponentState.Hit )
        {
            // Return scale to normal 
            transform.localScale = new Vector3 ( transform.localScale.x, transform.localScale.y, 10.0f * transform.localScale.z );
            rewardMat.SetColor ( "_EmissionColor", origRewardMatEmisCol );
        }

        // Clear the list of hands
        HandSituations.Clear();
    }

    protected override void OnEnable ( )
    {
        base.OnEnable();
        SetRends ( rhythmMat );

        damage = false;
        speed = 2.0f;

        origRewardMatEmisCol = rewardMat.GetColor ( "_EmissionColor" );

        // Set our start position based on the beat on which we want to reach the player
        // Move in the -Z and -Y directions
        Vector3 direction = Quaternion.AngleAxis ( -20f, Vector3.right ) * Vector3.forward;
        direction.Normalize ( );

        // Find our distance
        float timeToTarget = cycleCount * cycleDuration;
        float distance = speed * timeToTarget;
        Vector3 target = TargetPos( );

        // Find our starting position 
        // Move and rotate
        transform.position = target + direction * distance;
        transform.rotation = Quaternion.LookRotation ( target - transform.position );
    }

    #endregion
}
