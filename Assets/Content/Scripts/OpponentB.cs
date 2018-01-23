using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentB : Opponent
{
    #region private data

    private float distToCore = 100.0f;
    private float radius = 1.0f;
    private float scoreTimer = 0.0f;
    private bool left = false;
    
    #endregion

    #region public functions

    #endregion

    #region private functions

    private int Mod( int a, int b )
    {
        return ( a % b + b ) % b;
    }

    // The actual target
    private Vector3 TargetPos( )
    {
        Vector3 offsetY = 0.5f * ( GameLord.instance.Player.head.transform.position.y - GameLord.instance.FloorCenter.y ) * Vector3.up;
        // We want the last beat to pulse right in front of the player before being considered passed the player.
        float dist = speed * cycleDuration * 1.5f;
        Vector3 offsetZ = -dist * Vector3.forward;

        // Vary which hand we pursue
        Vector3 offsetX;
        if ( left )
        {
            //offsetX = 0.2f * GameLord.instance.Player.leftHand.transform.position - Vector3.right;
        }
        else
        {
            //offsetX = 0.2f * GameLord.instance.Player.rightHand.transform.position + Vector3.right;
        }

        // Offset is between -1/2 and 1/2
        offsetX = Offset * Vector3.right;

        Vector3 target = offsetX + offsetY + offsetZ + GameLord.instance.FloorCenter;

        return target;
    }

    // Target 5 units past the player
    private Vector3 TargetPosMod( )
    {
        Vector3 offsetY = 0.5f * ( GameLord.instance.Player.head.transform.position.y - GameLord.instance.FloorCenter.y ) * Vector3.up;
        // We want the last beat to pulse right in front of the player before being considered passed the player.
        float dist = speed * cycleDuration * 1.5f;
        Vector3 offsetZ = -dist * Vector3.forward;

        // Vary which hand we pursue
        Vector3 offsetX;
        if ( left )
        {
            //offsetX = 0.2f * GameLord.instance.Player.leftHand.transform.position - Vector3.right;
        }
        else
        {
            //offsetX = 0.2f * GameLord.instance.Player.rightHand.transform.position + Vector3.right;
        }

        // Offset is between -1/2 and 1/2
        offsetX = Offset * Vector3.right;

        Vector3 target = offsetX + offsetY + offsetZ + GameLord.instance.FloorCenter;

        // Set target 5.0 units past the real target
        // This direction can only depend on the target up until we pass the target
        if ( state != OpponentState.Passed && Vector3.Magnitude ( target - transform.localPosition ) > 1.0f )
        {
            transform.rotation = Quaternion.LookRotation ( target - transform.position );
        }
        target += 5.0f * transform.forward;

        return target;
    }

    private void InCollision( )
    {
        distToCore = 0.0f;

        // Iterate through the list of hands
        for (int i = 0; i < HandSituations.Count; i++)
        {
            Vector3 handRelative = HandSituations[i].hand.transform.InverseTransformPoint(transform.position);

            // In future, omit Sqrt to optimize.
            HandSituations[i].myDistToCore = Mathf.Sqrt(Mathf.Pow(handRelative.x, 2) + Mathf.Pow(handRelative.y, 2));

            // Make distToCore be from 0 to 1
            HandSituations[i].myDistToCore /= radius;
            if (debug) Debug.Log(i + " mydistToCore " + HandSituations[i].myDistToCore);

            // Pulse intentity increases with proximity
            int pulseIntensity = Mathf.RoundToInt(999.0f / HandSituations[i].myDistToCore); // High is 3999
            HandSituations[i].hand.HapticPulse(pulseIntensity);
            if (debug) Debug.Log(i + " pulseIntensity " + pulseIntensity);

            // Register a point for every x seconds in collision
            scoreTimer += Time.deltaTime;
            if (scoreTimer > 2.0f)
            {
                OpponentLord.RegisterHit(this);
                scoreTimer = 0.0f;
            }

            // Place the hand fx on the fingertips.
            HandSituations[i].fx_hitHand.transform.position = HandSituations[i].hand.transform.position + 0.15f * HandSituations[i].hand.transform.forward;

            // Set the total distToCore based on how many hands there are.
            distToCore += HandSituations[i].myDistToCore/HandSituations.Count;
        }
    }

    #endregion

    #region inherited functions

    private void OnCollisionEnter( Collision collision )
    {
        if (debug) Debug.Log("OnCollisionEnter: " + collision.gameObject.name);

        VRNodeMinion enteringVrNodeMinion = collision.gameObject.GetComponent<VRNodeMinion>();
        if (enteringVrNodeMinion.glove.isActiveAndEnabled)
        {
            return;
        }

        // The collider is oversized. Use the distance from center to determine intensity of the effect. 
        // This can only collide with a "Hand" Layer, which has a VRNodeMinion component. So, no check needed.
        var sit = new HandSituation();
        HandSituations.Add(sit);
        sit.hand = enteringVrNodeMinion;

        state = OpponentState.Hit;
        SetRends( rewardMat );

        // Arbitrarily assign fx one to left and fx two to right
        if (sit.hand.GetIsLeftHand())
        {
            sit.fx_hitHand = fx_hitHand_one;
        }
        else
        {
            sit.fx_hitHand = fx_hitHand_two;
        }

        sit.fx_hitHand.SetActive( true );
        sit.fx_hitHand.transform.position = sit.hand.transform.position;

        if ( MyMusicTrackIndex == -1 )
        {
            MyMusicTrackIndex = GameLord.instance.MusicLord.NextStemOnIndex ( );
            if(debug) Debug.Log("next track index: " + MyMusicTrackIndex);
        }
    }

    private void OnCollisionExit( Collision collision )
    {
        if (debug) Debug.Log("OnCollisionExit");
        VRNodeMinion exitingVrNodeMinion = collision.gameObject.GetComponent<VRNodeMinion>();

        if (exitingVrNodeMinion != null )
        {
            // Iterate through the list of hands
            for (int i = HandSituations.Count-1; i >= 0; i--)
            {
                if (exitingVrNodeMinion == HandSituations[i].hand)
                {
                    HandSituations.RemoveAt(i);
                }
            }

            // Adjust the state based on number of current collisions
            if (HandSituations.Count == 0)
            {
                state = OpponentState.Untouched;
                SetRends(rhythmMat);
                rewardMat.SetColor("_EmissionColor", rewardMat.color * 0.5f);
                if (debug) Debug.Log("OnCollisionExit: Untouched");
            }
            else
            {
                state = OpponentState.Hit;
            }
        }
    }

    protected void Update( )
    {
        // Whatever the state, travel past the player
        Vector3 target = TargetPosMod();
        transform.localPosition = Vector3.MoveTowards( transform.localPosition, target, speed * Time.deltaTime );

        // We are within the collider and should provide feedback.
        if ( state == OpponentState.Hit )
        {
            // Handle individual hand feedback
            InCollision( );

            // Handle accumulative hand feedback
            // Volume increases with proximity
            GameLord.instance.MusicLord.SetStemVolume(.25f / distToCore, MyMusicTrackIndex);

            // Emission increases with proximity
            float emissionIntensity = Mathf.Clamp(0.4f / distToCore, 0.0f, 2.0f);
            rewardMat.SetColor("_EmissionColor", rewardMat.color * emissionIntensity);
        }
        else
        {
            // Fade out the volume
            if ( MyMusicTrackIndex > -1 )
            {
                float vol = GameLord.instance.MusicLord.GetStemVolume(MyMusicTrackIndex);
                if (vol > 0.0f)
                {
                    GameLord.instance.MusicLord.SetStemVolume(vol - Time.deltaTime, MyMusicTrackIndex);
                }
            }

            if ( state == OpponentState.Untouched )
            {
                // Check if we are passed the player. The target is 5 units passed the player. 
                if ( Vector3.Magnitude( target - transform.position ) < 5.0f )
                {
                    SetRends( punishMat );
                    state = OpponentState.Passed;
                }
            }
            else if ( state == OpponentState.Passed )
            {
                // Store disable this opponent after a given time
                timer -= Time.deltaTime;
                if ( timer < 0.0f )
                    ToDisable();
            }
            // There is no OpponentState.Missed for Opponent B
        }
    }

    private void OnDisable()
    {
        rewardMat.SetColor("_EmissionColor", rewardMat.color * 0.5f);
        fx_hitHand_one.transform.localPosition = Vector3.zero;
        fx_hitHand_two.transform.localPosition = Vector3.zero;

        // Clear the list of hands
        HandSituations.Clear();
    }

    protected override void OnEnable( )
    {
        base.OnEnable( );

        // Opp B is always "on beat"
        SetRends( rhythmMat );

        // Flag that we have no music track assigned.
        MyMusicTrackIndex = -1;
        left = Mod ( Random.Range ( 2, 3 ), 2 ) == 0;

        speed = .8f;

        // Set our start position based on the beat on which we want to reach the player
        // Move in the -Z and -Y directions
        Vector3 direction = Quaternion.AngleAxis ( -20f, Vector3.right ) * Vector3.forward;
        direction.Normalize ( );

        // Find our distance
        float timeToTarget = cycleCount * cycleDuration;
        float distance = speed * timeToTarget;
        Vector3 target = TargetPos( );

        // Find our starting position 
        transform.position = target + direction * distance;

        // Move and rotate
        transform.rotation = Quaternion.LookRotation ( target - transform.position );

        // Store our radius
        radius = GetComponent<CapsuleCollider>().radius;
        rewardMat.SetColor("_EmissionColor", rewardMat.color * 0.5f );
    }

    #endregion
}
