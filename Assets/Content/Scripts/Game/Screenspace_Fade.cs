using UnityEngine;
using System.Collections;

public class Screenspace_Fade : MonoBehaviour
{
    #region public data

    public Material mat;
    public enum ScreenFade { Default = -1, FadeToBlack, FadeFromBlack }
    public ScreenFade screenFade;
    private float mult = 5.0f;

    #endregion

    #region public functions

    #endregion

    #region private functions

    void OnRenderImage ( RenderTexture src, RenderTexture dest )
    {
        Graphics.Blit ( src, dest, mat );
    }

    #endregion

    #region inherited functions

    private void Update ( )
    {
        if( screenFade == ScreenFade.Default )
        {
            return;
        }
        else
        {
            Color color = mat.GetColor ( "_FadeToColor" );
            if ( screenFade == ScreenFade.FadeToBlack )
            {
                if ( color.r > 0.0f )
                {
                    float offset = 0.5f * Time.deltaTime;
                    color.r -= offset;
                    color.g -= offset;
                    color.b -= offset;

                    mat.SetColor ( "_FadeToColor", color );
                }
                else
                {
                    screenFade = ScreenFade.Default;
                    GameLord.instance.OnBlackout ( );
                }
            }
            else if ( screenFade == ScreenFade.FadeFromBlack )
            {
                if ( color.r < 1.0f )
                {
                    float offset = 0.5f * Time.deltaTime;
                    color.r += offset;
                    color.g += offset;
                    color.b += offset;

                    mat.SetColor ( "_FadeToColor", color );
                }
                else
                {
                    screenFade = ScreenFade.Default;
                }
            }
        }
    }

    private void Start ( )
    {
        screenFade = ScreenFade.Default;
        mat.SetColor ( "_FadeToColor", Color.black );
    }

    #endregion

}