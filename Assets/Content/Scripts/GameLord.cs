using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;
using UnityEngine.PostProcessing;

// How will you handle the water scene? Maybe it is not in this lineup. 
public enum Loading { Default = -1, Loaded }
public enum Scene { Init = -2, Title = -1, Level_1, Level_2, Level_3, Credits }

public class GameLord : MonoBehaviour
{
    #region public data

    public static GameLord instance;
    public VRNodeLord Player { get; set; }
    public MusicLord MusicLord { get; set; }
    public OpponentLord OpponentLord { get; set; }
    public Screenspace_Fade Screenspace_Fade { get; set; }
    public SceneRoot TitleRoot { get; set; }
    public Scene Scene { get; set; }
    public Loading loading { get; set; }
    public enum DeviceType { Oculus, Vive }
    public Vector3 FloorCenter { get; set; }

    #endregion

    #region private data

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject musicLordPrefab;
    [SerializeField] GameObject opponentLordPrefab;
    private AsyncOperation[] async;
    private SceneRoot[] sceneRoots;
    [SerializeField] PostProcessingProfile ppProfile;
    private DeviceType deviceType;
    private float blackoutTimer;
    private float blackoutLength;
    private bool debug = false;

    #endregion

    #region public functions

    public DeviceType GetDeviceType ( )
    {
        return deviceType;
    }

    public void SetSceneRoots ( int buildIndex, SceneRoot sceneRoot )
    {
        sceneRoots [ buildIndex - 2 ] = sceneRoot;
        sceneRoots [ buildIndex - 2 ].gameObject.SetActive ( false );
    }

    public SceneRoot GetCurrentSceneRoot ( )
    {
        if( Scene == Scene.Init || Scene == Scene.Title )
        {
            return TitleRoot;
        }
        // nature scene root index = 1
        // nature Scene number = 1
        return sceneRoots [ ( int ) Scene ];
    }

    public void IterateState ( )
    {
        if( (int) Scene == SceneManager.sceneCountInBuildSettings - 3 )
        {
            Scene = Scene.Init;
        }

        if ( (int) Scene == -2 )
        {
            TitleRoot.gameObject.SetActive ( true );
            SceneManager.SetActiveScene ( SceneManager.GetSceneByBuildIndex ( 1 ) );
            Player.transform.SetParent ( TitleRoot.transform );
            Scene = Scene.Title;
        }
        else if ( (int) Scene == -1 )
        {
            TitleRoot.gameObject.SetActive ( false );
            async [ 0 ].allowSceneActivation = true;
            sceneRoots [ 0 ].gameObject.SetActive ( true );
            SceneManager.SetActiveScene ( SceneManager.GetSceneByBuildIndex ( 2 ) );
            Player.transform.SetParent ( sceneRoots [ 0 ].transform );
            Scene = Scene.Level_1;
        }
        else
        {
            async [ ( int ) Scene ].allowSceneActivation = false;
            sceneRoots [ ( int ) Scene ].gameObject.SetActive ( false );
            async [ ( int ) Scene + 1 ].allowSceneActivation = true;
            sceneRoots [ ( int ) Scene + 1 ].gameObject.SetActive ( true );
            SceneManager.SetActiveScene ( SceneManager.GetSceneByBuildIndex ( ( int ) Scene + 3 ) );
            Player.transform.SetParent ( sceneRoots [ ( int ) Scene + 1 ].transform );
            Scene++;
        }

        if ( debug ) Debug.Log ( "In IterateState. Scene is " + Scene + " and active scene is " + SceneManager.GetActiveScene() );
    }

    public void OnBlackout ( )
    {
        // Change the scene.
        if ( debug ) Debug.Log ( "in scene switch" );

        // Switch scene.
        IterateState ( );

        // Set player avatar configuration start position and floor center
        Transform playerStartPosition = GetCurrentSceneRoot().transform.Find("PlayerStartPosition");
            
        if ( playerStartPosition != null )
        {
            Player.transform.localPosition = playerStartPosition.position;
            FloorCenter = playerStartPosition.position;
        }
        else
        {
            Player.transform.localPosition = Vector3.zero;
            FloorCenter = Vector3.zero;
        }

        // Change the music.
        MusicLord.OnSceneSwitch ( );

        // Clear global objects and parent colliding objects to the active scene.
        OpponentLord.OnSceneSwitch ( );

        blackoutTimer = blackoutLength;
    }

    #endregion

    #region private functions

