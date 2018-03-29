using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_Collectables_m: MonoBehaviour
{
    #region private functions

    // Update is called once per frame
    private void Update ( )
    {
        if ( PlayerCurriculum.instance.LeftADown ( ) )
        {
            CollectableController_m.instance.CreateCollectable ( PlayerCurriculum.instance.GetLeftHand ( ).position );
        }

        if ( PlayerCurriculum.instance.RightADown ( ) )
        {
            CollectableController_m.instance.CreateCollectable ( PlayerCurriculum.instance.GetRightHand ( ).position );
        }

        if ( PlayerCurriculum.instance.LeftThumbClickDown ( ) )
        {
            CollectableController_m.instance.CreateAllCollectables ( PlayerCurriculum.instance.GetLeftHand ( ).position + 2.5f * PlayerCurriculum.instance.GetLeftHand ( ).forward );
        }

        if ( PlayerCurriculum.instance.RightThumbClickDown ( ) )
        {
            CollectableController_m.instance.CreateAllCollectables ( PlayerCurriculum.instance.GetRightHand ( ).position + 2.5f * PlayerCurriculum.instance.GetRightHand ( ).forward );
        }
    }

    #endregion
}
