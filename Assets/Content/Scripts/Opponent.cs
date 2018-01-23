using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    #region public data

    public enum OpponentType{ A, B }
    public OpponentType opponentType;

    // This is set by OpponentLord.cs on instantiation.
    public OpponentLord OpponentLord { protected get; set; }

    // This is only for Opponenet B.
    public int MyMusicTrackIndex { get; set; }

    // Spatial offset of target
    public float Offset { get; set; }

    #endregion

    #region private data

    [SerializeField] protected Material baseMat;
    [SerializeField] protected Material rewardMat;
    [SerializeField] protected Material punishMat;
    [SerializeField] protected Material rhythmMat;
    [SerializeField] protected GameObject fx_hitHand_one;
    [SerializeField] protected GameObject fx_hitHand_two;
    [SerializeField] protected GameObject fx_hitTrigger;
    [SerializeField] protected Renderer [ ] rends;
    public enum OpponentState { Untouched, Passed, Missed, Hit }
    public OpponentState state;
    protected float timer;
    protected float speed;
    protected float cycleCount;
    protected float cycleDuration;
    protected bool finished;
    protected bool debug = true;
    protected List<HandSituation> HandSituations = new List<HandSituation>();

    #endregion

    protected class HandSituation
    {
        public VRNodeMinion hand;
        public GameObject fx_hitHand;
        public float myDistToCore = 1.0f;
    }

    #region protected functions

    protected void ToDisable()
    {
        if ( debug ) Debug.Log("to disable");
        finished = false;
        OpponentLord.StoreOpponent( this );
    }

    protected void SetRends ( Material mat )
    {
        for(int i = 0; i < rends.Length; i++ )
        {
            rends [ i ].material = mat;
        }
    }

    #endregion

    #region virtual functions

    protected virtual void OnEnable()
    {
        // If this is just part of GameLord.Instance.InitScenes(), bail.
        if ( GameLord.instance.loading != Loading.Loaded )
            return;

        finished = false;
        state = OpponentState.Untouched;
        timer = 2.0f;

        // Opponent can be hit on the 0 or 2 beats, which is half the BPM. 
        cycleDuration = 60.0f / GameLord.instance.MusicLord.GetBPM ( ) / 2.0f;
        cycleCount = 32.0f; 

        fx_hitHand_one.SetActive(false);
        fx_hitHand_two.SetActive(false);
        fx_hitTrigger.SetActive(false);
    }

    #endregion
}
