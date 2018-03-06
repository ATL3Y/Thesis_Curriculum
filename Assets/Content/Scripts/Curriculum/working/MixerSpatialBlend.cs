using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerSpatialBlend : MixerFeature
{

    #region inherited functions

    public override void FillTo ( float targetFillAmount )
    {
        base.FillTo ( targetFillAmount );
    }

    protected override void Update ( )
    {
        base.Update ( );
        mixerController.SetSpatialBlend ( currentFillAmount );
    }

    protected override void Start ( )
    {
        base.Start ( );
    }

    #endregion

}
