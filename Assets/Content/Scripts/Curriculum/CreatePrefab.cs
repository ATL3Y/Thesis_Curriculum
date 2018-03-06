using UnityEngine;
using System.Collections;

public class CreatePrefab : MonoBehaviour
{
    #region private data 

    private bool debug = false;
    [SerializeField] GameObject itemToCreate;
    [SerializeField] Vector3 spawnPosition;

    #endregion

    #region private functions

    private void Start ( )
    {

    }

    private void OnTriggerEnter ( Collider other )
    {
        if ( other.gameObject.layer == LayerMask.NameToLayer("Hand") )
        {
            //Create a prefab
            if ( debug ) Debug.Log ( "Spawning: " + itemToCreate );
            Instantiate ( itemToCreate, spawnPosition, Quaternion.identity );
        }
    }

    #endregion
}
