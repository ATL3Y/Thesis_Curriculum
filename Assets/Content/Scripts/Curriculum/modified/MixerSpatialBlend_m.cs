using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerSpatialBlend_m : MixerFeature_m
{

    #region inherited functions

    public override void FillTo ( float targetFillAmount )
    {
        base.FillTo ( targetFillAmount );
    }

    private void Update ( )
    {
        mixerController.SetSpatialBlend ( currentFillAmount );
    }

    private void Start ( )
    {

    }

    #endregion

}
