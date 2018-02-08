using UnityEngine;
using System.Collections.Generic;

public class MusicLord : MonoBehaviour
{
    #region public data

    // Keep track of which stems are on or off
    public bool [ ] StemsOn { get; set; }

    #endregion

    #region private data
    [SerializeField] GameObject stemContainer;

    private AudioSource[] clipSources;
    private AudioSource[] stemSources;
    private int sampleDataLength = 2048; // 160ms
    private float[] clipSampleData;
    private enum ClipType { Call, Error, ResponseGhost, Response, Reward }
    private Vector3 drawPos = Vector3.zero;
    private float baseRange = 3.0f;
    private float sinRange = 2.5f;
    private bool debug = true;
    private float BPM = 96.0f;
    private int rewardCount;
    private int stemIndex = 0;
    private float timer = 0.0f;
    private int val = 0;

    #endregion

    #region public functions

    public void OnSceneSwitch ( )
    {
        // This fades the scene root ambient track up or down 
        GameLord.instance.GetCurrentSceneRoot ( ).sceneState = SceneRoot.SceneState.FadeOutAmbient;

        // Restart stems for levels 
        if ( GameLord.instance.Scene == Scene.Level_2 )
        {
            // Automatically play stems on level load
            MusicStart ( );
        }
    }

    public float GetBPM ( )
    {
        return BPM;
    }

    public void SetStemVolume ( float vol, int index )
    {
        // Set the stem at that index to that volume.
        stemSources [ index ].volume = vol;
    }

    public float GetStemVolume ( int index )
    {
        // Return the volume of the stem at that index.
        return stemSources [ index ].volume;
    }

    // Called from player OnDamage()
    public void PlayErrorClip ( Vector3 pos )
    {
        if ( debug ) Debug.Log ( "playing error clip" );
        clipSources [ ( int ) ClipType.Error ].transform.position = pos;
        clipSources [ ( int ) ClipType.Error ].Play ( );
    }

    // Iterate through OppBTracks and return the next that is on.
    public int NextStemOnIndex ( )
    {
        return 0;
        var startIndex = stemIndex;
        if ( startIndex == 0 && stemSources.Length <= 1 )
        {
            return -1;
        }
        /*
        while ( true )
        {
            stemIndex++;
            if ( stemIndex >= stemSources.Length )
            {
                stemIndex = 1;
            }
            if ( StemsOn [ stemIndex ] )
            {
                return stemIndex;
            }
            if ( startIndex == stemIndex )
            {
                return -1;
            }
        }
        */
    }

    // Called from OpponentA
    public void PlayResponseClip ( Vector3 pos )
    {
        if ( debug ) Debug.Log ( "playing reward clip" );
        clipSources [ rewardCount ].transform.position = pos;
        clipSources [ rewardCount ].Play ( );
        rewardCount++;
        if ( rewardCount > clipSources.Length - 1 )
        {
            rewardCount = ( int ) ClipType.Reward;
        }
    }

    #endregion

    #region private functions

    private void MusicStart ( )
    {
        for ( int i = 0; i < stemSources.Length; i++ )
        {
            if ( !stemSources [ i ].GetComponent<AudioSource> ( ).isPlaying )
            {
                stemSources [ i ].GetComponent<AudioSource> ( ).Play ( );
            }
        }
    }
    
    private void Init ( )
    {
        Object[] clipsArray = Resources.LoadAll ( "Music/Clips" );
        clipSources = new AudioSource [ clipsArray.Length ];

        for ( int i = 0; i < clipsArray.Length; i++ )
        {
            GameObject MusicMinion = new GameObject();
            MusicMinion.transform.SetParent ( this.transform );
            clipSources [ i ] = MusicMinion.AddComponent<AudioSource> ( );
            clipSources [ i ].clip = ( AudioClip ) clipsArray [ i ];
            clipSources [ i ].playOnAwake = false;
            clipSources [ i ].spatialBlend = 1.0f;
            clipSources [ i ].loop = false;
            clipSources [ i ].volume = 0.6f;
        }

        Object[] stemsArray = Resources.LoadAll ( "Music/Stems" );
        stemSources = new AudioSource [ stemsArray.Length ];

        for ( int i = 0; i < stemSources.Length; i++ )
        {
            stemSources [ i ] = gameObject.AddComponent<AudioSource> ( );
            stemSources [ i ].clip = ( AudioClip ) stemsArray [ i ];
            stemSources [ i ].playOnAwake = false;
            stemSources [ i ].spatialBlend = 1.0f;
            stemSources [ i ].loop = true;
            stemSources [ i ].volume = ( i < 1 ) ? 1.0f : 0.0f; // Only set the first track's volume to 1.
            // Color color = new Color (Random.Range (0.6f, 1.2f), Random.Range (0.6f, 1.2f), Random.Range (0.6f, 1.2f), Random.Range (0.8f, 1.0f));
        }

        StemsOn = new bool [ stemSources.Length ];
    }

    #endregion

    #region inherited functions

    private void Update ( )
    {
        // Make sure Init() has run first. 
        if( stemSources.Length == 0 )
        {
            return;
        }

        // Update which stems are on and off.
        for ( int i = 0; i < stemSources.Length; i++ )
        {
            stemSources [ i ].clip.GetData ( clipSampleData, stemSources [ i ].timeSamples );

            float stemVol = 0f;
            foreach ( float sample in clipSampleData )
            {
                stemVol += Mathf.Abs ( sample );
            }

            // if( debug ) Debug.Log ( "Stem Volume: " + stemVol);
            if ( stemVol < 6.0f )
            {
                StemsOn [ i ] = false;
            }
            else
            {
                StemsOn [ i ] = true;
            }
        }

        // Update timer 
        timer -= Time.deltaTime;
        if( timer <= 0.0f )
        {
            // 60 sec / number of beats per min = the length in time of one beat.
            // Multiply this by 4 to unleash an opponent once per measure.
            GameLord.instance.OpponentLord.UnleashOpponent ( val );
            val++;
            if( val > 3 )
            {
                val = 0;
            }
            timer = 60.0f / BPM * 4.0f;
        }
    }

    private void Start ( )
    {
        clipSampleData = new float [ sampleDataLength ];
        Init ( );
        MusicStart ( );
    }

    #endregion
}