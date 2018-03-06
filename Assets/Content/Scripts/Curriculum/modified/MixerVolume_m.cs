using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerVolume_m : MixerFeature_m
{

    #region inherited functions

    public override void FillTo ( float targetFillAmount )
    {
        base.FillTo ( targetFillAmount );
    }

    private void Update ( )
    {
        mixerController.SetVolume ( currentFillAmount );
    }

    private void Start ( )
    {

    }

    #endregion
}