    private void InitScenes ( )
    {
        // Init and Tutorial aren't async
        int numAsyncScenes = SceneManager.sceneCountInBuildSettings - 2; 
        async = new AsyncOperation [ numAsyncScenes ];

        // Start in the Tutorial
        SceneManager.LoadScene ( "Title", LoadSceneMode.Additive );

        // Load other scenes async
        for ( int i = 0; i < numAsyncScenes; i++ )
        {
            async [ i ] = SceneManager.LoadSceneAsync ( i + 2, LoadSceneMode.Additive );
            async [ i ].allowSceneActivation = false;
        }

        sceneRoots = new SceneRoot [ numAsyncScenes ];
    }

    private void FireTutorialOnAllScenesLoaded ( )
    {
        if ( loading == Loading.Loaded )
        {
            return;
        }

        loading = Loading.Loaded;
        for ( int i = 0; i < async.Length; i++ )
        {
            if ( async [ i ] == null || !async [ i ].isDone )
            {
                loading = Loading.Default;
                break;
            }
        }

        // Trigger the tutorial
        if ( loading == Loading.Loaded )
        {
            if ( debug ) Debug.Log ( "loaded" );
            IterateState ( );
        }
    }

    private void InitPlayer ( )
    {
        GameObject temp = GameObject.Instantiate ( playerPrefab );
        if(temp.GetComponent<VRNodeLord>() != null )
        {
            Player = temp.GetComponent<VRNodeLord> ( );
            Player.transform.SetParent ( this.transform );
            Screenspace_Fade = Player.head.GetComponent<Screenspace_Fade> ( );
        }
        else
        {
            Debug.LogError ( "Player prefab does not have a VRNodeLord component." );
        }
    }

    private void InitMusicLord ( )
    {
        GameObject temp = GameObject.Instantiate ( musicLordPrefab );
        if ( temp.GetComponent<MusicLord> ( ) != null )
        {
            MusicLord = temp.GetComponent<MusicLord> ( );
            MusicLord.transform.SetParent ( Player.transform );
        }
        else
        {
            Debug.LogError ( "MusicLord prefab does not have a MusicLord component." );
        }
    }

    private void InitOpponentLord ( )
    {
        GameObject temp = GameObject.Instantiate ( opponentLordPrefab );
        if ( temp.GetComponent<OpponentLord> ( ) != null )
        {
            OpponentLord = temp.GetComponent<OpponentLord> ( );
            OpponentLord.transform.SetParent ( this.transform );
        }
        else
        {
            Debug.LogError ( "OpponentLord prefab does not have a OpponentLord component." );
        }
    }

    #endregion

    #region inherited functions

    void Awake ()
    {
        instance = this;
        DontDestroyOnLoad ( gameObject );
        Cursor.visible = false;

        // Tell Oculus to use roomscale
        if ( UnityEngine.XR.XRDevice.isPresent )
        {
            if ( debug ) Debug.Log ( "UnityEngine.XR.XRDevice.model: " + UnityEngine.XR.XRDevice.model );
            if ( UnityEngine.XR.XRDevice.model == "Oculus Rift CV1")
            {
                if ( debug ) Debug.Log("in roomscale set");
                UnityEngine.XR.XRDevice.SetTrackingSpaceType ( UnityEngine.XR.TrackingSpaceType.RoomScale );
            }
        }

        Scene = Scene.Init;
        loading = Loading.Default;

        string model = UnityEngine.XR.XRDevice.model != null ? UnityEngine.XR.XRDevice.model : "";
        if ( model.IndexOf ( "Rift" ) >= 0 )
        {
            deviceType = DeviceType.Oculus;
        }
        else
        {
            deviceType = DeviceType.Vive;
        }

        blackoutLength = 2.0f;
        blackoutTimer = blackoutLength;

        InitPlayer ( );
        InitMusicLord ( );
        InitOpponentLord ( );
        InitScenes ( );
    }

    void Start ( )
    {

    }

    void Update()
    {
        if ( Input.GetKeyDown ( KeyCode.Space ) || Player.leftHand.ButtonADown || Player.rightHand.ButtonADown )
        {
            //IterateState ( );
            Screenspace_Fade.screenFade = Screenspace_Fade.ScreenFade.FadeToBlack;
        }

        if ( Input.GetKeyDown ( KeyCode.S ) )
        {
            GetCurrentSceneRoot ( ).availableTriggers [ 0 ].OnHit ( );
        }

        if ( blackoutTimer > 0.0f )
        {
            blackoutTimer -= Time.deltaTime;
            if ( blackoutTimer <= 0.0f )
            {
                Screenspace_Fade.screenFade = Screenspace_Fade.ScreenFade.FadeFromBlack;
            }
        }

        FireTutorialOnAllScenesLoaded ( );
    }



    #endregion
}
