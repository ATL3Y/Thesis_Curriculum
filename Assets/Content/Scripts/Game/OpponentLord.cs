using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OpponentLord : MonoBehaviour
{
    #region public data

    public List<Opponent> availableOpponentAs = new List<Opponent>();
    public List<Opponent> availableOpponentBs = new List<Opponent>();
    public int GetScore ( ) { return scoreOpponentAHit; }

    #endregion

    #region private data

    private bool debug = true;
    private enum Difficulty { NotPossible, Hard, Medium, Easy, SuperEasy, SpeedThrough }
    [SerializeField] Difficulty difficulty = Difficulty.SpeedThrough;

    [SerializeField] GameObject opponentAPrefab;
    private Opponent[] opponentAs = new Opponent[30];
    private int headA;
    private int scoreOpponentAHit = 0;
    private int scoreTriggerHit = 0;
    //private float attackAngle;

    [SerializeField] GameObject opponentBPrefab;
    private Opponent[] opponentBs = new Opponent[20];
    private int headB;
    private int scoreOpponentBHit = 0;

    #endregion

    #region public functions

    public void OnSceneSwitch ( )
    {
        scoreOpponentAHit = 0;
        scoreTriggerHit = 0;

        for ( int i = 0; i < availableOpponentAs.Count; i++ )
        {
            availableOpponentAs [ i ].gameObject.SetActive ( false );
        }
        availableOpponentAs.Clear ( );

        for (int i = 0; i < availableOpponentBs.Count; i++)
        {
            availableOpponentBs[i].gameObject.SetActive(false);
        }
        availableOpponentBs.Clear();
    }

    public void StoreOpponent ( Opponent op )
    {
        if ( op.opponentType == Opponent.OpponentType.A )
        {
            availableOpponentAs.Remove(op);
            if (debug) Debug.Log("In remove: availableHitOpponents count: " + availableOpponentAs.Count);
        }
        else if ( op.opponentType == Opponent.OpponentType.B )
        {
            availableOpponentBs.Remove(op);
            if (debug) Debug.Log("In remove: availableHitOpponents count: " + availableOpponentBs.Count);
        }

        op.gameObject.SetActive(false);
        op.transform.position = Vector3.zero;
    }

    // The player hit Opponent A
    public void RegisterHit ( Opponent op )
    {
        if (op.opponentType == Opponent.OpponentType.A )
        {
            scoreOpponentAHit++;
        }
        else if (op.opponentType == Opponent.OpponentType.B )
        {
            scoreOpponentBHit++;
        }
    }

    // The player hit a Trigger T with Opponent A
    public void RegisterHitT ( )
    {
        scoreTriggerHit++;
        Debug.Log ( "score is: " + scoreTriggerHit + " out of " + GameLord.instance.GetCurrentSceneRoot ( ).GetOriginalTriggerCount ( ) / ( ( int ) difficulty + 1 ) );
    }

    private void OnUnleashOpponent ( float index, float val, Opponent[] opponentArray, int head )
    {
        opponentArray [ head ].gameObject.SetActive ( true );
        opponentArray [ head ].Offset = index / val - 0.5f + index / ( val * 2.0f ); // Center the row.
        if ( debug ) Debug.Log ( " index: " + index + " val " + val + " offset " + opponentArray [ head ].Offset );
    }

    public void UnleashOpponent ( int val )
    {
        // val is from 0-3, but I still want to spawn index 0
        if ( val == 0 )
        {
            val = 1;
        }

        if ( GameLord.instance.Scene == Scene.Level_1 )
        {
            for( int i = 0; i < val; i++ )
            {
                if ( headA == -1 )
                {
                    headA = opponentAs.Length - 1;
                    Debug.Log ( "reset headA to " + headA );
                }
                OnUnleashOpponent ( (float) i, (float) val, opponentAs, headA );
                availableOpponentAs.Add ( opponentAs [ headA ] );
                headA--;
                if ( debug ) Debug.Log ( "In unleash: availableHitOpponentAs count: " + availableOpponentAs.Count );
            }
        }
        else if ( GameLord.instance.Scene == Scene.Level_2 )
        {
            for ( int i = 0; i < val; i++ )
            {
                if ( headB == -1 )
                {
                    headB = opponentBs.Length - 1;
                    Debug.Log ( "reset headB to " + headB );
                }
                OnUnleashOpponent ( ( float ) i, ( float ) val, opponentBs, headB );
                availableOpponentBs.Add ( opponentBs [ headB ] );
                headB--;
                if ( debug ) Debug.Log ( "In unleash: availableHitOpponentBs count: " + availableOpponentBs.Count );
            }
        }
    }

    #endregion

    #region private functions

    void Spawn ( )
    {
        for ( int i = 0; i < opponentAs.Length; i++ )
        {
            GameObject newGameObject = GameObject.Instantiate(opponentAPrefab);
            if ( newGameObject.GetComponent<Opponent> ( ) != null )
            {
                opponentAs [ i ] = newGameObject.GetComponent<Opponent> ( );
                opponentAs [ i ].OpponentLord = this;
                opponentAs [ i ].transform.SetParent ( this.transform );
                opponentAs [ i ].gameObject.SetActive ( false );
            }
            else
            {
                Debug.LogError ( "Prefab does not have an Opponent component" );
                return;
            }
        }

        for ( int i = 0; i < opponentBs.Length; i++ )
        {
            GameObject newGameObject = GameObject.Instantiate( opponentBPrefab );
            if ( newGameObject.GetComponent<Opponent> ( ) != null )
            {
                opponentBs [ i ] = newGameObject.GetComponent<Opponent> ( );
                opponentBs [ i ].OpponentLord = this;
                opponentBs [ i ].transform.SetParent ( this.transform );
                opponentBs [ i ].gameObject.SetActive ( false );
            }
            else
            {
                Debug.LogError ( "Prefab does not have an Opponent component" );
                return;
            }
        }
    }

    #endregion

    #region inherited functions

    private void OnEnable ( )
    {
        headA = opponentAs.Length - 1;
        headB = opponentBs.Length - 1;
        Spawn ( );
    }

    void Update ( )
    {
        if( GameLord.instance.Scene == Scene.Level_1 )
        {
            if ( scoreTriggerHit >= GameLord.instance.GetCurrentSceneRoot ( ).GetOriginalTriggerCount ( ) / ((int)difficulty + 1 ) )
            {
                if ( debug ) Debug.Log ( "Player is levelling up. Score A: " + scoreOpponentAHit + ", ScoreT: " + scoreTriggerHit + ", difficulty is " + difficulty );
                
                // Reset the score.
                scoreOpponentAHit = 0;
                scoreTriggerHit = 0;

                // Store any leftover opponents.
                for ( int i = availableOpponentAs.Count - 1; i >= 0; i-- )
                {
                    if ( availableOpponentAs [ i ] != null && availableOpponentAs [ i ].isActiveAndEnabled )
                    {
                        StoreOpponent ( availableOpponentAs [ i ] );
                    }
                }

                // Deliver "Level Up" feedback.
                GameLord.instance.GetCurrentSceneRoot ( ).LevelUpFeedback ( );
            }
        }
        else if ( GameLord.instance.Scene == Scene.Level_2 )
        {
            if ( scoreOpponentBHit > 50 )
            {
                if ( debug ) Debug.Log ( "Player is levelling up. Score B: " + scoreOpponentBHit + ", difficulty is " + difficulty );

                // Reset the score.
                scoreOpponentBHit = 0;

                // Store any leftover opponents.
                for ( int i = availableOpponentBs.Count - 1; i >= 0; i-- )
                {
                    if ( availableOpponentBs [ i ] != null && availableOpponentBs [ i ].isActiveAndEnabled )
                    {
                        StoreOpponent ( availableOpponentBs [ i ] );
                    }
                }

                // Deliver "Level Up" feedback.
                GameLord.instance.GetCurrentSceneRoot ( ).LevelUpFeedback ( );
            }
        }
    }

    #endregion
}
