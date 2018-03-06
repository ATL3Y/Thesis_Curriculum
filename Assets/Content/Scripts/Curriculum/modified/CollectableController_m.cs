using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController_m: MonoBehaviour
{
    #region public data

    public static CollectableController_m instance;

    #endregion

    #region private data

    private List<Collectable_m> collectables;
    private int constraint = 5;
    [SerializeField] GameObject prefab;

    #endregion

    #region public functions

    public void RemoveCollectable ( Collectable_m collectable )
    {
        collectables.Remove ( collectable );
    }

    public void CreateAllCollectables ( Vector3 pos )
    {
        // Fix or fill this in (#ATL).

    }

    public void CreateCollectable ( Vector3 pos )
    {
        GameObject newGameObject = Instantiate( prefab );
        newGameObject.transform.position = pos;
        newGameObject.transform.localScale = new Vector3 ( .4f, .4f, .4f );
        newGameObject.GetComponent<Renderer> ( ).material.color = 1.2f * new Color ( Random.value, Random.value, Random.value );
        Collectable_m newCollectable = newGameObject.GetComponent<Collectable_m>();
        newCollectable.collectableController = this;
        collectables.Add ( newCollectable );
    }

    #endregion

    #region inherited functions
    // Use this for initialization
    private void Start ( )
    {
        instance = this;
        collectables = new List<Collectable_m> ( );
    }

    #endregion
}
