using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Damage : MonoBehaviour
{

    #region public data

    #endregion

    #region private data

    private Quaternion startRot = Quaternion.Euler(90.0f, 0.0f, 0.0f);
    private Quaternion endRot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
    private Renderer rend;
    private bool debug = false;

    #endregion

    #region public functions

    public void OnDamage ( )
    {
        if (debug) Debug.Log ( "On Damage called" );
        transform.localRotation = startRot;
        rend.enabled = true;
    }

    #endregion

    #region private functions

    #endregion

    #region inherited functions

    private void Start ( )
    {
        rend = GetComponent<Renderer> ( );
        rend.enabled = false;
    }

    private void Update ( )
    {
        transform.localRotation = Quaternion.Lerp ( transform.localRotation, endRot, 2.0f * Time.deltaTime );

        if( transform.localRotation.eulerAngles.x > 270.0f && transform.localRotation.eulerAngles.x < 275.0f )
        {
            rend.enabled = false;
        }
    }

    #endregion

}
