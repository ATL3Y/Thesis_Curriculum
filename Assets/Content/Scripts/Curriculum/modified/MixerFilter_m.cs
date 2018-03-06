using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerFilter_m : MixerFeature_m
{

    #region inherited functions

    public override void FillTo ( float targetFillAmount )
    {
        base.FillTo ( targetFillAmount );
    }

    private void Update ( )
    {
        mixerController.SetFilter ( currentFillAmount );
    }

    private void Start ( )
    {

    }

    #endregion

}
