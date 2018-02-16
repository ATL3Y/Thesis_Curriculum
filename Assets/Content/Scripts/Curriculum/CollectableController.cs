using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    #region public data

    public static CollectableController instance;

    #endregion

    #region private data

    private List<Collectable> collectables;
    private float constraint = 5.0f;
    [SerializeField] GameObject prefab;

    #endregion

    #region public functions

    public void RemoveCollectable ( Collectable collectable )
    {
        collectables.Remove ( collectable );
    }

    public void CreateAllCollectables ( Vector3 pos )
    {
        Vector3 startPos = new Vector3( pos.x, 0.0f, pos.y );

        for ( int i = 0; i < constraint; i++ )
        {
            CreateCollectable ( new Vector3 ( i, i, i ) );
        }
    }

    public void CreateCollectable ( Vector3 pos )
    {
        GameObject newGameObject = Instantiate( prefab );
        newGameObject.transform.position = pos;
        newGameObject.transform.localScale = new Vector3 ( .4f, .4f, .4f );
        newGameObject.GetComponent<Renderer> ( ).material.color = 1.2f * new Color ( Random.value, Random.value, Random.value );
        Collectable newCollectable = newGameObject.GetComponent<Collectable>();
        newCollectable.collectableController = this;
        collectables.Add ( newCollectable );
    }

    #endregion

    #region inherited functions
    // Use this for initialization
    private void Start ( )
    {
        instance = this;
        collectables = new List<Collectable> ( );
    }

    #endregion
}
