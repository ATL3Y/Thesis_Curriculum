using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class SceneRoot : MonoBehaviour
{
    #region public data

    // Objects remove themselves from these lists on disable or finished.  
    // The hands check these lists.
    public List<Shrink> availableTriggers = new List<Shrink>();
    public List<FillCircuit> availableCircuits = new List<FillCircuit>();
    public enum SceneState { Default = -1, FadeInAmbient, FadeOutAmbient }
    public SceneState sceneState { get; set; }

    #endregion

    #region private data

    private bool debug = true;

    // Full arrays collected on enable
    private Shrink[] triggers;
    private FillCircuit[] circuits;

    // Store the ambient audio
    private AudioSource[] ambientSources;

    #endregion

    #region public functions

    public void FadeAmbient ( )
    {
        // Fade up ambientSources
        if ( sceneState == SceneState.FadeInAmbient )
        {
            for ( int i = 0; i < ambientSources.Length; i++ )
            {
                ambientSources [ i ].volume += Time.deltaTime;
            }

            if ( ambientSources.Length > 0 && ambientSources [ ambientSources.Length - 1 ].volume >= .99f )
            {
                sceneState = SceneState.Default;
            }
        }
        // Fade down ambientSources.
        else if ( sceneState == SceneState.FadeOutAmbient )
        {
            for ( int i = 0; i < ambientSources.Length; i++ )
            {
                ambientSources [ i ].volume -= Time.deltaTime;
            }

            if ( ambientSources.Length > 0 && ambientSources [ ambientSources.Length - 1 ].volume <= .01f )
            {
                sceneState = SceneState.Default;
            }
        }
    }

    public int GetOriginalTriggerCount ( )
    {
        return triggers.Count();
    }

    public void LevelUpFeedback ( )
    {
        if( GameLord.instance.Scene == Scene.Level_1 )
        {
            for ( int i = 0; i < availableCircuits.Count; i++ )
            {
                availableCircuits [ i ].FillTo ( 1.0f );
            }
        }
        else if( GameLord.instance.Scene == Scene.Level_2 )
        {
            
        }

        DelayedCallback(3.0f, () =>
        {
            GameLord.instance.Screenspace_Fade.screenFade = Screenspace_Fade.ScreenFade.FadeToBlack;
        });
    }

    private void DelayedCallback(float delay, Action action)
    {
        StartCoroutine(Co_DelayedCallback(delay, action));
    }

    private IEnumerator Co_DelayedCallback(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    #endregion

    #region private functions

    private void LoadAudio ( )
    {
        Object[] clips = Resources.LoadAll( "Music/Ambient" );
        if ( debug ) Debug.Log ( "clipsStart length is: " + clips.Length );

        ambientSources = new AudioSource [ clips.Length ];
        for ( int i = 0; i < ambientSources.Length; i++ )
        {
            ambientSources [ i ] = gameObject.AddComponent<AudioSource> ( );
            ambientSources [ i ].playOnAwake = true;
            ambientSources [ i ].loop = false;
            ambientSources [ i ].spatialBlend = 1.0f;
            ambientSources [ i ].volume = 1.0f;
            if ( ( AudioClip ) clips [ i ] != null )
            {
                ambientSources [ i ].clip = ( AudioClip ) clips [ i ];
            }
            else
            {
                Debug.LogError ( "Music/" + gameObject.scene.name + "/Ambient/Start should only contain AudioClip objects." );
            }
        }
    }

    private void Init ( )
    {
        triggers = GetComponentsInChildren<Shrink> ( );
        if ( debug ) Debug.Log ( triggers.Length + " Triggers in scene " + gameObject.scene.name );

        for ( int i = 0; i < triggers.Length; i++ )
        {
            availableTriggers.Add ( triggers [ i ] );
            // triggers [ i ].OnHit ( ); // for testing
        }

        circuits = GetComponentsInChildren<FillCircuit> ( );
        if ( debug ) Debug.Log ( circuits.Length + " Circuits in scene " + gameObject.scene.name );

        // Does this include gloves? It shouldn't. 
        for ( int i = 0; i < circuits.Length; i++ )
        {
            availableCircuits.Add ( circuits [ i ] );
            availableCircuits [ i ].FillTo ( 0.0f ); 
        }

        if ( GetComponentInChildren<Light> ( ) != null )
        {
            if ( gameObject.scene.buildIndex != 2 )
            {
                Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                GetComponentInChildren<Light>().color = color;
            }
        }

        LoadAudio ( );
    }

    #endregion

    #region inherited functions

    private void Update ( )
    {
        if ( sceneState == SceneState.FadeInAmbient || sceneState == SceneState.FadeOutAmbient )
        {
            FadeAmbient ( );
        }
    }

    void Start ( )
    {
        if ( gameObject.scene.buildIndex == 1 )
        {
            GameLord.instance.TitleRoot = this;
        }
        else
        {
            GameLord.instance.SetSceneRoots ( gameObject.scene.buildIndex, this );
        }

        sceneState = SceneState.FadeInAmbient;

        Init ( );
    }

    #endregion
}
